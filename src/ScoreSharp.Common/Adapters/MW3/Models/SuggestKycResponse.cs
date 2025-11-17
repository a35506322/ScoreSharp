namespace ScoreSharp.Common.Adapters.MW3.Models;

public class SuggestKycResponse
{
    /// <summary>
    /// 訊息
    /// </summary>
    [JsonPropertyName("response")]
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// 回傳代碼
    /// </summary>
    [JsonPropertyName("statusCode")]
    public string StatusCode { get; set; } = string.Empty;

    /// <summary>
    /// EAIHUB 結果
    /// </summary>
    [JsonPropertyName("info")]
    public SuggestKycMW3ResponseInfo Info { get; set; } = new();
}

public class SuggestKycMW3ResponseInfo
{
    /// <summary>
    /// M000 = 成功
    /// </summary>
    [JsonPropertyName("rc")]
    public string Rc { get; set; } = string.Empty;

    [JsonPropertyName("msg")]
    public string Msg { get; set; } = string.Empty;

    /// <summary>
    /// 等於
    /// </summary>
    [JsonPropertyName("rc2")]
    public string Rc2 { get; set; } = string.Empty;

    [JsonPropertyName("msg2")]
    public string Msg2 { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔結果
    /// </summary>
    [JsonPropertyName("result")]
    public SuggestKycMW3Result Result { get; set; } = new();
}

public class SuggestKycMW3Result
{
    /// <summary>
    /// 交易代號
    /// </summary>
    /// <remarks>
    /// KYC00SECREDIT
    /// </remarks>
    [JsonPropertyName("txn")]
    public string Txn { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔回傳資料
    /// </summary>
    [JsonPropertyName("data")]
    public SuggestKycMW3Data Data { get; set; } = new();
}

public class SuggestKycMW3Data
{
    /// <summary>
    /// KYC代碼
    /// </summary>
    /// <remarks>
    /// /// 定義值
    /// K0000：成功
    ///
    ///</remarks>
    [JsonPropertyName("KycCode")]
    public string KycCode { get; set; } = string.Empty;

    /// <summary>
    /// 回傳錯誤訊息
    /// </summary>
    [JsonPropertyName("ErrMsg")]
    public string ErrMsg { get; set; } = string.Empty;
}
