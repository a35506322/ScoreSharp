using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.InsertHighFinancialSecrecyCountry;

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry
{
    public partial class HighFinancialSecrecyCountryController
    {
        /// <summary>
        /// 新增單筆高金融保密國家
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(新增單筆高金融保密國家_2000_ReqEx),
            ExampleType = ExampleType.Request,
            ParameterName = "request",
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(新增單筆高金融保密國家_2000_ResEx),
            typeof(新增單筆高金融保密國家資料已存在_4002_ResEx),
            typeof(新增單筆高金融保密國家查無國籍設定資料_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertHighFinancialSecrecyCountry")]
        public async Task<IResult> InsertHighFinancialSecrecyCountry([FromBody] InsertHighFinancialSecrecyCountryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.InsertHighFinancialSecrecyCountry
{
    public record Command(InsertHighFinancialSecrecyCountryRequest insertHighFinancialSecrecyCountryRequest)
        : IRequest<ResultResponse<string>>;

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
            var insertHighFinancialSecrecyCountryRequest = request.insertHighFinancialSecrecyCountryRequest;

            var citizenship = await _context
                .SetUp_Citizenship.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CitizenshipCode == insertHighFinancialSecrecyCountryRequest.HighFinancialSecrecyCountryCode);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");

            var single = await _context
                .SetUp_HighFinancialSecrecyCountry.AsNoTracking()
                .SingleOrDefaultAsync(x =>
                    x.HighFinancialSecrecyCountryCode == insertHighFinancialSecrecyCountryRequest.HighFinancialSecrecyCountryCode
                );

            if (single is not null)
                return ApiResponseHelper.DataAlreadyExists<string>(
                    null,
                    insertHighFinancialSecrecyCountryRequest.HighFinancialSecrecyCountryCode
                );

            var entity = _mapper.Map<SetUp_HighFinancialSecrecyCountry>(insertHighFinancialSecrecyCountryRequest);

            await _context.SetUp_HighFinancialSecrecyCountry.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertHighFinancialSecrecyCountryRequest.HighFinancialSecrecyCountryCode,
                insertHighFinancialSecrecyCountryRequest.HighFinancialSecrecyCountryCode
            );
        }
    }
}
