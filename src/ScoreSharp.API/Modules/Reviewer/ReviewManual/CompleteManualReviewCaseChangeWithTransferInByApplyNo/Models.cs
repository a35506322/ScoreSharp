using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferInByApplyNo;

public class CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest : IValidatableObject
{
    [Display(Name = "Handle序號")]
    [Required]
    public string SeqNo { get; set; }

    /// <summary>
    /// 案件編號
    /// </summary>
    [Display(Name = "案件編號")]
    [Required]
    public string ApplyNo { get; set; }

    [Display(Name = "案件異動動作")]
    [Required]
    [ValidEnumValue]
    public ManualReviewAction CaseChangeAction { get; set; }

    /// <summary>
    /// 是否完成
    /// 前端帶入CompleteManualReviewCaseChangeByApplyNo 的IsComplete
    /// </summary>
    [Display(Name = "是否完成")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsCompleted { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (
            CaseChangeAction == ManualReviewAction.排入撤件
            || CaseChangeAction == ManualReviewAction.排入退件
            || CaseChangeAction == ManualReviewAction.排入補件
            || CaseChangeAction == ManualReviewAction.排入核卡
        )
        {
            yield return new ValidationResult("權限內不能進行排入撤件、排入退件、排入補件、排入核卡", new[] { nameof(CaseChangeAction) });
        }
    }
}

public class CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// KYC 回傳代碼，例如 K0000
    /// </summary>
    public string KYC_RtnCode { get; set; }

    /// <summary>
    /// KYC 回傳訊息，例如 成功
    /// </summary>
    public string KYC_Message { get; set; }

    /// <summary>
    /// EAIHUB 代碼
    /// </summary>
    public string KYC_Rc2 { get; set; }

    /// <summary>
    /// 例外訊息
    /// </summary>
    public string KYC_Exception { get; set; }

    /// <summary>
    /// 需要重試的檢核項目
    /// </summary>
    public List<RetryCheck> RetryChecks { get; set; } = [];
}

public class KYCContext
{
    /// <summary>
    /// KYC 回傳代碼，例如 K0000
    /// </summary>
    public string KYC_RtnCode { get; set; } = string.Empty;

    /// <summary>
    /// KYC 回傳訊息，例如 成功
    /// </summary>
    public string KYC_Message { get; set; } = string.Empty;

    /// <summary>
    /// EAIHUB 代碼
    /// </summary>
    public string KYC_Rc2 { get; set; } = string.Empty;

    /// <summary>
    /// 例外訊息
    /// </summary>
    public string KYC_Exception { get; set; } = string.Empty;
}

public class CaseDataBundle
{
    public Reviewer_ApplyCreditCardInfoMain Main { get; set; }
    public Reviewer_FinanceCheckInfo? MainFinanceCheck { get; set; }
    public List<Reviewer_ApplyCreditCardInfoHandle> Handles { get; set; }
    public Reviewer_ApplyCreditCardInfoSupplementary? Supplementary { get; set; }
    public Reviewer_FinanceCheckInfo? SupplementaryFinanceCheck { get; set; }
    public Reviewer_BankTrace? BankTrace { get; set; }
    public Dictionary<string, string> CardDict { get; set; }
}
