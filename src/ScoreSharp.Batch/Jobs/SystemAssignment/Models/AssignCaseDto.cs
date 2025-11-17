namespace ScoreSharp.Batch.Jobs.SystemAssignment.Models;

public class AssignCaseDto
{
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 月收入確認人員
    /// </summary>
    public string? MonthlyIncomeCheckUserId { get; set; }

    public string[] ID { get; set; } = null!;

    public bool IsAssigned { get; set; } = false;

    /// <summary>
    /// 推廣人員
    /// </summary>
    public string? PromotionUser { get; set; }
}
