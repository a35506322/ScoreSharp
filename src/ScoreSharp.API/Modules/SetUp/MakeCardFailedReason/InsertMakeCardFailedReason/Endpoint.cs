using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.InsertMakeCardFailedReason;

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason
{
    public partial class MakeCardFailedReasonController
    {
        /// <summary>
        /// 新增單筆製卡失敗原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增製卡失敗原因_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增製卡失敗原因_2000_ResEx),
            typeof(新增製卡失敗原因資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertMakeCardFailedReason")]
        public async Task<IResult> InsertMakeCardFailedReason([FromBody] InsertMakeCardFailedReasonRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.InsertMakeCardFailedReason
{
    public record Command(InsertMakeCardFailedReasonRequest insertMakeCardFailedReasonRequest) : IRequest<ResultResponse<string>>;

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
            var insertMakeCardFailedReasonRequest = request.insertMakeCardFailedReasonRequest;

            var single = await _context
                .SetUp_MakeCardFailedReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.MakeCardFailedReasonCode == insertMakeCardFailedReasonRequest.MakeCardFailedReasonCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertMakeCardFailedReasonRequest.MakeCardFailedReasonCode);

            var entity = _mapper.Map<SetUp_MakeCardFailedReason>(insertMakeCardFailedReasonRequest);

            await _context.SetUp_MakeCardFailedReason.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertMakeCardFailedReasonRequest.MakeCardFailedReasonCode,
                insertMakeCardFailedReasonRequest.MakeCardFailedReasonCode
            );
        }
    }
}
