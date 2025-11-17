using ScoreSharp.API.Modules.SetUp.RejectionReason.InsertRejectionReason;

namespace ScoreSharp.API.Modules.SetUp.RejectionReason
{
    public partial class RejectionReasonController
    {
        /// <summary>
        ///  新增單筆退件原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(新增退件原因_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增退件原因_2000_ResEx),
            typeof(新增退件原因資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertRejectionReason")]
        public async Task<IResult> InsertRejectionReason([FromBody] InsertRejectionReasonRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.RejectionReason.InsertRejectionReason
{
    public record Command(InsertRejectionReasonRequest insertRejectionReasonRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IMapper mapper, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _mapper = mapper;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertRejectionReasonRequest = request.insertRejectionReasonRequest;

            var single = await _context
                .SetUp_RejectionReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.RejectionReasonCode == insertRejectionReasonRequest.RejectionReasonCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertRejectionReasonRequest.RejectionReasonCode);

            var entity = _mapper.Map<SetUp_RejectionReason>(insertRejectionReasonRequest);
            entity.AddTime = DateTime.Now;
            entity.AddUserId = _jwthelper.UserId;

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(
                insertRejectionReasonRequest.RejectionReasonCode,
                insertRejectionReasonRequest.RejectionReasonCode
            );
        }
    }
}
