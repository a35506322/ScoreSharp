using System.Text;
using Hangfire.Console.Extensions.Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ScoreSharp.Batch.Infrastructures.Logging;

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
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
            .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", LogEventLevel.Warning)
            .MinimumLevel.Override("Hangfire", LogEventLevel.Information)
            .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
            .MinimumLevel.Override("ScoreSharp.Common.Adapters.MW3", LogEventLevel.Information) // MW3Adapter 的 Log 設定為 Error 為最小輸出
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http.HttpClient.Default.LogicalHandler", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http.HttpClient.Default.ClientHandler", LogEventLevel.Warning)
            .Enrich.FromLogContext() // 可以增加Log輸出欄位 https://www.cnblogs.com/wd4j/p/15043489.html
            .Enrich.WithHangfireContext() // 紀錄 Hangfire的Context
            .Enrich.WithProperty("Application", "ScoreSharp.Batch")
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:yyyy/MM/dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{RequestBody}{ResponseBody}{NewLine}{Exception}"
            )
            .WriteTo.File("..//ScoreSharp.Logs//ScoreSharp.Batch/log.txt", rollingInterval: RollingInterval.Day, shared: true)
            .WriteTo.Hangfire(restrictedToMinimumLevel: LogEventLevel.Information); // 紀錄 Hangfire的Context

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}
