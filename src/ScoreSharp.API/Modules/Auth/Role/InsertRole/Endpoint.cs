using ScoreSharp.API.Modules.Auth.Role.InsertRole;

namespace ScoreSharp.API.Modules.Auth.Role
{
    public partial class RoleController
    {
        /// <summary>
        ///  新增單筆角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增角色_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增角色_2000_ResEx),
            typeof(新增角色資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertRole")]
        public async Task<IResult> InsertRole([FromBody] InsertRoleRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Role.InsertRole
{
    public record Command(InsertRoleRequest insertRoleRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;
        private readonly IJWTProfilerHelper _jwthelper;
        private readonly IFusionCache _fusionCache;

        public Handler(ScoreSharpContext context, IMapper mapper, IJWTProfilerHelper jwthelper, IFusionCache fusionCache)
        {
            _context = context;
            _mapper = mapper;
            _jwthelper = jwthelper;
            _fusionCache = fusionCache;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertRoleRequest = request.insertRoleRequest;

            var single = await _context.Auth_Role.AsNoTracking().SingleOrDefaultAsync(x => x.RoleId == insertRoleRequest.RoleId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertRoleRequest.RoleId);

            var entity = _mapper.Map<Auth_Role>(insertRoleRequest);
            entity.AddTime = DateTime.Now;
            entity.AddUserId = _jwthelper.UserId;

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(insertRoleRequest.RoleId, insertRoleRequest.RoleId);
        }
    }
}
