namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QuerySearchCusDataResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = null!;

    [JsonPropertyName("rtnCode")]
    public string RtnCode { get; set; } = null!;

    [JsonPropertyName("info")]
    public SearchCusDataTable Info { get; set; } = null;
}

public class SearchCusDataTable
{
    [JsonPropertyName("Table")]
    public List<客戶資訊> 客戶資訊 { get; set; } = new();

    [JsonPropertyName("Table1")]
    public List<財富管理客戶> 財富管理客戶 { get; set; } = new();

    [JsonPropertyName("Table2")]
    public List<定存明細資訊> 定存明細資訊 { get; set; } = new();

    [JsonPropertyName("Table3")]
    public List<活期存款明細資訊> 活期存款明細資訊 { get; set; } = new();

    [JsonPropertyName("Table4")]
    public List<支票存款明細資訊> 支票存款明細資訊 { get; set; } = new();

    [JsonPropertyName("Table5")]
    public List<授信逾期狀況> 授信逾期狀況 { get; set; } = new();
}

public class 客戶資訊
{
    [JsonPropertyName("id")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("sn")]
    public string SN { get; set; } = null!;

    [JsonPropertyName("cate")]
    public string Cate { get; set; } = null!;
}

public class 財富管理客戶
{
    [JsonPropertyName("iCountFlag")]
    public string ICountFlag { get; set; } = null!;
}

public class 定存明細資訊
{
    [JsonPropertyName("id")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("cate")]
    public string Cate { get; set; } = null!;

    [JsonPropertyName("Currency")]
    public string Currency { get; set; } = null!;

    [JsonPropertyName("InterestD")]
    public string InterestD { get; set; } = null!;

    [JsonPropertyName("ExpirationD")]
    public string ExpirationD { get; set; } = null!;

    [JsonPropertyName("Amount")]
    public int Amount { get; set; }
}

public class 活期存款明細資訊
{
    [JsonPropertyName("id")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("cate")]
    public string Cate { get; set; } = null!;

    [JsonPropertyName("Currency")]
    public string Currency { get; set; } = null!;

    [JsonPropertyName("Account")]
    public string Account { get; set; } = null!;

    [JsonPropertyName("OpenAcountD")]
    public string OpenAcountD { get; set; } = null!;

    [JsonPropertyName("CreditD")]
    public string CreditD { get; set; } = null!;

    [JsonPropertyName("Last3MavgCredit")]
    public int Last3MavgCredit { get; set; }

    [JsonPropertyName("ThreeMavgCredit")]
    public int ThreeMavgCredit { get; set; }

    [JsonPropertyName("TwoMavgCredit")]
    public int TwoMavgCredit { get; set; }

    [JsonPropertyName("OneMavgCredit")]
    public int OneMavgCredit { get; set; }

    [JsonPropertyName("Credit")]
    public int Credit { get; set; }
}

public class 支票存款明細資訊
{
    [JsonPropertyName("id")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("cate")]
    public string Cate { get; set; } = null!;

    [JsonPropertyName("Account")]
    public string Account { get; set; } = null!;

    [JsonPropertyName("OpenAcountD")]
    public string OpenAcountD { get; set; } = null!;

    [JsonPropertyName("CreditD")]
    public string CreditD { get; set; } = null!;

    [JsonPropertyName("Last3MavgCredit")]
    public int Last3MavgCredit { get; set; }

    [JsonPropertyName("ThreeMavgCredit")]
    public int ThreeMavgCredit { get; set; }

    [JsonPropertyName("TwoMavgCredit")]
    public int TwoMavgCredit { get; set; }

    [JsonPropertyName("OneMavgCredit")]
    public int OneMavgCredit { get; set; }

    [JsonPropertyName("Credit")]
    public int Credit { get; set; }
}

public class 授信逾期狀況
{
    [JsonPropertyName("cID")]
    public string ID { get; set; } = null!;

    [JsonPropertyName("cAccount")]
    public string Account { get; set; } = null!;

    [JsonPropertyName("cOverStatus")]
    public string OverStatus { get; set; } = null!;
}
