using ScoreSharp.API.Modules.SetUp.HighRiskCountry.InsertHighRiskCountry;

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry
{
    public partial class HighRiskCountryController
    {
        /// <summary>
        /// 新增單筆洗錢及資恐高風險國家
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(新增洗錢及資恐高風險國家_2000_ReqEx),
            ExampleType = ExampleType.Request,
            ParameterName = "request",
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(新增洗錢及資恐高風險國家_2000_ResEx),
            typeof(新增洗錢及資恐高風險國家資料已存在_4002_ResEx),
            typeof(新增洗錢及資恐高風險國家查無國籍設定資料_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertHighRiskCountry")]
        public async Task<IResult> InsertHighRiskCountry([FromBody] InsertHighRiskCountryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.InsertHighRiskCountry
{
    public record Command(InsertHighRiskCountryRequest insertHighRiskCountryRequest) : IRequest<ResultResponse<string>>;

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
            var insertHighRiskCountryRequest = request.insertHighRiskCountryRequest;

            var citizenship = await _context
                .SetUp_Citizenship.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CitizenshipCode == insertHighRiskCountryRequest.HighRiskCountryCode);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");

            var single = await _context
                .SetUp_HighRiskCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.HighRiskCountryCode == insertHighRiskCountryRequest.HighRiskCountryCode);

            if (single is not null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertHighRiskCountryRequest.HighRiskCountryCode);

            var entity = _mapper.Map<SetUp_HighRiskCountry>(insertHighRiskCountryRequest);

            await _context.SetUp_HighRiskCountry.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertHighRiskCountryRequest.HighRiskCountryCode,
                insertHighRiskCountryRequest.HighRiskCountryCode
            );
        }
    }
}
