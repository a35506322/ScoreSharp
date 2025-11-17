namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryUspAID1422Response
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = null!;

    [JsonPropertyName("rtnCode")]
    public string RtnCode { get; set; } = null!;

    [JsonPropertyName("info")]
    public UspAID1422Table Info { get; set; } = null!;
}

public class UspAID1422Table
{
    [JsonPropertyName("Table")]
    public List<UspAID1422Data> Table { get; set; } = new();
}

public class UspAID1422Data
{
    [JsonPropertyName("DATATYPE")]
    public string DataType { get; set; } = null!;

    [JsonPropertyName("PID")]
    public string PID { get; set; } = null!;

    [JsonPropertyName("WARNINGDATE")]
    public string WarningDate { get; set; } = null!;

    [JsonPropertyName("EXPIREDATE")]
    public string ExpireDate { get; set; } = null!;

    [JsonPropertyName("ISSUER")]
    public string Issuer { get; set; } = null!;

    [JsonPropertyName("CreateDate")]
    public string CreateDate { get; set; } = null!;
}
