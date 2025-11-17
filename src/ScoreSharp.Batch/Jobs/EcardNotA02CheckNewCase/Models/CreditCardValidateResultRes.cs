namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class CreditCardValidateResult
{
    public CheckCaseRes<QueryBranchInfo> QueryBranchInfoRes { get; set; } = new();
    public CheckCaseRes<Query929Info> Check929Res { get; set; } = new();
    public CheckCaseRes<ConcernDetailInfo> CheckFocusRes { get; set; } = new();
    public CheckCaseRes<CheckSameIP> SameIPCheckRes { get; set; } = new();
    public CheckCaseRes<bool> InternalIPCheckRes { get; set; } = new();
    public CheckCaseRes<CheckSameWebCaseEmail> SameEmailCheckRes { get; set; } = new();
    public CheckCaseRes<CheckSameWebCaseMobile> SameMobileCheckRes { get; set; } = new();
    public CheckCaseRes<CheckShortTimeID> ShortTimeIDCheckRes { get; set; } = new();
    public CheckCaseRes<QueryCheckName> CheckNameRes { get; set; } = new();
    public CheckCaseRes<CheckInternalEmailSameResult> CheckInternalEmailSameRes { get; set; } = new();
    public CheckCaseRes<CheckInternalMobileSameResult> CheckInternalMobileSameRes { get; set; } = new();

    public CheckCaseRes<bool> CheckRepeatApplyRes { get; set; } = new();
}
