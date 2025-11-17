using ScoreSharp.API.Modules.SetUp.AMLJobLevel.InsertAMLJobLevel;

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel
{
    public partial class AMLJobLevelController
    {
        /// <summary>
        /// 新增單筆AML職級別
        /// </summary>
        /// <param name="request"></param>
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
        [OpenApiOperation("InsertAMLJobLevel")]
        public async Task<IResult> InsertAMLJobLevel([FromBody] InsertAMLJobLevelRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.InsertAMLJobLevel
{
    public record Command(InsertAMLJobLevelRequest insertAMLJobLevelRequest) : IRequest<ResultResponse<string>>;

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
            var insertAMLJobLevelRequest = request.insertAMLJobLevelRequest;

            var single = await _context
                .SetUp_AMLJobLevel.AsNoTracking()
                .SingleOrDefaultAsync(x => x.AMLJobLevelCode == insertAMLJobLevelRequest.AMLJobLevelCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(
                    insertAMLJobLevelRequest.AMLJobLevelCode,
                    insertAMLJobLevelRequest.AMLJobLevelCode
                );

            var entity = _mapper.Map<SetUp_AMLJobLevel>(insertAMLJobLevelRequest);

            await _context.SetUp_AMLJobLevel.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(insertAMLJobLevelRequest.AMLJobLevelCode, insertAMLJobLevelRequest.AMLJobLevelCode);
        }
    }
}
