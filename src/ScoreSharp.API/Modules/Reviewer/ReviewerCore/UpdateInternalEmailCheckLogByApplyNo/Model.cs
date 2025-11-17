namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalEmailCheckLogByApplyNo;

public class UpdateInternalEmailCheckLogByApplyNoRequest : IValidatableObject
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
    /// 1. 於月收入確認簽核時，當 InternalEmailSame_Flag =Y，需填寫原因
    /// </summary>
    [Required]
    [Display(Name = "確認紀錄")]
    public string CheckRecord { get; set; }

    /// <summary>
    /// 是否異常，Y｜Ｎ
    /// </summary>
    [Required]
    [RegularExpression("Y|N")]
    [Display(Name = "是否異常")]
    public string IsError { get; set; }

    /// <summary>
    /// 確認關係
    /// </summary> <summary>
    /// 當 是否異常 = N，需填寫確認關係
    /// </summary>
    /// <value>
    /// 1. 父母
    /// 2. 子女
    /// 3. 配偶
    /// 4. 兄弟姊妹
    /// 5. 配偶父母
    /// 6. 其他關係
    /// 7. 無關係
    /// </value>
    [Display(Name = "確認關係")]
    [ValidEnumValue]
    public SameDataRelation? Relation { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsError == "N" && Relation == null)
        {
            yield return new ValidationResult("當是否異常為N時，確認關係為必填", new[] { "Relation" });
        }
    }
}
