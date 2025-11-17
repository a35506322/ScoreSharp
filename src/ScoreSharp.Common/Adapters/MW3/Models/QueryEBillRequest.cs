namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryEBillRequest
{
    [JsonPropertyName("SPName")]
    public string SPName { get; set; } = string.Empty;

    [JsonPropertyName("info")]
    public QueryEBillRequestInfo Info { get; set; } = new();
}

public class QueryEBillRequestInfo
{
    [JsonPropertyName("EMail")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("RtnCode")]
    public string RtnCode { get; set; } = string.Empty;
}
