namespace ScoreSharp.PaperMiddleware.Common.Helpers;

/// <summary>
/// 通用的返回Response
/// </summary>
/// <typeparam name="T">資料回傳類型</typeparam>
/// <param name="ReturnCodeStatus">狀態碼</param>
/// <param name="ReturnMessage">回應訊息</param>
/// <param name="ErrorDetail">錯誤詳細訊息</param>
/// <param name="SuccessData">資料或者錯誤</param>
/// <param name="ValidationErrors">驗證資料錯誤訊息</param>
/// <param name="ErrorMessage">錯誤訊息</param>
/// <param name="TraceId">追蹤ID</param>
///
public record ResultResponse<T>(
    ReturnCodeStatus ReturnCodeStatus = ReturnCodeStatus.成功,
    string ReturnMessage = "",
    T SuccessData = default,
    object? ErrorDetail = null,
    Dictionary<string, string[]> ValidationErrors = null,
    string ErrorMessage = "",
    string? TraceId = null
);

/// <summary>
/// 通用的返回Response
/// </summary>
/// <param name="ReturnCodeStatus">狀態碼</param>
/// <param name="ReturnMessage">回應訊息</param>
/// <param name="SuccessData">資料或者錯誤</param>
/// <param name="ErrorDetail">錯誤詳細訊息</param>
/// <param name="ValidationErrors">驗證資料錯誤訊息</param>
/// <param name="ErrorMessage">錯誤訊息</param>
/// <param name="TraceId">追蹤ID</param>
public record ResultResponse(
    ReturnCodeStatus ReturnCodeStatus = ReturnCodeStatus.成功,
    string ReturnMessage = "",
    object? SuccessData = null,
    object? ErrorDetail = null,
    Dictionary<string, string[]>? ValidationErrors = null,
    string ErrorMessage = "",
    string? TraceId = null
);

/// <summary>
/// 狀態碼枚舉
/// </summary>
public enum ReturnCodeStatus
{
    成功 = 2000,
    格式驗證失敗 = 4000,
    資料庫定義值錯誤 = 4001,
    查無此資料 = 4002,
    商業邏輯有誤 = 4003,
    標頭驗證失敗 = 4004,
    內部程式失敗 = 5000,
    外部服務錯誤 = 5001,
    資料庫執行失敗 = 5002,
}

/// <summary>
/// API 回應輔助類
/// </summary>
public static class ApiResponseHelper
{
    /// <summary>
    /// [2000]成功的回應
    /// </summary>
    /// <typeparam name="T">資料回傳類型</typeparam>
    /// <param name="data">回應資料</param>
    /// <param name="message">回應訊息</param>
    /// <returns>成功的回應對象</returns>
    public static ResultResponse<T> Success<T>(T data, string traceId = "", string message = "") =>
        new ResultResponse<T>(
            ReturnCodeStatus: ReturnCodeStatus.成功,
            ReturnMessage: message,
            SuccessData: data,
            ErrorDetail: null,
            ValidationErrors: null,
            ErrorMessage: "",
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [5000]內部服務器錯誤的回應
    /// </summary>
    /// <param name="message">錯誤訊息</param>
    /// <returns>內部服務器錯誤的回應對象</returns>
    public static ResultResponse InternalServerError(string message, string traceId = "", string errorDetail = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.內部程式失敗,
            ReturnMessage: ReturnCodeStatus.內部程式失敗.ToString(),
            SuccessData: null,
            ErrorDetail: errorDetail,
            ValidationErrors: null,
            ErrorMessage: message,
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [4000]格式驗證失敗
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static ResultResponse BadRequest(Dictionary<string, string[]> errors, string traceId = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.格式驗證失敗,
            ReturnMessage: ReturnCodeStatus.格式驗證失敗.ToString(),
            SuccessData: null,
            ErrorDetail: null,
            ValidationErrors: errors,
            ErrorMessage: "",
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [4001]資料庫定義值錯誤
    /// </summary>
    /// <typeparam name="T">資料回傳類型</typeparam>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static ResultResponse DatabaseDefinitionValueError(Dictionary<string, string[]> errors, string traceId = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.資料庫定義值錯誤,
            ReturnMessage: ReturnCodeStatus.資料庫定義值錯誤.ToString(),
            SuccessData: default,
            ErrorDetail: null,
            ValidationErrors: errors,
            ErrorMessage: "",
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [4002]查無此資料
    /// </summary>
    /// <typeparam name="T">資料回傳類型</typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultResponse NotFound(string message, string traceId = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.查無此資料,
            ReturnMessage: ReturnCodeStatus.查無此資料.ToString(),
            SuccessData: default,
            ErrorDetail: null,
            ValidationErrors: null,
            ErrorMessage: message,
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [5001]外部服務錯誤
    /// </summary>
    /// <param name="message">錯誤訊息</param>
    /// <returns>外部服務錯誤的回應對象</returns>
    public static ResultResponse ExternalServiceError(string message, string traceId = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.外部服務錯誤,
            ReturnMessage: ReturnCodeStatus.外部服務錯誤.ToString(),
            SuccessData: null,
            ErrorDetail: null,
            ValidationErrors: null,
            ErrorMessage: message,
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [4003]商業邏輯有誤
    /// </summary>
    /// <param name="message">錯誤訊息</param>
    /// <returns>商業邏輯有誤的回應對象</returns>
    public static ResultResponse BusinessBadRequestError(string message, string traceId = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.商業邏輯有誤,
            ReturnMessage: ReturnCodeStatus.商業邏輯有誤.ToString(),
            SuccessData: null,
            ErrorDetail: null,
            ValidationErrors: null,
            ErrorMessage: message,
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [5002]資料庫執行失敗
    /// </summary>
    /// <param name="message">錯誤訊息</param>
    /// <param name="errorDetail">錯誤詳細訊息</param>
    /// <returns>資料庫執行失敗的回應對象</returns>
    public static ResultResponse DatabaseExecuteError(string message, string traceId = "", string errorDetail = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.資料庫執行失敗,
            ReturnMessage: ReturnCodeStatus.資料庫執行失敗.ToString(),
            SuccessData: null,
            ErrorDetail: errorDetail,
            ValidationErrors: null,
            ErrorMessage: message,
            TraceId: Activity.Current?.Id ?? traceId
        );

    /// <summary>
    /// [4004]標頭驗證失敗
    /// </summary>
    /// <param name="message">錯誤訊息</param>
    /// <returns>標頭驗證失敗的回應對象</returns>
    public static ResultResponse HeaderValidationError(string message, string traceId = "") =>
        new ResultResponse(
            ReturnCodeStatus: ReturnCodeStatus.標頭驗證失敗,
            ReturnMessage: ReturnCodeStatus.標頭驗證失敗.ToString(),
            SuccessData: null,
            ErrorDetail: null,
            ValidationErrors: null,
            ErrorMessage: message,
            TraceId: Activity.Current?.Id ?? traceId
        );
}
