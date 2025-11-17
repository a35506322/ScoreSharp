using ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertReviewerSummary;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 新增單筆照會摘要
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增單筆照會摘要_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增單筆照會摘要_2000_ResEx),
            typeof(新增單筆照會摘要查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertReviewerSummary")]
        public async Task<IResult> InsertReviewerSummary([FromBody] InsertReviewerSummaryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertReviewerSummary
{
    public record Command(InsertReviewerSummaryRequest insertReviewerSummaryRequest) : IRequest<ResultResponse<string>>;

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
            var dto = request.insertReviewerSummaryRequest;

            var single = await _context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().SingleOrDefaultAsync(x => x.ApplyNo == dto.ApplyNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, dto.ApplyNo);

            var entity = _mapper.Map<Reviewer_ReviewerSummary>(dto);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            var pkNo = entity.SeqNo;

            return ApiResponseHelper.InsertSuccess(pkNo.ToString(), pkNo.ToString());
        }
    }
}
