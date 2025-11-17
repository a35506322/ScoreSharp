namespace ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderByQueryString;

public class GetStakeholderByQueryStringRequest
{
    /// <summary>
    /// 使用者帳號
    /// </summary>
    [MaxLength(30)]
    public string? UserId { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}

public class GetStakeholderByQueryStringResponse
{
    /// <summary>
    /// PK
    /// 自增
    /// </summary>
    [Display(Name = "PK")]
    public long SeqNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    [Display(Name = "身分證字號")]
    public string ID { get; set; }

    /// <summary>
    /// 使用者帳號
    /// 目前來源為 AD Server 以及自建
    /// </summary>
    [Display(Name = "使用者帳號")]
    public string UserId { get; set; }

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

    /// <summary>
    /// 是否啟用
    /// Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string IsActive { get; set; }
}
