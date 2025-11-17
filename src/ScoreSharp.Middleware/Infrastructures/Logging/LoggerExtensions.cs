using Serilog.Context;
using Serilog.Core.Enrichers;

namespace ScoreSharp.Middleware.Infrastructures.Logging;

public static class LoggerExtensions
{
    public static IDisposable PushProperties(this Microsoft.Extensions.Logging.ILogger logger, params (string key, object value)[] properties)
    {
        var enrichers = properties.Select(p => new PropertyEnricher(p.key, p.value)).ToArray();
        return LogContext.Push(enrichers);
    }
}
