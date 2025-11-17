namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryOCSI929Response
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = null!;

    [JsonPropertyName("rtnCode")]
    public string RtnCode { get; set; } = null!;

    [JsonPropertyName("info")]
    public OCSI929Table Info { get; set; } = null!;
}

public class OCSI929Table
{
    [JsonPropertyName("Table")]
    public List<OCSI929Data> Table { get; set; } = new();
}

public class OCSI929Data
{
    [JsonPropertyName("TxnDate")]
    public string TxnDate { get; set; } = null!;

    [JsonPropertyName("BrachCode")]
    public string BrachCode { get; set; } = null!;

    [JsonPropertyName("BrachEmp")]
    public string BrachEmp { get; set; } = null!;

    [JsonPropertyName("BusinessCode")]
    public string BusinessCode { get; set; } = null!;

    [JsonPropertyName("LoginDate")]
    public string LoginDate { get; set; } = null!;

    [JsonPropertyName("ChName")]
    public string ChName { get; set; } = null!;

    [JsonPropertyName("ApplyCause")]
    public string ApplyCause { get; set; } = null!;

    [JsonPropertyName("ApplyReMark")]
    public string ApplyReMark { get; set; } = null!;
}
