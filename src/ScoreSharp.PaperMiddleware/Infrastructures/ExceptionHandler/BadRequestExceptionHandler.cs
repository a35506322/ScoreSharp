namespace ScoreSharp.PaperMiddleware.Infrastructures.ExceptionHandler;

public class BadRequestExceptionHandler
{
    public static BadRequestObjectResult TryHandler(ActionContext context)
    {
        var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
        var diagnosticContext = context.HttpContext.RequestServices.GetRequiredService<IDiagnosticContext>();
        diagnosticContext.Set("X-Trace-Id", traceId);
        var errors = context.ModelState.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
        return new BadRequestObjectResult(ApiResponseHelper.BadRequest(errors, traceId));
    }
}
