namespace ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDaysByQueryString;

public class GetWorkDaysByQueryStringRequest : IValidatableObject
{
    /// <summary>
    /// 指定單日，格式 yyyyMMdd
    /// </summary>
    [Display(Name = "指定日期")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "日期格式必須為 yyyyMMdd")]
    public string? Date { get; set; }

    /// <summary>
    /// 起始日期 (區間查詢)，格式 yyyyMMdd
    /// </summary>
    [Display(Name = "起始日期")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "起始日期格式必須為 yyyyMMdd")]
    public string? StartDate { get; set; }

    /// <summary>
    /// 結束日期 (區間查詢)，格式 yyyyMMdd
    /// </summary>
    [Display(Name = "結束日期")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "結束日期格式必須為 yyyyMMdd")]
    public string? EndDate { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [Display(Name = "年份")]
    public string? Year { get; set; }

    /// <summary>
    /// 是否放假：Y / N
    /// </summary>
    [Display(Name = "是否放假")]
    [RegularExpression("[YN]", ErrorMessage = "是否放假只能是 Y 或 N")]
    public string? IsHoliday { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // 檢查起訖日期是否同時存在
        if (!String.IsNullOrWhiteSpace(StartDate) && String.IsNullOrWhiteSpace(EndDate))
        {
            yield return new ValidationResult("當起始日期有值時，結束日期也必須有值", new[] { nameof(EndDate) });
        }

        if (!String.IsNullOrWhiteSpace(EndDate) && String.IsNullOrWhiteSpace(StartDate))
        {
            yield return new ValidationResult("當結束日期有值時，起始日期也必須有值", new[] { nameof(StartDate) });
        }

        // 只有當起訖日期都有值時，才進行格式和邏輯驗證
        if (!String.IsNullOrWhiteSpace(StartDate) && !String.IsNullOrWhiteSpace(EndDate))
        {
            // 檢查日期格式
            bool startDateValid = DateTime.TryParseExact(StartDate, "yyyyMMdd", null, DateTimeStyles.None, out DateTime startDateTime);
            bool endDateValid = DateTime.TryParseExact(EndDate, "yyyyMMdd", null, DateTimeStyles.None, out DateTime endDateTime);

            if (!startDateValid || !endDateValid)
            {
                yield return new ValidationResult("起始日期或結束日期格式不正確", new[] { nameof(StartDate), nameof(EndDate) });
            }
            else if (startDateTime > endDateTime)
            {
                yield return new ValidationResult("起始日期不能大於結束日期", new[] { nameof(StartDate) });
            }
        }
    }
}

public class GetWorkDaysByQueryStringResponse
{
    /// <summary>
    /// 工作日日期，格式 yyyyMMdd
    /// </summary>
    [Display(Name = "日期")]
    public string Date { get; set; } = null!;

    /// <summary>
    /// 所屬年份
    /// </summary>
    [Display(Name = "年份")]
    public string Year { get; set; } = null!;

    /// <summary>
    /// 節日／紀念日名稱
    /// </summary>
    [Display(Name = "節日名稱")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 是否放假：Y / N
    /// </summary>
    [Display(Name = "是否放假")]
    public string IsHoliday { get; set; } = null!;

    /// <summary>
    /// 假日分類
    /// </summary>
    [Display(Name = "假日分類")]
    public string? HolidayCategory { get; set; }

    /// <summary>
    /// 補充說明
    /// </summary>
    [Display(Name = "補充說明")]
    public string? Description { get; set; }

    /// <summary>
    /// 建立者帳號
    /// </summary>
    [Display(Name = "建立者帳號")]
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 建立時間
    /// </summary>
    [Display(Name = "建立時間")]
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 最後更新者帳號
    /// </summary>
    [Display(Name = "最後更新者帳號")]
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 最後更新時間
    /// </summary>
    [Display(Name = "最後更新時間")]
    public DateTime? UpdateTime { get; set; }
}
