namespace ScoreSharp.Common.Adapters.MW3;

public interface IMW3APAPIAdapter
{
    /// <summary>
    /// 查詢進件資料
    /// </summary>
    /// <param name="applyNo"></param>
    /// <returns></returns>
    Task<BaseMW3Response<EcardNewCaseData>> QueryEcardNewCase(string applyNo);

    /// <summary>
    /// 查詢姓名檢核
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callUser"></param>
    /// <param name="traceId"></param>
    /// <returns></returns>
    Task<BaseMW3Response<QueryNameCheckResponse>> QueryNameCheck(string name, string callUser, string traceId);

    /// <summary>
    /// 入檔KYC
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<BaseMW3Response<SyncKycResponse>> SyncKYC(SyncKycMW3Info request);

    /// <summary>
    /// 更改建議核准KYC
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<BaseMW3Response<SuggestKycResponse>> SuggestKYC(SuggestKycMW3Info request);

    /// <summary>
    /// 簡易查詢KYC風險等級
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<BaseMW3Response<QueryCustKYCRiskLevelResponse>> QueryCustKYCRiskLevel(QueryCustKYCRiskLevelRequestInfo request);
}
