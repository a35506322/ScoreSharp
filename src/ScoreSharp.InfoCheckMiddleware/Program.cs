var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

// 註冊 Serillog
SerilogConfig.AddSerilLog(config, env);

try
{
    Log.Information($"Starting InfoCheckMiddleware web host ({env.EnvironmentName})");

    // Add services to the container.
    builder
        .Services.AddControllers(options =>
        {
            // 從資源檔設定 ModelBinding 的錯誤訊息
            var provider = options.ModelBindingMessageProvider;
            provider.SetAttemptedValueIsInvalidAccessor((x, y) => string.Format(ModelBindingMessage.AttemptedValueIsInvalid, x, y));
            provider.SetMissingBindRequiredValueAccessor(x => string.Format(ModelBindingMessage.MissingBindRequiredValue, x));
            provider.SetMissingKeyOrValueAccessor(() => ModelBindingMessage.MissingKeyOrValue);
            provider.SetMissingRequestBodyRequiredValueAccessor(() => ModelBindingMessage.MissingRequestBodyRequiredValue);
            provider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => string.Format(ModelBindingMessage.NonPropertyAttemptedValueIsInvalid, x));
            provider.SetNonPropertyUnknownValueIsInvalidAccessor(() => ModelBindingMessage.NonPropertyUnknownValueIsInvalid);
            provider.SetNonPropertyValueMustBeANumberAccessor(() => ModelBindingMessage.NonPropertyValueMustBeANumber);
            provider.SetUnknownValueIsInvalidAccessor(x => string.Format(ModelBindingMessage.UnknownValueIsInvalid, x));
            provider.SetValueIsInvalidAccessor(x => string.Format(ModelBindingMessage.ValueIsInvalid, x));
            provider.SetValueMustBeANumberAccessor(x => string.Format(ModelBindingMessage.NonPropertyValueMustBeANumber, x));
            provider.SetValueMustNotBeNullAccessor(x => string.Format(ModelBindingMessage.ValueMustNotBeNull, x));

            // 從資源檔設定 ValidationMetadata 的錯誤訊息
            options.ModelMetadataDetailsProviders.Add(new LocalizationValidationMetadataProvider(typeof(ValidationMetadataMessage)));
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // Customize the response when the model state is invalid.
            options.InvalidModelStateResponseFactory = context => BadRequestExceptionHandler.TryHandler(context);
        });

    // Error Handler
    // https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8?utm_source=YouTube&utm_medium=social&utm_campaign=25.03.2024#configuring-iexceptionhandler-implementations
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    // MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    // Serilog
    builder.Services.AddSerilog();

    // NSwag
    builder.Services.AddNSwag(env);

    // HttpContextAccessor
    builder.Services.AddHttpContextAccessor();

    // EF Core
    builder.Services.AddEFCore(config, env);

    // DI
    builder.Services.AddDependencyInjection();

    // Add HttpClient
    builder.Services.AddHttpClient();

    // HealthCheck
    builder.Services.AddHealthCheck(config);

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    // NSwag
    // serve OpenAPI/Swagger documents
    app.UseOpenApi();
    // serve Swagger UI
    app.UseSwaggerUi();
    // serve ReDoc UI
    app.UseReDoc((config) => config.Path = "/redoc");

    // Log request and response
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = SerilogConfig.EnrichFromRequest);

    // Error Handler
    app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapHealthChecks("/healthz", new HealthCheckOptions { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })
        .ShortCircuit();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// 增加 Program 類別公開宣告 => 整合測試使用
public partial class Program { }
