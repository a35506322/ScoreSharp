namespace ScoreSharp.API.Modules.SetUp.EUCountry.InsertEUCountry;

public class InsertEUCountryRequest
{
    /// <summary>
    /// EU國家代碼，範例 : TW
    /// </summary>
    [Display(Name = "EU國家代碼")]
    [RegularExpression(@"^[A-Z]+$")]
    [MaxLength(5)]
    [Required]
    public string EUCountryCode { get; set; }

    /// <summary>
    /// EU國家名稱
    /// </summary>
    [Display(Name = "EU國家名稱")]
    [MaxLength(50)]
    [Required]
    public string EUCountryName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
