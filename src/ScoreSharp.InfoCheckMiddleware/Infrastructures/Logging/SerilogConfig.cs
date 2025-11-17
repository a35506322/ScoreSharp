using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.MsSqlServer.Destructurers;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.Logging;

public static class SerilogConfig
{
    public static void AddSerilLog(IConfiguration configuration, IWebHostEnvironment environment)
    {
        // 全域設定
        /*  🔔new CompactJsonFormatter()
         *  由於 Log 的欄位很多，使用 Console Sink 會比較看不出來，改用 Serilog.Formatting.Compact 來記錄 JSON 格式的 Log 訊息會清楚很多！
         */
        Console.OutputEncoding = Encoding.UTF8;

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information() // 設定最小Log輸出
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // 設定 Microsoft.AspNetCore 訊息為 Warning 為最小輸出
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning) // 設定 Microsoft.EntityFrameworkCore 訊息為 Warning 為最小輸出
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
            .Enrich.FromLogContext() // 可以增加Log輸出欄位 https://www.cnblogs.com/wd4j/p/15043489.html
            //.Enrich.WithCustomHttpContext() // 客製化追蹤屬性
            .Enrich.WithProperty("Application", "ScoreSharp.InfoCheckMiddleware")
            .Enrich.WithExceptionDetails(
                new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers(new[] { new SqlExceptionDestructurer() })
                    .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
            )
            .WriteTo.Map( // 寫入txt => 按照 level
                evt => evt.Level,
                (level, wt) =>
                    wt.File(
                        new CompactJsonFormatter(),
                        path: string.Format(configuration.GetValue<string>("SerilLogConfig:LogPath"), level),
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        rollOnFileSizeLimit: true,
                        shared: true,
                        rollingInterval: RollingInterval.Day
                    )
            ) // 寫入Seq
            .WriteTo.Seq(configuration.GetValue<string>("SerilLogConfig:SeqUrl"), eventBodyLimitBytes: 10485760) // 寫入Seq
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:yyyy/MM/dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{RequestBody}{ResponseBody}{NewLine}{Exception}"
            );

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    public static async void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        var request = httpContext.Request;
        var requestBody = httpContext.Items["RequestBody"]?.ToString() ?? string.Empty;
        diagnosticContext.Set("RequestBody", requestBody);

        string responseBodyPayload = await ReadResponseBody(httpContext.Response);
        diagnosticContext.Set("ResponseBody", responseBodyPayload);

        // Set all the common properties available for every request
        diagnosticContext.Set("Host", request.Host);
        diagnosticContext.Set("Scheme", request.Scheme);
        diagnosticContext.Set("Headers", request.Headers);

        string ip = request.Headers["X-Forwarded-For"].FirstOrDefault() ?? httpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        diagnosticContext.Set("RemoteIp", ip);

        // Only set it if available. You're not sending sensitive data in a querystring right?!
        if (request.QueryString.HasValue)
        {
            diagnosticContext.Set("QueryString", request.QueryString.Value);
        }

        // Set the content-type of the Response at this point
        diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

        // Retrieve the IEndpointFeature selected for the request
        var endpoint = httpContext.GetEndpoint();
        if (endpoint is object) // endpoint != null
        {
            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
        }
    }

    private static async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return $"{responseBody}";
    }
}
