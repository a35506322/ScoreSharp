using System.Data;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ScoreSharp.API.Infrastructures.Security;

/// <summary>
/// 主要實作政策邏輯
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    private IPermissionAuthorizationProvider _permissionAuthorizationProvider;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(
        IPermissionAuthorizationProvider permissionAuthorizationProvider,
        ILogger<PermissionAuthorizationHandler> logger
    )
    {
        _permissionAuthorizationProvider = permissionAuthorizationProvider;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        // _logger.LogInformation("PermissionAuthorizationHandler => 處理政策為: {PolicyName}", requirement.PolicyName);

        // 1. 判斷是否有通過Token驗證
        if (context.User.Identity.IsAuthenticated == false)
        {
            context.Fail(new AuthorizationFailureReason(this, "目前請求沒有通過Token驗證"));
            return;
        }

        // 2. 判斷是否為自定義政策
        if (requirement.PolicyName == "Policy-RBAC")
        {
            // 1. 因Token驗證 通過，取得使用者的角色及UserId
            var roles = context.User?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray() ?? Array.Empty<string>();
            var userId = context.User.Identity.Name;

            // 2. 先比對 Action 是否有 IsCommon = Y，有的話直接通過
            // 3. 再比對 Action 是否有 IsCommon = N，有的話再比對是否有該Action
            // https://stackoverflow.com/questions/47809437/how-to-access-current-httpcontext-in-asp-net-core-2-custom-policy-based-authoriz
            if (context.Resource is HttpContext httpContext)
            {
                var endpoint = httpContext.GetEndpoint();
                var actionName = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>().ActionName;

                // 套上 RBAC Action 一定要註冊在 Auth_Action
                var currentAction = await _permissionAuthorizationProvider.GetActionByActionId(actionName);
                if (currentAction is null)
                {
                    throw new Exception($"Action {actionName} 未註冊在 Auth_Action");
                }

                if (currentAction.IsCommon == "N")
                {
                    // 4. 取得該啟用Role的權限及所有Action
                    var roleWithAction = await _permissionAuthorizationProvider.GetRoleWithActoinByRoleIds(roles);
                    var actionId = roleWithAction.FirstOrDefault(x => x == actionName);
                    if (actionId is null)
                    {
                        _logger.LogError(
                            "PermissionAuthorizationHandler => 用戶{userId}，未通過授權{requirement.PolicyName}，沒有此{actionName}權限",
                            userId,
                            requirement.PolicyName,
                            actionName
                        );

                        context.Fail(
                            new AuthorizationFailureReason(this, $"使用者{userId}，未通過授權{requirement.PolicyName}，沒有此{actionName}權限")
                        );
                    }
                }

                if (currentAction.IsActive == "N")
                {
                    _logger.LogError("PermissionAuthorizationHandler => 用戶{userId}，此{actionName}權限未啟用", userId, actionName);
                    context.Fail(new AuthorizationFailureReason(this, $"使用者{userId}，此{actionName}權限未啟用"));
                }
            }
        }

        if (context.HasFailed == false)
        {
            context.Succeed(requirement);
        }
    }
}
