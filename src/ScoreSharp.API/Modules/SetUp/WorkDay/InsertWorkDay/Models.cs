namespace ScoreSharp.API.Modules.SetUp.WorkDay.InsertWorkDay;

public class InsertWorkDayRequest : IValidatableObject
{
    /// <summary>
    /// 工作日日期，格式 yyyyMMdd
    /// </summary>
    [Display(Name = "日期")]
    [Required(ErrorMessage = "日期為必填")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "日期格式必須為 yyyyMMdd")]
    public string Date { get; set; } = null!;

    /// <summary>
    /// 所屬年份
    /// </summary>
    [Display(Name = "年份")]
    [Required(ErrorMessage = "年份為必填")]
    [Range(1900, 2999, ErrorMessage = "年份必須在 1900 到 2999 之間")]
    public string Year { get; set; } = null!;

    /// <summary>
    /// 節日／紀念日名稱
    /// </summary>
    [Display(Name = "節日名稱")]
    [Required(ErrorMessage = "節日名稱為必填")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 是否放假：Y / N
    /// </summary>
    [Display(Name = "是否放假")]
    [Required(ErrorMessage = "是否放假為必填")]
    [RegularExpression("[YN]", ErrorMessage = "是否放假只能是 Y 或 N")]
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

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!String.IsNullOrWhiteSpace(Date) && !DateTime.TryParseExact(Date, "yyyyMMdd", null, DateTimeStyles.None, out _))
        {
            yield return new ValidationResult("日期格式不正確", new[] { nameof(Date) });
        }
    }
}
