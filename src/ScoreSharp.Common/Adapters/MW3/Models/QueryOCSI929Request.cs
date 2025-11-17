namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryOCSI929Request
{
    [JsonPropertyName("SPName")]
    public string SPName { get; set; } = "OCSI929API";

    [JsonPropertyName("info")]
    public OCSI929Info Info { get; set; } = null!;
}

public class OCSI929Info
{
    [JsonPropertyName("ID")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("RtnCode")]
    public string RtnCode { get; set; } = string.Empty;
}
