namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QuerySearchCusDataRequest
{
    [JsonPropertyName("SPName")]
    public string SPName { get; set; } = "SearchCusDataAPI";

    [JsonPropertyName("info")]
    public SearchCusDataInfo Info { get; set; } = null!;
}

public class SearchCusDataInfo
{
    [JsonPropertyName("ID")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("RtnCode")]
    public string RtnCode { get; set; } = string.Empty;
}
