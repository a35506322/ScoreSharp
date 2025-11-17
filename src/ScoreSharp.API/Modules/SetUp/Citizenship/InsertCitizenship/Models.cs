namespace ScoreSharp.API.Modules.SetUp.Citizenship.InsertCitizenship;

public class InsertCitizenshipRequest
{
    /// <summary>
    /// 國籍代碼，範例 : TW
    /// </summary>
    [Display(Name = "國籍代碼")]
    [RegularExpression(@"^[A-Z]+$")]
    [MaxLength(5)]
    [Required]
    public string CitizenshipCode { get; set; }

    /// <summary>
    /// 國籍名稱
    /// </summary>
    [Display(Name = "國籍名稱")]
    [MaxLength(50)]
    [Required]
    public string CitizenshipName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
