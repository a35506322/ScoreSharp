namespace ScoreSharp.API.Modules.SetUp.RejectionReason.ExportRejectionReasonByQueryString;

public class ExportRejectionReasonByQueryStringResponse
{
    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}

public class RejectionReasonByQueryStringExportDto
{
    /// <summary>
    /// 退件代碼，範例: 01
    /// </summary>
    [ExcelColumnName("退件代碼")]
    public string RejectionReasonCode { get; set; }

    /// <summary>
    /// 退件名稱
    /// </summary>
    [ExcelColumnName("退件名稱")]
    public string RejectionReasonName { get; set; }

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [ExcelColumnName("是否啟用")]
    public string IsActive { get; set; }
}

public class RejectionReasonByQueryStringRequest
{
    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    public string? IsActive { get; set; }

    /// <summary>
    /// 退件代碼，範例: 01
    /// </summary>
    public string? RejectionReasonCode { get; set; }

    /// <summary>
    /// 退件名稱
    /// </summary>
    public string? RejectionReasonName { get; set; }
}
