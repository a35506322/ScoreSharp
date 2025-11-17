namespace ScoreSharp.API.Modules.Auth.Action.GetActionsByQueryString;

public class GetActionByQueryStringRequest
{
    /// <summary>
    /// Y/N，如果是Y 不檢查權限
    /// </summary>
    [Display(Name = "是否是通用資料")]
    [RegularExpression("[YN]")]
    public string? IsCommon { get; set; } = null!;

    /// <summary>
    /// Y/N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; } = null!;

    /// <summary>
    /// 關聯Auth_Router
    /// </summary>
    [Display(Name = "路由PK")]
    [MaxLength(50)]
    public string? RouterId { get; set; } = null!;
}

public class GetActionsByQueryStringResponse
{
    /// <summary>
    /// 英數字，API Action 名稱
    /// </summary>
    public string ActionId { get; set; } = null!;

    /// <summary>
    /// 中文，前端顯示功能
    /// </summary>
    public string ActionName { get; set; } = null!;

    /// <summary>
    /// Y/N，如果是Y 不檢查全縣
    /// </summary>
    public string IsCommon { get; set; } = null!;

    /// <summary>
    /// Y/N
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

    /// <summary>
    /// 關聯Auth_Router
    /// </summary>
    public string RouterId { get; set; } = null!;
}
