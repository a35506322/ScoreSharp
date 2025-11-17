namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUserById;

public class GetUserByIdResponse
{
    /// <summary>
    /// 使用者帳號，目前來源為 AD Server
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>

    public string IsActive { get; set; }

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
    /// 角色
    /// </summary>
    public string[] RoleId { get; set; }

    /// <summary>
    /// 是否AD
    /// </summary>
    public string IsAD { get; set; } = null!;

    /// <summary>
    /// 最後變更密碼時間
    /// </summary>
    public DateTime? LastUpdateMimaTime { get; set; }

    /// <summary>
    /// 組織代碼
    /// 1. 關聯 OrgSetUp_Organize
    /// 2.跟派案組織無關
    /// </summary>
    public string? OrganizeCode { get; set; }

    /// <summary>
    /// 停用原因，IsActive = N 需有值
    /// </summary>
    public string? StopReason { get; set; }

    /// <summary>
    /// 密碼錯誤次數
    /// 1. 預設 0
    /// 2. 達 5 次 IsActive = N，StopReason　= 密碼輸入超過5次
    /// </summary>
    public int MimaErrorCount { get; set; }

    /// <summary>
    /// 組織名稱
    /// </summary>
    public string? OrganizeName { get; set; }

    /// <summary>
    /// 派案組織
    /// </summary>
    public List<CaseDispatchGroupModel>? CaseDispatchGroups { get; set; }

    /// <summary>
    /// 員工編號
    /// </summary>
    public string? EmployeeNo { get; set; }
}

public class CaseDispatchGroupModel
{
    public CaseDispatchGroup CaseDispatchGroup { get; set; }
    public string CaseDispatchGroupName
    {
        get => CaseDispatchGroup.ToString();
    }
}
