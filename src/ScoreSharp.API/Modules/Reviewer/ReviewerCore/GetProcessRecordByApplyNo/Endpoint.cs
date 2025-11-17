using System.Data;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetProcessRecordByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 查詢申請紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router: /ReviewerCore/GetProcessRecordByApplyNo/20241127X8342
        ///
        ///     Content:
        ///     1. 申請書進度紀錄
        ///     2. 影像入檔紀錄
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetProcessRecordByApplyNoResponse>))]
        [EndpointSpecificExample(typeof(取得申請紀錄_2000_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [OpenApiOperation("GetProcessRecordByApplyNo")]
        public async Task<IResult> GetProcessRecordByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetProcessRecordByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetProcessRecordByApplyNoResponse>>;

    public class Handler(IScoreSharpDapperContext scoreSharpDapperContext) : IRequestHandler<Query, ResultResponse<GetProcessRecordByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetProcessRecordByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken) =>
            ApiResponseHelper.Success(await GetLatestBankRecordByApplyNo(request.applyNo));

        private async Task<GetProcessRecordByApplyNoResponse> GetLatestBankRecordByApplyNo(string applyNo)
        {
            List<ApplyCreditCardInfoProcess> applyCreditCardInfoProcesses;
            List<ApplyFileLog> applyFileLogs;
            using (var conn = scoreSharpDapperContext.CreateScoreSharpConnection())
            {
                SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                    sql: "Usp_GetProcessRecord",
                    param: new { applyNo = applyNo },
                    commandType: CommandType.StoredProcedure
                );
                applyCreditCardInfoProcesses = results.Read<ApplyCreditCardInfoProcess>().ToList();
                applyFileLogs = results.Read<ApplyFileLog>().ToList();
            }

            var firstApplyFileLog = applyFileLogs.FirstOrDefault();
            string isHistory = firstApplyFileLog.IsHistory;
            string dbName = firstApplyFileLog.DBName;

            var applyFileDict = new Dictionary<Guid, string>();
            using (var connection = GetSqlConnection(isHistory, dbName))
            {
                var sql = $"SELECT * FROM [{dbName}].[dbo].[Reviewer_ApplyFile] WHERE ApplyNo = @ApplyNo";
                var applyFiles = await connection.QueryAsync<Reviewer_ApplyFile>(sql, new { ApplyNo = applyNo });
                applyFileDict = applyFiles.ToDictionary(x => x.FileId, x => x.FileName);
            }

            foreach (var applyFile in applyFileLogs)
            {
                applyFile.FileName = applyFileDict[applyFile.FileId];
            }

            GetProcessRecordByApplyNoResponse response = new()
            {
                ApplyCreditCardInfoProcess = applyCreditCardInfoProcesses,
                ApplyFileLog = applyFileLogs,
            };
            return response;
        }

        private SqlConnection GetSqlConnection(string isHistory, string dbName) =>
            isHistory switch
            {
                "Y" => scoreSharpDapperContext.CreateScoreSharpFileHisConnection(dbName),
                _ => scoreSharpDapperContext.CreateScoreSharpFileConnection(),
            };
    }
}
