namespace ScoreSharp.Common.Adapters.MW3;

public interface IMW3MSAPIAdapter
{
    /// <summary>
    /// 查詢關注名單
    /// A.告誡名單 = restriction
    /// B.受警示企業戶之負責人 = warningCompany
    /// C.風險帳戶 = riskAccount
    /// D.聯徵資料─行方不明 = fled
    /// E.聯徵資料─收容遣返 = punish
    /// F.聯徵資料─出境 = immi
    /// G.失蹤人口 = missingPersons
    /// </summary>
    Task<BaseMW3Response<QueryConcernDetailResponse>> QueryConcernDetail(string id);
}
