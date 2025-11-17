using Dapper;
using ScoreSharp.Middleware.Modules.Test.DownloadApplicationDoc;

namespace ScoreSharp.Middleware.Modules.Test
{
    public partial class TestController
    {
        ///<summary>
        /// 下載申請書相關文件至共享資料夾
        /// </summary>
        /// <param name="applyId">PK</param>
        /// <returns></returns>
        [HttpPost("{applyId}")]
        [OpenApiOperation("GetDocumentsByApplyId")]
        public async Task<IResult> GetDocumentsByApplyId([FromRoute] string applyId) => await _mediator.Send(new Command(applyId));
    }
}

namespace ScoreSharp.Middleware.Modules.Test.DownloadApplicationDoc
{
    public record Command(string applyId) : IRequest<IResult>;

    public class Handler(ScoreSharpContext context, IScoreSharpDapperContext scoreSharpDapperContext) : IRequestHandler<Command, IResult>
    {
        public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var main = await context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.CardAppId == request.applyId);

                if (main == null)
                    return Results.BadRequest("案件不存在");

                string applyNo = main.ApplyNo;
                var files = await GetDocByApplyId(request.applyId);

                string targetPath = "";

                // 根據年度歸檔
                string thisyear = DateTime.Now.Year.ToString();
                string yearFolderPath = Path.Combine(targetPath, thisyear);
                if (!Directory.Exists(yearFolderPath))
                {
                    Directory.CreateDirectory(yearFolderPath);
                }

                // 根據案件編號建立資料夾
                string folderPath = Path.Combine(yearFolderPath, applyNo);
                Directory.CreateDirectory(folderPath);

                if (files == null || files.Count() == 0)
                    return Results.BadRequest("未提供檔案");

                var tasks = new List<Task>();
                foreach (var value in files)
                {
                    var fileNames = new Dictionary<string, byte[]>
                    {
                        { "idPic1", value.IdPic1 },
                        { "idPic2", value.IdPic2 },
                        { "upload1", value.Upload1 },
                        { "upload2", value.Upload2 },
                        { "upload3", value.Upload3 },
                        { "upload4", value.Upload4 },
                        { "upload5", value.Upload5 },
                        { "upload6", value.Upload6 },
                        { "uploadPDF", value.UploadPDF },
                    };
                    tasks.AddRange(
                        fileNames
                            .Where(x => x.Value != null)
                            .Select(fileName =>
                            {
                                string extension = fileName.Key == "uploadPDF" ? "pdf" : "jpg";
                                string filePath = Path.Combine(folderPath, $"{applyNo}_{fileName.Key}.{extension}");
                                return File.WriteAllBytesAsync(filePath, fileName.Value);
                            })
                    );
                }

                await Task.WhenAll(tasks);
                return Results.Ok("檔案上傳成功");
            }
            catch (Exception ex)
            {
                return Results.Problem($"檔案上傳失敗: {ex.Message}", statusCode: 500);
            }
        }

        public async Task<IEnumerable<ApplyFile>> GetDocByApplyId(string applyId)
        {
            string sql =
                @" SELECT [idPic1],
                            [idPic2],
                            [upload1],
                            [upload2],
                            [upload3],
                            [upload4],
                            [upload5],
                            [upload6],
                            [uploadPDF]
                            FROM [eCard_file].[dbo].[ApplyFile]
                            WHERE [cCard_AppId] = @ApplyId ";

            using var conn = scoreSharpDapperContext.CreateECardFileConnection();
            var result = await conn.QueryAsync<ApplyFile>(sql, new { ApplyId = applyId });
            return result;
        }
    }
}
