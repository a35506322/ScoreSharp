namespace ScoreSharp.API.Modules.Reviewer3rd.GetFocus2ByApplyNo;

public class QueryConernDetailIDDto
{
    public string ID { get; set; }
    public UserType UserType { get; set; }
}

public class ConcernDetailInfoDto : ResDto
{
    /// <summary>
    /// 關注名單1
    /// </summary>
    public List<string> Focus1HitList { get; set; } = new();

    /// <summary>
    /// 關注名單1 是否命中
    /// </summary>
    public string Focus1Checked => Focus1HitList.Any() ? "Y" : "N";

    /// <summary>
    /// 關注名單2
    /// </summary>
    public List<string> Focus2HitList { get; set; } = new();

    /// <summary>
    /// 關注名單2 是否命中
    /// </summary>
    public string Focus2Checked => Focus2HitList.Any() ? "Y" : "N";
}

public class ResDto
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
