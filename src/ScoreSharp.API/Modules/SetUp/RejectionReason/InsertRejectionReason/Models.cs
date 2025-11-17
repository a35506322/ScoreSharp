namespace ScoreSharp.API.Modules.SetUp.RejectionReason.InsertRejectionReason;

public class InsertRejectionReasonRequest
{
    /// <summary>
    /// 退件代碼，範例: 01
    /// </summary>
    [Display(Name = "退件代碼")]
    [Required]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    public string RejectionReasonCode { get; set; }

    /// <summary>
    /// 退件名稱
    /// </summary>
    [Display(Name = "退件名稱")]
    [Required]
    public string RejectionReasonName { get; set; }

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }
}
