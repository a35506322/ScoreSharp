using System.Net;

namespace ScoreSharp.API.Infrastructures.HealthCheck;

public static class HealthCheckConfig
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var securityHelper = services.BuildServiceProvider().GetRequiredService<IUITCSecurityHelper>();
        var scoreSharpConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ScoreSharp")!);
        var eCardFileConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ECardFile")!);
        var scoreSharpFileConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ScoreSharpFile")!);
        var redisConfig = configuration.GetSection("RedisConfig").Get<RedisConfigOptions>();

        services
            .AddHealthChecks()
            .AddSqlServer(scoreSharpConnectionString, name: "ScoreSharpDB", tags: new[] { "database" })
            .AddSqlServer(eCardFileConnectionString, name: "ECardFileDB", tags: new[] { "database" })
            .AddSqlServer(scoreSharpFileConnectionString, name: "ScoreSharpFileDB", tags: new[] { "database" })
            .AddRedis(
                connectionMultiplexerFactory: (serviceProvider) => CreateSentinelConnection(redisConfig),
                name: "Redis-Sentinel",
                tags: new[] { "redis", "sentinel" }
            )
            .AddCheck<MW3HealthyCheck>("MW3", tags: new[] { "mw3" })
            .AddCheck<NameCheckHealthyCheck>("NameCheckHealthyCheck", tags: new[] { "mw3" })
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
