using ScoreSharp.API.Modules.SetUp.AMLProfession.InsertAMLProfession;

namespace ScoreSharp.API.Modules.SetUp.AMLProfession
{
    public partial class AMLProfessionController
    {
        /// <summary>
        /// 新增單筆AML職業別
        /// </summary>
        /// <param name="request"></param>
        /// <response code="400">
        /// 檢查 Version (版本) 格式是否為 yyyyMMdd
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增AML職級別_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增AML職級別_2000_ResEx),
            typeof(新增AML職級別資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(新增AML職級別版本不符合格式_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("InsertAMLProfession")]
        public async Task<IResult> InsertAMLProfession([FromBody] InsertAMLProfessionRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLProfession.InsertAMLProfession
{
    public record Command(InsertAMLProfessionRequest insertAMLProfessionRequest) : IRequest<ResultResponse<string>>;

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
            var insertAMLProfessionRequest = request.insertAMLProfessionRequest;

            var single = await _context
                .SetUp_AMLProfession.AsNoTracking()
                .SingleOrDefaultAsync(x =>
                    x.AMLProfessionCode == insertAMLProfessionRequest.AMLProfessionCode && x.Version == insertAMLProfessionRequest.Version
                );

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, single.SeqNo);

            var entity = _mapper.Map<SetUp_AMLProfession>(insertAMLProfessionRequest);

            entity.SeqNo = Ulid.NewUlid().ToString();

            await _context.SetUp_AMLProfession.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(entity.SeqNo, entity.SeqNo);
        }
    }
}
