using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountriesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry
{
    public partial class HighFinancialSecrecyCountryController
    {
        /// <summary>
        /// 查詢多筆高金融保密國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;HighFinancialSecrecyCountryName=台&amp;HighFinancialSecrecyCountryCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(ResultResponse<List<GetHighFinancialSecrecyCountriesByQueryStringResponse>>)
        )]
        [EndpointSpecificExample(
            typeof(查詢多筆高金融保密國家_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetHighFinancialSecrecyCountriesByQueryString")]
        public async Task<IResult> GetHighFinancialSecrecyCountriesByQueryString(
            [FromQuery] GetHighFinancialSecrecyCountriesByQueryStringRequest request
        )
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountriesByQueryString
{
    public record Query(GetHighFinancialSecrecyCountriesByQueryStringRequest getHighFinancialSecrecyCountriesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetHighFinancialSecrecyCountriesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetHighFinancialSecrecyCountriesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetHighFinancialSecrecyCountriesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getHighFinancialSecrecyCountriesByQueryStringRequest = request.getHighFinancialSecrecyCountriesByQueryStringRequest;

            var entities = await _context
                .SetUp_HighFinancialSecrecyCountry.Where(x =>
                    string.IsNullOrEmpty(getHighFinancialSecrecyCountriesByQueryStringRequest.HighFinancialSecrecyCountryCode)
                    || x.HighFinancialSecrecyCountryCode
                        == getHighFinancialSecrecyCountriesByQueryStringRequest.HighFinancialSecrecyCountryCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getHighFinancialSecrecyCountriesByQueryStringRequest.HighFinancialSecrecyCountryName)
                    || x.HighFinancialSecrecyCountryName.Contains(
                        getHighFinancialSecrecyCountriesByQueryStringRequest.HighFinancialSecrecyCountryName
                    )
                )
                .Where(x =>
                    string.IsNullOrEmpty(getHighFinancialSecrecyCountriesByQueryStringRequest.IsActive)
                    || x.IsActive == getHighFinancialSecrecyCountriesByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetHighFinancialSecrecyCountriesByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
