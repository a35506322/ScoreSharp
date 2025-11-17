namespace ScoreSharp.API.Modules.Auth.Role.UpdateRoleById;

public class UpdateRoleByIdRequest
{
    /// <summary>
    /// 角色PK(英數字)
    /// </summary>
    [Display(Name = "角色PK")]
    [MaxLength(50)]
    [Required]
    public string RoleId { get; set; }

    /// <summary>
    /// 角色名稱(中文)
    /// </summary>
    [Display(Name = "角色名稱")]
    [MaxLength(30)]
    public string? RoleName { get; set; }

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}
