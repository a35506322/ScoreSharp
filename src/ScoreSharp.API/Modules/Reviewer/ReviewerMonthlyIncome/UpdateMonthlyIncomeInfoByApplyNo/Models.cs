namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateMonthlyIncomeInfoByApplyNo;

public class UpdateMonthlyIncomeInfoByApplyNoRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Required]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 現職月收入(元)
    /// </summary>
    [Required]
    public int? CurrentMonthIncome { get; set; }

    /// <summary>
    /// 徵信代碼
    /// 關聯 SetUp_CreditCheckCode
    /// </summary>
    public string? CreditCheckCode { get; set; }
}
