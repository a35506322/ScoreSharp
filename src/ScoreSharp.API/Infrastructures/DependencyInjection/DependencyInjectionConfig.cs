using ScoreSharp.API.Modules.Manage.Common.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers;

namespace ScoreSharp.API.Infrastructures.DependencyInjection;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        // other
        services.AddSingleton<IJWTHelper, JWTHelper>();
        services.AddSingleton<IScoreSharpDapperContext, ScoreSharpDapperContext>();
        services.AddScoped<IJWTProfilerHelper, JWTProfilerHelper>();
        services.AddSingleton<IMiniExcelHelper, MiniExcelHelper>();
        services.AddSingleton<IUITCSecurityHelper, UITCSecurityHelper>();
        services.AddScoped<ILDAPHelper, LDAPHelper>();
        services.AddScoped<IReviewerHelper, ReviewerHelper>();
        services.AddScoped<IManageHelper, ManageHelper>();
        services.AddScoped<IReviewerValidateHelper, ReviewerValidateHelper>();
    }
}
