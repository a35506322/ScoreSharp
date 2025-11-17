namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountriesByQueryString;

public class GetHighFinancialSecrecyCountriesByQueryStringRequest
{
    /// <summary>
    /// 高金融保密國家代碼，舉例：TW
    /// </summary>
    [Display(Name = "高金融保密國家代碼")]
    public string? HighFinancialSecrecyCountryCode { get; set; }

    /// <summary>
    /// 高金融保密國家名稱
    /// </summary>
    [Display(Name = "高金融保密國家名稱")]
    public string? HighFinancialSecrecyCountryName { get; set; }

    /// <summary>
    /// 是否啟用，範例：Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetHighFinancialSecrecyCountriesByQueryStringResponse
{
    /// <summary>
    /// 高金融保密國家代碼，舉例：TW
    /// </summary>
    public string HighFinancialSecrecyCountryCode { get; set; } = null!;

    /// <summary>
    /// 高金融保密國家名稱
    /// </summary>
    public string HighFinancialSecrecyCountryName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，舉例：Y | N
    /// </summary>
    public string IsActive { get; set; } = null!;

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
