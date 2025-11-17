namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 附卡人金融檢核欄位。
/// </summary>
public sealed class ReviewerFinanceCheckSupplementaryContext
{
    /// <summary>
    /// 身分證字號。
    /// </summary>
    public string ID { get; init; }

    /// <summary>
    /// 使用者類型。
    /// </summary>
    public UserType UserType { get; init; } = UserType.附卡人;

    /// <summary>
    /// 附卡人中文姓名。
    /// </summary>
    public string? CHName { get; init; }

    /// <summary>
    /// 附卡人 929 檢核結果。
    /// </summary>
    public string? Checked929 { get; init; }

    /// <summary>
    /// 附卡人 929 檢核日期時間。
    /// </summary>
    public DateTime? Q929_QueryTime { get; init; }

    /// <summary>
    /// 附卡人姓名檢核結果。
    /// </summary>
    public string? NameChecked { get; init; }

    /// <summary>
    /// 關注名單一檢核結果。
    /// </summary>
    public string? Focus1Check { get; init; }

    /// <summary>
    /// 關注名單一命中代碼。
    /// </summary>
    public string? Focus1Hit { get; init; }

    /// <summary>
    /// 關注名單一檢核日期時間。
    /// </summary>
    public DateTime? Focus1_QueryTime { get; init; }

    /// <summary>
    /// 關注名單二檢核結果。
    /// </summary>
    public string? Focus2Check { get; init; }

    /// <summary>
    /// 關注名單二命中代碼。
    /// </summary>
    public string? Focus2Hit { get; init; }

    /// <summary>
    /// 關注名單二檢核日期時間。
    /// </summary>
    public DateTime? Focus2_QueryTime { get; init; }
}
