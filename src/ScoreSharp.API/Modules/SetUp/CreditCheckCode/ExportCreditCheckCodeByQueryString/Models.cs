namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.ExportCreditCheckCodeByQueryString;

public class ExportCreditCheckCodeByQueryStringResponse
{
    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}

public class CreditCheckCodeByQueryStringExportDto
{
    /// <summary>
    /// 徵信代碼代碼，範例: A01
    /// </summary>
    [ExcelColumnName("徵信代碼代碼")]
    public string? CreditCheckCode { get; set; }

    /// <summary>
    /// 徵信代碼名稱
    /// </summary>
    [ExcelColumnName("徵信代碼名稱")]
    public string? CreditCheckCodeName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [ExcelColumnName("是否啟用")]
    public string? IsActive { get; set; }
}

public class CreditCheckCodeByQueryStringRequest
{
    /// <summary>
    /// 徵信代碼代碼，範例: A01
    /// </summary>

    public string? CreditCheckCode { get; set; }

    /// <summary>
    /// 徵信代碼名稱
    /// </summary>

    public string? CreditCheckCodeName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>

    public string? IsActive { get; set; }
}
