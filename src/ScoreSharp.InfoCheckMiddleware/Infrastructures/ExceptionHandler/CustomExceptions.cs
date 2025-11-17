namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.ExceptionHandler;

public abstract class BusinessException : Exception
{
    public ReturnCodeStatus ReturnCodeStatus { get; }
    public int HttpStatusCode { get; }

    protected BusinessException(ReturnCodeStatus returnCodeStatus, string message, int httpStatusCode = StatusCodes.Status400BadRequest)
        : base(message)
    {
        ReturnCodeStatus = returnCodeStatus;
        HttpStatusCode = httpStatusCode;
    }

    protected BusinessException(
        ReturnCodeStatus returnCodeStatus,
        string message,
        Exception innerException,
        int httpStatusCode = StatusCodes.Status400BadRequest
    )
        : base(message, innerException)
    {
        ReturnCodeStatus = returnCodeStatus;
        HttpStatusCode = httpStatusCode;
    }
}

public class BadRequestException : BusinessException
{
    public Dictionary<string, string[]> ValidationErrors { get; }

    public BadRequestException(Dictionary<string, string[]> errors)
        : base(ReturnCodeStatus.格式驗證失敗, "格式驗證失敗", StatusCodes.Status400BadRequest)
    {
        ValidationErrors = errors;
    }
}

public class BusinessBadRequestException : BusinessException
{
    public BusinessBadRequestException(string message)
        : base(ReturnCodeStatus.商業邏輯有誤, message, StatusCodes.Status400BadRequest) { }
}

public class NotFoundException : BusinessException
{
    public NotFoundException(string id)
        : base(ReturnCodeStatus.查無此資料, $"查無此資料: {id}", StatusCodes.Status404NotFound) { }
}

public class DatabaseDefinitionException : BusinessException
{
    public Dictionary<string, string[]> ValidationErrors { get; }

    public DatabaseDefinitionException(Dictionary<string, string[]> errors)
        : base(ReturnCodeStatus.資料庫定義值錯誤, "資料庫定義值錯誤", StatusCodes.Status400BadRequest)
    {
        ValidationErrors = errors;
    }
}

public class InternalServerException : BusinessException
{
    public InternalServerException(string message)
        : base(ReturnCodeStatus.內部程式失敗, message, StatusCodes.Status500InternalServerError) { }

    public InternalServerException(string message, Exception innerException)
        : base(ReturnCodeStatus.內部程式失敗, message, innerException, StatusCodes.Status500InternalServerError) { }
}

public class DatabaseExecuteException : BusinessException
{
    public DatabaseExecuteException(string message, Exception innerException)
        : base(ReturnCodeStatus.資料庫執行失敗, message, innerException, StatusCodes.Status500InternalServerError) { }
}

public class ExternalServiceException : BusinessException
{
    public string ServiceName { get; }

    public ExternalServiceException(string serviceName, string message)
        : base(ReturnCodeStatus.外部服務錯誤, $"外部服務 '{serviceName}' 錯誤: {message}", StatusCodes.Status502BadGateway)
    {
        ServiceName = serviceName;
    }

    public ExternalServiceException(string serviceName, string message, Exception innerException)
        : base(ReturnCodeStatus.外部服務錯誤, $"外部服務 '{serviceName}' 錯誤: {message}", innerException, StatusCodes.Status502BadGateway)
    {
        ServiceName = serviceName;
    }
}

public class HeaderValidationException : BusinessException
{
    public HeaderValidationException(string message)
        : base(ReturnCodeStatus.標頭驗證失敗, message, StatusCodes.Status401Unauthorized) { }
}
