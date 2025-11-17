using ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountriesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry
{
    public partial class UNSanctionCountryController
    {
        /// <summary>
        /// 查詢多筆 UN制裁國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;UNSanctionCountryName=國&amp;UNSanctionCountryCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetUNSanctionCountriesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得UN制裁國家_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetUNSanctionCountriesByQueryString")]
        public async Task<IResult> GetUNSanctionCountriesByQueryString([FromQuery] GetUNSanctionCountriesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountriesByQueryString
{
    public record Query(GetUNSanctionCountriesByQueryStringRequest getUNSanctionCountriesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetUNSanctionCountriesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetUNSanctionCountriesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetUNSanctionCountriesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getUNSanctionCountriesByQueryStringRequest = request.getUNSanctionCountriesByQueryStringRequest;

            var entites = await _context
                .SetUp_UNSanctionCountry.Where(x =>
                    string.IsNullOrEmpty(getUNSanctionCountriesByQueryStringRequest.UNSanctionCountryCode)
                    || x.UNSanctionCountryCode == getUNSanctionCountriesByQueryStringRequest.UNSanctionCountryCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getUNSanctionCountriesByQueryStringRequest.UNSanctionCountryName)
                    || x.UNSanctionCountryName.Contains(getUNSanctionCountriesByQueryStringRequest.UNSanctionCountryName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getUNSanctionCountriesByQueryStringRequest.IsActive)
                    || x.IsActive == getUNSanctionCountriesByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetUNSanctionCountriesByQueryStringResponse>>(entites);

            return ApiResponseHelper.Success(result);
        }
    }
}
