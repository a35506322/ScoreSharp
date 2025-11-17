namespace ScoreSharp.API.Modules.Manage.CaseStatistics.GetCaseStatisticsByQueryString;

public class GetCaseStatisticsByQueryStringRequest : IValidatableObject
{
    /// <summary>
    /// 員工編號
    /// </summary>
    [Display(Name = "員工編號")]
    public string? UserId { get; set; }

    /// <summary>
    /// 操作類型
    ///
    /// Query: 查詢
    /// Export: 匯出
    /// </summary>
    [Display(Name = "操作類型")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// 派案日期
    /// </summary>
    [Display(Name = "派案日期")]
    public DateTime? Addtime { get; set; }

    /// <summary>
    /// 派案類型
    ///
    /// 系統派案 = 1,
    /// 整批派案 = 2,
    /// 調撥案件 = 3,
    /// 強制派案 = 4,
    /// 待派案_人工 = 5,
    /// 調撥_人工 = 6,
    /// </summary>
    [Display(Name = "派案類型")]
    [ValidEnumValue]
    public CaseStatisticType? CaseType { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Type != "Query" && Type != "Export")
        {
            yield return new ValidationResult("操作類型僅能為Query或Export", new[] { "Type" });
        }

        if (string.IsNullOrEmpty(UserId) && !Addtime.HasValue)
        {
            yield return new ValidationResult("員工編號、派案日期需至少填寫一種條件", new[] { "UserId", "Addtime" });
        }
    }
}

public class GetCaseStatisticsByQueryStringResponse
{
    /// <summary>
    /// 查詢結果
    /// </summary>
    public List<GetCaseStatisticsDto> QueryData { get; set; } = [];

    /// <summary>
    /// 匯出檔案
    /// </summary>
    public ExportGetCaseStatisticsToExcelDto? ExportData { get; set; }

    /// <summary>
    /// 操作類型
    ///
    /// Query: 查詢
    /// Export: 匯出
    /// </summary>
    public string Type { get; set; }
}

public class GetCaseStatisticsDto
{
    /// <summary>
    /// 員工編號
    /// </summary>
    [ExcelColumnName("員工編號")]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 員工姓名
    /// </summary>
    [ExcelColumnName("員工姓名")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 派案日期
    /// </summary>
    [ExcelColumnName("派案日期")]
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 派案類型
    /// </summary>
    [ExcelColumnName("派案類型")]
    public string CaseType { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    [ExcelColumnName("申請書編號")]
    public string ApplyNo { get; set; } = null!;
}

public class ExportGetCaseStatisticsToExcelDto
{
    public byte[] FileContent { get; set; }

    public string FileName { get; set; }
}

public class CaseStatisticsToExcelDto
{
    /// <summary>
    /// 員工編號
    /// </summary>
    [ExcelColumnName("員工編號")]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 員工姓名
    /// </summary>
    [ExcelColumnName("員工姓名")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 派案日期
    /// </summary>
    [ExcelColumnName("派案日期")]
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 派案類型
    /// </summary>
    [ExcelColumnName("派案類型")]
    public string CaseType { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    [ExcelColumnName("申請書編號")]
    public string ApplyNo { get; set; } = null!;
}
