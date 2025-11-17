using NJsonSchema.Generation;

namespace ScoreSharp.API.Infrastructures.Nswag;

public static class NSwagConfig
{
    public static void AddNSwag(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddExampleProviders(AppDomain.CurrentDomain.GetAssemblies());
        services.AddOpenApiDocument(
            (settings, provider) =>
            {
                settings.Title = $"信用卡徵審系統 API ({env.EnvironmentName})";
                settings.Version = "v1";
                settings.Description =
                    @"資料庫規格書 - https://docs.google.com/document/d/1vCVwmLYmgMUE7VSo5PlaGgwN6QKYfXis2c-wwisqETA/edit <br />
                                    API規格書 - https://docs.google.com/document/d/17JcQYJYV2BFjQmiPPCHVolEaQWhkTjHEnFpGG6lsEcw/edit";

                // 這個 OpenApiSecurityScheme 物件請勿加上 Name 與 In 屬性，否則產生出來的 OpenAPI Spec 格式會有錯誤！
                var apiScheme = new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT", // for documentation purposes (OpenAPI only)
                    Description = "Copy JWT Token into the value field: {token}",
                };

                // 這裡會同時註冊 SecurityDefinition (.components.securitySchemes) 與 SecurityRequirement (.security)
                // settings.AddSecurity("Bearer", Enumerable.Empty<string>(), apiScheme)
                settings.AddSecurity("Bearer", apiScheme);
                // 這段是為了將 "Bearer" 加入到 OpenAPI Spec 裡 Operator 的 security (Security requirements) 中
                settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());

                settings.AddExamples(provider);

                // 新增 Enum 註解
                settings.SchemaSettings.SchemaProcessors.Add(new EnumTypeDocumentProcessor());
            }
        );
    }
}

public class EnumTypeDocumentProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        var schema = context.Schema;

        if (context.ContextualType.OriginalType.IsEnum)
        {
            var type = context.ContextualType.OriginalType;

            List<string> enumDescriptions = schema
                .Enumeration.Select(enumValue => $"{Enum.GetName(type, enumValue)} = {enumValue}")
                .ToList();

            schema.Description += $"<br><div>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}</div>";
        }
    }
}
