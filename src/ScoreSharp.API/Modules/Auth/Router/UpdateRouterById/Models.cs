namespace ScoreSharp.API.Modules.Auth.Router.UpdateRouterById;

public class UpdateRouterByIdRequest
{
    /// <summary>
    /// 英數字，前端顯示不在網址，如：Todo
    /// </summary>
    [Display(Name = "路由PK")]
    [MaxLength(50)]
    [Required]
    public string RouterId { get; set; }

    /// <summary>
    /// 中文，前端顯示SideBar頁面名稱
    /// </summary>
    [Display(Name = "路由名稱")]
    [MaxLength(30)]
    [Required]
    public string RouterName { get; set; }

    /// <summary>
    /// 給前端串接參數使用，如：/Todo/1 或 /Todo?params=
    /// </summary>
    [Display(Name = "動態參數")]
    [MaxLength(100)]
    public string? DynamicParams { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }

    /// <summary>
    /// 用於裝飾前端網頁
    /// </summary>
    [Display(Name = "icon")]
    [MaxLength(15)]
    public string? Icon { get; set; }

    /// <summary>
    /// 關聯Auth_RouterCategory
    /// </summary>
    [Display(Name = "路由類別(英文)")]
    [Required]
    public string RouterCategoryId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Display(Name = "排序")]
    [Required]
    [Range(1, 99)]
    public int Sort { get; set; }
}
