using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.InsertIDCardRenewalLocation;

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation
{
    public partial class IDCardRenewalLocationController
    {
        /// <summary>
        /// 新增單筆身分證換發地點
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增身分證換發地點_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增身分證換發地點_2000_ResEx),
            typeof(新增身分證換發地點資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertIDCardRenewalLocation")]
        public async Task<IResult> InsertIDCardRenewalLocation([FromBody] InsertIDCardRenewalLocationRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.InsertIDCardRenewalLocation
{
    public record Command(InsertIDCardRenewalLocationRequest insertIDCardRenewalLocationRequest) : IRequest<ResultResponse<string>>;

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
            var insertIDCardRenewalLocationRequest = request.insertIDCardRenewalLocationRequest;

            var single = await _context
                .SetUp_IDCardRenewalLocation.AsNoTracking()
                .SingleOrDefaultAsync(x => x.IDCardRenewalLocationCode == insertIDCardRenewalLocationRequest.IDCardRenewalLocationCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertIDCardRenewalLocationRequest.IDCardRenewalLocationCode);

            var entity = _mapper.Map<SetUp_IDCardRenewalLocation>(insertIDCardRenewalLocationRequest);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertIDCardRenewalLocationRequest.IDCardRenewalLocationCode,
                insertIDCardRenewalLocationRequest.IDCardRenewalLocationCode
            );
        }
    }
}
