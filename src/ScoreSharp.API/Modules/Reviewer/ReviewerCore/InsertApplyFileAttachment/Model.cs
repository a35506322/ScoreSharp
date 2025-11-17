namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertApplyFileAttachment;

public class InsertApplyFileAttachmentRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Required]
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    /// </summary>
    [Required]
    public string ID { get; set; } = null!;

    /// <summary>
    /// 使用者類型
    /// 1. 正卡人
    /// 2. 附卡人
    /// </summary>
    [Required]
    public UserType Type { get; set; }

    /// <summary>
    /// 附件檔案
    /// </summary>
    [Required]
    public IFormFile AttachmentFile { get; set; }
}
