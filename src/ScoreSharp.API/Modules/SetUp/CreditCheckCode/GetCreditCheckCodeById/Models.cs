namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodeById;

public class GetCreditCheckCodeByIdResponse
{
    /// <summary>
    /// 徵信代碼代碼，範例: A01
    /// </summary>
    public string CreditCheckCode { get; set; }

    /// <summary>
    /// 徵信代碼名稱
    /// </summary>
    public string CreditCheckCodeName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    public string IsActive { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; }

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
