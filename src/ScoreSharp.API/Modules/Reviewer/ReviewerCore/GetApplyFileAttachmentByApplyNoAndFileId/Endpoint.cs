using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyFileAttachmentByApplyNoAndFileId;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得申請書案件檔案 By 檔名
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router:
        ///
        ///     /ReviewerCore/GetApplyFileAttachmentByApplyNoAndFileId/20241203X1101/def18b1a-ef3c-4ad6-a54b-0e96254fe564
        ///
        /// </remarks>
        /// <param name="applyNo">申請書號</param>
        /// <param name="fileId">檔案ID</param>
        /// <returns></returns>
        [HttpGet("{applyNo}/{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplyFileAttachmentByApplyNoAndFileIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得申請書案件檔案_2000_ResEx),
            typeof(取得申請書案件檔案查無檔案資訊_4001_ResEx),
            typeof(取得申請書案件檔案資訊已刪除_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyFileAttachmentByApplyNoAndFileId")]
        public async Task<IResult> GetApplyFileAttachmentByApplyNoAndFileId([FromRoute] string applyNo, [FromRoute] Guid fileId)
        {
            var result = await _mediator.Send(new Query(applyNo, fileId));

            if (result.ReturnCodeStatus != ReturnCodeStatus.成功)
                return Results.Ok(result);

            return Results.File(result.ReturnData.FileContent, result.ReturnData.ContentType, result.ReturnData.FileName);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyFileAttachmentByApplyNoAndFileId
{
    public record Query(string applyNo, Guid fileId) : IRequest<ResultResponse<GetApplyFileAttachmentByApplyNoAndFileIdResponse>>;

    public class Handler(ScoreSharpContext context, IScoreSharpDapperContext dapperContext)
        : IRequestHandler<Query, ResultResponse<GetApplyFileAttachmentByApplyNoAndFileIdResponse>>
    {
        public async Task<ResultResponse<GetApplyFileAttachmentByApplyNoAndFileIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var fileInfo = await context
                .Reviewer_ApplyFileAttachment.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplyNo == request.applyNo && x.FileId == request.fileId);

            if (fileInfo is null)
                return ApiResponseHelper.NotFound<GetApplyFileAttachmentByApplyNoAndFileIdResponse>(null, "找不到指定的檔案資訊");

            if (fileInfo.DeleteTime is not null && String.IsNullOrEmpty(fileInfo.DeleteUserId))
                return ApiResponseHelper.NotFound<GetApplyFileAttachmentByApplyNoAndFileIdResponse>(null, "檔案資訊已刪除");

            using var connection = GetSqlConnection(fileInfo.IsHistory, fileInfo.DBName);
            connection.Open();
            var sql = $"SELECT * FROM [{fileInfo.DBName}].[dbo].[Reviewer_ApplyFile] WHERE FileId = @FileId";
            var applyFile = await connection.QueryFirstOrDefaultAsync<Reviewer_ApplyFile>(sql, new { FileId = request.fileId });

            return ApiResponseHelper.Success(
                new GetApplyFileAttachmentByApplyNoAndFileIdResponse
                {
                    FileContent = applyFile.FileContent,
                    FileName = applyFile.FileName,
                    ContentType = Path.GetExtension(applyFile.FileName).ToLower() switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        _ => "application/octet-stream",
                    },
                },
                "取得申請書案件檔案成功"
            );
        }

        private SqlConnection GetSqlConnection(string isHistory, string dbName) =>
            isHistory switch
            {
                "Y" => dapperContext.CreateScoreSharpFileHisConnection(dbName),
                _ => dapperContext.CreateScoreSharpFileConnection(),
            };
    }
}
