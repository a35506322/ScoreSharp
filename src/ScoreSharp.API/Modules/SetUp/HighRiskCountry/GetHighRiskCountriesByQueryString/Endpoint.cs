using ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountriesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry
{
    public partial class HighRiskCountryController
    {
        /// <summary>
        /// 查詢多筆洗錢及資恐高風險國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;HighRiskCountryName=台&amp;HighRiskCountryCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetHighRiskCountriesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得洗錢及資恐高風險國家_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetHighRiskCountriesByQueryString")]
        public async Task<IResult> GetHighRiskCountriesByQueryString([FromQuery] GetHighRiskCountriesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountriesByQueryString
{
    public record Query(GetHighRiskCountriesByQueryStringRequest getHighRiskCountriesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetHighRiskCountriesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetHighRiskCountriesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetHighRiskCountriesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getHighRiskCountriesByQueryStringRequest = request.getHighRiskCountriesByQueryStringRequest;

            var entities = await _context
                .SetUp_HighRiskCountry.Where(x =>
                    string.IsNullOrEmpty(getHighRiskCountriesByQueryStringRequest.HighRiskCountryCode)
                    || x.HighRiskCountryCode == getHighRiskCountriesByQueryStringRequest.HighRiskCountryCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getHighRiskCountriesByQueryStringRequest.HighRiskCountryName)
                    || x.HighRiskCountryName.Contains(getHighRiskCountriesByQueryStringRequest.HighRiskCountryName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getHighRiskCountriesByQueryStringRequest.IsActive)
                    || x.IsActive == getHighRiskCountriesByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetHighRiskCountriesByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
