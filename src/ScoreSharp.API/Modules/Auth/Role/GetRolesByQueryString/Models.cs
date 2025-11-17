namespace ScoreSharp.API.Modules.Auth.Role.GetRolesByQueryString;

public class GetRolesByQueryStringRequest
{
    /// <summary>
    /// 是否啟用，Y|N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}

public class GetRolesByQueryStringResponse
{
    /// <summary>
    /// 角色PK(英數字)
    /// </summary>
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色名稱(中文)
    /// </summary>
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    public string IsActive { get; set; } = null!;

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
