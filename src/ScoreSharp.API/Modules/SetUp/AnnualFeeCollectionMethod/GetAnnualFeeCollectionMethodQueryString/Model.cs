namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodQueryString;

public class GetAnnualFeeCollectionMethodQueryStringRequest
{
    /// <summary>
    /// 是否啟用
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("^[YN]$")]
    public string? IsActive { get; set; }

    /// <summary>
    /// 年費收取名稱
    /// </summary>
    [Display(Name = "年費收取名稱")]
    public string? AnnualFeeCollectionName { get; set; }
}

public class GetAnnualFeeCollectionMethodQueryStringResponse
{
    /// <summary>
    /// 年費收取代碼
    /// 範例: 1
    /// PK
    /// </summary>
    [Display(Name = "年費收取代碼")]
    public string AnnualFeeCollectionCode { get; set; }

    /// <summary>
    /// 年費收取名稱
    /// </summary>
    [Display(Name = "年費收取名稱")]
    public string AnnualFeeCollectionName { get; set; }

    /// <summary>
    /// 是否啟用
    /// Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string IsActive { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    [Display(Name = "新增員工")]
    public string AddUserId { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    [Display(Name = "新增時間")]
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    [Display(Name = "修正員工")]
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    [Display(Name = "修正時間")]
    public DateTime? UpdateTime { get; set; }
}
