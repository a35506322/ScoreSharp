namespace ScoreSharp.API.Modules.Manage.Common.Helpers.Models;

/// <summary>
/// 未分派案件基礎資料
/// </summary>
public class UnassignedCaseBaseData
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 來源：1.ECARD, 2.APP, 3.紙本
    /// </summary>
    public Source Source { get; set; }

    /// <summary>
    /// 正卡人姓名檢核結果
    /// </summary>
    public string? MainNameChecked { get; set; }

    /// <summary>
    /// 附卡人姓名檢核結果
    /// </summary>
    public string? SuppNameChecked { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 申請卡別
    /// </summary>
    public string ApplyCardType { get; set; }
}
