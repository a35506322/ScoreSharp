using ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountryById;

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry
{
    public partial class EUSanctionCountryController
    {
        /// <summary>
        /// 查詢單筆EU制裁國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///       /EUSanctionCountry/GetEUSanctionCountryById/TW
        ///
        /// </remarks>
        /// <param name="code">EU制裁國家代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetEUSanctionCountryByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得EU制裁國家_2000_ResEx),
            typeof(取得EU制裁國家查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetEUSanctionCountryById")]
        public async Task<IResult> GetEUSanctionCountryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountryById
{
    public record Query(string code) : IRequest<ResultResponse<GetEUSanctionCountryByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetEUSanctionCountryByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetEUSanctionCountryByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_EUSanctionCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.EUSanctionCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetEUSanctionCountryByIdResponse>(null, request.code);

            var result = _mapper.Map<GetEUSanctionCountryByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
