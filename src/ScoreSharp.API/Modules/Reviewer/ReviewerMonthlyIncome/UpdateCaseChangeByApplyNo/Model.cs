namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateCaseChangeByApplyNo;

public class UpdateCaseChangeByApplyNoRequest : IValidatableObject
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "PK")]
    [Required]
    public string SeqNo { get; set; }

    /// <summary>
    /// 案件編號
    /// </summary>
    [Display(Name = "案件編號")]
    [Required]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 案件異動動作
    /// </summary>
    [Display(Name = "案件異動動作")]
    [ValidEnumValue]
    [Required]
    public IncomeConfirmationAction CaseChangeAction { get; set; }

    #region 補件
    /// <summary>
    /// 補件原因代碼
    /// 若補件原因代碼為複數個，請以陣列回傳
    /// </summary>
    [Display(Name = "補件原因代碼")]
    public string[]? SupplementReasonCode { get; set; }

    /// <summary>
    /// 其他補件原因
    /// </summary>
    [Display(Name = "其他補件原因")]
    [MaxLength(100)]
    public string? OtherSupplementReason { get; set; }

    /// <summary>
    /// 補件註記
    /// </summary>
    [Display(Name = "補件註記")]
    [MaxLength(100)]
    public string? SupplementNote { get; set; }

    /// <summary>
    /// 補件寄送地址
    /// 1. 帳單地址
    /// 2. 戶籍地址
    /// 3. 公司地址
    /// 4. 居住地址
    /// 需檢驗選擇的地址完整性
    /// 查看 Reviewer_ApplyCreditCardInfoAddress 完整地址
    /// </summary>
    [Display(Name = "補件寄送地址")]
    [ValidEnumValue]
    public MailingAddressType? SupplementSendCardAddr { get; set; }

    #endregion

    #region 撤件
    /// <summary>
    /// 撤件註記
    /// </summary>
    [Display(Name = "撤件註記")]
    [MaxLength(100)]
    public string? WithdrawalNote { get; set; }

    #endregion

    #region 退件
    /// <summary>
    /// 退件原因代碼
    /// 若退件原因代碼為複數個，請以陣列回傳
    /// </summary>
    [Display(Name = "退件原因代碼")]
    public string[]? RejectionReasonCode { get; set; }

    /// <summary>
    /// 其他退件原因
    /// </summary>
    [Display(Name = "其他退件原因")]
    [MaxLength(100)]
    public string? OtherRejectionReason { get; set; }

    /// <summary>
    /// 退件註記
    /// </summary>
    [Display(Name = "退件註記")]
    [MaxLength(100)]
    public string? RejectionNote { get; set; }

    /// <summary>
    /// 退件寄送地址
    /// 1. 帳單地址
    /// 2. 戶籍地址
    /// 3. 公司地址
    /// 4. 居住地址
    /// 需檢驗選擇的地址完整性
    /// 查看 Reviewer_ApplyCreditCardInfoAddress 完整地址
    /// </summary>
    [Display(Name = "退件寄送地址")]
    [ValidEnumValue]
    public MailingAddressType? RejectionSendCardAddr { get; set; }

    #endregion

    /// <summary>
    /// 是否列印簡訊、紙本通知函，Y｜N
    /// 撤件 就是 => N
    /// </summary>
    [Display(Name = "是否列印簡訊、紙本通知函")]
    [RegularExpression("[YN]")]
    public string? IsPrintSMSAndPaper { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CaseChangeAction == IncomeConfirmationAction.撤件作業)
        {
            if (string.IsNullOrEmpty(WithdrawalNote))
                yield return new ValidationResult("當異動為撤件時，撤件註記為必填。", new[] { "WithdrawalNote" });
        }
        else if (CaseChangeAction == IncomeConfirmationAction.退件作業)
        {
            // 1. 驗證退件原因代碼
            if (RejectionReasonCode is null || RejectionReasonCode?.Length == 0)
            {
                yield return new ValidationResult("當異動為退件時，退件原因代碼為必填。", new[] { "RejectionReasonCode" });
            }

            // 2. 驗證退件寄送地址
            if (RejectionSendCardAddr is null)
            {
                yield return new ValidationResult("當異動為退件時，退件寄送地址為必填。", new[] { "RejectionSendCardAddr" });
            }

            if (string.IsNullOrEmpty(IsPrintSMSAndPaper))
                yield return new ValidationResult("當異動為退件時，是否列印簡訊、紙本通知函必填。", new[] { "IsPrintSMSAndPaper" });
        }
        else if (CaseChangeAction == IncomeConfirmationAction.補件作業)
        {
            // 1. 驗證補件原因代碼
            if (SupplementReasonCode is null || SupplementReasonCode?.Length == 0)
            {
                yield return new ValidationResult("當異動為補件時，補件原因代碼為必填。", new[] { "SupplementReasonCode" });
            }

            // 2. 驗證補件寄送地址
            if (SupplementSendCardAddr is null)
            {
                yield return new ValidationResult("當異動為補件時，補件寄送地址為必填。", new[] { "SupplementSendCardAddr" });
                yield break;
            }

            if (string.IsNullOrEmpty(IsPrintSMSAndPaper))
                yield return new ValidationResult("當異動為補件時，是否列印簡訊、紙本通知函必填。", new[] { "IsPrintSMSAndPaper" });
        }
    }
}
