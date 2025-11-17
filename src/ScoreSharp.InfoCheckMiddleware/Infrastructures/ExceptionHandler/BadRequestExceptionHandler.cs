namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.ExceptionHandler;

public class BadRequestExceptionHandler
{
    public static BadRequestObjectResult TryHandler(ActionContext context)
    {
        var errors = context.ModelState.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
        return new BadRequestObjectResult(ApiResponseHelper.BadRequest(errors));
    }
}
