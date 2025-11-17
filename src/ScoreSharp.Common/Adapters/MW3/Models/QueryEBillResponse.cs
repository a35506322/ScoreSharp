namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryEBillResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = null!;

    [JsonPropertyName("rtnCode")]
    public string RtnCode { get; set; } = null!;

    [JsonPropertyName("info")]
    public QueryEBillResponseInfo Info { get; set; } = null!;
}

public class QueryEBillResponseInfo
{
    [JsonPropertyName("Table")]
    public List<QueryEBillTable> Table { get; set; } = new();
}

public class QueryEBillTable
{
    /// <summary>
    /// EMAIL帳單會員身分證
    /// </summary>
    /// <value></value>
    [JsonPropertyName("ID")]
    public string ID { get; set; } = null!;

    /// <summary>
    /// 會員姓名
    /// </summary>
    /// <value></value>
    [JsonPropertyName("Name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 會員電子信箱
    /// </summary>
    /// <value></value>
    [JsonPropertyName("EMail")]
    public string Email { get; set; } = null!;
}
