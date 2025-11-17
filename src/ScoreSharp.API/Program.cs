var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

// 註冊 Serillog
SerilogConfig.AddSerilLog(config, env);

// External trace sources will be enabled until the returned handle is disposed.
// 偵測效率，以後有需要再打開
// using var _ = new ActivityListenerConfiguration()
//     .Instrument.AspNetCoreRequests(opts =>
//     {
//         opts.IncomingTraceParent = IncomingTraceParent.Trust;
//         opts.PostSamplingFilter = httpContext => !httpContext.Request.Path.StartsWithSegments("/healthz");
//     })
//     .Instrument.SqlClientCommands(opts =>
//     {
//         opts.IncludeCommandText = true;
//     })
//     .InitialLevel.Override("ZiggyCreatures.Caching.Fusion", LogEventLevel.Debug)
//     .TraceToSharedLogger();

try
{
    Log.Information($"Starting web host ({env.EnvironmentName})");

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

    builder.Services.AddSerilog();

    // Error Handler
    // https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8?utm_source=YouTube&utm_medium=social&utm_campaign=25.03.2024#configuring-iexceptionhandler-implementations
    builder.Services.AddExceptionHandler<InternalServerExceptionHandler>();
    builder.Services.AddProblemDetails();

    // JWT
    builder.Services.AddJWT(config);

    // DI
    builder.Services.AddDependencyInjection();

    // EF Core
    builder.Services.AddEFCore(config, env);

    // Adapter
    builder.Services.AddAdapters(config);

    //MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    // NSwag
    builder.Services.AddNSwag(env);

    // AutoMapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // HttpContextAccessor
    builder.Services.AddHttpContextAccessor();

    //  Policy
    builder.Services.AddPolicy();

    // Redis
    builder.Services.AddRedis(config, env);

    // Cors
    builder.Services.ConfigureCors(env);

    // appSetting Options
    builder.Services.AddOptionsConfig(config);

    // HealthCheck
    builder.Services.AddHealthCheck(config);

    // Razor Template
    builder.Services.AddRazorTemplate();

    var app = builder.Build();

    // NSwag
    // serve OpenAPI/Swagger documents
    app.UseOpenApi(x =>
    {
        x.CreateDocumentCacheKey = request => request.GetServerUrl();
        x.PostProcess = (doc, request) =>
        {
            // fix https://github.com/RicoSuter/NSwag/issues/3645
            foreach (
                var parameter in doc
                    .Operations.SelectMany(x => x.Operation.Parameters)
                    .Where(x =>
                        x.Schema.OneOf.Count == 1 && x.Schema.OneOf.Single().Reference != null && x.Schema.OneOf.Single().Reference.IsEnumeration
                    )
            )
            {
                if (parameter.Kind == OpenApiParameterKind.Query)
                {
                    parameter.Schema.AllOf.Add(parameter.Schema.OneOf.Single());
                    parameter.Schema.OneOf.Clear();
                    parameter.Description += parameter.ActualSchema.Description;
                }
            }

            // fix: https://github.com/RicoSuter/NSwag/issues/2441
            // 調整 Servers URL 因代理關係，需要配合
            var serverUrl = request.GetServerUrl();
            doc.Servers.Clear();
            doc.Servers.Add(new OpenApiServer { Url = serverUrl });
        };
    });

    // serve Swagger UI
    app.UseSwaggerUi(x =>
    {
        x.TransformToExternalPath = (url, request) =>
        {
            /*
                fix: https://github.com/RicoSuter/NSwag/issues/1914
                修正當前端(172.28.251.78) => 轉發透過 ARR 需要 URL 問題
                TransformToExternalPath 不修正他會使用 request.PathBase 導致無法正確轉發
                目前測試區需要使用 /scoresharp_yarp/api
            */
            if (request.Headers.ContainsKey("X-Forwarded-Prefix"))
            {
                var prefix = request.Headers["X-Forwarded-Prefix"];
                return $"{prefix}{url}";
            }
            return request.PathBase + url;
        };
    });
    // serve ReDoc UI
    app.UseReDoc((config) => config.Path = "/redoc");

    // Log request and response
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = SerilogConfig.EnrichFromRequest);

    // Error Handler
    app.UseExceptionHandler();

    app.UseCors();

    // 先驗證再授權
    app.UseAuthentication();
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
