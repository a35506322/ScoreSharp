namespace ScoreSharp.Common.Adapters.MW3;

public interface IMW3ProcAdapter
{
    /// <summary>
    /// 929發查
    /// </summary>
    /// <param name="id">身分證字號</param>
    /// <param name="rtnCode">回傳碼(預設空值)</param>
    /// <returns></returns>
    Task<BaseMW3Response<QueryOCSI929Response>> QueryOCSI929(string id, string rtnCode = "");

    /// <summary>
    /// 查詢分行資訊
    /// </summary>
    /// <param name="id">身分證字號</param>
    /// <param name="rtnCode">回傳碼(預設空值)</param>
    /// <returns></returns>
    Task<BaseMW3Response<QuerySearchCusDataResponse>> QuerySearchCusData(string id, string rtnCode = "");

    /// <summary>
    /// 查詢國旅卡資訊
    /// </summary>
    /// <param name="id">身分證字號</param>
    /// <param name="rtnCode">回傳碼(預設空值)</param>
    /// <returns></returns>
    Task<BaseMW3Response<QueryTravelCardCustomerResponse>> QueryTravelCardCustomer(string id, string rtnCode = "");

    /// <summary>
    /// 查詢卡人信用使用狀況（IBM7020）
    /// </summary>
    /// <param name="id">身分證字號</param>
    /// <returns></returns>
    Task<BaseMW3Response<QueryIBM7020Response>> QueryIBM7020(string id);

    /// <summary>
    /// 查詢原持卡人資料
    /// </summary>
    /// <param name="id">身分證字號</param>
    /// <param name="email">電子郵件</param>
    /// <param name="mobile">手機號碼</param>
    /// <returns></returns>
    Task<BaseMW3Response<QueryOriginalCardholderDataResponse>> QueryOriginalCardholderData(string id = "", string email = "", string mobile = "");

    /// <summary>
    /// 查詢原持卡人資料
    /// </summary>
    /// <param name="email">電子郵件</param>
    /// <returns></returns>
    Task<BaseMW3Response<QueryEBillResponse>> QueryEBill(string email);
}
