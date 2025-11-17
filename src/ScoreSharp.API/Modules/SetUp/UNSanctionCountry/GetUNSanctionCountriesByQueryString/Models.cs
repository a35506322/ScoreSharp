namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountriesByQueryString;

public class GetUNSanctionCountriesByQueryStringRequest
{
    /// <summary>
    /// UN制裁國家代碼
    /// </summary>
    [Display(Name = "UN制裁國家代碼")]
    public string? UNSanctionCountryCode { get; set; }

    /// <summary>
    /// UN制裁國家名稱
    /// </summary>
    [Display(Name = "UN制裁國家名稱")]
    public string? UNSanctionCountryName { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetUNSanctionCountriesByQueryStringResponse
{
    /// <summary>
    /// UN制裁國家代碼，範例 : TW
    /// </summary>
    public string UNSanctionCountryCode { get; set; } = null!;

    /// <summary>
    /// UN制裁國家名稱
    /// </summary>
    public string UNSanctionCountryName { get; set; } = null!;

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
