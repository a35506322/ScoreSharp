namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryNameCheckRequest
{
    [JsonPropertyName("apiName")]
    public string ApiName { get; set; } = "AML000001";

    [JsonPropertyName("headers")]
    public QueryNameCheckHeaders Headers { get; set; } = new();

    [JsonPropertyName("info")]
    public QueryNameCheckInfo Info { get; set; } = new();
}

public class QueryNameCheckHeaders
{
    [JsonPropertyName("Authorization")]
    public string Authorization { get; set; } = string.Empty;
}

public class QueryNameCheckInfo
{
    [JsonPropertyName("_RestType")]
    public string RestType { get; set; } = string.Empty;

    [JsonPropertyName("BankNo")]
    public string BankNo { get; set; } = string.Empty;

    [JsonPropertyName("BranchNo")]
    public string BranchNo { get; set; } = string.Empty;

    [JsonPropertyName("Channel")]
    public string Channel { get; set; } = string.Empty;

    [JsonPropertyName("DOB")]
    public string DOB { get; set; } = string.Empty;

    [JsonPropertyName("EnglishName")]
    public string EnglishName { get; set; } = string.Empty;

    [JsonPropertyName("ID")]
    public string ID { get; set; } = string.Empty;

    [JsonPropertyName("Nationality")]
    public string Nationality { get; set; } = string.Empty;

    [JsonPropertyName("Non-EnglishName")]
    public string NonEnglishName { get; set; } = string.Empty;

    [JsonPropertyName("ReferenceNumber")]
    public string ReferenceNumber { get; set; } = string.Empty;

    [JsonPropertyName("TellerName")]
    public string TellerName { get; set; } = string.Empty;
}
