namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetCaseChangeByApplyNo;

public class GetCaseChangeByApplyNoDto
{
    public string SeqNo { get; set; }

    public string ApplyNo { get; set; }

    public IncomeConfirmationAction? CaseChangeAction { get; set; }

    public CardStatus CardStatus { get; set; }

    public string CardStatusName => CardStatus.ToString();

    public string ApplyCardType { get; set; }

    public string ApplyCardTypeName { get; set; }

    public string? CreditCheckCode { get; set; }

    public string CardPromotionCode { get; set; }

    public string? SupplementReasonCode { get; set; }

    public string? OtherSupplementReason { get; set; }

    public string? SupplementNote { get; set; }

    public MailingAddressType? SupplementSendCardAddr { get; set; }

    public string? WithdrawalNote { get; set; }

    public string? RejectionReasonCode { get; set; }

    public string? OtherRejectionReason { get; set; }

    public string? RejectionNote { get; set; }

    public MailingAddressType? RejectionSendCardAddr { get; set; }

    public string? IsPrintSMSAndPaper { get; set; }

    public UserType UserType { get; set; }
}

public class GetCaseChangeByApplyNoResponse
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "PK")]
    public string SeqNo { get; set; }

    /// <summary>
    /// 案件編號
    /// </summary>
    [Display(Name = "案件編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 案件異動動作
    /// </summary>
    [Display(Name = "案件異動動作")]
    public IncomeConfirmationAction? CaseChangeAction { get; set; }

    /// <summary>
    /// 動作名稱
    /// </summary>
    [Display(Name = "動作名稱")]
    public string? CaseChangeActionName { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    [Display(Name = "狀態")]
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 狀態名稱
    /// </summary>
    [Display(Name = "狀態名稱")]
    public string CardStatusName => CardStatus.ToString();

    /// <summary>
    /// 卡別代碼
    /// </summary>
    [Display(Name = "卡別代碼")]
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 卡別名稱
    /// </summary>
    [Display(Name = "卡別名稱")]
    public string ApplyCardTypeName { get; set; }

    /// <summary>
    /// 優惠辦法
    /// </summary>
    [Display(Name = "優惠辦法")]
    public string CardPromotionCode { get; set; }

    /// <summary>
    /// 補件原因代碼
    /// </summary>
    [Display(Name = "補件原因代碼")]
    public CodeInfo[]? SupplementReasonCode { get; set; }

    /// <summary>
    /// 其他補件原因
    /// </summary>
    [Display(Name = "其他補件原因")]
    public string? OtherSupplementReason { get; set; }

    /// <summary>
    /// 補件註記
    /// </summary>
    [Display(Name = "補件註記")]
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
    public MailingAddressType? SupplementSendCardAddr { get; set; }

    /// <summary>
    /// 補件寄送地址 名稱
    /// </summary>
    [Display(Name = "補件寄送地址名稱")]
    public string? SupplementSendCardAddrName { get; set; }

    /// <summary>
    /// 撤件註記
    /// </summary>
    [Display(Name = "撤件註記")]
    public string? WithdrawalNote { get; set; }

    /// <summary>
    /// 退件原因代碼
    /// </summary>
    [Display(Name = "退件原因代碼")]
    public CodeInfo[]? RejectionReasonCode { get; set; }

    /// <summary>
    /// 其他退件原因
    /// </summary>
    [Display(Name = "其他退件原因")]
    public string? OtherRejectionReason { get; set; }

    /// <summary>
    /// 退件註記
    /// </summary>
    [Display(Name = "退件註記")]
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
    public MailingAddressType? RejectionSendCardAddr { get; set; }

    /// <summary>
    /// 退件寄送地址 名稱
    /// </summary>
    [Display(Name = "退件寄送地址名稱")]
    public string? RejectionSendCardAddrName { get; set; }

    /// <summary>
    /// 是否列印簡訊、紙本通知函，Y｜N
    /// </summary>
    [Display(Name = "是否列印簡訊、紙本通知函")]
    public string? IsPrintSMSAndPaper { get; set; }

    /// <summary>
    /// 正附卡
    /// </summary>
    [Display(Name = "正附卡")]
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    [Display(Name = "正附卡名稱")]
    public string UserTypeName => UserType == UserType.正卡人 ? "正卡" : "附卡";
}

public class CodeInfo
{
    public string Code { get; set; }
    public string Name { get; set; }
}
