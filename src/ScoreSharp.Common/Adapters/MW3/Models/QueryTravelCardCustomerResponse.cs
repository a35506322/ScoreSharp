namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryTravelCardCustomerResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = null!;

    [JsonPropertyName("rtnCode")]
    public string RtnCode { get; set; } = null!;

    [JsonPropertyName("info")]
    public object Info { get; set; } = null!;
}
