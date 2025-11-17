namespace ScoreSharp.API.Modules.SetUp.SupplementReason.InsertSupplementReason;

public class InsertSupplementReasonRequest
{
    /// <summary>
    /// 補件代碼，範例: 01
    /// </summary>
    [Display(Name = "補件代碼")]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    [Required]
    public string SupplementReasonCode { get; set; } = null!;

    /// <summary>
    /// 補件名稱
    /// </summary>
    [Display(Name = "補件名稱")]
    [MaxLength(100)]
    [Required]
    public string SupplementReasonName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; } = null!;
}
