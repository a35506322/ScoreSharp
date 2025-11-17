namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.SignKYCStrongReview;

public class SignKYCStrongReviewRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Required]
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 簽核加強審查執行詳細資料
    /// 按照 GetApplyCreditCardInfoByApplyNo 的Json格式，填入 KYC_StrongReDetailJson
    /// 切記會有版本之分，版本號碼會在 GetApplyCreditCardInfoByApplyNo 的 Json 中
    ///
    /// 前端回傳幫忙 JSON.stringify 解析
    /// </summary>
    [Required]
    [Display(Name = "簽核加強審查執行詳細資料")]
    public string KYC_StrongReDetailJson { get; set; }

    /// <summary>
    /// 建議核准
    /// </summary>
    [Required]
    [Display(Name = "建議核准")]
    [RegularExpression(@"^[YN]$")]
    public string KYC_Suggestion { get; set; }

    /// <summary>
    /// 加強審核執行狀態
    /// </summary>
    [Required]
    [ValidEnumValue]
    [Display(Name = "加強審核執行狀態")]
    public KYCStrongReStatusReq KYC_StrongReStatus { get; set; }
}

public enum KYCStrongReStatusReq
{
    [EnumIsActive(true)]
    送審中 = 2,

    [EnumIsActive(true)]
    核准 = 3,

    [EnumIsActive(true)]
    駁回 = 4,
}
