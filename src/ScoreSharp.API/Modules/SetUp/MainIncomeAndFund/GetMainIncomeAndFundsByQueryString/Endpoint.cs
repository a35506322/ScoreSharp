using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund
{
    public partial class MainIncomeAndFundController
    {
        /// <summary>
        /// 查詢多筆主要所得及資金來源
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;MainIncomeAndFundName=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetMainIncomeAndFundsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得主要所得及資金來源_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetMainIncomeAndFundsByQueryString")]
        public async Task<IResult> GetMainIncomeAndFundsByQueryString([FromQuery] GetMainIncomeAndFundsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundsByQueryString
{
    public record Query(GetMainIncomeAndFundsByQueryStringRequest getMainIncomeAndFundsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetMainIncomeAndFundsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetMainIncomeAndFundsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetMainIncomeAndFundsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getMainIncomeAndFundsByQueryStringRequest = request.getMainIncomeAndFundsByQueryStringRequest;

            var entities = await _context
                .SetUp_MainIncomeAndFund.Where(x =>
                    string.IsNullOrEmpty(getMainIncomeAndFundsByQueryStringRequest.MainIncomeAndFundName)
                    || x.MainIncomeAndFundName.Contains(getMainIncomeAndFundsByQueryStringRequest.MainIncomeAndFundName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getMainIncomeAndFundsByQueryStringRequest.IsActive)
                    || x.IsActive == getMainIncomeAndFundsByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetMainIncomeAndFundsByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
