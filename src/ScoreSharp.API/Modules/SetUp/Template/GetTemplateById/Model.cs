namespace ScoreSharp.API.Modules.SetUp.Template.GetTemplateById;

public class GetTemplateByIdResponse
{
    /// <summary>
    /// 樣板ID，PK，A01
    /// </summary>
    [Display(Name = "樣板ID")]
    public string TemplateId { get; set; }

    /// <summary>
    /// 樣板名稱，範例:補件函
    /// </summary>
    [Display(Name = "樣板名稱")]
    public string TemplateName { get; set; }

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
