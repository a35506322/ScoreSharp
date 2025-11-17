using ScoreSharp.API.Modules.SetUp.EUCountry.InsertEUCountry;

namespace ScoreSharp.API.Modules.SetUp.EUCountry
{
    public partial class EUCountryController
    {
        /// <summary>
        /// 新增單筆EU國家
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增EU國家_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增EU國家_2000_ResEx),
            typeof(新增EU國家資料已存在_4002_ResEx),
            typeof(新增EU國家查無國籍設定資料_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertEUCountry")]
        public async Task<IResult> InsertEUCountry([FromBody] InsertEUCountryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUCountry.InsertEUCountry
{
    public record Command(InsertEUCountryRequest insertEUCountryRequest) : IRequest<ResultResponse<string>>;

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
            var insertEUCountryRequest = request.insertEUCountryRequest;

            var citizenship = await _context
                .SetUp_Citizenship.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CitizenshipCode == insertEUCountryRequest.EUCountryCode);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");

            var single = await _context
                .SetUp_EUCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.EUCountryCode == insertEUCountryRequest.EUCountryCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertEUCountryRequest.EUCountryCode);

            var entity = _mapper.Map<SetUp_EUCountry>(insertEUCountryRequest);

            await _context.SetUp_EUCountry.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(insertEUCountryRequest.EUCountryCode, insertEUCountryRequest.EUCountryCode);
        }
    }
}
