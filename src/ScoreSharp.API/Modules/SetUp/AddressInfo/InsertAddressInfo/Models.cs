namespace ScoreSharp.API.Modules.SetUp.AddressInfo.InsertAddressInfo;

public class InsertAddressInfoRequest
{
    /// <summary>
    /// 郵遞區號
    /// </summary>
    [Display(Name = "郵遞區號")]
    [Required(ErrorMessage = "郵遞區號為必填")]
    public string ZIPCode { get; set; } = null!;

    /// <summary>
    /// 縣市，例如新北市
    /// </summary>
    [Display(Name = "縣市")]
    [Required(ErrorMessage = "縣市為必填")]
    public string City { get; set; } = null!;

    /// <summary>
    /// 區域，例如蘆洲區
    /// </summary>
    [Display(Name = "區域")]
    [Required(ErrorMessage = "區域為必填")]
    public string Area { get; set; } = null!;

    /// <summary>
    /// 街道
    /// </summary>
    [Display(Name = "街道")]
    [Required(ErrorMessage = "街道為必填")]
    public string Road { get; set; } = null!;

    /// <summary>
    /// 號判斷郵遞區號規則
    /// </summary>
    [Display(Name = "號段範圍")]
    [Required(ErrorMessage = "號段範圍為必填")]
    public string Scope { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required(ErrorMessage = "是否啟用為必填")]
    [RegularExpression("[YN]", ErrorMessage = "是否啟用只能是 Y 或 N")]
    public string IsActive { get; set; } = null!;
}
