namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundById;

public class GetMainIncomeAndFundByIdResponse
{
    /// <summary>
    /// 主要所得及資金來源代碼，範例: 1
    /// </summary>
    public string MainIncomeAndFundCode { get; set; }

    /// <summary>
    /// 主要所得及資金來源名稱
    /// </summary>
    public string MainIncomeAndFundName { get; set; } = null!;

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
