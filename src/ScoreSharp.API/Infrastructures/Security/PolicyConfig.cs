namespace ScoreSharp.API.Infrastructures.Security;

public static class PolicyConfig
{
    public static void AddPolicy(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddSingleton<IPermissionAuthorizationProvider, PermissionAuthorizationProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, PermissionAuthorizationMiddlewareResultHandler>();
    }
}
