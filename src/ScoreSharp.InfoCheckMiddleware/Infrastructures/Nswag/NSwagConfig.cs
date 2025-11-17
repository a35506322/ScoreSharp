using NJsonSchema.Generation;

namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.Nswag;

public static class NSwagConfig
{
    public static void AddNSwag(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddExampleProviders(AppDomain.CurrentDomain.GetAssemblies());
        services.AddOpenApiDocument(
            (settings, provider) =>
            {
                settings.Title = $"信用卡徵審系統 API InfoCheckMiddleware ({env.EnvironmentName})";
                settings.Version = "v1";
                settings.Description = @"ScoreSharp InfoCheckMiddleware API";

                //// 這個 OpenApiSecurityScheme 物件請勿加上 Name 與 In 屬性，否則產生出來的 OpenAPI Spec 格式會有錯誤！
                //var apiScheme = new OpenApiSecurityScheme()
                //{
                //    Type = OpenApiSecuritySchemeType.Http,
                //    Scheme = JwtBearerDefaults.AuthenticationScheme,
                //    BearerFormat = "JWT", // for documentation purposes (OpenAPI only)
                //    Description = "Copy JWT Token into the value field: {token}"
                //};

                //// 這裡會同時註冊 SecurityDefinition (.components.securitySchemes) 與 SecurityRequirement (.security)
                //// settings.AddSecurity("Bearer", Enumerable.Empty<string>(), apiScheme)
                //settings.AddSecurity("Bearer", apiScheme);
                //// 這段是為了將 "Bearer" 加入到 OpenAPI Spec 裡 Operator 的 security (Security requirements) 中
                //settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());

                settings.AddExamples(provider);

                // 新增 Enum 註解
                settings.SchemaSettings.SchemaProcessors.Add(new EnumTypeDocumentProcessor());

                // 調整 Servers
                settings.PostProcess = (document) =>
                {
                    // 清空預設的服務器設定
                    document.Servers.Clear();

                    // 從 HttpContext 獲取當前的 host
                    var httpContext = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>().HttpContext;

                    if (httpContext != null)
                    {
                        var request = httpContext.Request;
                        var host = request.Host.Value;
                        var scheme = request.Scheme;

                        document.Servers.Add(new OpenApiServer { Url = $"{scheme}://{host}", Description = $"當前環境: {env.EnvironmentName}" });
                    }
                };
            }
        );
    }
}

public class EnumTypeDocumentProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        var schema = context.Schema;

        if (context.Type.IsEnum)
        {
            var type = context.Type;

            List<string> enumDescriptions = schema.Enumeration.Select(enumValue => $"{Enum.GetName(type, enumValue)} = {enumValue}").ToList();

            schema.Description += $"<br><div>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}</div>";
        }
    }
}
