using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace ScoreSharp.API.Infrastructures.Redis;

public static class RedisConfig
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddFusionCache()
            .WithDefaultEntryOptions(opt =>
            {
                opt.Duration = TimeSpan.FromMinutes(5);

                // 20251028 失效保護機制目前怪怪的
                // opt.IsFailSafeEnabled = true;
                // opt.FailSafeMaxDuration = TimeSpan.FromMinutes(30);
                // opt.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);

                // opt.FactorySoftTimeout = TimeSpan.FromMilliseconds(200);
                // opt.FactoryHardTimeout = TimeSpan.FromSeconds(3);

                // opt.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(2);
                // opt.DistributedCacheHardTimeout = TimeSpan.FromSeconds(5);
                // opt.AllowBackgroundDistributedCacheOperations = true;

                // opt.JitterMaxDuration = TimeSpan.FromSeconds(2);
            })
            // Warn: 發布到 192.168.233.40 要註解,因為那邊連不到測市區 Redis
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
