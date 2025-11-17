using ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountryById;

namespace ScoreSharp.API.Modules.SetUp.EUCountry
{
    public partial class EUCountryController
    {
        /// <summary>
        /// 查詢單筆EU國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///       /EUCountry/GetEUCountryById/TW
        ///
        /// </remarks>
        /// <param name="code">EU國家代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetEUCountryByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得EU國家_2000_ResEx),
            typeof(取得EU國家查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetEUCountryById")]
        public async Task<IResult> GetEUCountryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountryById
{
    public record Query(string code) : IRequest<ResultResponse<GetEUCountryByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetEUCountryByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetEUCountryByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_EUCountry.AsNoTracking().SingleOrDefaultAsync(x => x.EUCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetEUCountryByIdResponse>(null, request.code);

            var result = _mapper.Map<GetEUCountryByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
