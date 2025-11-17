namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.GetWebRetryCaseByQueryString;

public class GetWebRetryCaseByQueryStringRequest
{
    /// <summary>
    /// 申請日期
    /// </summary>
    [DisplayName("申請日期")]
    [RegularExpression("^(?:19|20)\\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\\d|3[01])$")]
    [MaxLength(10)]
    public string? ApplyDate { get; set; }
}

public class GetWebRetryCaseByQueryStringResponse
{
    /// <summary>
    /// PK
    /// </summary>
    [DisplayName("PK")]
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    [DisplayName("申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// Request
    /// </summary>
    [DisplayName("Request")]
    public string Request { get; set; }

    /// <summary>
    /// 回應代碼
    /// 當以下代碼會由資訊人員重新發送
    /// 0005
    /// 0006
    /// 0007
    /// </summary>
    [DisplayName("回應代碼")]
    public string ReturnCode { get; set; }

    /// <summary>
    /// 錯誤紀錄
    /// </summary>
    [DisplayName("錯誤紀錄")]
    public string CaseErrorLog { get; set; }

    /// <summary>
    /// 最後寄送員工
    /// </summary>
    [DisplayName("最後寄送員工")]
    public string LastSendUserId { get; set; }

    /// <summary>
    /// 最後寄送時間
    /// </summary>
    [DisplayName("最後寄送時間")]
    public string LastSendTtime { get; set; }

    /// <summary>
    /// 是否寄送
    /// </summary>
    [DisplayName("是否寄送")]
    [RegularExpression("[YN]")]
    public string IsSend { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    [DisplayName("新增時間")]
    public DateTime AddTime { get; set; }
}
