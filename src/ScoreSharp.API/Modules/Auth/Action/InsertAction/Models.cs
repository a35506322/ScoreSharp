namespace ScoreSharp.API.Modules.Auth.Action.InsertAction;

public class InsertActionRequest
{
    /// <summary>
    /// 英數字，API Action 名稱
    /// </summary>
    [Display(Name = "API Action名稱(英文)")]
    [MaxLength(100)]
    [Required]
    public string ActionId { get; set; } = null!;

    /// <summary>
    /// 中文，前端顯示功能
    /// </summary>

    [Display(Name = "API Action 中文")]
    [MaxLength(30)]
    [Required]
    public string ActionName { get; set; } = null!;

    /// <summary>
    /// Y/N，如果是Y 不檢查權限
    /// </summary>
    [Display(Name = "是否是通用資料")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsCommon { get; set; } = null!;

    /// <summary>
    /// Y/N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; } = null!;

    /// <summary>
    /// 關聯Auth_Router
    /// </summary>
    [Display(Name = "路由PK")]
    [MaxLength(50)]
    [Required]
    public string RouterId { get; set; } = null!;
}
