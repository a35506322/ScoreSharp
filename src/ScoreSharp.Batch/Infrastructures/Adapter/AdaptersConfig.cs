using Polly;
using ScoreSharp.Batch.Infrastructures.Adapter.Options;
using ScoreSharp.Batch.Infrastructures.Adapter.PaperMiddleware;
using ScoreSharp.Common.Adapters.EcardMiddleware;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Adapters.MW3.Options;
using ScoreSharp.Common.Helpers.MW3Security;

namespace ScoreSharp.Batch.Infrastructures.Adapter;

public static class AdaptersConfig
{
    public static void AddAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MW3ProcInterceptorLogString>();
        services.AddScoped<MW3MSAPIInterceptorLogString>();
        services.AddScoped<MW3APAPIInterceptorLogString>();

        services
            .AddOptions<MW3AdapterConfigurationOptions>()
            .BindConfiguration("MW3AdapterConfiguration")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddKeyedSingleton<IMW3SecurityHelper>(
            "PROC",
            (provider, key) =>
            {
                var options = provider.GetRequiredService<IOptions<MW3AdapterConfigurationOptions>>().Value;
                return new MW3SecurityHelper(options.Key_1, options.IV_1);
            }
        );
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
            .ConfigurePrimaryHttpMessageHandler<MW3ProcInterceptorLogString>()
            .AddStandardResilienceHandler();

        services
            .AddHttpClient<IMW3MSAPIAdapter, MW3MSAPIAdapter>()
            .ConfigurePrimaryHttpMessageHandler<MW3MSAPIInterceptorLogString>()
            .AddStandardResilienceHandler();

        services
            .AddHttpClient<IMW3APAPIAdapter, MW3APAPIAdapter>()
            .ConfigurePrimaryHttpMessageHandler<MW3APAPIInterceptorLogString>()
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

        services.AddOptions<EmailAdapterOption>().Bind(configuration.GetSection("EmailAdapter")).ValidateDataAnnotations().ValidateOnStart();

        services.AddScoped<EmailInterceptor>();
        services.AddHttpClient<IEmailAdapter, EmailAdapter>().ConfigurePrimaryHttpMessageHandler<EmailInterceptor>().AddStandardResilienceHandler();

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

        services
            .AddOptions<PaperMiddlewareAdapterOption>()
            .Bind(configuration.GetSection("PaperMiddlewareAdapter"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<PaperMiddlewareInterceptor>();
        services
            .AddHttpClient<IPaperMiddlewareAdapter, PaperMiddlewareAdapter>()
            .ConfigurePrimaryHttpMessageHandler<PaperMiddlewareInterceptor>()
            .AddStandardResilienceHandler();
    }
}
