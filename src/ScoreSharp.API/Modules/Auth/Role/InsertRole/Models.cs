namespace ScoreSharp.API.Modules.Auth.Role.InsertRole;

public class InsertRoleRequest
{
    /// <summary>
    /// 角色PK(英數字)
    /// </summary>
    [Display(Name = "角色PK")]
    [MaxLength(50)]
    [Required]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色名稱(中文)
    /// </summary>
    [Display(Name = "角色名稱")]
    [MaxLength(30)]
    [Required]
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; } = null!;
}
