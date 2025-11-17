using ScoreSharp.API.Modules.SetUp.InternalIP.InsertInternalIP;

namespace ScoreSharp.API.Modules.SetUp.InternalIP
{
    public partial class InternalIPController
    {
        /// <summary>
        /// 新增單筆行內IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增行內IP_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增行內IP_2000_ResEx),
            typeof(新增行內IP資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertInternalIP")]
        public async Task<IResult> InsertInternalIP([FromBody] InsertInternalIPRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.InternalIP.InsertInternalIP
{
    public record Command(InsertInternalIPRequest InsertInternalIPRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.InsertInternalIPRequest;

            var single = await _context.SetUp_InternalIP.SingleOrDefaultAsync(x => x.IP == dto.IP);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.IP);

            SetUp_InternalIP setUp_InternalIP = new SetUp_InternalIP()
            {
                IP = dto.IP,
                IsActive = dto.IsActive,
                AddUserId = _jwthelper.UserId,
                AddTime = DateTime.Now,
            };

            await _context.AddAsync(setUp_InternalIP);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(setUp_InternalIP.IP, setUp_InternalIP.IP);
        }
    }
}
