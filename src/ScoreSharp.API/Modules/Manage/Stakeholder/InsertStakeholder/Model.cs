namespace ScoreSharp.API.Modules.Manage.Stakeholder.InsertStakeholder;

public class InsertStakeholderRequest
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    [Display(Name = "身分證字號")]
    [Required]
    [TWID]
    public string ID { get; set; }

    /// <summary>
    /// 使用者帳號
    /// 目前來源為 AD Server 以及自建
    /// </summary>
    [Display(Name = "使用者帳號")]
    [MaxLength(30)]
    [Required]
    public string UserId { get; set; }

    /// <summary>
    /// 是否啟用
    /// Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }
}
