using ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountryById;

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry
{
    public partial class HighRiskCountryController
    {
        /// <summary>
        /// 查詢單筆洗錢及資恐高風險國家
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /HighRiskCountry/GetHighRiskCountryById/TW
        ///
        /// </remarks>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(查詢單筆洗錢及資恐高風險國家_2000_ResEx),
            typeof(查詢單筆洗錢及資恐高風險國家查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetHighRiskCountryById")]
        public async Task<IResult> GetHighRiskCountryById([FromRoute] string code)
        {
            var reuslt = await _mediator.Send(new Query(code));
            return Results.Ok(reuslt);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountryById
{
    public record Query(string code) : IRequest<ResultResponse<GetHighRiskCountryByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetHighRiskCountryByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetHighRiskCountryByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_HighRiskCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.HighRiskCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetHighRiskCountryByIdResponse>(null, request.code);

            var result = _mapper.Map<GetHighRiskCountryByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
