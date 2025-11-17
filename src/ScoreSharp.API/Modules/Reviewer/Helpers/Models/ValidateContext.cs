namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

public class ValidateContext : IValidatableObject
{
    /// <summary>
    /// 電子郵件
    /// </summary>
    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "電子郵件格式錯誤")]
    public string? Email { get; set; }

    /// <summary>
    /// 正卡人_出生年月日
    /// </summary>
    [Display(Name = "正卡人_出生年月日")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? M_BirthDay { get; set; }

    /// <summary>
    /// 正卡人_身分證發證日期
    /// </summary>
    [Display(Name = "正卡人_身分證發證日期")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? M_IDIssueDate { get; set; }

    /// <summary>
    /// 正卡人_居留證發證日期
    /// </summary>
    [Display(Name = "正卡人_居留證發證日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? M_ResidencePermitIssueDate { get; set; }

    /// <summary>
    /// 正卡人_居留證期限
    /// </summary>
    [Display(Name = "正卡人_居留證期限")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? M_ResidencePermitDeadline { get; set; }

    /// <summary>
    /// 正卡人_護照日期
    /// </summary>
    [Display(Name = "正卡人_護照日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? M_PassportDate { get; set; }

    /// <summary>
    /// 正卡人_外籍人士指定效期
    /// </summary>
    [Display(Name = "正卡人_外籍人士指定效期")]
    [ValidDate(format: "yyyyMM", isROC: false)]
    public string? M_ExpatValidityPeriod { get; set; }

    /// <summary>
    /// 附卡人_身分證字號
    /// </summary>
    [TWID]
    [Display(Name = "附卡人_身分證字號")]
    public string? S1_ID { get; set; }

    /// <summary>
    /// 附卡人_出生年月日
    /// </summary>
    [Display(Name = "附卡人_出生年月日")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? S1_BirthDay { get; set; }

    /// <summary>
    /// 附卡人_身分證發證日期
    /// </summary>
    [Display(Name = "附卡人_身分證發證日期")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? S1_IDIssueDate { get; set; }

    /// <summary>
    /// 附卡人_居留證發證日期
    /// </summary>
    [Display(Name = "附卡人_居留證發證日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? S1_ResidencePermitIssueDate { get; set; }

    /// <summary>
    /// 附卡人_居留證期限
    /// </summary>
    [Display(Name = "附卡人_居留證期限")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? S1_ResidencePermitDeadline { get; set; }

    /// <summary>
    /// 附卡人_護照日期
    /// </summary>
    [Display(Name = "附卡人_護照日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? S1_PassportDate { get; set; }

    /// <summary>
    /// 附卡人_外籍人士指定效期
    /// </summary>
    [Display(Name = "附卡人_外籍人士指定效期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? S1_ExpatValidityPeriod { get; set; }

    /// <summary>
    /// 是否為完成月收入確認
    /// </summary>
    [Display(Name = "是否為完成月收入確認")]
    [Required]
    public bool IsCompleteMonthlyIncome { get; set; }

    /// <summary>
    /// 現職月收入(元)
    /// </summary>
    public decimal? CurrentMonthIncome { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsCompleteMonthlyIncome && CurrentMonthIncome == null)
        {
            yield return new ValidationResult("現職月收入需必填", new[] { nameof(CurrentMonthIncome) });
        }
    }
}
