namespace ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoryById;

public class GetRouterCategoryByIdResponse
{
    /// <summary>
    /// 英數字, 前端顯示網址; 例: TodoCategory
    /// </summary>
    public string RouterCategoryId { get; set; } = null!;

    /// <summary>
    /// 中文, 前端顯示SideBar類別
    /// </summary>
    public string RouterCategoryName { get; set; } = null!;

    /// <summary>
    /// Y | N
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
    /// 修改員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修改時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 用於裝飾前端網頁
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
