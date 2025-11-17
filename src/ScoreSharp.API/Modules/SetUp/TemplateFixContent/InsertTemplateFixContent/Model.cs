namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.InsertTemplateFixContent;

public class InsertTemplateFixContentRequest
{
    /// <summary>
    /// 樣板ID，關聯  SetUp_Template
    /// </summary>
    [Display(Name = "樣板ID")]
    [MaxLength(3)]
    [Required]
    public string TemplateId { get; set; }

    /// <summary>
    /// Key，程式相關人員設定
    /// </summary>
    [Display(Name = "Key")]
    [MaxLength(50)]
    [Required]
    public string TemplateKey { get; set; }

    /// <summary>
    /// Value，行員可以自行設定
    /// </summary>
    [Display(Name = "Value")]
    [MaxLength(256)]
    [Required]
    public string TemplateValue { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
