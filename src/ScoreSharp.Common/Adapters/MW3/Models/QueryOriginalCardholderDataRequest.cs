namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryOriginalCardholderDataRequest
{
    [JsonPropertyName("SPName")]
    public string SPName { get; set; } = string.Empty;

    [JsonPropertyName("info")]
    public QueryOriginalCardholderDataInfo Info { get; set; } = new();
}

public class QueryOriginalCardholderDataInfo
{
    [JsonPropertyName("ID")]
    public string ID { get; set; } = string.Empty;

    [JsonPropertyName("Phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("RtnCode")]
    public string RtnCode { get; set; } = string.Empty;
}
