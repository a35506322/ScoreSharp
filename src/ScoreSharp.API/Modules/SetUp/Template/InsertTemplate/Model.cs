namespace ScoreSharp.API.Modules.SetUp.Template.InsertTemplate;

public class InsertTemplateRequest
{
    /// <summary>
    /// 樣板ID，PK，A01
    /// </summary>
    [Display(Name = "樣板ID")]
    [MaxLength(3)]
    [Required]
    public string TemplateId { get; set; }

    /// <summary>
    /// 樣板名稱，範例:補件函
    /// </summary>
    [Display(Name = "樣板名稱")]
    [MaxLength(50)]
    [Required]
    public string TemplateName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }
}
