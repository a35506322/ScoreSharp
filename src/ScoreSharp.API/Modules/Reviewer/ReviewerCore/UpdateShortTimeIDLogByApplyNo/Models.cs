namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateShortTimeIDLogByApplyNo;

public class UpdateShortTimeIDLogByApplyNoRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Required]
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 確認紀錄
    /// </summary>
    [Required]
    [Display(Name = "確認紀錄")]
    public string? CheckRecord { get; set; }

    /// <summary>
    /// 是否異常 Y/N
    /// </summary>
    [Required]
    [Display(Name = "是否異常 Y/N")]
    public string? IsError { get; set; }
}
