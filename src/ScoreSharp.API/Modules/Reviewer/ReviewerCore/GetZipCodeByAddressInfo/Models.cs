namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetZipCodeByAddressInfo;

public class GetZipCodeByAddressInfoRequest
{
    /// <summary>
    /// 縣市
    /// </summary>
    [Required]
    [Display(Name = "縣市")]
    public string City { get; set; }

    /// <summary>
    /// 區域
    /// </summary>
    [Required]
    [Display(Name = "區域")]
    public string District { get; set; }

    /// <summary>
    /// 街道
    /// </summary>
    [Required]
    [Display(Name = "街道")]
    public string Road { get; set; }

    /// <summary>
    /// 門牌號碼
    /// </summary>
    [Required]
    [Display(Name = "門牌號碼")]
    public int Number { get; set; }

    /// <summary>
    /// 門牌號碼2
    /// </summary>
    [Display(Name = "門牌號碼2")]
    public int SubNumber { get; set; }

    /// <summary>
    /// 巷子
    /// </summary>
    [Display(Name = "巷子")]
    public int Lane { get; set; }
}
