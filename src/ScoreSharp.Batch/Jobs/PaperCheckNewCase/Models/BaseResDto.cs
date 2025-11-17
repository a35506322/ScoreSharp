namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

public class BaseResDto
{
    /// <summary>
    /// 回傳代碼
    /// </summary>
    public string? RtnCode { get; set; }

    /// <summary>
    /// 回傳訊息
    /// </summary>
    public string? RtnMsg { get; set; }

    /// <summary>
    /// 查詢時間
    /// </summary>
    public DateTime? QueryTime { get; set; }

    /// <summary>
    /// TraceId
    /// </summary>
    public string? TraceId { get; set; }
}
