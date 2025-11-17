var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

// 註冊 Serillog
SerilogConfig.AddSerilLog(config, env);

try
{
    // Add services to the container.
    builder.Services.AddControllers();

    // Dependency Injection
    builder.Services.AddDependencyInjection();

    // Hangfire
    builder.Services.ConfigureHangfire(config);

    // Serilog
    builder.Services.AddSerilog();

    // Nswag
    builder.Services.AddNSwag(env);

    // EFCore
    builder.Services.AddEFCore(config, env);

    // Adapter
    builder.Services.AddAdapters(config);

    // AutoMapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Options
    builder.Services.AddOptionsConfig(config);

    // RazorTemplate
    builder.Services.AddRazorTemplate();

    // Email
    builder.Services.AddEmail(config);

    // HealthCheck
    builder.Services.AddHealthCheck(config);

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseOpenApi();
    // serve Swagger UI
    app.UseSwaggerUi();

    app.UseAuthorization();

    app.UseHangfireDashboard();
    app.SetupHangfire(config, env);

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
