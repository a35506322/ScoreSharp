namespace ScoreSharp.API.Modules.Auth.Router.GetRouterById;

public class GetRouterByIdResponse
{
    /// <summary>
    /// 英數字，前端顯示不在網址，如：Todo
    /// </summary>
    public string RouterId { get; set; }

    /// <summary>
    /// 中文，前端顯示SideBar頁面名稱
    /// </summary>
    public string RouterName { get; set; }

    /// <summary>
    /// 給前端串接參數使用，如：/Todo/1 或 /Todo?params=
    /// </summary>
    public string? DynamicParams { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    public string IsActive { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; }

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
    /// 關聯Auth_RouterCategory
    /// </summary>
    public string RouterCategoryId { get; set; }

    /// <summary>
    /// 用於裝飾前端網頁
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
