namespace ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonByQueryString;

public class GetRejectionReasonByQueryStringRequest
{
    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }

    /// <summary>
    /// 退件名稱
    /// </summary>
    [Display(Name = "退件名稱")]
    public string? RejectionReasonName { get; set; }

    /// <summary>
    /// 退件代碼，範例: 01
    /// </summary>
    [Display(Name = "退件代碼")]
    public string? RejectionReasonCode { get; set; }
}

public class GetRejectionReasonByQueryStringResponse
{
    /// <summary>
    /// 退件代碼，範例: 01
    /// </summary>
    public string RejectionReasonCode { get; set; } = null!;

    /// <summary>
    /// 退件名稱
    /// </summary>
    public string RejectionReasonName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，範例: Y | N
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
