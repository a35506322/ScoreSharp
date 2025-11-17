namespace ScoreSharp.Batch.Jobs.SystemAssignment.Models;

public class CaseAssignmentStatisticDto
{
    /// <summary>
    /// 派案統計結果
    /// </summary>
    public List<(string userId, int assignedCount)> CaseAssignmentStatistics { get; set; }

    /// <summary>
    /// 最後派案人員UserId
    /// </summary>
    public string LastAssignedUserId { get; set; }
}
