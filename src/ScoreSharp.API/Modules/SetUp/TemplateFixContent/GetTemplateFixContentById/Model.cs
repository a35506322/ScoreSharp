namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentById;

public class GetTemplateFixContentByIdResponse
{
    /// <summary>
    /// PK，自增
    /// </summary>
    [Display(Name = "PK")]
    public long SeqNo { get; set; }

    /// <summary>
    /// 樣板ID，關聯  SetUp_Template
    /// </summary>
    [Display(Name = "樣板ID")]
    public string TemplateId { get; set; }

    /// <summary>
    /// 樣板名稱，關聯  SetUp_Template
    /// </summary>
    [Display(Name = "樣板名稱")]
    public string TemplateName { get; set; }

    /// <summary>
    /// 樣板Key，程式相關人員設定
    /// </summary>
    [Display(Name = "樣板Key")]
    public string TemplateKey { get; set; }

    /// <summary>
    /// 樣板Value，行員可以自行設定
    /// </summary>
    [Display(Name = "樣板Value")]
    public string TemplateValue { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string IsActive { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    [Display(Name = "新增員工")]
    public string AddUserId { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    [Display(Name = "新增時間")]
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    [Display(Name = "修正員工")]
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    [Display(Name = "修正時間")]
    public DateTime? UpdateTime { get; set; }
}
