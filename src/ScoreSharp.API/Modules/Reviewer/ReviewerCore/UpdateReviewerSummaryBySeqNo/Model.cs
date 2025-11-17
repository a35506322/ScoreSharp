namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateReviewerSummaryBySeqNo;

public class UpdateReviewerSummaryBySeqNoRequest
{
    /// <summary>
    /// PK
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 紀錄
    /// </summary>
    public string Record { get; set; } = null!;
}
