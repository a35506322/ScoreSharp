using ScoreSharp.PaperMiddleware.Common.Filters;

namespace ScoreSharp.PaperMiddleware.Infrastructures.DependencyInjection;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IUITCSecurityHelper, UITCSecurityHelper>();

        services.AddScoped<HeaderValidationFilter>();
    }
}
