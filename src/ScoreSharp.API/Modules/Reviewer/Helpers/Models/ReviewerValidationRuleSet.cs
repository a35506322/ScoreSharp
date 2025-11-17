namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 徵審驗證規則集合，支援旗標組合。
/// </summary>
[Flags]
public enum ReviewerValidationRuleSet
{
    /// <summary>
    /// 不執行任何驗證。
    /// </summary>
    None = 0,

    /// <summary>
    /// 執行資料格式驗證。
    /// </summary>
    DataFormat = 1 << 0,

    /// <summary>
    /// 執行地址欄位驗證。
    /// </summary>
    Addresses = 1 << 1,

    /// <summary>
    /// 執行銀行追蹤驗證。
    /// </summary>
    BankTrace = 1 << 2,

    /// <summary>
    /// 執行金融檢核驗證。
    /// </summary>
    FinanceCheck = 1 << 3,
}
