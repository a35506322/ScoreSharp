using ScoreSharp.API.Modules.Reviewer.ReviewerCore.DeleteReviewerSummaryBySeqNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 刪除修改照會摘要 ByPK
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/DeleteReviewerSummaryBySeqNo/5
        ///
        /// </remarks>
        /// <param name="seqNo">PK</param>
        /// <returns></returns>
        [HttpDelete("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除照會摘要_2000_ResEx),
            typeof(刪除照會摘要查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteReviewerSummaryBySeqNo")]
        public async Task<IResult> DeleteReviewerSummaryBySeqNo([FromRoute] int seqNo)
        {
            var result = await _mediator.Send(new Command(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.DeleteReviewerSummaryBySeqNo
{
    public record Command(int seqNo) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.Reviewer_ReviewerSummary.SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(request.seqNo.ToString(), request.seqNo.ToString());

            _context.Reviewer_ReviewerSummary.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess<string>(request.seqNo.ToString());
        }
    }
}
