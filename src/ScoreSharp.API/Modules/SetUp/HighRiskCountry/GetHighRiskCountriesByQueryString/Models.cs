namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountriesByQueryString;

public class GetHighRiskCountriesByQueryStringRequest
{
    /// <summary>
    /// 洗錢及資恐高風險國家代碼，範例：TW
    /// </summary>
    [Display(Name = "洗錢及資恐高風險國家代碼")]
    public string? HighRiskCountryCode { get; set; }

    /// <summary>
    /// 洗錢及資恐高風險國家名稱
    /// </summary>
    [Display(Name = "洗錢及資恐高風險國家名稱")]
    public string? HighRiskCountryName { get; set; }

    /// <summary>
    /// 是否啟用，範例：Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetHighRiskCountriesByQueryStringResponse
{
    /// <summary>
    /// 洗錢及資恐高風險國家代碼，範例：TW
    /// </summary>
    public string HighRiskCountryCode { get; set; } = null!;

    /// <summary>
    /// 洗錢及資恐高風險國家名稱
    /// </summary>
    public string HighRiskCountryName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，範例：Y | N
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
