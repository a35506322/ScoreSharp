namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateApplyNoteBySeqNo;

public class UpdateApplyNoteBySeqNoRequest
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 備註
    /// 用於徵審人員備註資料
    /// </summary>
    public string Note { get; set; }
}
