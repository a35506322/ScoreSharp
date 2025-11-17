using ScoreSharp.API.Modules.SetUp.UNSanctionCountry.InsertUNSanctionCountry;

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry
{
    public partial class UNSanctionCountryController
    {
        /// <summary>
        /// 新增單筆UN制裁國家
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增UN制裁國家_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增UN制裁國家_2000_ResEx),
            typeof(新增UN制裁國家資料已存在_4002_ResEx),
            typeof(新增UN制裁國家查無國籍設定資料_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertUNSanctionCountry")]
        public async Task<IResult> InsertUNSanctionCountry([FromBody] InsertUNSanctionCountryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.InsertUNSanctionCountry
{
    public record Command(InsertUNSanctionCountryRequest insertUNSanctionCountryRequest) : IRequest<ResultResponse<string>>;

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
            var insertUNSanctionCountryRequest = request.insertUNSanctionCountryRequest;

            var citizenship = await _context
                .SetUp_Citizenship.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CitizenshipCode == insertUNSanctionCountryRequest.UNSanctionCountryCode);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");

            var single = await _context
                .SetUp_UNSanctionCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.UNSanctionCountryCode == insertUNSanctionCountryRequest.UNSanctionCountryCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertUNSanctionCountryRequest.UNSanctionCountryCode);

            var entity = _mapper.Map<SetUp_UNSanctionCountry>(insertUNSanctionCountryRequest);

            await _context.SetUp_UNSanctionCountry.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(
                insertUNSanctionCountryRequest.UNSanctionCountryCode,
                insertUNSanctionCountryRequest.UNSanctionCountryCode
            );
        }
    }
}
