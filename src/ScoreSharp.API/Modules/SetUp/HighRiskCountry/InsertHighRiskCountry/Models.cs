namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.InsertHighRiskCountry;

public class InsertHighRiskCountryRequest
{
    /// <summary>
    /// 洗錢及資恐高風險國家代碼，範例：TW
    /// </summary>
    [Display(Name = "洗錢及資恐高風險國家代碼")]
    [RegularExpression(@"^[A-Z]+$")]
    [MaxLength(5)]
    [Required]
    public string HighRiskCountryCode { get; set; }

    /// <summary>
    /// 洗錢及資恐高風險國家名稱
    /// </summary>
    [Display(Name = "洗錢及資恐高風險國家名稱")]
    [MaxLength(50)]
    [Required]
    public string HighRiskCountryName { get; set; }

    /// <summary>
    /// 是否啟用，範例：Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
