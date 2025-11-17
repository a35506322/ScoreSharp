namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.UpdateApplicationCategoryById;

public class UpdateApplicationCategoryByIdRequest
{
    /// <summary>
    /// 申請書類別代碼，範例: AF00002
    /// </summary>
    [Display(Name = "申請書類別代碼")]
    [MaxLength(20)]
    [Required]
    public string ApplicationCategoryCode { get; set; }

    /// <summary>
    /// 申請書類別名稱
    /// </summary>
    [Display(Name = "申請書類別名稱")]
    [Required]
    public string ApplicationCategoryName { get; set; }

    /// <summary>
    /// 是否為OCR表單，Y | N
    /// </summary>
    [Display(Name = "是否為OCR表單")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsOCRForm { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }

    /// <summary>
    /// 卡片資訊
    /// </summary>
    [Display(Name = "卡片資訊")]
    [Required]
    [MinLength(1)]
    public List<string> BINCodes { get; set; } = new List<string>();
}
