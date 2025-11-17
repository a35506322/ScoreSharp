namespace ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountriesByQueryString;

public class GetEUCountriesByQueryStringRequest
{
    /// <summary>
    /// EU國家代碼，範例 : TW
    /// </summary>
    [Display(Name = "EU國家代碼")]
    public string? EUCountryCode { get; set; }

    /// <summary>
    /// EU國家名稱
    /// </summary>
    [Display(Name = "EU國家名稱")]
    public string? EUCountryName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetEUCountriesByQueryStringResponse
{
    /// <summary>
    /// EU國家代碼，範例 : TW
    /// </summary>
    public string EUCountryCode { get; set; } = null!;

    /// <summary>
    /// EU國家名稱
    /// </summary>
    public string EUCountryName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
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
