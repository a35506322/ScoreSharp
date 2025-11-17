namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.Data;

public static class EFCoreConifg
{
    public static void AddEFCore(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
    {
        services.AddDbContext<ScoreSharpContext>(
            (serviceProvider, options) =>
            {
                var securityHelper = serviceProvider.GetRequiredService<IUITCSecurityHelper>();
                var connectionString = securityHelper.DecryptConn(config.GetConnectionString("ScoreSharp")!);
                options.UseSqlServer(connectionString);

                // 啟用敏感資訊記錄
                if (!environment.IsProduction())
                {
                    options.EnableSensitiveDataLogging();
                }
            }
        );
    }
}
