using Microsoft.Extensions.Options;
using Polly;
using ScoreSharp.API.Infrastructures.Adapter.Options;
using ScoreSharp.Common.Adapters.EcardMiddleware;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Adapters.MW3.Options;
using ScoreSharp.Common.Helpers.MW3Security;

namespace ScoreSharp.API.Infrastructures.Adapter;

public static class AdaptersConfig
{
    public static void AddAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MW3ProcInterceptorSerilLog>();
        services
            .AddOptions<MW3AdapterConfigurationOptions>()
            .Bind(configuration.GetSection("MW3AdapterConfiguration"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // 因為 MW3 組別有分 KEY IV 都不同，所以這邊要DI 注入雖然同個實體但參數不同，MW3ProcInterceptorSerilLog 已經使用 PROC
        // 所以這邊也要註冊PROC
        services.AddKeyedSingleton<IMW3SecurityHelper>(
            "PROC",
            (provider, key) =>
            {
                var options = provider.GetRequiredService<IOptions<MW3AdapterConfigurationOptions>>().Value;
                return new MW3SecurityHelper(options.Key_1, options.IV_1);
            }
        );

        // 因為 MW3APAPI 組別有分 KEY IV 都不同，所以這邊要DI 注入雖然同個實體但參數不同，MW3APAPIInterceptorSerilLog 已經使用 APAPI
        // 所以這邊也要註冊APAPI
        services.AddKeyedSingleton<IMW3SecurityHelper>(
            "APAPI",
            (provider, key) =>
            {
                var options = provider.GetRequiredService<IOptions<MW3AdapterConfigurationOptions>>().Value;
                return new MW3SecurityHelper(options.Key_2, options.IV_2);
            }
        );

        services
            .AddHttpClient<IMW3ProcAdapter, MW3ProcAdapter>()
            .ConfigurePrimaryHttpMessageHandler<MW3ProcInterceptorSerilLog>()
            .AddStandardResilienceHandler();

        services.AddScoped<LDAPInterceptor>();
        services
            .AddOptions<LDAPAdapterConfigurationOptions>()
            .Bind(configuration.GetSection("LDAPAdapterConfiguration"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<ILDAPAdapter, LDAPAdapter>().ConfigurePrimaryHttpMessageHandler<LDAPInterceptor>().AddStandardResilienceHandler();

        services.AddScoped<MW3MSAPIInterceptorSerilLog>();
        services
            .AddHttpClient<IMW3MSAPIAdapter, MW3MSAPIAdapter>()
            .ConfigurePrimaryHttpMessageHandler<MW3MSAPIInterceptorSerilLog>()
            .AddStandardResilienceHandler();

        services.AddScoped<MW3APAPIInterceptorSerilLog>();
        services
            .AddHttpClient<IMW3APAPIAdapter, MW3APAPIAdapter>()
            .ConfigurePrimaryHttpMessageHandler<MW3APAPIInterceptorSerilLog>()
            .AddStandardResilienceHandler(options =>
            {
                // 單次嘗試超時設為 90 秒
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(90);

                // 總請求超時設為 5 分鐘
                options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(5);

                // 減少重試次數，因為每次都要等很久
                options.Retry.MaxRetryAttempts = 2;

                // 使用較長的重試間隔
                options.Retry.BackoffType = DelayBackoffType.Exponential;

                // 修正 Circuit Breaker 採樣持續時間，至少要是嘗試超時的兩倍
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(180);
            });

        services
            .AddOptions<MiddlewareAdapterOption>()
            .Bind(configuration.GetSection("MiddlewareAdapter"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<MiddlewareInterceptor>();
        services
            .AddHttpClient<IMiddlewareAdapter, MiddlewareAdapter>()
            .ConfigurePrimaryHttpMessageHandler<MiddlewareInterceptor>()
            .AddStandardResilienceHandler();
    }
}
