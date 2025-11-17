namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class BaseResDto
{
    /// <summary>
    /// 申請單號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType? UserType { get; set; } = null;

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
