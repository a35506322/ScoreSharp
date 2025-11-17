namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.ReturnInManualReviewByApplyNo;

public class ReturnInManualReviewByApplyNoRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Required]
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 註記
    /// </summary>
    [Display(Name = "註記")]
    [Required]
    public string Note { get; set; }
}
