namespace ScoreSharp.API.Modules.Report.BatchReportDownload.GetReportFileBySeqNo;

public class GetReportFileBySeqNoResponse
{
    /// <summary>
    /// 檔案內容
    /// </summary>
    public byte[] FileContent { get; set; }

    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 檔案類型
    /// </summary>
    public string ContentType { get; set; }
}
