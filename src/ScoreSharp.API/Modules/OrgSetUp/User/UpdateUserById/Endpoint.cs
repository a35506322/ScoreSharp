using ScoreSharp.API.Modules.OrgSetUp.User.UpdateUserById;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 單筆修改使用者
        /// </summary>
        /// <remarks>
        /// 1. 使用者無法修改 AD 或者 非AD 記號，主因 UserId 是 AD 的唯一識別碼，無法修改
        /// 2. 如果使用者是非 AD ，可以修改姓名反之不行
        /// </remarks>
        /// <param name="userId">PK</param>
        /// <param name="request"></param>
        /// <response code="400">
        /// 檢查
        /// 1. IsAD(是否AD) ="Y" => 不能有密碼
        /// 2. IsAD(是否AD) ="N" => 密碼檢查，不能為空且長度至少8個字
        /// </response>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(修改使用者_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(修改使用者_2000_ResEx),
            typeof(修改使用者查無此資料_4001_ResEx),
            typeof(修改使用者非AD請輸入使用者名稱_4003_ResEx),
            typeof(修改使用者路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(修改使用者自創帳號密碼不能為空_4000_ResEx),
            typeof(修改使用者自創帳號密碼長度至少8個字_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("UpdateUserById")]
        public async Task<IResult> UpdateUserById([FromRoute] string userId, [FromBody] UpdateUserByIdRequest request)
        {
            var result = await _mediator.Send(new Command(userId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.UpdateUserById
{
    public record Command(string userId, UpdateUserByIdRequest updateUserByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateUserByIdRequest = request.updateUserByIdRequest;

            if (updateUserByIdRequest.UserId != request.userId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.OrgSetUp_User.SingleOrDefaultAsync(x => x.UserId == request.userId);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.userId);

            var roles = await _context.Auth_Role.Where(x => x.IsActive == "Y").Select(x => x.RoleId).ToListAsync();
            if (!updateUserByIdRequest.RoleId.All(roleId => roles.Contains(roleId)))
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "RoleId 不符合，請重新確認");
            }

            var entities = updateUserByIdRequest
                .RoleId.Select(roleId => new Auth_User_Role { UserId = updateUserByIdRequest.UserId, RoleId = roleId })
                .ToList();

            if (!String.IsNullOrWhiteSpace(updateUserByIdRequest.OrganizeCode))
            {
                var organize = _context
                    .OrgSetUp_Organize.AsNoTracking()
                    .Where(x => x.OrganizeCode == updateUserByIdRequest.OrganizeCode)
                    .FirstOrDefault();

                if (organize is null)
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, "組織代碼不存在");
            }

            bool isAD = entity.IsAD == "Y";
            if (!isAD)
            {
                if (string.IsNullOrWhiteSpace(updateUserByIdRequest.UserName))
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, "非AD 請輸入使用者名稱");
            }

            // 更新
            entity.IsActive = updateUserByIdRequest.IsActive;
            if (!isAD)
                entity.UserName = updateUserByIdRequest.UserName;
            entity.OrganizeCode = updateUserByIdRequest.OrganizeCode;
            entity.CaseDispatchGroup =
                updateUserByIdRequest.CaseDispatchGroups.Count != 0 ? string.Join(",", updateUserByIdRequest.CaseDispatchGroups.Cast<int>()) : null;
            entity.StopReason = updateUserByIdRequest.StopReason;
            entity.EmployeeNo = updateUserByIdRequest.EmployeeNo;

            await _context.Auth_User_Role.Where(x => x.UserId == request.userId).ExecuteDeleteAsync();
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.userId, request.userId);
        }
    }
}
