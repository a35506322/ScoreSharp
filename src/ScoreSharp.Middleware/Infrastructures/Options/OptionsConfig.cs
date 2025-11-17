namespace ScoreSharp.Middleware.Infrastructures.OthersCode;

public static class OptionsConfig
{
    public static void AddOptionsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<FTPOption>().Bind(configuration.GetSection("FTP")).ValidateDataAnnotations().ValidateOnStart();
    }
}
