namespace ScoreSharp.API.Modules.Auth.RouterCategory.InsertRouterCategory;

public class InsertRouterCategoryRequest
{
    /// <summary>
    /// 英數字, 前端顯示網址; 例: TodoCategory
    /// </summary>
    [Display(Name = "路由類別(英文)")]
    [Required]
    public string RouterCategoryId { get; set; }

    /// <summary>
    /// 中文, 前端顯示SideBar類別
    /// </summary>
    [Display(Name = "路由類別(中文)")]
    [Required]
    public string RouterCategoryName { get; set; }

    /// <summary>
    /// 是否啟用 Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }

    /// <summary>
    /// 用於裝飾前端網頁
    /// </summary>
    [Display(Name = "Icon")]
    public string? Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Display(Name = "排序")]
    [Required]
    [Range(1, 99)]
    public int Sort { get; set; }
}
