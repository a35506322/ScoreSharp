namespace ScoreSharp.API.Modules.SetUp.SupplementReason.ExportSupplementReasonByQueryString;

public class ExportSupplementReasonByQueryStringResponse
{
    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}

public class SupplementReasonByQueryStringExportDto
{
    /// <summary>
    /// 補件代碼，範例: 01
    /// </summary>
    [ExcelColumnName("補件代碼")]
    public string SupplementReasonCode { get; set; } = null!;

    /// <summary>
    /// 補件名稱
    /// </summary>
    [ExcelColumnName("補件名稱")]
    public string SupplementReasonName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    [ExcelColumnName("是否啟用")]
    public string IsActive { get; set; } = null!;
}

public class SupplementReasonByQueryStringRequest
{
    /// <summary>
    /// 補件代碼，範例: 01
    /// </summary>
    public string? SupplementReasonCode { get; set; }

    /// <summary>
    /// 補件名稱
    /// </summary>
    public string? SupplementReasonName { get; set; }

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    public string? IsActive { get; set; }
}
