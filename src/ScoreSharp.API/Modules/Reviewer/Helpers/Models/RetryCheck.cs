namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

public class RetryCheck
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 對應欄位名稱
    /// 例如：InternalEmailSame_Flag
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// API URL
    /// </summary>
    public string APIUrl { get; set; } = string.Empty;

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 正附卡：1. 正卡人 2. 附卡人
    /// </summary>
    public UserType UserType { get; set; }
}
