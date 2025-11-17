using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoFileByApplyNoAndFileId;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得申請書附件 By ApplyNo And  FileId
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router:
        ///     /ReviewerCore/GetApplyCreditCardInfoFileByApplyNoAndFileId/20241127X8342/F05A94A1-9FE4-4B2D-B002-96AAC9AA3923
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}/{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplyCreditCardInfoFileByApplyNoAndFileIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得申請書_附件_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyCreditCardInfoFileByApplyNoAndFileId")]
        public async Task<IResult> GetApplyCreditCardInfoFileByApplyNoAndFileId(
            [FromRoute] string applyNo,
            [FromRoute] Guid fileId,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(new Query(applyNo, fileId));

            if (result.ReturnCodeStatus != ReturnCodeStatus.成功)
                return Results.Ok(result);

            return Results.File(result.ReturnData.FileContent, result.ReturnData.ContentType, result.ReturnData.FileName);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoFileByApplyNoAndFileId
{
    public record Query(string applyNo, Guid fileId) : IRequest<ResultResponse<GetApplyCreditCardInfoFileByApplyNoAndFileIdResponse>>;

    public class Handler(ScoreSharpContext efContext, IScoreSharpDapperContext dapperContext)
        : IRequestHandler<Query, ResultResponse<GetApplyCreditCardInfoFileByApplyNoAndFileIdResponse>>
    {
        public async Task<ResultResponse<GetApplyCreditCardInfoFileByApplyNoAndFileIdResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var fileInfo = await efContext.Reviewer_ApplyCreditCardInfoFile.FirstOrDefaultAsync(
                t => t.ApplyNo == request.applyNo && t.FileId == request.fileId,
                cancellationToken
            );

            if (fileInfo is null)
            {
                return ApiResponseHelper.NotFound<GetApplyCreditCardInfoFileByApplyNoAndFileIdResponse>(null, "找不到指定的檔案資訊");
            }

            using var connection = GetSqlConnection(fileInfo.IsHistory, fileInfo.DBName);
            connection.Open();
            var sql = $"SELECT * FROM [{fileInfo.DBName}].[dbo].[Reviewer_ApplyFile] WHERE FileId = @FileId";
            var applyFile = await connection.QueryFirstOrDefaultAsync<Reviewer_ApplyFile>(sql, new { FileId = request.fileId });

            if (applyFile is null)
            {
                return ApiResponseHelper.NotFound<GetApplyCreditCardInfoFileByApplyNoAndFileIdResponse>(null, "實體檔案不存在");
            }

            return ApiResponseHelper.Success(
                new GetApplyCreditCardInfoFileByApplyNoAndFileIdResponse
                {
                    FileContent = applyFile.FileContent,
                    FileName = applyFile.FileName,
                    ContentType = Path.GetExtension(applyFile.FileName).ToLower() switch
                    {
                        ".pdf" => "application/pdf",
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        _ => "application/octet-stream",
                    },
                },
                "取得申請書附件成功"
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
