namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryUspAID1422Request
{
    [JsonPropertyName("SPName")]
    public string SPName { get; set; } = "uspAID1422_API";

    [JsonPropertyName("info")]
    public UspAID1422Info Info { get; set; } = null!;
}

public class UspAID1422Info
{
    [JsonPropertyName("CID")]
    public string CID { get; set; } = null!;

    [JsonPropertyName("RtnCode")]
    public string RtnCode { get; set; } = string.Empty;
}
