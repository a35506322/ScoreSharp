namespace ScoreSharp.API.Modules.OrgSetUp.User.UpdateUserById;

public class UpdateUserByIdRequest : IValidatableObject
{
    /// <summary>
    /// 使用者帳號
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
    /// 使用者姓名
    /// </summary>
    [Display(Name = "使用者姓名")]
    public string? UserName { get; set; }

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
    /// 停用原因，IsActive = N 需有值
    /// </summary>
    [Display(Name = "停用原因")]
    [MaxLength(100)]
    public string? StopReason { get; set; }

    /// <summary>
    /// 員工編號
    /// </summary>
    [Display(Name = "員工編號")]
    [MaxLength(50)]
    public string? EmployeeNo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsActive == "N" && string.IsNullOrEmpty(StopReason))
        {
            yield return new ValidationResult("是否啟用為否時，停用原因不能為空", new[] { "StopReason" });
        }

        if (IsActive == "Y" && !string.IsNullOrEmpty(StopReason))
        {
            yield return new ValidationResult("是否啟用為是時，停用原因不能有值", new[] { "StopReason" });
        }
    }
}
