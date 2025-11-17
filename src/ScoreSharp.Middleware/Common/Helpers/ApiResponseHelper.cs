namespace ScoreSharp.Middleware.Common.Helpers;

/// <summary>
/// 通用的返回Response
/// </summary>
/// <typeparam name="T">資料回傳類型</typeparam>
/// <param name="ReturnCodeStatus">狀態碼</param>
/// <param name="ReturnMessage">回應訊息</param>
/// <param name="ReturnData">資料或者錯誤</param>
public record ResultResponse<T>(
    ReturnCodeStatus ReturnCodeStatus = ReturnCodeStatus.成功,
    string ReturnMessage = "",
    T ReturnData = default
);

/// <summary>
/// 通用的返回Response不帶泛型
/// </summary>
/// <typeparam name="T">資料回傳類型</typeparam>
/// <param name="ReturnCodeStatus">狀態碼</param>
/// <param name="ReturnMessage">回應訊息</param>
/// <param name="ReturnData">資料或者錯誤</param>
public record ResultResponse(
    ReturnCodeStatus ReturnCodeStatus = ReturnCodeStatus.成功,
    string ReturnMessage = "",
    object ReturnData = null
);

/// <summary>
/// 狀態碼枚舉
/// </summary>
public enum ReturnCodeStatus
{
    成功 = 2000,
    ModelBinding驗證失敗 = 4000,
    內部程式失敗 = 5000,
    資料庫操作失敗 = 5001,
    // 可以根據需求添加其他狀態碼
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
    public static ResultResponse<T> Success<T>(T data, string message = "") => new ResultResponse<T>(ReturnCodeStatus.成功, message, data);

    /// <summary>
    /// [5000]內部服務器錯誤的回應
    /// </summary>
    /// <typeparam name="T">資料回傳類型</typeparam>
    /// <param name="message">錯誤訊息</param>
    /// <returns>內部服務器錯誤的回應對象</returns>
    public static ResultResponse<ProblemDetails> InternalServerError<ProblemDetails>(ProblemDetails data, string message = "") =>
        new ResultResponse<ProblemDetails>(ReturnCodeStatus.內部程式失敗, message, data);

    /// <summary>
    /// [4000]ModelBinding驗證失敗
    /// </summary>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultResponse<Dictionary<string, IEnumerable<string>>> BadRequest(
        Dictionary<string, IEnumerable<string>> data,
        string message = ""
    ) => new ResultResponse<Dictionary<string, IEnumerable<string>>>(ReturnCodeStatus.ModelBinding驗證失敗, message, data);
}
