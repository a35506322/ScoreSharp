namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonByQueryString;

public class GetMakeCardFailedReasonByQueryStringRequest
{
    /// <summary>
    /// 製卡失敗原因代碼，範例: 01、AZ
    /// </summary>
    [Display(Name = "製卡失敗原因代碼")]
    [MaxLength(2)]
    public string? MakeCardFailedReasonCode { get; set; }

    /// <summary>
    /// 製卡失敗原因名稱
    /// </summary>
    [Display(Name = "製卡失敗原因名稱")]
    [MaxLength(30)]
    public string? MakeCardFailedReasonName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}

public class GetMakeCardFailedReasonByQueryStringResponse
{
    /// <summary>
    /// 製卡失敗原因代碼，範例: 01、AZ
    /// </summary>
    public string MakeCardFailedReasonCode { get; set; } = null!;

    /// <summary>
    /// 製卡失敗原因名稱
    /// </summary>
    public string MakeCardFailedReasonName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    public string IsActive { get; set; } = null!;

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
