namespace ScoreSharp.API.Modules.Report.CardHolderChangeReport.GetCardHolderChangeReportByQueryString;

/// <summary>
/// 報表類型列舉
/// </summary>
public enum ReportType
{
    /// <summary>
    /// 查詢（返回 JSON）
    /// </summary>
    Query = 1,

    /// <summary>
    /// 匯出（返回 Excel 檔案）
    /// </summary>
    Export = 2,
}

/// <summary>
/// 查詢請求
/// </summary>
public class GetCardHolderChangeReportByQueryStringRequest : IValidatableObject
{
    /// <summary>
    /// 報表類型：1=查詢, 2=匯出
    /// </summary>
    [Required(ErrorMessage = "報表類型為必填")]
    [Display(Name = "報表類型")]
    [ValidEnumValue]
    public ReportType Type { get; set; }

    /// <summary>
    /// 申請書編號（選填）
    /// </summary>
    public string? ApplyNo { get; set; }

    /// <summary>
    /// 變更人員帳號（選填）
    /// </summary>
    public string? ChangeUserId { get; set; }

    /// <summary>
    /// 開始日期（選填）格式：YYYY/MM/DD
    /// </summary>
    [RegularExpression(@"^\d{4}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "日期格式錯誤，請使用 YYYY/MM/DD")]
    public string? StartDate { get; set; }

    /// <summary>
    /// 結束日期（選填）格式：YYYY/MM/DD
    /// </summary>
    [RegularExpression(@"^\d{4}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "日期格式錯誤，請使用 YYYY/MM/DD")]
    public string? EndDate { get; set; }

    /// <summary>
    /// 卡別類型（選填：1=正卡, 2=附卡）
    /// </summary>
    [ValidEnumValue]
    public UserType? UserType { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // 至少輸入一個條件
        if (
            string.IsNullOrEmpty(ApplyNo)
            && string.IsNullOrEmpty(ChangeUserId)
            && (string.IsNullOrEmpty(StartDate) && string.IsNullOrEmpty(EndDate))
            && UserType == null
        )
        {
            yield return new ValidationResult(
                "至少輸入一個條件",
                new[] { nameof(ApplyNo), nameof(ChangeUserId), nameof(StartDate), nameof(EndDate), nameof(UserType) }
            );
        }

        // 驗證日期邏輯
        if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
        {
            if (
                DateTime.TryParseExact(StartDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate)
                && DateTime.TryParseExact(EndDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate)
            )
            {
                if (startDate > endDate)
                {
                    yield return new ValidationResult("開始日期不可大於結束日期", new[] { nameof(StartDate), nameof(EndDate) });
                }

                if (endDate > DateTime.Now)
                {
                    yield return new ValidationResult("結束日期不可大於今天", new[] { nameof(EndDate) });
                }

                if (startDate.AddMonths(6) < endDate)
                {
                    yield return new ValidationResult("查詢範圍最多六個月", new[] { nameof(StartDate), nameof(EndDate) });
                }
            }
        }
    }
}

/// <summary>
/// 查詢回應（Type=Query 時使用）
/// </summary>
public class GetCardHolderChangeReportByQueryStringResponse
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 類型：1=正卡人, 2=附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 卡別類型名稱
    /// </summary>
    public string UserTypeName => UserType == UserType.正卡人 ? "正卡" : "附卡";

    /// <summary>
    /// 附卡人ID
    /// </summary>
    public string? SupplementaryID { get; set; }

    /// <summary>
    /// 變更時間
    /// </summary>
    public DateTime ChangeDateTime { get; set; }

    /// <summary>
    /// 變更人員編號
    /// </summary>
    public string ChangeUserId { get; set; } = null!;

    /// <summary>
    /// 變更人員姓名
    /// </summary>
    public string? ChangeUserName { get; set; }

    /// <summary>
    /// 變更來源（API, Batch, Middleware, PaperMiddleware）
    /// </summary>
    public string? ChangeSource { get; set; }

    /// <summary>
    /// 變更 API 端點
    /// </summary>
    public string? ChangeAPIEndpoint { get; set; }

    /// <summary>
    /// 變更前行動電話
    /// </summary>
    public string? BeforeMobile { get; set; }

    /// <summary>
    /// 變更後行動電話
    /// </summary>
    public string? AfterMobile { get; set; }

    /// <summary>
    /// 變更前 Email
    /// </summary>
    public string? BeforeEmail { get; set; }

    /// <summary>
    /// 變更後 Email
    /// </summary>
    public string? AfterEmail { get; set; }

    /// <summary>
    /// 變更前帳單地址
    /// </summary>
    public string? BeforeBillAddress { get; set; }

    /// <summary>
    /// 變更後帳單地址
    /// </summary>
    public string? AfterBillAddress { get; set; }

    /// <summary>
    /// 變更前寄卡地址
    /// </summary>
    public string? BeforeSendCardAddress { get; set; }

    /// <summary>
    /// 變更後寄卡地址
    /// </summary>
    public string? AfterSendCardAddress { get; set; }
}

/// <summary>
/// 資料庫查詢 DTO
/// </summary>
public class CardHolderChangeReportDto
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 類型：1=正卡人, 2=附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 附卡人ID
    /// </summary>
    public string? SupplementaryID { get; set; }

    /// <summary>
    /// 變更時間
    /// </summary>
    public DateTime ChangeDateTime { get; set; }

    /// <summary>
    /// 變更人員編號
    /// </summary>
    public string ChangeUserId { get; set; } = null!;

    /// <summary>
    /// 變更人員姓名
    /// </summary>
    public string? ChangeUserName { get; set; }

    /// <summary>
    /// 變更來源（API, Batch, Middleware, PaperMiddleware）
    /// </summary>
    public string? ChangeSource { get; set; }

    /// <summary>
    /// 變更 API 端點
    /// </summary>
    public string? ChangeAPIEndpoint { get; set; }

    /// <summary>
    /// 變更前行動電話
    /// </summary>
    public string? BeforeMobile { get; set; }

    /// <summary>
    /// 變更後行動電話
    /// </summary>
    public string? AfterMobile { get; set; }

    /// <summary>
    /// 變更前 Email
    /// </summary>
    public string? BeforeEmail { get; set; }

    /// <summary>
    /// 變更後 Email
    /// </summary>
    public string? AfterEmail { get; set; }

    /// <summary>
    /// 變更前帳單地址
    /// </summary>
    public string? BeforeBillAddress { get; set; }

    /// <summary>
    /// 變更後帳單地址
    /// </summary>
    public string? AfterBillAddress { get; set; }

    /// <summary>
    /// 變更前寄卡地址
    /// </summary>
    public string? BeforeSendCardAddress { get; set; }

    /// <summary>
    /// 變更後寄卡地址
    /// </summary>
    public string? AfterSendCardAddress { get; set; }
}

/// <summary>
/// 匯出回應（Type=Export 時使用）
/// </summary>
public class ExportCardHolderChangeReportResponse
{
    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// 檔案內容（Excel 二進位資料）
    /// </summary>
    public byte[] FileContent { get; set; } = null!;
}

/// <summary>
/// Excel 匯出 DTO
/// </summary>
public class ExportToExcelDto
{
    [ExcelColumnName("申請書編號")]
    public string 申請書編號 { get; set; } = null!;

    [ExcelColumnName("卡別類型")]
    public string 卡別類型 { get; set; } = null!;

    [ExcelColumnName("附卡人身分證")]
    public string 附卡人身分證 { get; set; } = null!;

    [ExcelColumnName("更改帳號")]
    public string 更改帳號 { get; set; } = null!;

    [ExcelColumnName("更改人員")]
    public string 更改人員 { get; set; } = null!;

    [ExcelColumnName("更改時間")]
    public string 更改時間 { get; set; } = null!;

    [ExcelColumnName("更改來源")]
    public string 更改來源 { get; set; } = null!;

    [ExcelColumnName("更改API端點")]
    public string 更改API端點 { get; set; } = null!;

    [ExcelColumnName("更改前行動電話")]
    public string 更改前行動電話 { get; set; } = null!;

    [ExcelColumnName("更改後行動電話")]
    public string 更改後行動電話 { get; set; } = null!;

    [ExcelColumnName("更改前Email")]
    public string 更改前Email { get; set; } = null!;

    [ExcelColumnName("更改後Email")]
    public string 更改後Email { get; set; } = null!;

    [ExcelColumnName("更改前帳單地址")]
    public string 更改前帳單地址 { get; set; } = null!;

    [ExcelColumnName("更改後帳單地址")]
    public string 更改後帳單地址 { get; set; } = null!;

    [ExcelColumnName("更改前寄卡地址")]
    public string 更改前寄卡地址 { get; set; } = null!;

    [ExcelColumnName("更改後寄卡地址")]
    public string 更改後寄卡地址 { get; set; } = null!;
}
