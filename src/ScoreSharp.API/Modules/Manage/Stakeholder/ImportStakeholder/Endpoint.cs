using System.Data;
using MiniExcelLibs;
using ScoreSharp.API.Modules.Manage.Stakeholder.ImportStakeholder;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Manage.Stakeholder
{
    public partial class StakeholderController
    {
        /// <summary>
        /// 匯入利害關係人
        /// </summary>
        /// <remarks>
        /// 僅接受 .txt 檔案，格式為 `ID,UserId`，每行一筆資料。<br />
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(匯入利害關係人_2000_ResEx),
            typeof(匯入利害關係人_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [HttpPost]
        [OpenApiOperation("ImportStakeholder")]
        public async Task<IResult> ImportStakeholder([FromForm] ImportStakeholderRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.Stakeholder.ImportStakeholder
{
    public record Command(ImportStakeholderRequest importStakeholderRequest) : IRequest<ResultResponse<string>>;

    public class Handler(
        IJWTProfilerHelper jwtHelper,
        IUITCSecurityHelper _uitcSecurityHelper,
        Microsoft.Extensions.Configuration.IConfiguration _configuration
    ) : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly string _tableName = "[dbo].[Reviewer_Stakeholder]";

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            IFormFile file = request.importStakeholderRequest.File;

            // 驗證檔案
            var validationResult = 驗證檔案(file);
            if (validationResult != null)
                return validationResult;

            // 轉換資料
            var dataTable = await 轉換成DataTable(file, jwtHelper.UserId);

            bool isSuccess = false;
            // 匯入資料庫
            using (var conn = new SqlConnection(_uitcSecurityHelper.DecryptConn(_configuration.GetConnectionString("ScoreSharp")!)))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Truncate Table
                        var truncateCmd = conn.CreateCommand();
                        truncateCmd.Transaction = tran;
                        truncateCmd.CommandText = $"TRUNCATE TABLE {_tableName}";
                        truncateCmd.ExecuteNonQuery();

                        using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, tran))
                        {
                            bulkCopy.DestinationTableName = _tableName;

                            bulkCopy.ColumnMappings.Add("UserId", "UserId");
                            bulkCopy.ColumnMappings.Add("ID", "ID");
                            bulkCopy.ColumnMappings.Add("AddUserId", "AddUserId");
                            bulkCopy.ColumnMappings.Add("AddTime", "AddTime");
                            bulkCopy.ColumnMappings.Add("IsActive", "IsActive");

                            bulkCopy.WriteToServer(dataTable);
                        }

                        tran.Commit();
                        Console.WriteLine("✅ 匯入成功！");
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❎ 匯入失敗，已回滾：" + ex.ToString());
                        tran.Rollback();
                        isSuccess = false;
                    }
                }
            }

            if (isSuccess)
            {
                return ApiResponseHelper.InsertSuccess($"匯入 {file.FileName} 成功，共 {dataTable.Rows.Count} 筆資料", null);
            }
            else
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"匯入 {file.FileName} 失敗，請聯繫管理員。");
            }
        }

        private ResultResponse<string>? 驗證檔案(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "未上傳檔案或檔案為空");

            if (!Path.GetExtension(file.FileName).Equals(".txt", StringComparison.CurrentCultureIgnoreCase))
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "檔案格式錯誤，請上傳 .txt 檔案");

            return null;
        }

        private async Task<DataTable> 轉換成DataTable(IFormFile file, string currentUserId)
        {
            using var stream = file.OpenReadStream();

            var rows = await MiniExcel.QueryAsync(
                stream,
                excelType: ExcelType.CSV,
                useHeaderRow: false,
                configuration: new MiniExcelLibs.Csv.CsvConfiguration() { Seperator = ',' }
            );
            DateTime now = DateTime.Now;

            var result = rows.Select(r =>
                {
                    var dict = (IDictionary<string, object>)r;
                    return new StackeholderCsv
                    {
                        ID = dict.ElementAt(0).Value?.ToString() ?? string.Empty,
                        UserId = dict.ElementAt(1).Value?.ToString() ?? string.Empty,
                        AddUserId = currentUserId,
                        AddTime = now,
                        IsActive = "Y",
                    };
                })
                .DistinctBy(x => new { x.UserId, x.ID })
                .ListToDataTable();

            return result;
        }
    }
}
