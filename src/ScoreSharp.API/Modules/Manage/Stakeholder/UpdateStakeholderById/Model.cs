namespace ScoreSharp.API.Modules.Manage.Stakeholder.UpdateStakeholderById;

public class UpdateStakeholderByIdRequest
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
    /// 是否啟用
    /// Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string IsActive { get; set; }
}
