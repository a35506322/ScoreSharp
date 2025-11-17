using Microsoft.AspNetCore.Authorization.Policy;

namespace ScoreSharp.API.Infrastructures.Security;

/// <summary>
/// 這邊會覆蓋掉原本的 AuthorizationMiddlewareResultHandler 例如401、403的處理
/// 可以參考此篇，使用預設　https://learn.microsoft.com/zh-cn/aspnet/core/security/authorization/customizingauthorizationmiddlewareresponse?view=aspnetcore-8.0
/// </summary>
public class PermissionAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly ILogger<PermissionAuthorizationMiddlewareResultHandler> _logger;
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public PermissionAuthorizationMiddlewareResultHandler(ILogger<PermissionAuthorizationMiddlewareResultHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult
    )
    {
        // 自訂義 token 驗證失敗回應
        if (authorizeResult.Challenged)
        {
            // _logger.LogError("PermissionAuthorizationMiddlewareResultHandler => Token 驗證失敗 回傳401");

            string errMsg = JsonSerializer.Serialize(authorizeResult?.AuthorizationFailure?.FailureReasons);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(ApiResponseHelper.TokenCheckFailed("目前請求沒有通過Token驗證"));
            return;
        }

        var permissionAuthorizationRequirements = policy.Requirements.OfType<PermissionAuthorizationRequirement>();

        // 授權失敗
        if (authorizeResult.Forbidden && permissionAuthorizationRequirements.Any())
        {
            var messages = authorizeResult?.AuthorizationFailure?.FailureReasons.Select(x => x.Message);
            string errMsg = string.Join("、", messages);
            _logger.LogError("PermissionAuthorizationMiddlewareResultHandler => 授權失敗 回傳403，錯誤訊息{errMsg}", errMsg);

            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(ApiResponseHelper.PolicyCheckFailed(errMsg));
            return;
        }

        // Fall back to the default implementation.
        // https://learn.microsoft.com/zh-cn/aspnet/core/security/authorization/customizingauthorizationmiddlewareresponse?view=aspnetcore-8.0
        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
