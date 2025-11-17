using ScoreSharp.API.Modules.SetUp.ProjectCode.InsertProjectCode;

namespace ScoreSharp.API.Modules.SetUp.ProjectCode
{
    public partial class ProjectCodeController
    {
        ///<summary>
        /// 新增單筆專案代號
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增專案代號_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增專案代號_2000_ResEx),
            typeof(新增專案代號資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertProjectCode")]
        public async Task<IResult> InsertProjectCode([FromBody] InsertProjectCodeRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ProjectCode.InsertProjectCode
{
    public record Command(InsertProjectCodeRequest insertProjectCodeRequest) : IRequest<ResultResponse<string>>;

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
            var insertProjectCodeRequest = request.insertProjectCodeRequest;

            var single = await _context
                .SetUp_ProjectCode.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ProjectCode == insertProjectCodeRequest.ProjectCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertProjectCodeRequest.ProjectCode);

            var entity = _mapper.Map<SetUp_ProjectCode>(insertProjectCodeRequest);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(insertProjectCodeRequest.ProjectCode, insertProjectCodeRequest.ProjectCode);
        }
    }
}
