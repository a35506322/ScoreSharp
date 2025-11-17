using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace ScoreSharp.Middleware.Infrastructures.ExceptionHandler;

public class InternalServerExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _env;

    public InternalServerExceptionHandler(IHostEnvironment env)
    {
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var title = exception.Message;
        var details = exception.ToString();

        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Status = StatusCodes.Status500InternalServerError,
            Title = title,
            Detail = details,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
        };

        problemDetails.Extensions.TryAdd("RequestId", httpContext.TraceIdentifier);
        problemDetails.Extensions.TryAdd("TraceId", Activity.Current?.Id);

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(ApiResponseHelper.InternalServerError(problemDetails), cancellationToken);

        return true;
    }
}
