namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class UspCheckSameEmailResult
{
    public string IsHit { get; set; }
    public List<HitCheckSameEmailApplyNoInfo> HitApplyNoInfos { get; set; }
}

public class HitCheckSameEmailApplyNoInfo
{
    public string SameApplyNo { get; set; }
    public string CurrentID { get; set; }
    public string CurrentApplyNo { get; set; }
    public UserType CurrentUserType { get; set; }
    public CheckTraceType CheckType { get; set; }
}
