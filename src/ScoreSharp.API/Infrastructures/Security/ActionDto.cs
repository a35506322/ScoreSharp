namespace ScoreSharp.API.Infrastructures.Security;

public class ActionDto
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
}
