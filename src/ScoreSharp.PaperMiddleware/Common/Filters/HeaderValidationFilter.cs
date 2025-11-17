using Microsoft.AspNetCore.Mvc.Filters;

namespace ScoreSharp.PaperMiddleware.Common.Filters;

public class HeaderValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var headers = context.HttpContext.Request.Headers;
        var errors = new Dictionary<string, string[]>();

        string applyNo = null;
        if (!headers.TryGetValue("X-APPLYNO", out var applyNoValues) || string.IsNullOrWhiteSpace(applyNoValues.FirstOrDefault()))
        {
            errors.Add("X-APPLYNO", new[] { "X-APPLYNO 標頭為必填欄位" });
        }
        else
        {
            applyNo = applyNoValues.FirstOrDefault()!.Trim();
        }

        string syncUserId = null;
        if (!headers.TryGetValue("X-SYNCUSERID", out var syncUserIdValues) || string.IsNullOrWhiteSpace(syncUserIdValues.FirstOrDefault()))
        {
            errors.Add("X-SYNCUSERID", new[] { "X-SYNCUSERID 標頭為必填欄位" });
        }
        else
        {
            syncUserId = syncUserIdValues.FirstOrDefault()!.Trim();
        }

        if (errors.Any())
        {
            throw new HeaderValidationException(string.Join(", ", errors.SelectMany(x => x.Value)));
        }

        context.HttpContext.Items["ApplyNo"] = applyNo;
        context.HttpContext.Items["SyncUserId"] = syncUserId;

        await next();
    }
}
