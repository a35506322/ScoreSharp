using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class UspCheckSameMobileResult
{
    public string IsHit { get; set; }
    public List<HitCheckSameMobileApplyNoInfo> HitApplyNoInfos { get; set; }
}

public class HitCheckSameMobileApplyNoInfo
{
    public string SameApplyNo { get; set; }
    public string CurrentID { get; set; }
    public string CurrentApplyNo { get; set; }
    public UserType CurrentUserType { get; set; }
    public CheckTraceType CheckType { get; set; }
}
