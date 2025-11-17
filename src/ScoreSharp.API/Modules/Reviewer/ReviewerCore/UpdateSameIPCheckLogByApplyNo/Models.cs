namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameIPCheckLogByApplyNo;

public class UpdateSameIPCheckLogByApplyNoRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Required]
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 確認紀錄
    ///
    /// 1. 於月收入確認簽核時，當 SameIPChecked =　Y，需填寫原因
    /// </summary>
    [Required]
    [Display(Name = "確認紀錄")]
    [MaxLength(100)]
    public string CheckRecord { get; set; }

    /// <summary>
    /// 是否異常，Y｜Ｎ
    /// </summary>
    [Required]
    [RegularExpression("Y|N")]
    [Display(Name = "是否異常")]
    public string IsError { get; set; }
}
