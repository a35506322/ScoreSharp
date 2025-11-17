namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCasesSetUpByQueryString;

public class GetUserOrgCasesSetUpByQueryStringRequest { }

public class GetUserOrgCasesSetUpByQueryStringResponse
{
    /// <summary>
    /// 使用者帳號
    /// PK
    /// 關聯OrgSetUp_User
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 額度上限，預設0
    /// </summary>
    public int CardLimit { get; set; }

    /// <summary>
    /// 指定主管一
    /// </summary>
    public string? DesignatedSupervisor1 { get; set; }

    /// <summary>
    /// 指定主管一姓名
    /// </summary>
    public string? DesignatedSupervisor1Name { get; set; }

    /// <summary>
    /// 指定主管二
    /// </summary>
    public string? DesignatedSupervisor2 { get; set; }

    /// <summary>
    /// 指定主管二姓名
    /// </summary>
    public string? DesignatedSupervisor2Name { get; set; }

    /// <summary>
    /// 一般件預審分案，Y｜N
    /// </summary>
    public string IsPaperCase { get; set; }

    /// <summary>
    /// 快辦件預審分案，Y｜N
    /// </summary>
    public string IsWebCase { get; set; }

    /// <summary>
    /// 覆核比
    /// 每幾筆抽查案件
    /// 預設0
    /// </summary>
    public int CheckWeight { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 一般件預審比重排序
    /// </summary>
    public int PaperCaseSort { get; set; }

    /// <summary>
    /// 快辦件預審比重排序
    /// </summary>
    public int WebCaseSort { get; set; }

    /// <summary>
    /// 人工徵信件預審分案
    ///
    /// Y / N
    /// </summary>
    public string IsManualCase { get; set; } = null!;

    /// <summary>
    /// 人工徵信件預審比重排序
    ///
    /// 預設1
    /// </summary>
    public int ManualCaseSort { get; set; }

    /// <summary>
    /// 分派組織
    /// </summary>
    public List<CaseDispatchGroupModel>? CaseDispatchGroups { get; set; }
}

public class CaseDispatchGroupModel
{
    public CaseDispatchGroup CaseDispatchGroup { get; set; }
    public string CaseDispatchGroupName
    {
        get => CaseDispatchGroup.ToString();
    }
}

public class GetBeReviewerUsersDto
{
    /// <summary>
    /// 使用者帳號
    /// </summary>
    [Display(Name = "使用者帳號")]
    public string UserId { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    [Display(Name = "使用者姓名")]
    public string UserName { get; set; }
}
