using StackExchange.Redis;

namespace ScoreSharp.Middleware.Infrastructures.HealthCheck;

public static class HealthCheckConfig
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var securityHelper = services.BuildServiceProvider().GetRequiredService<IUITCSecurityHelper>();
        var scoreSharpConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ScoreSharp")!);
        var ecardFileConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ECardFile")!);
        var scoreSharpFileConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ScoreSharpFile")!);
        var redisConfig = configuration.GetSection("RedisConfig").Get<RedisConfigOptions>();
        var ftpHost = configuration.GetValue<string>("FTP:Host");
        var ftpPort = configuration.GetValue<int>("FTP:Port");
        var ftpUsername = configuration.GetValue<string>("FTP:Username");
        var ftpMima = configuration.GetValue<string>("FTP:Mima");

        services
            .AddHealthChecks()
            .AddSqlServer(scoreSharpConnectionString, name: "ScoreSharpDB", tags: new[] { "database" })
            .AddSqlServer(ecardFileConnectionString, name: "EcardFileDB", tags: new[] { "database" })
            .AddSqlServer(scoreSharpFileConnectionString, name: "ScoreSharpFileDB", tags: new[] { "database" })
            .AddFtpHealthCheck(
                setup => setup.AddHost($"ftp://{ftpHost}:{ftpPort}", credentials: new NetworkCredential(ftpUsername, ftpMima)),
                name: "FTP",
                tags: new[] { "ftp" }
            )
            .AddRedis(
                connectionMultiplexerFactory: (serviceProvider) => CreateSentinelConnection(redisConfig),
                name: "Redis-Sentinel",
                tags: new[] { "redis", "sentinel" }
            )
            .AddCheck<SeqHealthyCheck>("Seq", tags: new[] { "seq" });

        return services;
    }

    private static IConnectionMultiplexer CreateSentinelConnection(RedisConfigOptions redisConfig)
    {
        var configurationOptions = new ConfigurationOptions
        {
            Password = redisConfig.Mima,
            AbortOnConnectFail = false,
            ConnectTimeout = 5000,
            SyncTimeout = 3000,
            DefaultDatabase = redisConfig.DefaultDatabase,
            ServiceName = redisConfig.ServiceName,
            TieBreaker = "",
        };

        // 添加 Sentinel 端點
        foreach (var endpoint in redisConfig.EndPoints)
        {
            configurationOptions.EndPoints.Add(endpoint);
        }

        return ConnectionMultiplexer.Connect(configurationOptions);
    }
}
