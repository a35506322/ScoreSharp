namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

public class CaseInfoContext
{
    /// <summary>
    /// 卡別類型（正卡、附卡、正卡_附卡）。
    /// </summary>
    public CardOwner CardOwner { get; init; }

    /// <summary>
    /// 來源
    /// </summary>
    public Source Source { get; init; }

    /// <summary>
    /// 正卡人帳單類型。
    /// </summary>
    public BillType? M_BillType { get; init; }
}
