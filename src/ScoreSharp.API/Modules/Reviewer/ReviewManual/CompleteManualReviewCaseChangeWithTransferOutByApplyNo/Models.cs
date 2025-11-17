using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferOutByApplyNo;

public class CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest : IValidatableObject
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
            CaseChangeAction == ManualReviewAction.核卡作業
            || CaseChangeAction == ManualReviewAction.退件作業
            || CaseChangeAction == ManualReviewAction.補件作業
            || CaseChangeAction == ManualReviewAction.撤件作業
        )
        {
            yield return new ValidationResult("權限外不能進行核卡作業、退件作業、補件作業、撤件作業", new[] { nameof(CaseChangeAction) });
        }
    }
}

public class CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = string.Empty;

    /// <summary>
    /// 需要重試的檢核項目
    /// </summary>
    public List<RetryCheck> RetryChecks { get; set; } = [];
}

public class CaseDataBundle
{
    public Reviewer_ApplyCreditCardInfoMain Main { get; set; }
    public Reviewer_FinanceCheckInfo MainFinanceCheck { get; set; }
    public List<Reviewer_ApplyCreditCardInfoHandle> Handles { get; set; }
    public Reviewer_ApplyCreditCardInfoSupplementary? Supplementary { get; set; }
    public Reviewer_FinanceCheckInfo? SupplementaryFinanceCheck { get; set; }
    public Reviewer_BankTrace BankTrace { get; set; }
    public Dictionary<string, string> CardDict { get; set; }
}
