namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentCardStatusByQueryString;

public class GetObligedAssignmentCardStatusByQueryStringRequest
{
    /// <summary>
    /// 角色代碼
    /// </summary>
    [Display(Name = "角色代碼")]
    public string? RoleId { get; set; }
}

public class GetObligedAssignmentCardStatusByQueryStringResponse
{
    /// <summary>
    /// 角色代碼
    /// </summary>
    public List<string> RoleIds { get; set; } = new();

    /// <summary>
    /// 可查詢的案件狀態
    /// </summary>
    public CardStatus CaseQueryCardStatus { get; set; }

    /// <summary>
    /// 可查詢的案件狀態
    /// </summary>
    public string CaseQueryCardStatusName => CaseQueryCardStatus.ToName();
}
