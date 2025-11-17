namespace ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonsByQueryString;

public class GetLongTermReasonsByQueryStringRequest
{
    /// <summary>
    /// 長循分期戶理由名稱
    /// </summary>
    [Display(Name = "長循分期戶理由名稱")]
    public string? LongTermReasonName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetLongTermReasonsByQueryStringResponse
{
    /// <summary>
    /// 長循分期戶理由代碼，範例: 01、AZ
    /// </summary>
    public string LongTermReasonCode { get; set; } = null!;

    /// <summary>
    /// 長循分期戶理由名稱
    /// </summary>
    public string LongTermReasonName { get; set; } = null!;

    /// <summary>
    /// 理由強度，範圍 1-  99
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
