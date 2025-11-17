using ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountriesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.EUCountry
{
    public partial class EUCountryController
    {
        /// <summary>
        /// 查詢多筆 EU國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;EUCountryName=國&amp;EUCountryCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetEUCountriesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得EU國家_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetEUCountriesByQueryString")]
        public async Task<IResult> GetEUCountriesByQueryString([FromQuery] GetEUCountriesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountriesByQueryString
{
    public record Query(GetEUCountriesByQueryStringRequest getEUCountriesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetEUCountriesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetEUCountriesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetEUCountriesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getEUCountriesByQueryStringRequest = request.getEUCountriesByQueryStringRequest;

            var entites = await _context
                .SetUp_EUCountry.Where(x =>
                    string.IsNullOrEmpty(getEUCountriesByQueryStringRequest.EUCountryCode)
                    || x.EUCountryCode == getEUCountriesByQueryStringRequest.EUCountryCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getEUCountriesByQueryStringRequest.EUCountryName)
                    || x.EUCountryName.Contains(getEUCountriesByQueryStringRequest.EUCountryName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getEUCountriesByQueryStringRequest.IsActive)
                    || x.IsActive == getEUCountriesByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetEUCountriesByQueryStringResponse>>(entites);

            return ApiResponseHelper.Success(result);
        }
    }
}
