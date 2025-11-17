namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCommunicationNotesByApplyNo;

public class UpdateCommunicationNotesByApplyNoRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    [Required]
    [MaxLength(14)]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 徵審照會摘要_註記
    /// </summary>
    [Display(Name = "徵審照會摘要_註記")]
    [Required]
    public string CommunicationNotes { get; set; }
}
