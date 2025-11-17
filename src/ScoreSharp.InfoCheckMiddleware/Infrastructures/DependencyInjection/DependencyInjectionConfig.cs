namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.DependencyInjection;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IUITCSecurityHelper, UITCSecurityHelper>();
    }
}
