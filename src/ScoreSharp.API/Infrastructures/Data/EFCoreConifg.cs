using ScoreSharp.API.Infrastructures.Data.EFCoreInterceptors;

namespace ScoreSharp.API.Infrastructures.Data;

public static class EFCoreConifg
{
    public static void AddEFCore(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
    {
        services.AddSingleton<AuditInterceptor>();
        services.AddSingleton<CardHolderChangeInterceptor>();
        services.AddDbContext<ScoreSharpContext>(
            (serviceProvider, options) =>
            {
                var securityHelper = serviceProvider.GetRequiredService<IUITCSecurityHelper>();
                var connectionString = securityHelper.DecryptConn(config.GetConnectionString("ScoreSharp")!);
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(
                            // 重試次數設為 3 次
                            maxRetryCount: 3,
                            // 最大延遲時間保持 30 秒，第一次約 1 秒　第二次約 2-4 秒　第三次約 8-16 秒　不會超過 30 秒
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            // 錯誤號碼設為 null
                            errorNumbersToAdd: null
                        );
                        sqlServerOptions.CommandTimeout(180);
                    }
                );

                var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
                var cardHolderChangeInterceptor = serviceProvider.GetRequiredService<CardHolderChangeInterceptor>();
                options.AddInterceptors(auditInterceptor, cardHolderChangeInterceptor);

                // 啟用敏感資訊記錄
                if (!environment.IsProduction())
                {
                    options.EnableSensitiveDataLogging();
                }
            }
        );

        // 管理兩個 DB Context 需要分開註冊也要分Folder
        services.AddDbContext<ScoreSharpFileContext>(
            (serviceProvider, options) =>
            {
                var securityHelper = serviceProvider.GetRequiredService<IUITCSecurityHelper>();
                var connectionString = securityHelper.DecryptConn(config.GetConnectionString("ScoreSharpFile")!);
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(
                            // 重試次數設為 3 次
                            maxRetryCount: 3,
                            // 最大延遲時間保持 30 秒，第一次約 1 秒　第二次約 2-4 秒　第三次約 8-16 秒　不會超過 30 秒
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            // 錯誤號碼設為 null
                            errorNumbersToAdd: null
                        );
                        sqlServerOptions.CommandTimeout(180);
                    }
                );
            }
        );
    }
}
