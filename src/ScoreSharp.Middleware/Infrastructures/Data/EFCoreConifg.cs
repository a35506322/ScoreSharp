using System.Transactions;
using ScoreSharp.Middleware.Infrastructures.FileData;

namespace ScoreSharp.Middleware.Infrastructures.Data;

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

        // 管理兩個 DB Context 需要分開註冊也要分Folder
        services.AddDbContext<ScoreSharpFileContext>(
            (serviceProvider, options) =>
            {
                var securityHelper = serviceProvider.GetRequiredService<IUITCSecurityHelper>();
                var connectionString = securityHelper.DecryptConn(config.GetConnectionString("ScoreSharpFile")!);
                options.UseSqlServer(connectionString);
            }
        );
    }
}
