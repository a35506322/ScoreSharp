using ScoreSharp.API.Modules.OrgSetUp.User.GetUsersByQueryString;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 取得多筆使用者
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?UserId=&amp;IsActive=Y
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetUsersByQueryStringResponse>>))]
        [EndpointSpecificExample(typeof(取得使用者_2000_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [OpenApiOperation("GetUsersByQueryString")]
        public async Task<IResult> GetUsersByQueryString([FromQuery] GetUsersByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUsersByQueryString
{
    public record Query(GetUsersByQueryStringRequest getUsersByQueryStringRequest) : IRequest<ResultResponse<List<GetUsersByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetUsersByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetUsersByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var getUsersByQueryStringRequest = request.getUsersByQueryStringRequest;

            var entities = await _context
                .OrgSetUp_User.Where(x =>
                    string.IsNullOrEmpty(getUsersByQueryStringRequest.UserId) || x.UserId.Contains(getUsersByQueryStringRequest.UserId)
                )
                .Where(x => string.IsNullOrEmpty(getUsersByQueryStringRequest.IsActive) || x.IsActive == getUsersByQueryStringRequest.IsActive)
                .Where(x =>
                    string.IsNullOrEmpty(getUsersByQueryStringRequest.OrganizeCode) || x.OrganizeCode == getUsersByQueryStringRequest.OrganizeCode
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetUsersByQueryStringResponse>>(entities);

            var roleDic = await _context.Auth_Role.ToDictionaryAsync(x => x.RoleId, x => x.RoleName);
            var allUserRole = await _context.Auth_User_Role.Select(x => new { x.UserId, x.RoleId }).ToListAsync();
            var authUserRoleDic = allUserRole
                .GroupBy(x => x.UserId)
                .ToDictionary(x => x.Key, x => x.Select(y => new Role { RoleId = y.RoleId, RoleName = roleDic[y.RoleId] }).ToList());

            var organize = await _context.OrgSetUp_Organize.AsNoTracking().Select(x => new { x.OrganizeCode, x.OrganizeName }).ToListAsync();

            var caseDispatchGroupDic = entities.ToDictionary(
                k => k.UserId,
                v =>
                    string.IsNullOrWhiteSpace(v.CaseDispatchGroup)
                        ? []
                        : v
                            .CaseDispatchGroup.Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => new CaseDispatchGroupModel { CaseDispatchGroup = Enum.Parse<CaseDispatchGroup>(x) })
                            .ToList()
            );

            foreach (var item in result)
            {
                item.Roles = authUserRoleDic.TryGetValue(item.UserId, out var roles) ? roles : new List<Role>();
                item.OrganizeName = organize.FirstOrDefault(x => x.OrganizeCode == item.OrganizeCode)?.OrganizeName;
                item.CaseDispatchGroups = caseDispatchGroupDic.TryGetValue(item.UserId, out var groups) ? groups : new List<CaseDispatchGroupModel>();
            }

            return ApiResponseHelper.Success(result);
        }
    }
}
