using ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.GetSingleApplicableInterestRateLimit;

namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit
{
    public partial class ApplicableInterestRateLimitController
    {
        /// <summary>
        /// 查詢唯一一筆判斷適用利率額度
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///       /ApplicableInterestRateLimit/GetSingleApplicableInterestRateLimit
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetSingleApplicableInterestRateLimitResponse>))]
        [EndpointSpecificExample(
            typeof(取得判斷適用利率額度_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSingleApplicableInterestRateLimit")]
        public async Task<IResult> GetSingleApplicableInterestRateLimit()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.GetSingleApplicableInterestRateLimit
{
    public record Query() : IRequest<ResultResponse<GetSingleApplicableInterestRateLimitResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetSingleApplicableInterestRateLimitResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetSingleApplicableInterestRateLimitResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var single = await _context.SetUp_ApplicableInterestRateLimit.AsNoTracking().SingleAsync();

            var result = _mapper.Map<GetSingleApplicableInterestRateLimitResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
