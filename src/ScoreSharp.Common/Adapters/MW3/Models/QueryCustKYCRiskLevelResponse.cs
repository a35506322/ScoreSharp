namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryCustKYCRiskLevelResponse
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
    public QueryCustKYCRiskLevelResponseInfo Info { get; set; } = new();
}

public class QueryCustKYCRiskLevelResponseInfo
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
    public QueryCustKYCRiskLevelResult Result { get; set; } = new();
}

public class QueryCustKYCRiskLevelResult
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
    public QueryCustKYCRiskLevelData Data { get; set; } = new();
}

public class QueryCustKYCRiskLevelData
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; } = string.Empty;

    /// <summary>
    /// KYC錯誤訊息
    /// </summary>
    [JsonPropertyName("kycMsg")]
    public string KycMsg { get; set; }

    [JsonPropertyName("data")]
    public QueryCustKYCRiskLevelDetail Data { get; set; } = new();

    /// <summary>
    /// kyc代碼
    ///
    /// K0000 成功
    /// KD001 JSON格式錯誤
    /// KD002 資料錯誤
    /// KD100 KYC內無資料
    /// KD101 KYC客戶資料寫入失敗
    /// KD200 KYC資料編輯中/簽核中
    /// KC001 主機Timeout
    /// KM001 傳送主機時錯誤
    /// KP001 傳送主機時錯誤
    /// KE001 傳送主機時錯誤
    /// KO001 傳送主機時錯誤
    /// </summary>
    [JsonPropertyName("kyc")]
    public string KYC { get; set; }
}

public class QueryCustKYCRiskLevelDetail
{
    /// <summary>
    /// 國家代號
    /// </summary>
    [JsonPropertyName("cNationO")]
    public string NationO { get; set; }

    [JsonPropertyName("cBirthP")]
    public string BirthP { get; set; }

    [JsonPropertyName("cBirthPO")]
    public string BirthPO { get; set; }

    /// <summary>
    /// 身分類別:01自然人
    /// </summary>
    [JsonPropertyName("idType")]
    public string IdType { get; set; }

    /// <summary>
    /// 風險評估時間
    /// </summary>
    [JsonPropertyName("cRARTime")]
    public string RARTime { get; set; }

    /// <summary>
    /// 國籍1.中華民國2.其他(雙國籍1,2)
    /// </summary>
    [JsonPropertyName("cNation")]
    public string Nation { get; set; }

    [JsonPropertyName("bRRFlag")]
    public string BRRFlag { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [JsonPropertyName("cBirthD")]
    public string BirthD { get; set; }

    /// <summary>
    /// 客戶風險評估
    /// </summary>
    [JsonPropertyName("cRAreaFlag")]
    public string RAreaFlag { get; set; }

    /// <summary>
    /// 統一編號
    /// </summary>
    [JsonPropertyName("cId")]
    public string Id { get; set; }

    /// <summary>
    /// 風險等級:L、M、H
    /// </summary>
    [JsonPropertyName("cRARank")]
    public string RARank { get; set; }
}
