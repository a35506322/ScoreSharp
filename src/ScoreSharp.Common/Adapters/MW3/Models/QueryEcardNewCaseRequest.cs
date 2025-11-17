namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryEcardNewCaseRequest
{
    [JsonPropertyName("apiName")]
    public string ApiName { get; set; } = "PAPERLESS_QRY";

    [JsonPropertyName("headers")]
    public object Headers { get; set; } = new();

    [JsonPropertyName("info")]
    public QueryEcardNewCaseInfo Info { get; set; } = new();
}

public class QueryEcardNewCaseInfo
{
    [JsonPropertyName("applyNo")]
    public string ApplyNo { get; set; } = string.Empty;
}
