using ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountriesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry
{
    public partial class EUSanctionCountryController
    {
        /// <summary>
        /// 查詢多筆 EU制裁國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;EUSanctionCountryName=國&amp;EUSanctionCountryCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetEUSanctionCountriesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得EU制裁國家_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetEUSanctionCountriesByQueryString")]
        public async Task<IResult> GetEUSanctionCountriesByQueryString([FromQuery] GetEUSanctionCountriesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountriesByQueryString
{
    public record Query(GetEUSanctionCountriesByQueryStringRequest getEUSanctionCountriesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetEUSanctionCountriesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetEUSanctionCountriesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetEUSanctionCountriesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getEUSanctionCountriesByQueryStringRequest = request.getEUSanctionCountriesByQueryStringRequest;

            var entites = await _context
                .SetUp_EUSanctionCountry.Where(x =>
                    string.IsNullOrEmpty(getEUSanctionCountriesByQueryStringRequest.EUSanctionCountryCode)
                    || x.EUSanctionCountryCode == getEUSanctionCountriesByQueryStringRequest.EUSanctionCountryCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getEUSanctionCountriesByQueryStringRequest.EUSanctionCountryName)
                    || x.EUSanctionCountryName.Contains(getEUSanctionCountriesByQueryStringRequest.EUSanctionCountryName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getEUSanctionCountriesByQueryStringRequest.IsActive)
                    || x.IsActive == getEUSanctionCountriesByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetEUSanctionCountriesByQueryStringResponse>>(entites);

            return ApiResponseHelper.Success(result);
        }
    }
}
