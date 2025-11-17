namespace ScoreSharp.API.Modules.Report.SameMobileReport.ExportSameMobileReportByQueryString;

public class ExportSameMobileReportByQueryStringResponse
{
    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; }

    public byte[] FileContent { get; set; }
}

public class ExportSameMobileReportByQueryStringExportDto
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string CHName { get; set; }

    /// <summary>
    /// 申請書狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 公司名稱
    /// </summary>
    public string? CompName { get; set; }

    /// <summary>
    /// 手機號碼
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// OTP手機
    /// </summary>
    public string OTPMobile { get; set; }

    /// <summary>
    /// 推廣單位
    /// </summary>
    public string? PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    public string? PromotionUser { get; set; }

    /// <summary>
    /// 比對結果
    /// </summary>
    public string SameWebCaseMobileChecked { get; set; }

    /// <summary>
    /// 同手機號碼之申請書編號
    /// </summary>
    public string? SameApplyNo { get; set; }

    /// <summary>
    /// 同手機號碼之身分證字號
    /// </summary>
    public string? SameID { get; set; }

    /// <summary>
    /// 同手機號碼之姓名
    /// </summary>
    public string? SameName { get; set; }

    /// <summary>
    /// 同手機號碼之申請書狀態
    /// </summary>
    public CardStatus? SameCardStatus { get; set; }

    /// <summary>
    /// 同手機號碼之公司名稱
    /// </summary>
    public string? SameCompName { get; set; }

    /// <summary>
    /// 同手機號碼之OTP手機
    /// </summary>
    public string? SameOTPMobile { get; set; }

    /// <summary>
    /// 是否異常
    /// </summary>
    public string? IsError { get; set; }

    /// <summary>
    /// 確認紀錄
    /// </summary>
    public string? CheckRecord { get; set; }

    /// <summary>
    /// 確認人員
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 確認時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}

public class ExportToExcelDto
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [ExcelColumnName("申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    [ExcelColumnName("身分證字號")]
    public string ID { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [ExcelColumnName("姓名")]
    public string CHName { get; set; }

    /// <summary>
    /// 申請書狀態
    /// </summary>
    [ExcelColumnName("申請書狀態")]
    public string CardStatusName { get; set; }

    /// <summary>
    /// 公司名稱
    /// </summary>
    [ExcelColumnName("公司名稱")]
    public string? CompName { get; set; }

    /// <summary>
    /// 手機號碼
    /// </summary>
    [ExcelColumnName("手機號碼")]
    public string Mobile { get; set; }

    /// <summary>
    /// OTP手機
    /// </summary>
    [ExcelColumnName("OTP手機")]
    public string OTPMobile { get; set; }

    /// <summary>
    /// 推廣單位
    /// </summary>
    [ExcelColumnName("推廣單位")]
    public string? PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    [ExcelColumnName("推廣人員")]
    public string? PromotionUser { get; set; }

    /// <summary>
    /// 比對結果
    /// </summary>
    [ExcelColumnName("比對結果")]
    public string SameWebCaseMobileChecked { get; set; }

    /// <summary>
    /// 同手機號碼之申請書編號
    /// </summary>
    [ExcelColumnName("同手機號碼之申請書編號")]
    public string? SameApplyNo { get; set; }

    /// <summary>
    /// 同手機號碼之身分證字號
    /// </summary>
    [ExcelColumnName("同手機號碼之身分證字號")]
    public string? SameID { get; set; }

    /// <summary>
    /// 同手機號碼之姓名
    /// </summary>
    [ExcelColumnName("同手機號碼之姓名")]
    public string? SameName { get; set; }

    /// <summary>
    /// 同手機號碼之申請書狀態
    /// </summary>
    [ExcelColumnName("同手機號碼之申請書狀態")]
    public string? SameCardStatusName { get; set; }

    /// <summary>
    /// 同手機號碼之公司名稱
    /// </summary>
    [ExcelColumnName("同手機號碼之公司名稱")]
    public string? SameCompName { get; set; }

    /// <summary>
    /// 同手機號碼之OTP手機
    /// </summary>
    [ExcelColumnName("同手機號碼之OTP手機")]
    public string? SameOTPMobile { get; set; }

    /// <summary>
    /// 是否異常
    /// </summary>
    [ExcelColumnName("是否異常")]
    public string? IsError { get; set; }

    /// <summary>
    /// 確認紀錄
    /// </summary>
    [ExcelColumnName("確認紀錄")]
    public string? CheckRecord { get; set; }

    /// <summary>
    /// 確認人員
    /// </summary>
    [ExcelColumnName("確認人員")]
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 確認時間
    /// </summary>
    [ExcelColumnName("確認時間")]
    public string? UpdateTime { get; set; }
}

public class ExportSameMobileReportByQueryStringRequest : IValidatableObject
{
    /// <summary>
    /// 開始日期
    /// </summary>
    [Display(Name = "開始日期")]
    [Required]
    [RegularExpression(@"^\d{4}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$")]
    public string StartDate { get; set; }

    /// <summary>
    /// 結束日期
    /// </summary>
    [Display(Name = "結束日期")]
    [Required]
    [RegularExpression(@"^\d{4}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$")]
    public string EndDate { get; set; }

    /// <summary>
    /// 比對結果
    /// 只能選擇 Y 或 ALL (Y+N)
    /// </summary>
    [Display(Name = "比對結果")]
    [Required]
    [RegularExpression("^(Y|ALL)$")]
    public string ComparisonResult { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!DateTime.TryParseExact(StartDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _))
        {
            yield return new ValidationResult("無效的日期，請輸入有效的 YYYY/MM/DD 日期", new[] { nameof(StartDate) });
        }
        if (!DateTime.TryParseExact(EndDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _))
        {
            yield return new ValidationResult("無效的日期，請輸入有效的 YYYY/MM/DD 日期", new[] { nameof(EndDate) });
        }
        DateTime startDate = DateTime.Parse(StartDate);
        DateTime endDate = DateTime.Parse(EndDate);
        if (startDate > endDate)
        {
            yield return new ValidationResult("開始日期不可大於結束日期", new[] { nameof(StartDate), nameof(EndDate) });
        }
        else if (endDate > DateTime.Now)
        {
            yield return new ValidationResult("結束日期不可大於今天", new[] { nameof(EndDate) });
        }
        else if (startDate.AddMonths(3) < endDate)
        {
            yield return new ValidationResult("查詢範圍最多三個月", new[] { nameof(StartDate), nameof(EndDate) });
        }
    }
}
