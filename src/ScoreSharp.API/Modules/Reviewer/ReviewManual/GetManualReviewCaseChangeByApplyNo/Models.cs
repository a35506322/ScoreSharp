namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.GetManualReviewCaseChangeByApplyNo;

public class GetManualReviewCaseChangeByApplyNoResponse
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
    public ManualReviewAction? CaseChangeAction { get; set; }

    /// <summary>
    /// 動作名稱
    /// </summary>
    [Display(Name = "動作名稱")]
    public string? CaseChangeActionName => CaseChangeAction?.ToString();

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
    /// 正附卡
    /// </summary>
    [Display(Name = "正附卡")]
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    [Display(Name = "正附卡名稱")]
    public string UserTypeName => UserType == UserType.正卡人 ? "正卡" : "附卡";

    /// <summary>
    /// 徵信代碼
    /// </summary>
    [Display(Name = "徵信代碼")]
    public string? CreditCheckCode { get; set; }

    /// <summary>
    /// 優惠辦法
    /// </summary>
    [Display(Name = "優惠辦法")]
    public string? CardPromotionCode { get; set; }

    /// <summary>
    /// 處理備註
    /// </summary>
    [Display(Name = "處理備註")]
    public string? HandleNote { get; set; }

    /// <summary>
    /// 補件原因代碼
    /// </summary>
    [Display(Name = "補件原因代碼")]
    public CodeInfo[] SupplementReasonCode { get; set; } = Array.Empty<CodeInfo>();

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
    public CodeInfo[] RejectionReasonCode { get; set; } = Array.Empty<CodeInfo>();

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
    /// 建議額度同原持卡人額度
    /// 當為原卡友時，才能進行勾選，其餘都是Ｎ
    /// </summary>
    [Display(Name = "建議額度同原持卡人額度")]
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
    public string? IsForceCard { get; set; }

    /// <summary>
    /// 核卡註記
    /// </summary>
    [Display(Name = "核卡註記")]
    public string? NuclearCardNote { get; set; }

    /// <summary>
    /// 是否完成
    ///
    /// 申請核卡中
    /// 申請退件中
    /// 申請補件中
    /// 申請撤件中
    /// 退件_等待完成本案徵審
    /// 補件_等待完成本案徵審
    /// 撤件_等待完成本案徵審
    /// 待預審案件
    /// 補回件
    /// 退回重審
    /// 申請核卡_等待完成本案徵審
    /// 申請退件_等待完成本案徵審
    /// 申請補件_等待完成本案徵審
    /// 申請撤件_等待完成本案徵審
    ///
    /// 可以更新其餘不行，
    /// 給予前端判斷此資料是否可以傳入
    /// UpdateManualReviewCaseChangeByApplyNo 及
    /// CompleteManualReviewCaseChangeByApplyNo
    /// </summary>
    [Display(Name = "是否完成")]
    public string IsCompleted =>
        this.CardStatus == CardStatus.申請核卡中
        || this.CardStatus == CardStatus.申請退件中
        || this.CardStatus == CardStatus.申請補件中
        || this.CardStatus == CardStatus.申請撤件中
        || this.CardStatus == CardStatus.退件_等待完成本案徵審
        || this.CardStatus == CardStatus.補件_等待完成本案徵審
        || this.CardStatus == CardStatus.撤件_等待完成本案徵審
        || this.CardStatus == CardStatus.核卡_等待完成本案徵審
        || this.CardStatus == CardStatus.補回件
        || this.CardStatus == CardStatus.退回重審
        || this.CardStatus == CardStatus.申請核卡_等待完成本案徵審
        || this.CardStatus == CardStatus.申請退件_等待完成本案徵審
        || this.CardStatus == CardStatus.申請補件_等待完成本案徵審
        || this.CardStatus == CardStatus.申請撤件_等待完成本案徵審
        || this.CardStatus == CardStatus.人工徵信中
            ? "N"
            : "Y";

    /// <summary>
    /// 是否為原持卡人
    /// </summary>
    [Display(Name = "是否為原持卡人")]
    public string IsOriginalCardholder { get; set; }
}

public class CodeInfo
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class GetManualReviewCaseChangeByApplyNoDto
{
    /// <summary>
    /// PK
    /// </summary>
    public string SeqNo { get; set; }

    /// <summary>
    /// 案件編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 案件異動動作
    /// </summary>
    public CaseChangeAction? CaseChangeAction { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 卡別代碼
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 卡別名稱
    /// </summary>
    public string ApplyCardTypeName { get; set; }

    /// <summary>
    /// 正附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 徵信代碼
    /// </summary>
    public string? CreditCheckCode { get; set; }

    /// <summary>
    /// 優惠辦法
    /// </summary>
    public string? CardPromotionCode { get; set; }

    /// <summary>
    /// 處理備註
    /// </summary>
    public string? HandleNote { get; set; }

    /// <summary>
    /// 補件原因代碼
    /// </summary>
    public string SupplementReasonCode { get; set; }

    /// <summary>
    /// 其他補件原因
    /// </summary>
    public string? OtherSupplementReason { get; set; }

    /// <summary>
    /// 補件註記
    /// </summary>
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
    public MailingAddressType? SupplementSendCardAddr { get; set; }

    /// <summary>
    /// 補件寄送地址 名稱
    /// </summary>
    public string? SupplementSendCardAddrName { get; set; }

    /// <summary>
    /// 撤件註記
    /// </summary>
    public string? WithdrawalNote { get; set; }

    /// <summary>
    /// 退件原因代碼
    /// </summary>
    public string? RejectionReasonCode { get; set; }

    /// <summary>
    /// 其他退件原因
    /// </summary>
    public string? OtherRejectionReason { get; set; }

    /// <summary>
    /// 退件註記
    /// </summary>
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
    public MailingAddressType? RejectionSendCardAddr { get; set; }

    /// <summary>
    /// 退件寄送地址 名稱
    /// </summary>
    public string? RejectionSendCardAddrName { get; set; }

    /// <summary>
    /// 是否列印簡訊、紙本通知函，Y｜N
    /// </summary>
    public string? IsPrintSMSAndPaper { get; set; }

    /// <summary>
    /// 建議額度同原持卡人額度
    /// 當為原卡友時，才能進行勾選，其餘都是Ｎ
    /// </summary>
    public string? IsOriginCardholderSameCardLimit { get; set; }

    /// <summary>
    /// 建議額度
    /// </summary>
    public int? CardLimit { get; set; }

    /// <summary>
    /// 是否強制發卡
    /// </summary>
    public string? IsForceCard { get; set; }

    /// <summary>
    /// 核卡註記
    /// </summary>
    public string? NuclearCardNote { get; set; }

    /// <summary>
    /// 是否為原持卡人
    /// </summary>
    public string? IsOriginalCardholder { get; set; }
}
