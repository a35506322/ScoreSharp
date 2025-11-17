namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCardStatus;

public class GetApplyCardStatusDto
{
    public string SeqNo { get; set; }
    public string ApplyCardType { get; set; }

    public string? CreditCheckCode { get; set; }

    public string? CardPromotionCode { get; set; }

    public CardStatus CardStatus { get; set; }

    public int? CardLimit { get; set; }

    public int? SupplementCount { get; set; }

    public UserType UserType { get; set; }
}

public class GetApplyCardStatusResponse
{
    [Display(Name = "PK")]
    public string SeqNo { get; set; }

    [Display(Name = "申請卡別")]
    public string ApplyCardType { get; set; }

    [Display(Name = "申請卡別名稱")]
    public string ApplyCardTypeName { get; set; }

    [Display(Name = "正附卡")]
    public UserType UserType { get; set; }

    [Display(Name = "正附卡名稱")]
    public string UserTypeName => UserType == UserType.正卡人 ? "正卡" : "附卡";

    [Display(Name = "徵信代碼")]
    public string? CreditCheckCode { get; set; }

    [Display(Name = "徵信代碼名稱")]
    public string? CreditCheckCodeName { get; set; }

    [Display(Name = "優惠辦法")]
    public string? CardPromotion { get; set; }

    [Display(Name = "優惠辦法名稱")]
    public string? CardPromotionName { get; set; }

    [Display(Name = "目前狀態")]
    public CardStatus CardStatus { get; set; }

    [Display(Name = "額度")]
    public int? CardLimit { get; set; }

    [Display(Name = "補件次數")]
    public int? SupplementCount { get; set; }

    [Display(Name = "卡片狀態名稱")]
    public string CardStatusName => CardStatus.ToString();
}
