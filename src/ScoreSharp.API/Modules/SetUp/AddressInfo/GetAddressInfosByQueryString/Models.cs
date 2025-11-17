namespace ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfosByQueryString;

public class GetAddressInfosByQueryStringRequest
{
    /// <summary>
    /// 郵遞區號
    /// </summary>
    [Display(Name = "郵遞區號")]
    public string? ZIPCode { get; set; }

    /// <summary>
    /// 縣市，例如新北市
    /// </summary>
    [Display(Name = "縣市")]
    public string? City { get; set; }

    /// <summary>
    /// 區域，例如蘆洲區
    /// </summary>
    [Display(Name = "區域")]
    public string? Area { get; set; }

    /// <summary>
    /// 街道
    /// </summary>
    [Display(Name = "街道")]
    public string? Road { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }
}

public class GetAddressInfosByQueryStringResponse
{
    /// <summary>
    /// 流水號
    /// </summary>
    [Display(Name = "流水號")]
    public string SeqNo { get; set; } = null!;

    /// <summary>
    /// 郵遞區號
    /// </summary>
    [Display(Name = "郵遞區號")]
    public string ZIPCode { get; set; } = null!;

    /// <summary>
    /// 縣市，例如新北市
    /// </summary>
    [Display(Name = "縣市")]
    public string City { get; set; } = null!;

    /// <summary>
    /// 區域，例如蘆洲區
    /// </summary>
    [Display(Name = "區域")]
    public string Area { get; set; } = null!;

    /// <summary>
    /// 街道
    /// </summary>
    [Display(Name = "街道")]
    public string Road { get; set; } = null!;

    /// <summary>
    /// 號判斷郵遞區號規則
    /// </summary>
    [Display(Name = "號段範圍")]
    public string Scope { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string IsActive { get; set; } = null!;

    /// <summary>
    /// 新增員工
    /// </summary>
    [Display(Name = "新增員工")]
    public string? AddUserId { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    [Display(Name = "新增時間")]
    public DateTime? AddTime { get; set; }

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
