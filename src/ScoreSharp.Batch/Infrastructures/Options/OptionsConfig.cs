using ScoreSharp.Batch.Infrastructures.Adapter.Options;

namespace ScoreSharp.Batch.Infrastructures.Options;

public static class OptionsConfig
{
    public static void AddOptionsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ReportPath>().Bind(configuration.GetSection("ReportPath")).ValidateDataAnnotations().ValidateOnStart();

        services
            .AddOptions<TemplateReportOption>()
            .Bind(configuration.GetSection("TemplateReportOption"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<EcardFtpOption>().Bind(configuration.GetSection("EcardFtp")).ValidateDataAnnotations().ValidateOnStart();
    }
}
