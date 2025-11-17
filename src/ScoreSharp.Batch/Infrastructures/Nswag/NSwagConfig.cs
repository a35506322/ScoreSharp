using NJsonSchema.Generation;

namespace ScoreSharp.Batch.Infrastructures.Nswag;

public static class NSwagConfig
{
    public static void AddNSwag(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddOpenApiDocument(
            (settings) =>
            {
                settings.Title = $"信用卡徵審系統 - 排程 ({env.EnvironmentName})";
                settings.Version = "v1";
                settings.Description = @"";

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

        if (context.Type.IsEnum)
        {
            var type = context.Type;

            List<string> enumDescriptions = schema
                .Enumeration.Select(enumValue => $"{Enum.GetName(type, enumValue)} = {enumValue}")
                .ToList();

            schema.Description += $"<br><div>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}</div>";
        }
    }
}
