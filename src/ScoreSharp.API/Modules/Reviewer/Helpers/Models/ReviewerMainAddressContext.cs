namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 正卡人地址驗證所需欄位集合。
/// </summary>
public sealed class ReviewerMainAddressContext
{
    /// <summary>
    /// 身分證字號。
    /// </summary>
    public string ID { get; init; }

    /// <summary>
    /// 使用者類型。
    /// </summary>
    public UserType UserType { get; init; } = UserType.正卡人;

    /// <summary>
    /// 正卡人中文姓名。
    /// </summary>
    public string? CHName { get; init; }

    /// <summary>
    /// 正卡人是否為原持卡人。
    /// </summary>
    public string IsOriginalCardholder { get; init; }

    /// <summary>
    /// 正卡人是否為學生身份。
    /// </summary>
    public string? IsStudent { get; init; }

    /// <summary>
    /// 正卡人戶籍地址郵遞區號。
    /// </summary>
    public string? Reg_ZipCode { get; init; }

    /// <summary>
    /// 正卡人戶籍地址縣市。
    /// </summary>
    public string? Reg_City { get; init; }

    /// <summary>
    /// 正卡人戶籍地址區域。
    /// </summary>
    public string? Reg_District { get; init; }

    /// <summary>
    /// 正卡人戶籍地址路名。
    /// </summary>
    public string? Reg_Road { get; init; }

    /// <summary>
    /// 正卡人戶籍地址門牌號。
    /// </summary>
    public string? Reg_Number { get; init; }

    /// <summary>
    /// 正卡人戶籍地址完整地址。
    /// </summary>
    public string? Reg_FullAddr { get; init; }

    /// <summary>
    /// 正卡人居住地址郵遞區號。
    /// </summary>
    public string? Live_ZipCode { get; init; }

    /// <summary>
    /// 正卡人居住地址縣市。
    /// </summary>
    public string? Live_City { get; init; }

    /// <summary>
    /// 正卡人居住地址區域。
    /// </summary>
    public string? Live_District { get; init; }

    /// <summary>
    /// 正卡人居住地址路名。
    /// </summary>
    public string? Live_Road { get; init; }

    /// <summary>
    /// 正卡人居住地址門牌號。
    /// </summary>
    public string? Live_Number { get; init; }

    /// <summary>
    /// 正卡人居住地址完整地址。
    /// </summary>
    public string? Live_FullAddr { get; init; }

    /// <summary>
    /// 正卡人帳單地址郵遞區號。
    /// </summary>
    public string? Bill_ZipCode { get; init; }

    /// <summary>
    /// 正卡人帳單地址縣市。
    /// </summary>
    public string? Bill_City { get; init; }

    /// <summary>
    /// 正卡人帳單地址區域。
    /// </summary>
    public string? Bill_District { get; init; }

    /// <summary>
    /// 正卡人帳單地址路名。
    /// </summary>
    public string? Bill_Road { get; init; }

    /// <summary>
    /// 正卡人帳單地址門牌號。
    /// </summary>
    public string? Bill_Number { get; init; }

    /// <summary>
    /// 正卡人帳單地址完整地址。
    /// </summary>
    public string? Bill_FullAddr { get; init; }

    /// <summary>
    /// 正卡人寄卡地址郵遞區號。
    /// </summary>
    public string? SendCard_ZipCode { get; init; }

    /// <summary>
    /// 正卡人寄卡地址縣市。
    /// </summary>
    public string? SendCard_City { get; init; }

    /// <summary>
    /// 正卡人寄卡地址區域。
    /// </summary>
    public string? SendCard_District { get; init; }

    /// <summary>
    /// 正卡人寄卡地址路名。
    /// </summary>
    public string? SendCard_Road { get; init; }

    /// <summary>
    /// 正卡人寄卡地址門牌號。
    /// </summary>
    public string? SendCard_Number { get; init; }

    /// <summary>
    /// 正卡人寄卡地址完整地址。
    /// </summary>
    public string? SendCard_FullAddr { get; init; }

    /// <summary>
    /// 正卡人家長地址郵遞區號。
    /// </summary>
    public string? ParentLive_ZipCode { get; init; }

    /// <summary>
    /// 正卡人家長地址縣市。
    /// </summary>
    public string? ParentLive_City { get; init; }

    /// <summary>
    /// 正卡人家長地址區域。
    /// </summary>
    public string? ParentLive_District { get; init; }

    /// <summary>
    /// 正卡人家長地址路名。
    /// </summary>
    public string? ParentLive_Road { get; init; }

    /// <summary>
    /// 正卡人家長地址門牌號。
    /// </summary>
    public string? ParentLive_Number { get; init; }

    /// <summary>
    /// 正卡人家長地址完整地址。
    /// </summary>
    public string? ParentLive_FullAddr { get; init; }
}
