namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationByQueryString;

public class GetIDCardRenewalLocationByQueryStringRequest
{
    /// <summary>
    /// 身分證換發地點代碼，範例: 09007000
    /// </summary>
    [Display(Name = "身分證換發地點代碼")]
    public string? IDCardRenewalLocationCode { get; set; }

    /// <summary>
    /// 身分證換發地點名稱，範例: 北市
    /// </summary>
    [Display(Name = "身分證換發地點名稱")]
    public string? IDCardRenewalLocationName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}

public class GetIDCardRenewalLocationByQueryStringResponse
{
    /// <summary>
    /// 身分證換發地點代碼，範例: 09007000
    /// </summary>
    public string IDCardRenewalLocationCode { get; set; } = null!;

    /// <summary>
    /// 身分證換發地點名稱，範例: 北市
    /// </summary>
    public string IDCardRenewalLocationName { get; set; } = null!;

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
