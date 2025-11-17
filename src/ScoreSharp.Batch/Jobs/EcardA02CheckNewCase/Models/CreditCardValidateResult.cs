namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class CreditCardValidateResult
{
    public CheckCaseRes<QueryOriginalCardholderData> QueryOriginalCardholderDataRes { get; set; } = new();
    public CheckCaseRes<CheckInternalEmailSameResult> CheckInternalEmailSameRes { get; set; } = new();
    public CheckCaseRes<CheckInternalMobileSameResult> CheckInternalMobileSameRes { get; set; } = new();
    public CheckCaseRes<Check929Info> Check929Res { get; set; } = new();
    public CheckCaseRes<bool> CheckInternalIPRes { get; set; } = new();
    public CheckCaseRes<CheckSameIP> CheckSameIPRes { get; set; } = new();
    public CheckCaseRes<CheckSameWebCaseEmail> CheckSameWebCaseEmailRes { get; set; } = new();
    public CheckCaseRes<CheckSameWebMobile> CheckSameWebMobileRes { get; set; } = new();
    public CheckCaseRes<CheckShortTimeID> CheckShortTimeIDRes { get; set; } = new();
    public CheckCaseRes<ConcernDetailInfo> CheckFocusRes { get; set; } = new();
    public CheckCaseRes<bool> CheckRepeatApplyRes { get; set; } = new();
}
