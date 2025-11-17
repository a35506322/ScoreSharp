namespace ScoreSharp.Common.Adapters.MW3.Models;

class QueryCustKYCRiskLevelRequest
{
    /// <summary>
    /// API 名稱
    /// </summary>
    /// <value>
    /// 固定值：KYC00QNBDRA
    /// </value>
    [JsonPropertyName("apiName")]
    public string ApiName { get; private set; } = "KYC00QNBDRA";

    /// <summary>
    /// 夾帶 header
    /// </summary>
    [JsonPropertyName("headers")]
    public QueryCustKYCRiskLevelHeaders Headers { get; set; } = new();

    /// <summary>
    /// EAIHUB Request
    /// </summary>
    [JsonPropertyName("info")]
    public QueryCustKYCRiskLevelRequestInfo Info { get; set; } = new();
}

public class QueryCustKYCRiskLevelHeaders
{
    /// <summary>
    /// 授權
    /// </summary>
    /// <value>
    /// TEST：Basic Y3JkU1M6Y3JkU1M= <br/>
    /// PROD：
    /// </value>
    [JsonPropertyName("Authorization")]
    public string Authorization { get; set; } = string.Empty;
}

public class QueryCustKYCRiskLevelRequestInfo
{
    /// <summary>
    /// 交易類型
    /// </summary>
    /// <value>
    /// 固定值：KYC00CREDIT
    /// </value>
    [JsonPropertyName("_RestType")]
    public string RestType { get; private set; } = "KYC00QNBDRA";

    /// <summary>
    /// 身份證字號
    /// </summary>
    [JsonPropertyName("uninumber")]
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 檢查碼A
    /// </summary>
    [JsonPropertyName("uniTail")]
    public string UniTail { get; set; } = "A";
}
