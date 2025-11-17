namespace ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonByQueryString;

public class GetSupplementReasonByQueryStringRequest
{
    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }

    /// <summary>
    /// 補件代碼，範例: 01
    /// </summary>
    [Display(Name = "補件代碼")]
    public string? SupplementReasonCode { get; set; }

    /// <summary>
    /// 補件名稱
    /// </summary>
    [Display(Name = "補件名稱")]
    [MaxLength(100)]
    public string? SupplementReasonName { get; set; }
}

public class GetSupplementReasonByQueryStringResponse
{
    /// <summary>
    /// 補件代碼，範例: 01
    /// </summary>
    public string SupplementReasonCode { get; set; }

    /// <summary>
    /// 補件名稱
    /// </summary>
    public string SupplementReasonName { get; set; }

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    public string IsActive { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; }

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
