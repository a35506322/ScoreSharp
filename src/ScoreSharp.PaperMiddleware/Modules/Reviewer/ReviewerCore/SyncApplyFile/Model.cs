using ScoreSharp.Common.Attributes;
using ScoreSharp.Common.Enums;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyFile;

public class SyncApplyFileRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    [MaxLength(14)]
    [Required]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 同步狀態
    /// </summary>
    [Display(Name = "同步狀態")]
    [ValidEnumValue]
    [Required]
    public SyncFileStatus SyncStatus { get; set; }

    /// <summary>
    /// 同步員編
    /// </summary>
    [Display(Name = "同步員編")]
    [MaxLength(30)]
    [Required]
    public string SyncUserId { get; set; }

    /// <summary>
    /// 檔案陣列
    /// </summary>
    [Display(Name = "檔案陣列")]
    [Required]
    public ApplyFileDto[] ApplyFiles { get; set; }
}

public class ApplyFileDto
{
    /// <summary>
    /// 申請書檔案申請書檔案
    /// </summary>
    [Display(Name = "申請書檔案")]
    [Required]
    public byte[] FileContent { get; set; }

    /// <summary>
    /// 檔案名稱
    /// </summary>
    [Display(Name = "檔案名稱")]
    [MaxLength(100)]
    [Required]
    public string FileName { get; set; }

    /// <summary>
    /// 檔案Key
    /// </summary>
    [Display(Name = "檔案Key")]
    [Required]
    public int FileId { get; set; }
}
