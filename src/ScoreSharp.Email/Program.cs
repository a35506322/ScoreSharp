using FluentEmail.Core.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 全域設定
/*  🔔new CompactJsonFormatter()
 *  由於 Log 的欄位很多，使用 Console Sink 會比較看不出來，改用 Serilog.Formatting.Compact 來記錄 JSON 格式的 Log 訊息會清楚很多！
 */
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // 設定最小Log輸出
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // 設定 Microsoft.AspNetCore 訊息為 Warning 為最小輸出
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning) // 設定 Microsoft.EntityFrameworkCore 訊息為 Warning 為最小輸出
    .Enrich.FromLogContext() // 可以增加Log輸出欄位 https://www.cnblogs.com/wd4j/p/15043489.html
    .Enrich.WithProperty("ApplicationName", "ScoreSharp.Email")
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
    )
    .WriteTo.Seq(configuration.GetValue<string>("SerilLogConfig:SeqUrl"))
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger(); // 寫入Seq

try
{
    Log.Information("Starting ScoreSharp.Email");

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder
        .Services.AddFluentEmail(configuration.GetValue<string>("Email:Sender:Email"))
        .AddSmtpSender(configuration.GetValue<string>("Email:Smtp:Host"), configuration.GetValue<int>("Email:Smtp:Port"));

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler();
    app.UseSerilogRequestLogging();

    app.MapPost(
        "/sendEmail",
        async ([FromServices] IFluentEmail email, [FromBody] EmailRequest request) =>
        {
            var to = request.To.Select(x => new Address(x.Address, x.Name)).ToArray();
            var result = await email.To(to).Subject(request.Subject).Body(request.Body, request.IsHtml).SendAsync();
            return Results.Ok(result);
        }
    );

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

public class EmailRequest
{
    public List<EmailAddressDto> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; } = false;
}

public class EmailAddressDto
{
    public string Name { get; set; }
    public string Address { get; set; }
}
