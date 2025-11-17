namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 附卡人地址驗證所需欄位集合。
/// </summary>
public sealed class ReviewerSupplementaryAddressContext
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
    /// 附卡人是否為原持卡人。
    /// </summary>
    public string? IsOriginalCardholder { get; init; }

    /// <summary>
    /// 附卡人寄卡地址郵遞區號。
    /// </summary>
    public string? SendCard_ZipCode { get; init; }

    /// <summary>
    /// 附卡人寄卡地址縣市。
    /// </summary>
    public string? SendCard_City { get; init; }

    /// <summary>
    /// 附卡人寄卡地址區域。
    /// </summary>
    public string? SendCard_District { get; init; }

    /// <summary>
    /// 附卡人寄卡地址路名。
    /// </summary>
    public string? SendCard_Road { get; init; }

    /// <summary>
    /// 附卡人寄卡地址門牌號。
    /// </summary>
    public string? SendCard_Number { get; init; }

    /// <summary>
    /// 附卡人寄卡地址完整地址。
    /// </summary>
    public string? SendCard_FullAddr { get; init; }
}
