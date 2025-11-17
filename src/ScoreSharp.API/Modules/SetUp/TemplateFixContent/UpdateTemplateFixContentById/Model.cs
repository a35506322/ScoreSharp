namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.UpdateTemplateFixContentById;

public class UpdateTemplateFixContentByIdRequest
{
    /// <summary>
    /// PK，自增
    /// </summary>
    [Display(Name = "PK")]
    [Required]
    public long SeqNo { get; set; }

    /// <summary>
    /// 樣板ID，關聯  SetUp_Template
    /// </summary>
    [Display(Name = "樣板ID")]
    [MaxLength(3)]
    public string? TemplateId { get; set; }

    /// <summary>
    /// 樣板Key，程式相關人員設定
    /// </summary>
    [Display(Name = "樣板Key")]
    [MaxLength(50)]
    public string? TemplateKey { get; set; }

    /// <summary>
    /// 樣板Value，行員可以自行設定
    /// </summary>
    [Display(Name = "樣板Value")]
    [MaxLength(256)]
    public string? TemplateValue { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}
