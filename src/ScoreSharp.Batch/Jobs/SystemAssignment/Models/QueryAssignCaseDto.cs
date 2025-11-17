namespace ScoreSharp.Batch.Jobs.SystemAssignment.Models;

public class QueryAssignCaseDto
{
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    ///
    /// 用逗號分隔的ID字串
    /// </summary>
    public string IDs { get; set; } = null!;

    /// <summary>
    /// 月收入確認人員
    /// </summary>
    public string? MonthlyIncomeCheckUserId { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    public string? PromotionUser { get; set; }
}
