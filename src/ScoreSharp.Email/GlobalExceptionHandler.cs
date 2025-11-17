using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace ScoreSharp.Email;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var title = exception.Message;
        var details = exception.ToString();

        logger.LogError(exception, "Unhandled exception");

        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Status = StatusCodes.Status500InternalServerError,
            Title = title,
            Detail = details,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
        };

        problemDetails.Extensions.TryAdd("requestId", httpContext.TraceIdentifier);
        problemDetails.Extensions.TryAdd("traceId", Activity.Current?.Id);

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
