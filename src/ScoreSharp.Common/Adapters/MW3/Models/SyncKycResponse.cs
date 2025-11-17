namespace ScoreSharp.Common.Adapters.MW3.Models;

public class SyncKycResponse
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
    public SyncKycMW3ResponseInfo Info { get; set; } = new();
}

public class SyncKycMW3ResponseInfo
{
    /// <remarks>
    /// M000 = 成功
    /// </remarks>
    [JsonPropertyName("rc")]
    public string Rc { get; set; } = string.Empty;

    [JsonPropertyName("msg")]
    public string Msg { get; set; } = string.Empty;

    /// <remarks>
    /// 等於
    /// </remarks>
    [JsonPropertyName("rc2")]
    public string Rc2 { get; set; } = string.Empty;

    [JsonPropertyName("msg2")]
    public string Msg2 { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔結果
    /// </summary>
    [JsonPropertyName("result")]
    public SyncKycMW3Result Result { get; set; } = new();
}

public class SyncKycMW3Result
{
    /// <summary>
    /// 交易代號
    /// </summary>
    /// <remarks>
    /// KYC00CREDIT
    /// </remarks>
    [JsonPropertyName("txn")]
    public string Txn { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔回傳資料
    /// </summary>
    [JsonPropertyName("data")]
    public SyncKycMW3Data Data { get; set; } = new();
}

public class SyncKycMW3Data
{
    /// <summary>
    /// 風險等級
    /// </summary>
    /// <remarks>
    /// 定義值
    /// L = 低風險
    /// M = 中風險
    /// H = 高風險
    /// </remarks>
    [JsonPropertyName("RaRank")]
    public string RaRank { get; set; } = string.Empty;

    /// <summary>
    /// 回傳代碼
    /// </summary>
    /// <remarks>
    /// 定義值
    /// K0000：成功
    /// KD001：JSON格式錯誤
    /// KD002：JSON欄位有缺
    /// KD003：資料驗證失敗
    /// KD004：客戶資料時效驗證失敗
    /// KD005：KYC資料編輯中/簽核中
    /// KD006：RMD錯誤
    /// KD007：KYC客戶資料寫入失敗
    /// KC001：主機 TimeOut
    /// KM001：傳送主機時錯誤
    /// KP001：傳送主機時錯誤
    /// KE001：傳送主機時錯誤
    /// K0001：傳送主機時錯誤
    /// KD008：建檔流程出現錯誤
    /// KD009：出生日期不符，不得強押覆蓋AMLKYC資料
    /// KD010：PEP欄位規則錯誤
    /// </remarks>
    [JsonPropertyName("KycCode")]
    public string KycCode { get; set; } = string.Empty;

    /// <summary>
    /// 有無命中警示戶負責人
    /// </summary>
    /// <value>
    /// Y/N
    /// </value>
    [JsonPropertyName("cWarnPic")]
    public string WarnPic { get; set; } = string.Empty;

    /// <summary>
    /// 風險評估時間時間
    /// </summary>
    [JsonPropertyName("RMDDate")]
    public string RMDDate { get; set; } = string.Empty;

    /// <summary>
    /// 回傳錯誤訊息
    /// </summary>
    [JsonPropertyName("ErrMsg")]
    public string ErrMsg { get; set; } = string.Empty;

    /// <summary>
    /// 有無命中風險帳戶名單
    /// </summary>
    /// <value>
    /// Y/N
    /// </value>
    [JsonPropertyName("cRiskAcc")]
    public string RiskAcc { get; set; } = string.Empty;

    /// <summary>
    /// 有無命中疑似涉詐
    /// </summary>
    /// <value>
    /// Y/N
    /// </value>
    [JsonPropertyName("cFrdId")]
    public string FrdId { get; set; } = string.Empty;
}
