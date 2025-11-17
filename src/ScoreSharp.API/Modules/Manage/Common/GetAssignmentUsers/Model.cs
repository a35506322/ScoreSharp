namespace ScoreSharp.API.Modules.Manage.Common.GetAssignmentUsers;

public class QueryAssignmentUsersResponse
{
    /// <summary>
    /// 使用者
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 派案組織
    /// </summary>
    public string CaseDispatchGroup { get; set; } = string.Empty;

    /// <summary>
    /// 一般件預審分案
    /// </summary>
    public string IsPaperCase { get; set; } = string.Empty;

    /// <summary>
    /// 一般件預審比重排序
    /// </summary>
    public int PaperCaseSort { get; set; }

    /// <summary>
    /// 快辦件預審分案
    /// </summary>
    public string IsWebCase { get; set; } = string.Empty;

    /// <summary>
    /// 快辦件預審比重排序
    /// </summary>
    public int WebCaseSort { get; set; }

    /// <summary>
    /// 人工徵信件預審分案
    /// </summary>
    public string IsManualCase { get; set; } = string.Empty;

    /// <summary>
    /// 人工徵信件預審比重排序
    /// </summary>
    public int ManualCaseSort { get; set; }
}

public class GetAssignmentUsersResponse
{
    /// <summary>
    /// 使用者
    /// </summary>
    [Display(Name = "使用者")]
    public string UserId { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    [Display(Name = "")]
    public string UserName { get; set; }

    /// <summary>
    /// 派案組織
    /// </summary>
    [Display(Name = "派案組織")]
    public List<CaseDispatchGroupInfo> CaseDispatchGroups { get; set; } = [];

    /// <summary>
    /// 是否在假
    /// </summary>
    [Display(Name = "是否在假")]
    public string IsVacation { get; set; }

    /// <summary>
    /// 一般件預審分案
    /// </summary>
    [Display(Name = "一般件預審分案")]
    public string IsPaperCase { get; set; }

    /// <summary>
    /// 快辦件預審分案
    /// </summary>
    [Display(Name = "快辦件預審分案")]
    public string IsWebCase { get; set; }

    /// <summary>
    /// 人工徵信件預審分案
    /// </summary>
    [Display(Name = "人工徵信件預審分案")]
    public string IsManualCase { get; set; }

    /// <summary>
    /// 一般件預審比重排序
    /// </summary>
    [Display(Name = "一般件預審比重排序")]
    public int PaperCaseSort { get; set; }

    /// <summary>
    /// 快辦件預審比重排序
    /// </summary>
    [Display(Name = "快辦件預審比重排序")]
    public int WebCaseSort { get; set; }

    /// <summary>
    /// 人工徵信件預審比重排序
    /// </summary>
    [Display(Name = "人工徵信件預審比重排序")]
    public int ManualCaseSort { get; set; }
}

public class CaseDispatchGroupInfo
{
    public CaseDispatchGroup CaseDispatchGroup { get; set; }
    public string CaseDispatchGroupName => CaseDispatchGroup.ToName();
}
