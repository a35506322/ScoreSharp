namespace ScoreSharp.API.Modules.OrgSetUp.Organize.InsertOrganize;

public class InsertOrganizeRequest
{
    /// <summary>
    /// 部門代碼
    /// </summary>
    [Display(Name = "部門代碼")]
    [MaxLength(10)]
    [Required]
    public string OrganizeCode { get; set; } = null!;

    /// <summary>
    /// 部門名稱
    /// </summary>
    [Display(Name = "部門名稱")]
    [MaxLength(30)]
    [Required]
    public string OrganizeName { get; set; } = null!;

    /// <summary>
    /// 法定代理人
    /// </summary>
    [Display(Name = "法定代理人")]
    [MaxLength(15)]
    [Required]
    public string LegalRepresentative { get; set; } = null!;

    /// <summary>
    /// 郵遞區號
    /// </summary>
    [Display(Name = "郵遞區號")]
    [MaxLength(5)]
    [Required]
    [RegularExpression(@"^\d+$")]
    public string ZIPCode { get; set; } = null!;

    /// <summary>
    /// 住址
    /// </summary>
    [Display(Name = "住址")]
    [MaxLength(100)]
    [Required]
    public string FullAddress { get; set; } = null!;
}
