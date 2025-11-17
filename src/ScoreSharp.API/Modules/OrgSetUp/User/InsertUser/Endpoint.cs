using ScoreSharp.API.Infrastructures.Adapter;
using ScoreSharp.API.Modules.OrgSetUp.User.InsertUser;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 新增單筆使用者
        /// </summary>
        /// <remarks>
        /// AD 帳號新增
        /// 1. 不能有密碼
        ///
        /// 自創帳號
        /// 1. 密碼至少8個字
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="400">
        /// 檢查
        /// 1. IsAD(是否AD) ="Y" => 不能有密碼
        /// 2. IsAD(是否AD) ="N" => 密碼檢查，不能為空且長度至少8個字
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(新增使用者_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(新增使用者_2000_ResEx),
            typeof(新增使用者資料已存在_4002_ResEx),
            typeof(使用者資訊與ADServer不符合_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(新增使用者自創帳號密碼不能為空_4000_ResEx),
            typeof(新增使用者自創帳號密碼長度至少8個字_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("InsertUser")]
        public async Task<IResult> InsertUser([FromBody] InsertUserRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.InsertUser
{
    public record Command(InsertUserRequest insertUserRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;
        private readonly ILDAPHelper _ldapHelper;
        private readonly ILDAPAdapter _ldapAdapter;
        private readonly IWebHostEnvironment _env;

        public Handler(ScoreSharpContext context, IMapper mapper, ILDAPHelper ldapHelper, ILDAPAdapter ldapAdapter, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _ldapHelper = ldapHelper;
            _ldapAdapter = ldapAdapter;
            _env = env;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertUserRequest = request.insertUserRequest;

            var single = await _context.OrgSetUp_User.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == insertUserRequest.UserId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertUserRequest.UserId);

            bool isAD = insertUserRequest.IsAD == "Y" ? true : false;

            if (isAD)
            {
                LDAPUserInfo? adUser = null;

                if (_env.IsDevelopment() || _env.IsProduction())
                {
                    adUser = _ldapHelper.SearchBySAMAccountName(insertUserRequest.UserId);
                }
                else
                {
                    var searchBySAMAccountName = await _ldapAdapter.SearchBySAMAccountName(insertUserRequest.UserId);
                    var adUserInfo = searchBySAMAccountName.IsSuccess ? searchBySAMAccountName.Data : null;
                    if (adUserInfo is not null)
                    {
                        adUser = new LDAPUserInfo
                        {
                            SAMAccountName = adUserInfo.SAMAccountName,
                            DisplayName = adUserInfo.DisplayName,
                            MemberOf = adUserInfo.MemberOf,
                            UserPrincipalName = adUserInfo.UserPrincipalName,
                        };
                    }
                }

                if (adUser is null)
                {
                    return ApiResponseHelper.NotFound<string>(null, insertUserRequest.UserId);
                }

                if (adUser.DisplayName != insertUserRequest.UserName)
                {
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, "此使用者資訊與 AD Server 不符合，請重新確認");
                }
            }

            var roles = await _context.Auth_Role.Where(x => x.IsActive == "Y").Select(x => x.RoleId).ToListAsync();

            if (!insertUserRequest.RoleId.All(roleId => roles.Contains(roleId)))
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "RoleId 不符合，請重新確認");
            }

            var entities = insertUserRequest
                .RoleId.Select(roleId => new Auth_User_Role { UserId = insertUserRequest.UserId, RoleId = roleId })
                .ToList();

            if (!String.IsNullOrWhiteSpace(insertUserRequest.OrganizeCode))
            {
                var organize = await _context
                    .OrgSetUp_Organize.AsNoTracking()
                    .SingleOrDefaultAsync(x => x.OrganizeCode == insertUserRequest.OrganizeCode);

                if (organize is null)
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, "組織代碼不存在");
            }

            OrgSetUp_User entity = new()
            {
                UserId = insertUserRequest.UserId,
                IsActive = insertUserRequest.IsActive,
                UserName = insertUserRequest.UserName,
                IsAD = insertUserRequest.IsAD,
                Mima = insertUserRequest.Mima,
                OrganizeCode = insertUserRequest.OrganizeCode,
                CaseDispatchGroup =
                    insertUserRequest.CaseDispatchGroups.Count != 0 ? string.Join(",", insertUserRequest.CaseDispatchGroups.Cast<int>()) : null,
                EmployeeNo = insertUserRequest.EmployeeNo,
            };

            await _context.OrgSetUp_User.AddAsync(entity);
            await _context.Auth_User_Role.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            var rolesList = string.Join(", ", insertUserRequest.RoleId);
            string returnMsg = $"{insertUserRequest.UserId}，Role : {rolesList}";

            return ApiResponseHelper.InsertSuccess(insertUserRequest.UserId, returnMsg);
        }
    }
}
