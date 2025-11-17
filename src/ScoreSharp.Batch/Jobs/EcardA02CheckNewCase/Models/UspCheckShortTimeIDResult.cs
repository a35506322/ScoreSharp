namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class UspCheckShortTimeIDResult
{
    public string IsHit { get; set; }
    public List<HitCheckShortTimeIDApplyNoInfo> HitApplyNoInfos { get; set; }
}

public class HitCheckShortTimeIDApplyNoInfo
{
    public string SameApplyNo { get; set; }
    public string CurrentID { get; set; }
    public string CurrentApplyNo { get; set; }
    public UserType CurrentUserType { get; set; }
    public CheckTraceType CheckType { get; set; }
}
