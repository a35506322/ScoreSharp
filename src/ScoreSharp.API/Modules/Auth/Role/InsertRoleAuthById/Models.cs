namespace ScoreSharp.API.Modules.Auth.Role.InsertRoleAuthById;

public class InsertRoleAuthByIdRequest
{
    /// <summary>
    /// 關聯Auth_Role
    /// </summary>
    [Required]
    public string RoleId { get; set; }

    /// <summary>
    /// 關聯Auth_Router
    /// </summary>
    [Required]
    public string RouterId { get; set; }

    /// <summary>
    /// 關聯Auth_Action
    /// </summary>
    [Required]
    public string ActionId { get; set; }
}
