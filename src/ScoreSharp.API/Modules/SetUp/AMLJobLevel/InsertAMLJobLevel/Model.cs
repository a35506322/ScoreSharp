namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.InsertAMLJobLevel;

public class InsertAMLJobLevelRequest
{
    /// <summary>
    /// AML職級別代碼，範例: 1
    /// </summary>
    [Display(Name = "AML職級別代碼")]
    [Required]
    [RegularExpression("^[0-9]+$")]
    public string AMLJobLevelCode { get; set; }

    /// <summary>
    /// AML職級別名稱
    /// </summary>
    [Display(Name = "AML職級別名稱")]
    [Required]
    public string AMLJobLevelName { get; set; }

    /// <summary>
    /// 是否為高階管理人，Y | N
    /// </summary>
    [Display(Name = "是否為高階管理人")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsSeniorManagers { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
