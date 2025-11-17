using ScoreSharp.API.Modules.SetUp.EUSanctionCountry.InsertEUSanctionCountry;

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry
{
    public partial class EUSanctionCountryController
    {
        /// <summary>
        /// 新增單筆EU制裁國家
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增EU制裁國家_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增EU制裁國家_2000_ResEx),
            typeof(新增EU制裁國家資料已存在_4002_ResEx),
            typeof(新增EU制裁國家查無國籍設定資料_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertEUSanctionCountry")]
        public async Task<IResult> InsertEUSanctionCountry([FromBody] InsertEUSanctionCountryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry.InsertEUSanctionCountry
{
    public record Command(InsertEUSanctionCountryRequest insertEUSanctionCountryRequest) : IRequest<ResultResponse<string>>;

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
            var insertEUSanctionCountryRequest = request.insertEUSanctionCountryRequest;

            var citizenship = await _context
                .SetUp_Citizenship.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CitizenshipCode == insertEUSanctionCountryRequest.EUSanctionCountryCode);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");

            var single = await _context
                .SetUp_EUSanctionCountry.AsNoTracking()
                .SingleOrDefaultAsync(x => x.EUSanctionCountryCode == insertEUSanctionCountryRequest.EUSanctionCountryCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertEUSanctionCountryRequest.EUSanctionCountryCode);

            var entity = _mapper.Map<SetUp_EUSanctionCountry>(insertEUSanctionCountryRequest);

            await _context.SetUp_EUSanctionCountry.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(
                insertEUSanctionCountryRequest.EUSanctionCountryCode,
                insertEUSanctionCountryRequest.EUSanctionCountryCode
            );
        }
    }
}
