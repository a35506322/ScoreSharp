namespace ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonByQueryString;

public class GetBlackListReasonByQueryStringRequest
{
    /// <summary>
    /// 黑名單理由代碼，範例: 01、AZ
    /// </summary>
    [Display(Name = "黑名單理由代碼")]
    [MaxLength(2)]
    public string? BlackListReasonCode { get; set; }

    /// <summary>
    /// 黑名單理由名稱
    /// </summary>
    [Display(Name = "黑名單理由名稱")]
    [MaxLength(30)]
    public string? BlackListReasonName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}

public class GetBlackListReasonByQueryStringResponse
{
    /// <summary>
    /// 黑名單理由代碼，範例: 01、AZ
    /// </summary>
    public string BlackListReasonCode { get; set; } = null!;

    /// <summary>
    /// 黑名單理由名稱
    /// </summary>
    public string BlackListReasonName { get; set; } = null!;

    /// <summary>
    /// 理由強度，範圍 1-  99，用來判斷黑名單理由是否顯示，目前系統只會顯示最強那筆
    /// </summary>
    public int ReasonStrength { get; set; }

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
