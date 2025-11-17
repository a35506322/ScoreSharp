using System.Text.Json;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace ScoreSharp.Middleware.Infrastructures.Redis;

public static class RedisConfig
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddFusionCache()
            .WithDefaultEntryOptions(opt =>
            {
                opt.Duration = TimeSpan.FromMinutes(1);
                opt.IsFailSafeEnabled = true;
                opt.FailSafeMaxDuration = TimeSpan.FromMinutes(30);
                opt.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);

                opt.FactorySoftTimeout = TimeSpan.FromMilliseconds(200);
                opt.FactoryHardTimeout = TimeSpan.FromSeconds(3);

                opt.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(2);
                opt.DistributedCacheHardTimeout = TimeSpan.FromSeconds(5);
                opt.AllowBackgroundDistributedCacheOperations = true;

                opt.JitterMaxDuration = TimeSpan.FromSeconds(2);
            })
            .WithDistributedCache(_ =>
            {
                var redisConfig = configuration.GetSection("RedisConfig").Get<RedisConfigOptions>();
                var configurationOptions = new ConfigurationOptions
                {
                    Password = redisConfig.Mima,
                    AbortOnConnectFail = false,
                    ConnectTimeout = 3000,
                    DefaultDatabase = redisConfig.DefaultDatabase,
                    ServiceName = redisConfig.ServiceName,
                    TieBreaker = "",
                };

                foreach (var endpoint in redisConfig.EndPoints)
                {
                    configurationOptions.EndPoints.Add(endpoint);
                }

                var options = new RedisCacheOptions
                {
                    ConfigurationOptions = configurationOptions,
                    InstanceName = environment.ApplicationName + "_" + environment.EnvironmentName,
                };
                return new RedisCache(options);
            })
            .WithSerializer(
                new FusionCacheSystemTextJsonSerializer(
                    new JsonSerializerOptions
                    {
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 允許更寬鬆的字元編碼
                    }
                )
            );
    }
}
