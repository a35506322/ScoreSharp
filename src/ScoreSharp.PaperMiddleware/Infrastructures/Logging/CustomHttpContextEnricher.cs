using Serilog.Configuration;
using Serilog.Core;

namespace ScoreSharp.PaperMiddleware.Infrastructures.Logging;

public class CustomHttpContextEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomHttpContextEnricher()
        : this(new HttpContextAccessor()) { }

    public CustomHttpContextEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            return;

        var applyNo = httpContext.Request.Headers["X-APPLYNO"].FirstOrDefault() ?? "";
        var syncUserId = httpContext.Request.Headers["X-SYNCUSERID"].FirstOrDefault() ?? "";
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        // 從 HttpContext.Items 中提取屬性
        if (!String.IsNullOrEmpty(applyNo))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("X-ApplyNo", applyNo));
        }

        if (!String.IsNullOrEmpty(syncUserId))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("X-SyncUserId", syncUserId));
        }

        if (!String.IsNullOrEmpty(traceId))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("X-TraceId", traceId));
        }
    }
}

public static class HttpContextLoggerConfigurationExtensions
{
    public static LoggerConfiguration WithCustomHttpContext(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null)
            throw new ArgumentNullException(nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With<CustomHttpContextEnricher>();
    }
}
