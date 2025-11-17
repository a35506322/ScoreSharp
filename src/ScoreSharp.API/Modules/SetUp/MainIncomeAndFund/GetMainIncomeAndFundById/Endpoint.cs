using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundById;

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund
{
    public partial class MainIncomeAndFundController
    {
        /// <summary>
        /// 查詢單筆主要所得及資金來源
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /MainIncomeAndFund/GetMainIncomeAndFundById/2
        ///
        /// </remarks>
        /// <param name="code">主要所得及資金來源代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetMainIncomeAndFundByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得主要所得及資金來源_2000_ResEx),
            typeof(取得主要所得及資金來源查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetMainIncomeAndFundById")]
        public async Task<IResult> GetMainIncomeAndFundById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundById
{
    public record Query(string code) : IRequest<ResultResponse<GetMainIncomeAndFundByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetMainIncomeAndFundByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetMainIncomeAndFundByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_MainIncomeAndFund.AsNoTracking()
                .SingleOrDefaultAsync(x => x.MainIncomeAndFundCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetMainIncomeAndFundByIdResponse>(null, request.code.ToString());

            var result = _mapper.Map<GetMainIncomeAndFundByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
