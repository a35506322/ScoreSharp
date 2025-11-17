using ScoreSharp.Common.Enums;

namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 申請卡片狀態資訊。
/// </summary>
public sealed class HandleInfoContext
{
    /// <summary>
    /// 卡片處理序號。
    /// </summary>
    public string HandleSeqNo { get; init; }

    /// <summary>
    /// 正附卡別。
    /// </summary>
    public UserType UserType { get; init; }

    /// <summary>
    /// 審核動作。
    /// </summary>
    public ReviewAction RecentAction { get; init; }

    /// <summary>
    /// 卡片流程階段。
    /// </summary>
    public CardStep? CardStep { get; init; }

    /// <summary>
    /// 申請卡別。
    /// </summary>
    public string ApplyCardType { get; init; }

    /// <summary>
    /// 是否為國旅卡。
    /// </summary>
    public string IsCITSCard { get; init; }
}
