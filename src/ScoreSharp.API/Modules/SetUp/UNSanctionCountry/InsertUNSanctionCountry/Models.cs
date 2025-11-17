namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.InsertUNSanctionCountry;

public class InsertUNSanctionCountryRequest
{
    /// <summary>
    /// UN制裁國家代碼，範例 : TW
    /// </summary>
    [Display(Name = "UN制裁國家代碼")]
    [RegularExpression(@"^[A-Z]+$")]
    [MaxLength(5)]
    [Required]
    public string UNSanctionCountryCode { get; set; }

    /// <summary>
    /// UN制裁國家名稱
    /// </summary>
    [Display(Name = "UN制裁國家名稱")]
    [MaxLength(50)]
    [Required]
    public string UNSanctionCountryName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
