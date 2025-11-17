namespace ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDayById;

public class GetWorkDayByIdResponse
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
