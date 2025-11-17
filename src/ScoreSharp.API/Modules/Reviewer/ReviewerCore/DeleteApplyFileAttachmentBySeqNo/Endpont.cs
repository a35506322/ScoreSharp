using ScoreSharp.API.Modules.Reviewer.ReviewerCore.DeleteApplyFileAttachmentBySeqNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 刪除徵審行員附件 BySeqNo
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/DeleteApplyFileAttachmentBySeqNo/2
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpDelete("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除徵審行員附件_2000_ResEx),
            typeof(刪除徵審行員附件查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteApplyFileAttachmentBySeqNo")]
        public async Task<IResult> DeleteApplyFileAttachmentBySeqNo([FromRoute] int seqNo)
        {
            var result = await _mediator.Send(new Command(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.DeleteApplyFileAttachmentBySeqNo
{
    public record Command(int seqNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, IJWTProfilerHelper _jwtHelper, IScoreSharpDapperContext dapperContext)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var fileInfo = await _context.Reviewer_ApplyFileAttachment.SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            if (fileInfo is null)
                return ApiResponseHelper.NotFound<string>(request.seqNo.ToString(), request.seqNo.ToString());

            using var connection = GetSqlConnection(fileInfo.IsHistory, fileInfo.DBName);
            connection.Open();
            var sql = $"DELETE FROM [{fileInfo.DBName}].[dbo].[Reviewer_ApplyFile] WHERE FileId = @FileId";
            await connection.ExecuteAsync(sql, new { FileId = fileInfo.FileId });

            DateTime now = DateTime.Now;
            fileInfo.DeleteTime = now;
            fileInfo.DeleteUserId = _jwtHelper.UserId;

            // 不要刪除FileId和DBName，主因要Mapping到GetApplyFileAttachmentsInfoByApplyNo的FileName
            // fileInfo.FileId = null;
            // fileInfo.DBName = null;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess<string>(request.seqNo.ToString());
        }

        private SqlConnection GetSqlConnection(string isHistory, string dbName) =>
            isHistory switch
            {
                "Y" => dapperContext.CreateScoreSharpFileHisConnection(dbName),
                _ => dapperContext.CreateScoreSharpFileConnection(),
            };
    }
}
