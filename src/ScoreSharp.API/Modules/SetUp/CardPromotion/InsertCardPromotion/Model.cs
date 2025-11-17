namespace ScoreSharp.API.Modules.SetUp.CardPromotion.InsertCardPromotion;

public class InsertCardPromotionRequest
{
    /// <summary>
    /// 優惠辦法代碼，範例 : 0001
    /// </summary>
    [Display(Name = "優惠辦法代碼")]
    [RegularExpression("^(?!0000)\\d{4}$")]
    [Required]
    public string CardPromotionCode { get; set; }

    /// <summary>
    /// 優惠辦法名稱
    /// </summary>
    [Display(Name = "優惠辦法名稱")]
    [Required]
    public string CardPromotionName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }

    /// <summary>
    /// 正卡使用POT，範例 : 01
    /// </summary>
    [Display(Name = "正卡使用POT")]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    [Required]
    public string PrimaryCardUsedPOT { get; set; }

    /// <summary>
    /// 附卡使用POT，範例 : 01
    /// </summary>
    [Display(Name = "附卡使用POT")]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    [Required]
    public string SupplementaryCardUsedPOT { get; set; }

    /// <summary>
    /// 使用POT截止月份，範例 : 01
    /// </summary>
    [Display(Name = "使用POT截止月份")]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    [Required]
    public string UsedPOTExpiryMonth { get; set; }

    /// <summary>
    /// 正卡預留POT，範例 : 01
    /// </summary>
    [Display(Name = "正卡預留POT")]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    public string? PrimaryCardReservedPOT { get; set; }

    /// <summary>
    /// 附卡預留POT，範例 : 01
    /// </summary>
    [Display(Name = "附卡預留POT")]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    public string? SupplementaryCardReservedPOT { get; set; }

    /// <summary>
    /// 預留優惠期限(月)，範例 : 01
    /// </summary>
    [Display(Name = "預留優惠期限(月)")]
    [RegularExpression("^(0[1-9]|[1-9][0-9])$")]
    public string? ReservePromotionPeriod { get; set; }

    /// <summary>
    /// 利率，範例 : 12.22
    /// </summary>
    [Display(Name = "利率")]
    public decimal? InterestRate { get; set; }
}
