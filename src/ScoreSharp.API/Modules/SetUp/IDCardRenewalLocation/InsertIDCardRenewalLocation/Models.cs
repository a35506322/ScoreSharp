namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.InsertIDCardRenewalLocation;

public class InsertIDCardRenewalLocationRequest
{
    /// <summary>
    /// 身分證換發地點代碼，範例: 09007000
    /// </summary>
    [Display(Name = "身分證換發地點代碼")]
    [MaxLength(8)]
    [Required]
    public string IDCardRenewalLocationCode { get; set; }

    /// <summary>
    /// 身分證換發地點名稱，範例: 北市
    /// </summary>
    [Display(Name = "身分證換發地點名稱")]
    [MaxLength(10)]
    [Required]
    public string IDCardRenewalLocationName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
