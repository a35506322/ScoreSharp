namespace ScoreSharp.API.Modules.OrgSetUp.User.InsertUser;

public class InsertUserRequest : IValidatableObject
{
    /// <summary>
    /// 使用者帳號，目前來源為 AD Server
    /// </summary>
    [Display(Name = "使用者帳號")]
    [Required]
    public string UserId { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [Display(Name = "角色")]
    [Required]
    [MinLength(1)]
    public string[] RoleId { get; set; }

    /// <summary>
    /// 使用者姓名，目前來源為 AD Server
    /// </summary>
    [Display(Name = "使用者姓名")]
    [Required]
    public string UserName { get; set; }

    /// <summary>
    /// 是否AD
    /// </summary>
    [Display(Name = "是否AD")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsAD { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    [Display(Name = "密碼")]
    public string? Mima { get; set; }

    /// <summary>
    /// 組織代碼
    /// 1. 關聯 OrgSetUp_Organize
    /// 2.跟派案組織無關
    /// </summary>
    [Display(Name = "組織代碼")]
    [Required]
    public string OrganizeCode { get; set; }

    /// <summary>
    /// 派案組織
    /// </summary>
    [Display(Name = "派案組織")]
    [ValidEnumList(typeof(CaseDispatchGroup))]
    public List<CaseDispatchGroup> CaseDispatchGroups { get; set; } = [];

    /// <summary>
    /// 員工編號
    /// </summary>
    [Display(Name = "員工編號")]
    [MaxLength(50)]
    public string? EmployeeNo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsAD == "Y")
        {
            if (!String.IsNullOrWhiteSpace(Mima))
                yield return new ValidationResult("AD 帳號不能有密碼", new[] { "Mima" });
        }
        else
        {
            if (Mima == null)
                yield return new ValidationResult("密碼不能為空", new[] { "Mima" });
            else if (Mima.Length < 8)
                yield return new ValidationResult("密碼長度至少8個字", new[] { "Mima" });
        }
    }
}
