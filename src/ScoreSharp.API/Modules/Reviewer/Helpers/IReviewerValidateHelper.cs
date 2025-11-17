using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Helpers;

public interface IReviewerValidateHelper
{
    /// <summary>
    /// 整體驗證入口，可依指定規則組合執行多項檢核。
    /// </summary>
    /// <param name="context">驗證所需欄位上下文。</param>
    /// <param name="rules">要執行的驗證規則旗標。</param>
    public ReviewerValidationResult Validate(ReviewerValidationContext context, ReviewerValidationRuleSet rules);

    /// <summary>
    /// 檢核資料格式（日期、Email、現職月收入等）。
    /// </summary>
    public ReviewerValidationResult ValidateDataFormat(
        CaseInfoContext caseInfo,
        ReviewerMainDataContext? main,
        ReviewerSupplementaryDataContext? supplementary,
        IReadOnlyCollection<HandleInfoContext> handleStatuses
    );

    /// <summary>
    /// 檢核主附卡地址欄位是否符合必填條件。
    /// </summary>
    public ReviewerValidationResult ValidateAddresses(
        CaseInfoContext caseInfo,
        ReviewerMainAddressContext? main,
        ReviewerSupplementaryAddressContext? supplementary
    );

    /// <summary>
    /// 檢核銀行追蹤命中後的異常註記與紀錄。
    /// </summary>
    public ReviewerValidationResult ValidateBankTrace(
        CaseInfoContext caseInfo,
        ReviewerMainBankTraceContext? main,
        ReviewerMainDataContext? mainData,
        IReadOnlyCollection<HandleInfoContext> handleStatuses
    );

    /// <summary>
    /// 檢核金融資料（929、姓名檢核、關注名單、KYC 等）。
    /// </summary>
    public ReviewerValidationResult ValidateFinanceChecks(
        CaseInfoContext caseInfo,
        ReviewerFinanceCheckMainContext? main,
        ReviewerFinanceCheckSupplementaryContext? supplementary,
        IReadOnlyCollection<HandleInfoContext> handleStatuses
    );
}
