namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryTravelCardCustomerRequest
{
    [JsonPropertyName("SPName")]
    public string SPName { get; set; } = "TravelCardCustomerAPI";

    [JsonPropertyName("info")]
    public TravelCardCustomerInfo Info { get; set; } = null!;
}

public class TravelCardCustomerInfo
{
    [JsonPropertyName("ID")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("RtnCode")]
    public string RtnCode { get; set; } = string.Empty;
}
