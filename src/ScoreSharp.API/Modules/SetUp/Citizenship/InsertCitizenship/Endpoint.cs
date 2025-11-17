using ScoreSharp.API.Modules.SetUp.Citizenship.InsertCitizenship;

namespace ScoreSharp.API.Modules.SetUp.Citizenship
{
    public partial class CitizenshipController
    {
        /// <summary>
        /// 新增單筆國籍
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(新增國籍_2000_ReqEx),
            ExampleType = ExampleType.Request,
            ParameterName = "request",
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(新增國籍_2000_ResEx),
            typeof(新增國籍資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertCitizenship")]
        public async Task<IResult> InsertCitizenship([FromBody] InsertCitizenshipRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Citizenship.InsertCitizenship
{
    public record Command(InsertCitizenshipRequest insertCitizenshipRequest) : IRequest<ResultResponse<string>>;

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
            var insertCitizenshipRequest = request.insertCitizenshipRequest;

            var single = await _context
                .SetUp_Citizenship.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CitizenshipCode == insertCitizenshipRequest.CitizenshipCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertCitizenshipRequest.CitizenshipCode);

            var entity = _mapper.Map<SetUp_Citizenship>(insertCitizenshipRequest);

            await _context.SetUp_Citizenship.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(insertCitizenshipRequest.CitizenshipCode, insertCitizenshipRequest.CitizenshipCode);
        }
    }
}
