using ScoreSharp.LDAP;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment.EnvironmentName;

// 全域設定
/*  🔔new CompactJsonFormatter()
 *  由於 Log 的欄位很多，使用 Console Sink 會比較看不出來，改用 Serilog.Formatting.Compact 來記錄 JSON 格式的 Log 訊息會清楚很多！
 */
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // 設定最小Log輸出
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // 設定 Microsoft.AspNetCore 訊息為 Warning 為最小輸出
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning) // 設定 Microsoft.EntityFrameworkCore 訊息為 Warning 為最小輸出
    .Enrich.FromLogContext() // 可以增加Log輸出欄位 https://www.cnblogs.com/wd4j/p/15043489.html
    .Enrich.WithProperty("ApplicationName", "ScoreSharp.LDAP")
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
    // options add
    builder
        .Services.AddOptions<LDAPOptions>()
        .Bind(builder.Configuration.GetSection("LDAPOptionConfig"))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    builder
        .Services.AddOptions<IPWhitelistOptions>()
        .Bind(builder.Configuration.GetSection("IPWhitelist"))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    // DI 注入
    builder.Services.AddScoped<ILDAPService, LDAPService>();
    builder.Services.AddScoped<IUITCSecurityHelper, UITCSecurityHelper>();

    // 添加 Swagger 服務
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = $"LDAP API ({environment})",
                Version = "v1",
                Description = "LDAP 服務 API",
            }
        );

        // 包含 XML 文件，用於生成 Swagger UI
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddSerilog();

    var app = builder.Build();

    app.UseMiddleware<IPWhitelistMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    // 驗證 LDAP 帳號
    app.MapPost(
            "/ldap/auth",
            async ([FromBody] LDAPAuthRequest request, [FromServices] ILDAPService ldapService) =>
            {
                var result = await ldapService.ValidateLDAPAuth(request.Username, request.Password);
                return Results.Ok(result);
            }
        )
        .Produces<bool>(StatusCodes.Status200OK)
        .WithName("ValidateLDAPAuth")
        .WithOpenApi(operation =>
        {
            operation.Summary = "驗證 LDAP 帳號";
            operation.Description = "驗證使用者的 LDAP 帳號密碼是否正確";
            return operation;
        });

    // 搜尋所有使用者
    app.MapGet("/ldap/users", async ([FromServices] ILDAPService ldapService) => Results.Ok(await ldapService.SearchUsersAll()))
        .Produces<IEnumerable<LDAPUserInfo>>(StatusCodes.Status200OK)
        .WithName("GetAllUsers")
        .WithOpenApi(operation =>
        {
            operation.Summary = "取得所有使用者";
            operation.Description = "搜尋並返回所有 LDAP 使用者資訊";
            return operation;
        });

    // 搜尋特定使用者
    app.MapGet(
            "/ldap/users/{samAccountName}",
            async ([FromRoute] string samAccountName, [FromServices] ILDAPService ldapService) =>
            {
                var user = await ldapService.SearchBySAMAccountName(samAccountName);
                if (user == null)
                    return Results.NotFound();
                return Results.Ok(user);
            }
        )
        .Produces<LDAPUserInfo>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetUserBySAMAccountName")
        .WithOpenApi(operation =>
        {
            operation.Summary = "搜尋特定使用者";
            operation.Description = "根據 SAM Account Name 搜尋特定使用者資訊";
            return operation;
        });

    // 取得 使用者 IPV4
    app.MapGet(
            "/ipv4",
            ([FromServices] IHttpContextAccessor httpContextAccessor) =>
                Results.Ok(httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString())
        )
        .Produces<string>(StatusCodes.Status200OK)
        .WithName("GetUserIPv4")
        .WithOpenApi(operation =>
        {
            operation.Summary = "取得使用者 IPV4";
            return operation;
        });

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

/// <summary>
/// LDAP 帳號驗證請求
/// </summary>
/// <param name="Username">AD 帳號</param>
/// <param name="Password">AD 密碼</param>
/// <returns></returns>
record LDAPAuthRequest(
    [property: Required] [Display(Name = "AD 帳號")] string Username,
    [property: Required] [Display(Name = "AD 密碼")] string Password
);
