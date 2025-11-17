namespace ScoreSharp.PaperMiddleware.Infrastructures.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment, IServiceProvider serviceProvider)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        ResultResponse errorResponse = null;
        if (exception is BadRequestException badRequestEx)
        {
            httpContext.Response.StatusCode = badRequestEx.HttpStatusCode;
            errorResponse = ApiResponseHelper.BadRequest(errors: badRequestEx.ValidationErrors, traceId: traceId);
        }
        else if (exception is NotFoundException notFoundEx)
        {
            httpContext.Response.StatusCode = notFoundEx.HttpStatusCode;
            errorResponse = ApiResponseHelper.NotFound(message: notFoundEx.Message, traceId: traceId);
        }
        else if (exception is DatabaseDefinitionException databaseDefinitionException)
        {
            httpContext.Response.StatusCode = databaseDefinitionException.HttpStatusCode;
            errorResponse = ApiResponseHelper.DatabaseDefinitionValueError(errors: databaseDefinitionException.ValidationErrors, traceId: traceId);
        }
        else if (exception is InternalServerException internalServerException)
        {
            httpContext.Response.StatusCode = internalServerException.HttpStatusCode;
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            string errorDetail = environment.IsDevelopment() ? exception.ToString() : "";
            errorResponse = ApiResponseHelper.InternalServerError(
                message: internalServerException.Message,
                traceId: traceId,
                errorDetail: errorDetail
            );
        }
        else if (exception is ExternalServiceException externalServiceException)
        {
            httpContext.Response.StatusCode = externalServiceException.HttpStatusCode;
            errorResponse = ApiResponseHelper.ExternalServiceError(message: externalServiceException.Message, traceId: traceId);
        }
        else if (exception is DatabaseExecuteException databaseExecuteException)
        {
            httpContext.Response.StatusCode = databaseExecuteException.HttpStatusCode;
            string errorDetail = environment.IsDevelopment() ? databaseExecuteException.ToString() : "";
            errorResponse = ApiResponseHelper.DatabaseExecuteError(
                message: databaseExecuteException.Message,
                traceId: traceId,
                errorDetail: errorDetail
            );
        }
        else if (exception is BusinessBadRequestException businessBadRequestException)
        {
            httpContext.Response.StatusCode = businessBadRequestException.HttpStatusCode;
            errorResponse = ApiResponseHelper.BusinessBadRequestError(message: businessBadRequestException.Message, traceId: traceId);
        }
        else if (exception is HeaderValidationException headerValidationException)
        {
            httpContext.Response.StatusCode = headerValidationException.HttpStatusCode;
            errorResponse = ApiResponseHelper.HeaderValidationError(message: headerValidationException.Message, traceId: traceId);
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            string errorDetail = environment.IsDevelopment() ? exception.ToString() : "";
            errorResponse = ApiResponseHelper.InternalServerError(message: exception.Message, traceId: traceId, errorDetail: errorDetail);
        }

        // 記 System_ErrorLog
        await AddSystemErrorLog(
            httpContext: httpContext,
            errorType: errorResponse.ReturnCodeStatus.ToString(),
            errorResponse: errorResponse,
            errorDetail: exception.ToString(),
            traceId: traceId
        );

        logger.LogError(
            "StatusCode: {StatusCode} ReturnCodeStatus: {ReturnCodeStatus} ErrorType: {ErrorType} ReturnMessage: {ReturnMessage} ValidationErrors: {ValidationErrors} ErrorDetail: {ErrorDetail}",
            (int)httpContext.Response.StatusCode,
            (int)errorResponse.ReturnCodeStatus,
            errorResponse.ReturnCodeStatus.ToString(),
            errorResponse.ReturnMessage,
            string.Join(",", errorResponse.ValidationErrors?.SelectMany(x => x.Value).ToList() ?? new List<string>()),
            exception.ToString()
        );

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }

    private async Task AddSystemErrorLog(
        HttpContext httpContext,
        string errorType,
        ResultResponse errorResponse,
        string? errorDetail,
        string? traceId
    )
    {
        using var scope = serviceProvider.CreateScope();
        var scoreSharpContext = scope.ServiceProvider.GetRequiredService<ScoreSharpContext>();

        var applyNo = httpContext.Request.Headers["X-APPLYNO"].FirstOrDefault() ?? "";
        var syncUserId = httpContext.Request.Headers["X-SYNCUSERID"].FirstOrDefault() ?? "";
        var path = httpContext.Request.Path;
        var requestBody = httpContext.Items["RequestBody"]?.ToString() ?? string.Empty;

        var errorLog = new System_ErrorLog
        {
            ApplyNo = applyNo,
            Type = errorType,
            Project = SystemErrorLogProjectConst.PAPERMIDDLEWARE,
            Source = path,
            SendStatus = SendStatus.等待,
            AddTime = DateTime.Now,
            Request = requestBody,
            ErrorMessage = string.Empty,
            ErrorDetail = errorDetail,
            Response = JsonHelper.序列化物件(errorResponse),
            Note = JsonHelper.序列化物件(
                new
                {
                    ApplyNo = applyNo,
                    SyncUserId = syncUserId,
                    TraceId = traceId,
                }
            ),
        };
        await scoreSharpContext.System_ErrorLog.AddAsync(errorLog);
        await scoreSharpContext.SaveChangesAsync();
    }
}
