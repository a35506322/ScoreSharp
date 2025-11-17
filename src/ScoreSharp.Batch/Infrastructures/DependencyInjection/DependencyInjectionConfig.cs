using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase;
using ScoreSharp.Batch.Jobs.PaperCheckNewCase;
using ScoreSharp.Watermark;

namespace ScoreSharp.Batch.Infrastructures.DependencyInjection;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        // other
        services.AddSingleton<IUITCSecurityHelper, UITCSecurityHelper>();

        // jobs
        services.AddScoped<IEcardNotA02CheckNewCaseRepository, EcardNotA02CheckNewCaseRepository>();
        services.AddScoped<IEcardNotA02CheckNewCaseService, EcardNotA02CheckNewCaseService>();
        services.AddScoped<IEcardA02CheckNewCaseRepository, EcardA02CheckNewCaseRepository>();
        services.AddScoped<IEcardA02CheckNewCaseService, EcardA02CheckNewCaseService>();
        services.AddScoped<IPaperCheckNewCaseRepository, PaperCheckNewCaseRepository>();
        services.AddScoped<IPaperCheckNewCaseService, PaperCheckNewCaseService>();

        // dapper
        services.AddSingleton<IScoreSharpDapperContext, ScoreSharpDapperContext>();

        services.AddSingleton<IWatermarkHelper, WatermarkHelper>();
    }
}
