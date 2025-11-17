namespace ScoreSharp.Batch.Jobs.SystemAssignment.Models;

public class UserOrgCaseSetUpDto
{
    /// <summary>
    /// 員工帳號
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 員工編號
    /// </summary>
    public string? EmployeeNo { get; set; }
}
