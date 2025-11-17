namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.InsertApplicationCategory;

public class InsertApplicationCategoryRequest
{
    /// <summary>
    /// 申請書類別代碼，範例: AF00002
    /// </summary>
    [Display(Name = "申請書類別代碼")]
    [MaxLength(20)]
    [RegularExpression("^[a-zA-Z0-9]+$")]
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
    [MinLength(1)]
    [Required]
    public List<string> BINCodes { get; set; } = new List<string>();
}
