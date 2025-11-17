using ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountryById;

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry
{
    public partial class UNSanctionCountryController
    {
        /// <summary>
        /// 查詢單筆UN制裁國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///       /UNSanctionCountry/GetUNSanctionCountryById/TW
        ///
        /// </remarks>
        /// <param name="code">UN制裁國家代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetUNSanctionCountryByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得UN制裁國家_2000_ResEx),
            typeof(取得UN制裁國家查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetUNSanctionCountryById")]
        public async Task<IResult> GetUNSanctionCountryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountryById
{
    public record Query(string code) : IRequest<ResultResponse<GetUNSanctionCountryByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetUNSanctionCountryByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetUNSanctionCountryByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_UNSanctionCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.UNSanctionCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetUNSanctionCountryByIdResponse>(null, request.code);

            var result = _mapper.Map<GetUNSanctionCountryByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
