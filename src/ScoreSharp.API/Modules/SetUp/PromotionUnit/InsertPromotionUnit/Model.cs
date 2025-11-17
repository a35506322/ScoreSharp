namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.InsertPromotionUnit;

public class InsertPromotionUnitRequest
{
    /// <summary>
    /// 推廣單位代碼，範例: 100，長度3碼
    /// </summary>
    [Display(Name = "推廣單位代碼")]
    [MaxLength(3)]
    [Required]
    public string PromotionUnitCode { get; set; }

    /// <summary>
    /// 推廣單位名稱
    /// </summary>
    [Display(Name = "推廣單位名稱")]
    [Required]
    public string PromotionUnitName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
