namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 銀行追蹤命中項目欄位。
/// </summary>
public sealed class ReviewerMainBankTraceContext
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
    /// 是否命中行內 IP 相同。
    /// </summary>
    public string? EqualInternalIP_Flag { get; init; }

    /// <summary>
    /// 行內 IP 相同確認紀錄。
    /// </summary>
    public string? EqualInternalIP_CheckRecord { get; init; }

    /// <summary>
    /// 行內 IP 相同是否判定異常。
    /// </summary>
    public string? EqualInternalIP_IsError { get; init; }

    /// <summary>
    /// 是否命中相同 IP 比對。
    /// </summary>
    public string? SameIP_Flag { get; init; }

    /// <summary>
    /// 相同 IP 比對確認紀錄。
    /// </summary>
    public string? SameIP_CheckRecord { get; init; }

    /// <summary>
    /// 相同 IP 比對是否判定異常。
    /// </summary>
    public string? SameIP_IsError { get; init; }

    /// <summary>
    /// 是否命中電子信箱相同。
    /// </summary>
    public string? SameEmail_Flag { get; init; }

    /// <summary>
    /// 電子信箱相同確認紀錄。
    /// </summary>
    public string? SameEmail_CheckRecord { get; init; }

    /// <summary>
    /// 電子信箱相同是否判定異常。
    /// </summary>
    public string? SameEmail_IsError { get; init; }

    /// <summary>
    /// 是否命中電話相同。
    /// </summary>
    public string? SameMobile_Flag { get; init; }

    /// <summary>
    /// 電話相同確認紀錄。
    /// </summary>
    public string? SameMobile_CheckRecord { get; init; }

    /// <summary>
    /// 電話相同是否判定異常。
    /// </summary>
    public string? SameMobile_IsError { get; init; }

    /// <summary>
    /// 是否命中行內 Email 相同。
    /// </summary>
    public string? InternalEmailSame_Flag { get; init; }

    /// <summary>
    /// 行內 Email 相同確認紀錄。
    /// </summary>
    public string? InternalEmailSame_CheckRecord { get; init; }

    /// <summary>
    /// 行內 Email 相同是否判定異常。
    /// </summary>
    public string? InternalEmailSame_IsError { get; init; }

    /// <summary>
    /// 是否命中行內手機相同。
    /// </summary>
    public string? InternalMobileSame_Flag { get; init; }

    /// <summary>
    /// 行內手機相同確認紀錄。
    /// </summary>
    public string? InternalMobileSame_CheckRecord { get; init; }

    /// <summary>
    /// 行內手機相同是否判定異常。
    /// </summary>
    public string? InternalMobileSame_IsError { get; init; }

    /// <summary>
    /// 是否命中短時間頻繁申請。
    /// </summary>
    public string? ShortTimeID_Flag { get; init; }

    /// <summary>
    /// 短時間頻繁申請確認紀錄。
    /// </summary>
    public string? ShortTimeID_CheckRecord { get; init; }

    /// <summary>
    /// 短時間頻繁申請是否判定異常。
    /// </summary>
    public string? ShortTimeID_IsError { get; init; }
}
