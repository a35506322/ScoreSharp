namespace ScoreSharp.API.Modules.SetUp.ProjectCode.InsertProjectCode;

public class InsertProjectCodeRequest
{
    /// <summary>
    /// 專案代碼，範例: 100，長度3碼
    /// </summary>
    [Display(Name = "專案代碼")]
    [MaxLength(3)]
    [Required]
    public string ProjectCode { get; set; }

    /// <summary>
    /// 活動名稱
    /// </summary>
    [Display(Name = "活動名稱")]
    [Required]
    public string ProjectName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
