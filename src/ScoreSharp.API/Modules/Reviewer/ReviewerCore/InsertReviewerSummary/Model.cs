namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertReviewerSummary;

public class InsertReviewerSummaryRequest
{
    /// <summary>
    /// 申請書編號，FK
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 紀錄
    /// </summary>
    public string Record { get; set; }
}
