namespace ScoreSharp.API.Modules.SetUp.Template.UpdateTemplateById;

public class UpdateTemplateByIdRequest
{
    /// <summary>
    /// 樣板ID，PK，A01
    /// </summary>
    [Display(Name = "樣板ID")]
    [Required]
    public string TemplateId { get; set; }

    /// <summary>
    /// 樣板名稱，範例:補件函
    /// </summary>
    [Display(Name = "樣板名稱")]
    [MaxLength(50)]
    public string? TemplateName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}
