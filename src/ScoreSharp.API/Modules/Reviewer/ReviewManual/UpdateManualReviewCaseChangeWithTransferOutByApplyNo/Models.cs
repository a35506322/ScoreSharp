namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.UpdateManualReviewCaseChangeWithTransferOutByApplyNo;

public class UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest : IValidatableObject
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
    public ManualReviewAction CaseChangeAction { get; set; }

    #region 補件
    /// <summary>
    /// 補件原因代碼
    /// 若補件原因代碼為複數個，請以陣列回傳
    /// </summary>
    [Display(Name = "補件原因代碼")]
    public string[]? SupplementReasonCode { get; set; } = Array.Empty<string>();

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
    public string[]? RejectionReasonCode { get; set; } = Array.Empty<string>();

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

    #region 核卡
    /// <summary>
    /// 建議額度同原持卡人額度
    /// 當為原卡友時，才能進行勾選，其餘都是Ｎ
    /// </summary>
    [Display(Name = "建議額度同原持卡人額度")]
    [RegularExpression("[YN]")]
    public string? IsOriginCardholderSameCardLimit { get; set; }

    /// <summary>
    /// 建議額度
    /// </summary>
    [Display(Name = "建議額度")]
    public int? CardLimit { get; set; }

    /// <summary>
    /// 是否強制發卡
    /// </summary>
    [Display(Name = "是否強制發卡")]
    [RegularExpression("[YN]")]
    public string? IsForceCard { get; set; }

    /// <summary>
    /// 核卡註記
    /// </summary>
    [Display(Name = "核卡註記")]
    [MaxLength(100)]
    public string? NuclearCardNote { get; set; }

    /// <summary>
    /// 徵信代碼
    /// </summary>
    [Display(Name = "徵信代碼")]
    public string? CreditCheckCode { get; set; }
    #endregion

    /// <summary>
    /// 是否列印簡訊、紙本通知函，Y｜N
    /// 撤件 就是 => N
    /// </summary>
    [Display(Name = "是否列印簡訊、紙本通知函")]
    [RegularExpression("[YN]")]
    public string? IsPrintSMSAndPaper { get; set; }

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

        // 排入核卡 必填欄位　=> 建議額度、徵信代碼
        if (CaseChangeAction == ManualReviewAction.排入核卡)
        {
            if (string.IsNullOrEmpty(CreditCheckCode))
            {
                yield return new ValidationResult("徵信代碼不能為空", new[] { nameof(CreditCheckCode) });
            }

            if (CardLimit == null)
            {
                yield return new ValidationResult("建議額度不能為空", new[] { nameof(CardLimit) });
            }
        }

        // 排入退件 必填欄位　=> 退件原因代碼 、 (是否列印簡訊、紙本通知函) 、 退件寄送地址
        if (CaseChangeAction == ManualReviewAction.排入退件)
        {
            if (RejectionReasonCode == null || RejectionReasonCode.Length == 0)
            {
                yield return new ValidationResult("退件原因代碼不能為空", new[] { nameof(RejectionReasonCode) });
            }

            if (string.IsNullOrWhiteSpace(IsPrintSMSAndPaper))
            {
                yield return new ValidationResult("是否列印簡訊、紙本通知函不能為空", new[] { nameof(IsPrintSMSAndPaper) });
            }

            if (RejectionSendCardAddr == null)
            {
                yield return new ValidationResult("退件寄送地址不能為空", new[] { nameof(RejectionSendCardAddr) });
            }
        }

        // 排入補件 必填欄位　=> 補件原因代碼 、 (是否列印簡訊、紙本通知函) 、 補件寄送地址
        if (CaseChangeAction == ManualReviewAction.排入補件)
        {
            if (SupplementReasonCode == null || SupplementReasonCode.Length == 0)
            {
                yield return new ValidationResult("補件原因代碼不能為空", new[] { nameof(SupplementReasonCode) });
            }

            if (string.IsNullOrWhiteSpace(IsPrintSMSAndPaper))
            {
                yield return new ValidationResult("是否列印簡訊、紙本通知函不能為空", new[] { nameof(IsPrintSMSAndPaper) });
            }

            if (SupplementSendCardAddr == null)
            {
                yield return new ValidationResult("補件寄送地址不能為空", new[] { nameof(SupplementSendCardAddr) });
            }
        }

        // 排入撤件 必填欄位　=> 撤件註記
        if (CaseChangeAction == ManualReviewAction.排入撤件)
        {
            if (string.IsNullOrWhiteSpace(WithdrawalNote))
            {
                yield return new ValidationResult("撤件註記不能為空", new[] { nameof(WithdrawalNote) });
            }
        }
    }
}
