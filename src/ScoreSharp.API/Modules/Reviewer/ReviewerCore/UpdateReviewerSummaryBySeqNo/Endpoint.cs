using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateReviewerSummaryBySeqNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 修改單筆照會摘要 ByPK
        /// </summary>
        /// <param name="seqNo">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改單筆照會摘要_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改單筆照會摘要_2000_ResEx),
            typeof(修改單筆照會摘要查無此資料_4001_ResEx),
            typeof(修改單筆照會摘要路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateReviewerSummaryBySeqNo")]
        public async Task<IResult> UpdateReviewerSummaryBySeqNo(
            [FromRoute] int seqNo,
            [FromBody] UpdateReviewerSummaryBySeqNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateReviewerSummaryBySeqNo
{
    public record Command(int seqNo, UpdateReviewerSummaryBySeqNoRequest updateReviewerSummaryBySeqNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var seqNo = request.seqNo;
            var updateReviewerSummaryBySeqNoRequest = request.updateReviewerSummaryBySeqNoRequest;

            if (updateReviewerSummaryBySeqNoRequest.SeqNo != seqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var single = await _context.Reviewer_ReviewerSummary.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            single.Record = updateReviewerSummaryBySeqNoRequest.Record;
            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(seqNo.ToString(), seqNo.ToString());
        }
    }
}
