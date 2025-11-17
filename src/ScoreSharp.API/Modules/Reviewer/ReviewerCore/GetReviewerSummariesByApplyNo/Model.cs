namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetReviewerSummariesByApplyNo;

public class GetReviewerSummariesByApplyNoResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 申請書編號，FK
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 紀錄
    /// </summary>
    public string Record { get; set; } = null!;

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 新增員工姓名
    /// </summary>
    public string AddUserName { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// 修正員工姓名
    /// </summary>
    public string? UpdateUserName { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
