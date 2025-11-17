namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryNameCheckResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = string.Empty;

    [JsonPropertyName("statusCode")]
    public string StatusCode { get; set; } = string.Empty;

    [JsonPropertyName("info")]
    public QueryNameCheckResponseInfo Info { get; set; } = new();
}

public class QueryNameCheckResponseInfo
{
    [JsonPropertyName("rc")]
    public string Rc { get; set; } = string.Empty;

    [JsonPropertyName("msg")]
    public string Msg { get; set; } = string.Empty;

    [JsonPropertyName("rc2")]
    public string Rc2 { get; set; } = string.Empty;

    [JsonPropertyName("msg2")]
    public string Msg2 { get; set; } = string.Empty;

    [JsonPropertyName("result")]
    public NameCheckResult Result { get; set; } = new();
}

public class NameCheckResult
{
    [JsonPropertyName("data")]
    public NameCheckData Data { get; set; } = new();

    [JsonPropertyName("txn")]
    public string Txn { get; set; } = string.Empty;
}

public class NameCheckData
{
    /// <summary>
    /// 命中分數: 0-100
    /// </summary>
    /// <value></value>
    [JsonPropertyName("RCScore")]
    public string RCScore { get; set; } = string.Empty;

    /// <summary>
    /// AML參考號，使用者拿此值去AML系統查詢
    /// </summary>
    /// <value></value>
    [JsonPropertyName("AMLReference")]
    public string AMLReference { get; set; } = string.Empty;

    /// <summary>
    /// 分行號
    /// </summary>
    /// <value></value>
    [JsonPropertyName("BranchNo")]
    public string BranchNo { get; set; } = string.Empty;

    /// <summary>
    /// 銀行號
    [JsonPropertyName("BankNo")]
    public string BankNo { get; set; } = string.Empty;

    /// <summary>
    /// 命中結果，Y: 命中，N: 未命中
    /// </summary>
    /// <value></value>
    [JsonPropertyName("MatchedResult")]
    public string MatchedResult { get; set; } = string.Empty;

    /// <summary>
    /// TraceId
    /// </summary>
    /// <value></value>
    [JsonPropertyName("ReferenceNumber")]
    public string ReferenceNumber { get; set; } = string.Empty;
}
