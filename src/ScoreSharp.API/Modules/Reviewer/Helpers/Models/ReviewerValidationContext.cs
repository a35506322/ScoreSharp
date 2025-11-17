namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 封裝徵審驗證所需的上下文資料。
/// </summary>
public sealed class ReviewerValidationContext
{
    /// <summary>
    /// 案件資訊。
    /// </summary>
    public CaseInfoContext CaseInfo { get; set; }

    /// <summary>
    /// 正卡人資料格式檢核所需欄位。
    /// </summary>
    public ReviewerMainDataContext? MainData { get; set; }

    /// <summary>
    /// 附卡人資料格式檢核所需欄位。
    /// </summary>
    public ReviewerSupplementaryDataContext? SupplementaryData { get; set; }

    /// <summary>
    /// 正卡人地址相關欄位。
    /// </summary>
    public ReviewerMainAddressContext? MainAddress { get; set; }

    /// <summary>
    /// 附卡人地址相關欄位。
    /// </summary>
    public ReviewerSupplementaryAddressContext? SupplementaryAddress { get; set; }

    /// <summary>
    /// 正卡人金融檢核資料。
    /// </summary>
    public ReviewerFinanceCheckMainContext? MainFinanceCheck { get; set; }

    /// <summary>
    /// 附卡人金融檢核資料。
    /// </summary>
    public ReviewerFinanceCheckSupplementaryContext? SupplementaryFinanceCheck { get; set; }

    /// <summary>
    /// 銀行追蹤檢核相關資料。
    /// </summary>
    public ReviewerMainBankTraceContext? MainBankTrace { get; set; }

    /// <summary>
    /// 申請卡片狀態清單，用於判斷額外檢核條件。
    /// </summary>
    public IReadOnlyCollection<HandleInfoContext> HandleStatuses { get; set; } = Array.Empty<HandleInfoContext>();
}
