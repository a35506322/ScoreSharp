using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.InsertMainIncomeAndFund;

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund
{
    public partial class MainIncomeAndFundController
    {
        /// <summary>
        /// 新增單筆主要所得及資金來源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增主要所得及資金來源_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增主要所得及資金來源_2000_ResEx),
            typeof(新增主要所得及資金來源資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertMainIncomeAndFund")]
        public async Task<IResult> InsertMainIncomeAndFund([FromBody] InsertMainIncomeAndFundRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.InsertMainIncomeAndFund
{
    public record Command(InsertMainIncomeAndFundRequest insertMainIncomeAndFundRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertMainIncomeAndFundRequest = request.insertMainIncomeAndFundRequest;

            var single = await _context
                .SetUp_MainIncomeAndFund.AsNoTracking()
                .SingleOrDefaultAsync(x => x.MainIncomeAndFundCode == insertMainIncomeAndFundRequest.MainIncomeAndFundCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertMainIncomeAndFundRequest.MainIncomeAndFundCode);

            var entity = _mapper.Map<SetUp_MainIncomeAndFund>(insertMainIncomeAndFundRequest);

            await _context.SetUp_MainIncomeAndFund.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(
                insertMainIncomeAndFundRequest.MainIncomeAndFundCode,
                insertMainIncomeAndFundRequest.MainIncomeAndFundCode
            );
        }
    }
}
