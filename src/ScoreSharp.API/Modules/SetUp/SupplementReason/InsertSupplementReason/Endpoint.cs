using ScoreSharp.API.Modules.SetUp.SupplementReason.InsertSupplementReason;

namespace ScoreSharp.API.Modules.SetUp.SupplementReason
{
    public partial class SupplementReasonController
    {
        ///<summary>
        /// 新增單筆補件原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增補件原因_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增補件原因_2000_ResEx),
            typeof(新增補件原因資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertSupplementReason")]
        public async Task<IResult> InsertSupplementReason([FromBody] InsertSupplementReasonRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.SupplementReason.InsertSupplementReason
{
    public record Command(InsertSupplementReasonRequest insertSupplementReasonRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper, IMapper mapper)
        {
            _context = context;
            _jwthelper = jwthelper;
            _mapper = mapper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertSupplementReasonRequest = request.insertSupplementReasonRequest;

            var single = await _context
                .SetUp_SupplementReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.SupplementReasonCode == insertSupplementReasonRequest.SupplementReasonCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertSupplementReasonRequest.SupplementReasonCode);

            var entity = _mapper.Map<SetUp_SupplementReason>(insertSupplementReasonRequest);
            entity.AddTime = DateTime.Now;
            entity.AddUserId = _jwthelper.UserId;

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertSupplementReasonRequest.SupplementReasonCode,
                insertSupplementReasonRequest.SupplementReasonCode
            );
        }
    }
}
