namespace ScoreSharp.API.Modules.Reviewer3rd.KYCSync;

public class KYCSyncRequest
{
    [Required]
    public string ApplyNo { get; set; }
}

public class KYCSyncResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 入檔KYC回傳代碼
    /// </summary>
    public string KYC_RtnCode { get; set; }

    /// <summary>
    /// 入檔KYC回傳訊息，如發生K0000以外的代碼，有此訊息
    /// </summary>
    public string KYC_Message { get; set; }

    /// <summary>
    /// 入檔KYC風險等級
    /// </summary>
    public string KYC_RiskLevel { get; set; }

    /// <summary>
    /// 入檔KYC查詢時間
    /// </summary>
    public DateTime KYC_QueryTime { get; set; }

    /// <summary>
    /// 入檔KYC例外訊息，如發送 API 失敗，有此訊息
    /// </summary>
    public string KYC_Exception { get; set; }
}
