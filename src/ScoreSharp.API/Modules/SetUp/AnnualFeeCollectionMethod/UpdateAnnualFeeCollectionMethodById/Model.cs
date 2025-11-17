namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.UpdateAnnualFeeCollectionMethodById;

public class UpdateAnnualFeeCollectionMethodByIdRequest
{
    /// <summary>
    /// 年費收取代碼
    /// </summary>
    [Display(Name = "年費收取代碼")]
    [Required]
    [MaxLength(2)]
    public string AnnualFeeCollectionCode { get; set; }

    /// <summary>
    /// 年費收取名稱
    /// </summary>
    [Display(Name = "年費收取名稱")]
    [Required]
    [MaxLength(30)]
    public string AnnualFeeCollectionName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("^[YN]$")]
    public string IsActive { get; set; }
}
