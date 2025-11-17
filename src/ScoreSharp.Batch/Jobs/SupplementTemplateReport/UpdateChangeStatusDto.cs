namespace ScoreSharp.Batch.Jobs.SupplementTemplateReport;

public class UpdateChangeStatusDto
{
    /// <summary>
    /// 處理序號
    /// </summary>
    public string HandleSeqNo { get; set; }

    /// <summary>
    /// 批次補卡時間
    /// </summary>
    public DateTime BatchSupplementTime { get; set; }
}
