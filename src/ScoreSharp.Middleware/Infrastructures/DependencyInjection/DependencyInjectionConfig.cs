using ScoreSharp.Watermark;

namespace ScoreSharp.Middleware.Infrastructures.DependencyInjection;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IUITCSecurityHelper, UITCSecurityHelper>();
        services.AddSingleton<IScoreSharpDapperContext, ScoreSharpDapperContext>();
        services.AddSingleton<IWatermarkHelper, WatermarkHelper>();
        services.AddScoped<EcardCreateReqHelper>();
        services.AddSingleton<IFTPHelper, FTPHelper>();
    }
}
