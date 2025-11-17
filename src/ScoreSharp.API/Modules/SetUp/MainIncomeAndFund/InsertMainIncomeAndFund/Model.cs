namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.InsertMainIncomeAndFund;

public class InsertMainIncomeAndFundRequest
{
    /// <summary>
    /// 主要所得及資金來源代碼，範例: 1
    /// </summary>
    [Display(Name = "主要所得及資金來源代碼")]
    [Required]
    [RegularExpression("^[0-9]+$")]
    public string MainIncomeAndFundCode { get; set; }

    /// <summary>
    /// 主要所得及資金來源名稱
    /// </summary>
    [Display(Name = "主要所得及資金來源名稱")]
    [Required]
    public string MainIncomeAndFundName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
