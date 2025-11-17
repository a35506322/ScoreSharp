namespace ScoreSharp.Batch.Infrastructures.RazorTemplate;

public static class RazorTemplateConfig
{
    public static void AddRazorTemplate(this IServiceCollection services)
    {
        services.AddRazorTemplating();
    }
}
