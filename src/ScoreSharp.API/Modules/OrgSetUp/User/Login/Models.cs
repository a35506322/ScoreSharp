namespace ScoreSharp.API.Modules.OrgSetUp.User.Login;

public class LoginRequest
{
    /// <summary>
    /// 帳號
    /// </summary>
    [Display(Name = "帳號")]
    public string UserId { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    [Display(Name = "密碼")]
    public string Mima { get; set; }
}

public class LoginResponse
{
    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 幾分鐘後到期
    /// </summary>
    public int ExpireMinutes { get; set; }
}

public class UserRoleDto
{
    public UserRoleDto(string userId, string roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    /// <summary>
    /// 使用者帳號
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 關聯Auth_Role
    /// </summary>
    public string RoleId { get; set; } = null!;
}
