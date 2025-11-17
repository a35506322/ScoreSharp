using ScoreSharp.API.Modules.SetUp.LongTermReason.InsertLongTermReason;

namespace ScoreSharp.API.Modules.SetUp.LongTermReason
{
    public partial class LongTermReasonController
    {
        /// <summary>
        /// 新增單筆長循分期戶理由碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增長循分期戶理由碼_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增長循分期戶理由碼_2000_ResEx),
            typeof(新增長循分期戶理由碼資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertLongTermReason")]
        public async Task<IResult> InsertLongTermReason([FromBody] InsertLongTermReasonRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.LongTermReason.InsertLongTermReason
{
    public record Command(InsertLongTermReasonRequest insertLongTermReasonRequest) : IRequest<ResultResponse<string>>;

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
            var insertLongTermReasonRequest = request.insertLongTermReasonRequest;

            var single = await _context
                .SetUp_LongTermReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.LongTermReasonCode == insertLongTermReasonRequest.LongTermReasonCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertLongTermReasonRequest.LongTermReasonCode);

            var entity = _mapper.Map<SetUp_LongTermReason>(insertLongTermReasonRequest);

            await _context.SetUp_LongTermReason.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(
                insertLongTermReasonRequest.LongTermReasonCode,
                insertLongTermReasonRequest.LongTermReasonCode
            );
        }
    }
}
