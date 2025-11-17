using ScoreSharp.API.Modules.Auth.Role.InsertRoleAuthById;

namespace ScoreSharp.API.Modules.Auth.Role
{
    public partial class RoleController
    {
        /// <summary>
        ///  新增單筆角色權限
        /// </summary>
        /// <remarks>
        ///  1. 注意此網址 RoleId 需要與 Request 的RoleId 完全一致，否則會出現[4003]RouterRoleId不符合
        ///  2. ActionId 與 RouterId 呈現一對一關係，如果有誤，會出現[4003]ActionId與RoleId不符合
        /// </remarks>
        /// <param name="roleId">角色ID</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增單筆角色權限_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增單筆角色權限_2000_ResEx),
            typeof(新增單筆角色權限RouterRoleId不符合_4003_ResEx),
            typeof(新增單筆角色權限查無此RoleId_4001_ResEx),
            typeof(新增單筆角色權限ActionId與RoleId不符合_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertRoleAuthById")]
        public async Task<IResult> InsertRoleAuthById([FromRoute] string roleId, [FromBody] IEnumerable<InsertRoleAuthByIdRequest> request)
        {
            var result = await _mediator.Send(new Command(roleId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Role.InsertRoleAuthById
{
    public record Command(string roleId, IEnumerable<InsertRoleAuthByIdRequest> insertRoleAuthByIdRequests)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;
        private readonly IFusionCache _fusionCache;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IMapper mapper, IFusionCache fusionCache, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _mapper = mapper;
            _fusionCache = fusionCache;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertRequest = request
                .insertRoleAuthByIdRequests.Select(x => JsonSerializer.Serialize(x))
                .Distinct()
                .Select(x => JsonSerializer.Deserialize<InsertRoleAuthByIdRequest>(x));

            var routerWithRoleId = request.roleId;

            bool isRouterRoleId = insertRequest.Select(x => x.RoleId).Where(x => x != routerWithRoleId).Count() > 0;
            if (isRouterRoleId)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "Router RoleId 不符合，請檢查");

            var single = await _context.Auth_Role.AsNoTracking().SingleOrDefaultAsync(x => x.RoleId == routerWithRoleId);
            if (single == null)
                return ApiResponseHelper.NotFound<string>(null, routerWithRoleId);

            var actions = await _context.Auth_Action.AsNoTracking().Where(x => x.IsActive == "Y").ToListAsync();
            var actionsDic = actions.ToDictionary(x => x.ActionId, x => x.RouterId);
            bool checkActionAndRole = true;
            foreach (var item in insertRequest)
            {
                bool isGetRouterId = actionsDic.TryGetValue(item.ActionId, out string routerId);
                if (!(isGetRouterId && item.RouterId == routerId))
                {
                    checkActionAndRole = false;
                    break;
                }
            }
            if (!checkActionAndRole)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "ActionId 與 RoleId 不符合，請檢查");

            var entities = _mapper.Map<IEnumerable<Auth_Role_Router_Action>>(insertRequest);

            var entity = await _context.Auth_Role.SingleOrDefaultAsync(x => x.RoleId == request.roleId);
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = _jwthelper.UserId;

            await _context.Auth_Role_Router_Action.Where(x => x.RoleId == routerWithRoleId).ExecuteDeleteAsync();
            await _context.Auth_Role_Router_Action.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            await _fusionCache.RemoveByTagAsync(SecurityConstants.PolicyRedisTag.RoleAction);

            return ApiResponseHelper.InsertSuccess<string>(routerWithRoleId, routerWithRoleId);
        }
    }
}
