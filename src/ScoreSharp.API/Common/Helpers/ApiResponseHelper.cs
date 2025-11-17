namespace ScoreSharp.API.Common.Helpers;

/// <summary>
/// 通用的返回Response
/// </summary>
/// <typeparam name="T">資料回傳類型</typeparam>
/// <param name="ReturnCodeStatus">狀態碼</param>
/// <param name="ReturnMessage">回應訊息</param>
/// <param name="ReturnData">資料或者錯誤</param>
public record ResultResponse<T>(ReturnCodeStatus ReturnCodeStatus = ReturnCodeStatus.成功, string ReturnMessage = "", T ReturnData = default);

/// <summary>
/// 通用的返回Response不帶泛型
/// </summary>
/// <typeparam name="T">資料回傳類型</typeparam>
/// <param name="ReturnCodeStatus">狀態碼</param>
/// <param name="ReturnMessage">回應訊息</param>
/// <param name="ReturnData">資料或者錯誤</param>
public record ResultResponse(ReturnCodeStatus ReturnCodeStatus = ReturnCodeStatus.成功, string ReturnMessage = "", object ReturnData = null);

/// <summary>
/// 狀態碼枚舉
/// </summary>
public enum ReturnCodeStatus
{
    成功 = 2000,
    ModelBinding驗證失敗 = 4000,
    查無此資料 = 4001,
    資料已存在 = 4002,
    商業邏輯驗證失敗 = 4003,
    Token驗證失敗 = 4004,
    授權失敗 = 4005,
    徵審商業邏輯驗證失敗 = 4006,
    內部程式失敗 = 5000,
    資料庫操作失敗 = 5001,
    發查第三方API失敗 = 5002,
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

    /// <summary>
    /// [2000]更新單筆成功
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ResultResponse<T> UpdateByIdSuccess<T>(T data, string id) =>
        new ResultResponse<T>(ReturnCodeStatus.成功, $"更新此ID成功：{id}", data);

    /// <summary>
    /// [4001]查無此資料
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    public static ResultResponse<T> NotFound<T>(T data, string id) =>
        new ResultResponse<T>(ReturnCodeStatus: ReturnCodeStatus.查無此資料, ReturnMessage: $"查無此ID：{id}", ReturnData: data);

    /// <summary>
    /// [5001]更新單筆失敗
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ResultResponse<T> UpdateByIdError<T>(T data, string id) =>
        new ResultResponse<T>(ReturnCodeStatus.資料庫操作失敗, $"更新此ID失敗：{id}", data);

    /// <summary>
    /// [2000]單筆新增成功
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ResultResponse<T> InsertSuccess<T>(T data, string id) => new ResultResponse<T>(ReturnCodeStatus.成功, $"新增此ID成功：{id}", data);

    /// <summary>
    /// [5001]單筆新增失敗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ResultResponse<T> InsertError<T>(T data, string id) =>
        new ResultResponse<T>(ReturnCodeStatus.資料庫操作失敗, $"新增此ID失敗：{id}", data);

    /// <summary>
    /// [4002]資料已存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ResultResponse<T> DataAlreadyExists<T>(T data, string id) =>
        new ResultResponse<T>(ReturnCodeStatus.資料已存在, $"資料已存在：{id}", data);

    /// <summary>
    /// [4003]商業邏輯驗證失敗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultResponse<T> BusinessLogicFailed<T>(T data, string message) =>
        new ResultResponse<T>(ReturnCodeStatus.商業邏輯驗證失敗, message, data);

    /// <summary>
    /// [2000]刪除單筆成功 ById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ResultResponse<T> DeleteByIdSuccess<T>(T id) =>
        new ResultResponse<T>(ReturnCodeStatus: ReturnCodeStatus.成功, ReturnMessage: $"刪除此ID成功：{id}", ReturnData: id);

    /// <summary>
    /// [4004] Token驗證失敗
    /// </summary>
    /// <param name="errorMsg">AuthorizationFailure.FailureReasons 字串化</param>
    /// <returns></returns>
    public static ResultResponse<string> TokenCheckFailed(string errorMsg) =>
        new ResultResponse<string>(ReturnCodeStatus: ReturnCodeStatus.Token驗證失敗, ReturnMessage: errorMsg, ReturnData: null);

    /// <summary>
    /// [4005] 授權失敗
    /// </summary>
    /// <param name="errorMsg">AuthorizationFailure.FailureReasons 字串化</param>
    /// <returns></returns>
    public static ResultResponse<string> PolicyCheckFailed(string errorMsg) =>
        new ResultResponse<string>(ReturnCodeStatus: ReturnCodeStatus.授權失敗, ReturnMessage: errorMsg, ReturnData: null);

    /// <summary>
    /// [4003]商業邏輯驗證失敗 => 路由與Req比對錯誤
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ResultResponse<T> 路由與Req比對錯誤<T>(T data) =>
        new ResultResponse<T>(ReturnCodeStatus.商業邏輯驗證失敗, "呼叫錯誤，請確認", data);

    /// <summary>
    /// [4003]商業邏輯驗證失敗 => 前端傳入關聯資料有誤
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="attr">屬性</param>
    /// <returns></returns>
    public static ResultResponse<T> 前端傳入關聯資料有誤<T>(T data, string attr, string id) =>
        new ResultResponse<T>(ReturnCodeStatus.商業邏輯驗證失敗, $"查無{attr}:{id}，請確認", data);

    /// <summary>
    /// [4003]商業邏輯驗證失敗 => 此資源已被使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ResultResponse<T> 此資源已被使用<T>(T data, string id) =>
        new ResultResponse<T>(ReturnCodeStatus.商業邏輯驗證失敗, $"此資源已被使用:{id}，請確認", data);

    /// <summary>
    /// [5002]發查第三方API失敗
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultResponse<T> CheckThirdPartyApiError<T>(T data, string id) =>
        new ResultResponse<T>(ReturnCodeStatus.發查第三方API失敗, $"此 {id} 發查第三方API失敗", data);

    /// <summary>
    /// [5000]內部服務器錯誤的回應
    /// </summary>
    /// <typeparam name="T">資料回傳類型</typeparam>
    /// <param name="message">錯誤訊息</param>
    /// <returns>內部服務器錯誤的回應對象</returns>
    public static ResultResponse<T> InternalServerErrorByException<T>(T data, string message = "") =>
        new ResultResponse<T>(ReturnCodeStatus.內部程式失敗, message, data);

    /// <summary>
    /// [5002]發查第三方API失敗，並附帶API名稱
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="checkThirdPartyApiName"></param>
    /// <returns></returns>
    public static ResultResponse<T> CheckThirdPartyApiErrorWithApiName<T>(T data, string checkThirdPartyApiName) =>
        new ResultResponse<T>(ReturnCodeStatus.發查第三方API失敗, $"發查第三方API失敗: {checkThirdPartyApiName}", data);

    /// <summary>
    /// [4006]徵審商業邏輯驗證失敗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultResponse<T> ReviewerBusinessLogicFailed<T>(T data, string message) =>
        new ResultResponse<T>(ReturnCodeStatus.徵審商業邏輯驗證失敗, message, data);
}
