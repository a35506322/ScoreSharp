using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.InsertUserOrgCaseSetUp;

public class InsertUserOrgCaseSetUpRequest : IValidatableObject
{
    /// <summary>
    /// 使用者帳號
    /// PK
    /// 關聯OrgSetUp_User
    /// </summary>
    [Required]
    [MaxLength(30)]
    [DisplayName("使用者帳號")]
    public string UserId { get; set; }

    /// <summary>
    /// 額度上限，預設0
    /// </summary>
    [Required]
    [DisplayName("額度上限")]
    public int CardLimit { get; set; }

    /// <summary>
    /// 指定主管一
    /// </summary>
    [DisplayName("指定主管一")]
    [MaxLength(30)]
    public string? DesignatedSupervisor1 { get; set; }

    /// <summary>
    /// 指定主管二
    /// </summary>
    [DisplayName("指定主管二")]
    [MaxLength(30)]
    public string? DesignatedSupervisor2 { get; set; }

    /// <summary>
    /// 一般件預審分案，Y｜N
    /// </summary>
    [Required]
    [RegularExpression("[YN]")]
    [DisplayName("一般件預審分案")]
    public string IsPaperCase { get; set; }

    /// <summary>
    /// 快辦件預審分案，Y｜N
    /// </summary>
    [Required]
    [RegularExpression("[YN]")]
    [DisplayName("快辦件預審分案")]
    public string IsWebCase { get; set; }

    /// <summary>
    /// 覆核比
    /// 每幾筆抽查案件
    /// 預設0
    /// </summary>
    [Required]
    [DisplayName("覆核比")]
    public int CheckWeight { get; set; }

    /// <summary>
    /// 一般件預審比重排序
    /// </summary>
    [Required]
    [DisplayName("一般件預審比重排序")]
    public int PaperCaseSort { get; set; }

    /// <summary>
    /// 快辦件預審比重排序
    /// </summary>
    [Required]
    [DisplayName("快辦件預審比重排序")]
    public int WebCaseSort { get; set; }

    /// <summary>
    /// 人工徵信件預審分案
    ///
    /// Y / N
    /// </summary>
    [Required]
    [RegularExpression("[YN]")]
    [DisplayName("人工徵信件預審分案")]
    public string IsManualCase { get; set; } = null!;

    /// <summary>
    /// 人工徵信件預審比重排序
    ///
    /// 預設1
    /// </summary>
    [Required]
    [DisplayName("人工徵信件預審比重排序")]
    public int ManualCaseSort { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        //指定主管無法指派給自己
        if (DesignatedSupervisor1 == UserId || DesignatedSupervisor2 == UserId)
            yield return new ValidationResult(
                "指定主管無法指派給自己！",
                new[] { this.GetDisplayName(nameof(DesignatedSupervisor1)), this.GetDisplayName(nameof(DesignatedSupervisor2)) }
            );
    }
}
