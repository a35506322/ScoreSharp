using System.Security.Cryptography;

namespace ScoreSharp.API.Modules.Report.GetBatchReportDownloadByQueryString;

public class GetBatchReportDownloadByQueryStringRequest
{
    /// <summary>
    /// 開始時間
    /// </summary>
    [Display(Name = "開始時間")]
    [Required]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    [Display(Name = "結束時間")]
    [Required]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 報表類型
    /// </summary>
    [Display(Name = "報表類型")]
    [ValidEnumValue]
    [Required]
    public ReportType? ReportType { get; set; }
}

public class GetBatchReportDownloadByQueryStringResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public string SeqNo { get; set; } = null!;

    /// <summary>
    /// 報表名稱
    /// </summary>
    public string ReportName { get; set; } = null!;

    /// <summary>
    /// 報表類型
    /// </summary>
    public ReportType ReportType { get; set; }

    /// <summary>
    /// 報表完整地址
    /// </summary>
    public string ReportFullAddr { get; set; } = null!;

    /// <summary>
    /// 新增日期
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 最後下載人員
    /// </summary>
    public string? LastDownloadUserId { get; set; }

    /// <summary>
    /// 最後下載人員名稱
    /// </summary>
    public string? LastDownloadUserName { get; set; }

    /// <summary>
    /// 報表類型名稱
    /// </summary>
    public string ReportTypeName => ReportType.ToString();
}
