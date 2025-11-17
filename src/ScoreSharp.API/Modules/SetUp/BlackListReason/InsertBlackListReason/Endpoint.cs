using ScoreSharp.API.Modules.SetUp.BlackListReason.InsertBlackListReason;

namespace ScoreSharp.API.Modules.SetUp.BlackListReason
{
    public partial class BlackListReasonController
    {
        /// <summary>
        /// 新增單筆黑名單理由
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增黑名單理由_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增黑名單理由_2000_ResEx),
            typeof(新增黑名單理由資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertBlackListReason")]
        public async Task<IResult> InsertBlackListReason([FromBody] InsertBlackListReasonRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BlackListReason.InsertBlackListReason
{
    public record Command(InsertBlackListReasonRequest insertBlackListReasonRequest) : IRequest<ResultResponse<string>>;

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
            var insertBlackListReasonRequest = request.insertBlackListReasonRequest;

            var single = await _context
                .SetUp_BlackListReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.BlackListReasonCode == insertBlackListReasonRequest.BlackListReasonCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertBlackListReasonRequest.BlackListReasonCode);

            var entity = _mapper.Map<SetUp_BlackListReason>(insertBlackListReasonRequest);

            await _context.SetUp_BlackListReason.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertBlackListReasonRequest.BlackListReasonCode,
                insertBlackListReasonRequest.BlackListReasonCode
            );
        }
    }
}
