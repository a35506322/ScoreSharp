using ScoreSharp.API.Modules.OrgSetUp.User.Login;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 登入成功
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// 注意此權限並無新增至 Auth_Action 而是掛 [AllowAnonymous]，
        /// 主因此為登入不需要權限管理
        /// </remarks>
        /// <response code="200">
        /// 登入成功 token => 瀏覽器 cookie 用 ExpireMinutes 設置 token 有效時間(分鐘)
        /// </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<LoginResponse>))]
        [EndpointSpecificExample(
            typeof(登入成功Admin_2000_ReqEx),
            typeof(登入成功資訊科人員_2000_ReqEx),
            typeof(登入成功徵審人員_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(登入成功_2000_ResEx),
            typeof(登入帳密有誤_4003_ResEx),
            typeof(使用者未啟用_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [AllowAnonymous]
        [OpenApiOperation("Login")]
        public async Task<IResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.Login
{
    public record Command(LoginRequest LoginRequest) : IRequest<ResultResponse<LoginResponse>>;

    public class Handler : IRequestHandler<Command, ResultResponse<LoginResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTHelper _jwtHelper;
        private readonly ILDAPHelper _ldapHelper;
        private readonly IWebHostEnvironment _env;
        private readonly ILDAPAdapter _ldapAdapter;
        private readonly ILogger<Handler> _logger;

        public Handler(
            ScoreSharpContext context,
            IJWTHelper jwtHelper,
            ILDAPHelper ldapHelper,
            IWebHostEnvironment env,
            ILDAPAdapter ldapAdapter,
            ILogger<Handler> logger
        )
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _ldapHelper = ldapHelper;
            _env = env;
            _ldapAdapter = ldapAdapter;
            _logger = logger;
        }

        public async Task<ResultResponse<LoginResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var loginRequest = request.LoginRequest;

            var user = await _context.OrgSetUp_User.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == loginRequest.UserId);

            if (user is null)
                return ApiResponseHelper.BusinessLogicFailed<LoginResponse>(null, "此使用者未申請權限，請聯絡資訊科人員協助");

            bool userIsActive = user.IsActive == "Y" ? true : false;
            if (!userIsActive)
                return ApiResponseHelper.BusinessLogicFailed<LoginResponse>(null, "使用者已停用，請聯絡資訊科人員協助");

            /*
             * 如果是 AD 帳號，則透過 LDAP 驗證
             * 如果是非 AD 帳號，則檢查 OrgSetUp_User 的 Mima 是否正確
             * 如果密碼驗證失敗，MimaErrorCount 加 1 ，達五次則鎖定帳號
             */

            bool isAD = user.IsAD == "Y";
            if (isAD)
            {
                /*
                * Development、Production 環境
                * => 透過 AD Server 驗收帳號是否在 AD Server 裡面

                * Testing 環境
                * => 透過 192.168.233.40 => LDAP Server 驗收帳號是否在 AD Server 裡面
                */

                if (_env.IsDevelopment() || _env.IsProduction())
                {
                    // var isVaildAuth = _ldapHelper.VaildLDAPAuth(loginRequest.UserId, loginRequest.Mima);
                    // if (!isVaildAuth)
                    // {
                    //     var mimaErrorCount = await MimaErrorCountErrorHandle(loginRequest.UserId);
                    //     return ApiResponseHelper.BusinessLogicFailed<LoginResponse>(null, $"帳密有誤，剩餘次數: {mimaErrorCount}");
                    // }
                }
                else if (_env.IsEnvironment("Testing"))
                {
                    //var isVaildAuthResponse = await _ldapAdapter.ValidateLDAPAuth(loginRequest.UserId, loginRequest.Mima);

                    //if (isVaildAuthResponse.IsSuccess && !isVaildAuthResponse.Data)
                    //{
                    //    var mimaErrorCount = await MimaErrorCountErrorHandle(loginRequest.UserId);
                    //    return ApiResponseHelper.BusinessLogicFailed<LoginResponse>(null, $"帳密有誤，剩餘次數: {mimaErrorCount}");
                    //}
                    //else if (!isVaildAuthResponse.IsSuccess)
                    //{
                    //    _logger.LogError("LDAP 發查失敗 {@isVaildAuthResponse}", isVaildAuthResponse);
                    //    return ApiResponseHelper.CheckThirdPartyApiError<LoginResponse>(null, loginRequest.UserId);
                    //}
                }
            }
            else
            {
                if (user.Mima != loginRequest.Mima)
                {
                    var mimaErrorCount = await MimaErrorCountErrorHandle(loginRequest.UserId);
                    return ApiResponseHelper.BusinessLogicFailed<LoginResponse>(null, $"帳密有誤，剩餘次數: {mimaErrorCount}");
                }
            }

            if (user.MimaErrorCount > 0)
                await MimaErrorCountSuccessHandle(loginRequest.UserId);

            var user_role = await _context
                .Auth_User_Role.Where(x => x.UserId == loginRequest.UserId)
                .Select(x => new UserRoleDto(x.UserId, x.RoleId))
                .ToListAsync();

            var roles = user_role.Select(x => x.RoleId).ToList();

            List<CaseDispatchGroup> caseDispatchGroups = string.IsNullOrWhiteSpace(user.CaseDispatchGroup)
                ? []
                : user.CaseDispatchGroup?.Split(',').Select(Enum.Parse<CaseDispatchGroup>).ToList();

            int expireMinutes = 480;
            var token = _jwtHelper.GenerateToken(user.UserId, user.UserName, roles, expireMinutes, user.OrganizeCode, caseDispatchGroups);
            var response = new LoginResponse { Token = token, ExpireMinutes = expireMinutes };

            return ApiResponseHelper.Success(response, "登入成功");
        }

        private async Task<int> MimaErrorCountErrorHandle(string userId)
        {
            var user = await _context.OrgSetUp_User.SingleOrDefaultAsync(x => x.UserId == userId);
            user.MimaErrorCount += 1;

            if (user.MimaErrorCount >= 5)
            {
                user.IsActive = "N";
                user.MimaErrorCount = 0;
                user.StopReason = "密碼錯誤次數達五次";
            }

            await _context.SaveChangesAsync();

            return user.MimaErrorCount == 0 ? 0 : 5 - user.MimaErrorCount;
        }

        private async Task<int> MimaErrorCountSuccessHandle(string userId)
        {
            var user = await _context.OrgSetUp_User.SingleOrDefaultAsync(x => x.UserId == userId);
            user.MimaErrorCount = 0;
            await _context.SaveChangesAsync();
            return user.MimaErrorCount;
        }
    }
}
