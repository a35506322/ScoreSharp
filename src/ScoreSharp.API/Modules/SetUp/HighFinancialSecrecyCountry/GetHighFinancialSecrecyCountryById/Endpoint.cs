using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountryById;

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry
{
    public partial class HighFinancialSecrecyCountryController
    {
        /// <summary>
        /// 查詢單筆高金融保密國家
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /HighFinancialSecrecyCountry/GetHighFinancialSecrecyCountryById/TW
        ///
        /// </remarks>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetHighFinancialSecrecyCountryByIdResponse>))]
        [EndpointSpecificExample(
            typeof(查詢單筆高金融保密國家_2000_ResEx),
            typeof(查詢單筆高金融保密國家查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetHighFinancialSecrecyCountryById")]
        public async Task<IResult> GetHighFinancialSecrecyCountryById([FromRoute] string code)
        {
            var reuslt = await _mediator.Send(new Query(code));
            return Results.Ok(reuslt);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountryById
{
    public record Query(string code) : IRequest<ResultResponse<GetHighFinancialSecrecyCountryByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetHighFinancialSecrecyCountryByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetHighFinancialSecrecyCountryByIdResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var single = await _context
                .SetUp_HighFinancialSecrecyCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.HighFinancialSecrecyCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetHighFinancialSecrecyCountryByIdResponse>(null, request.code);

            var result = _mapper.Map<GetHighFinancialSecrecyCountryByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
