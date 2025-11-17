namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class UspCheckSameIPResult
{
    public string IsHit { get; set; }
    public List<HitCheckSameIPApplyNoInfo> HitApplyNoInfos { get; set; }
}

public class HitCheckSameIPApplyNoInfo
{
    public string CurrentApplyNo { get; set; }
    public string SameApplyNo { get; set; }
    public string CurrentID { get; set; }

    /// <summary>
    /// 使用者類型
    ///
    /// 1. 正卡人
    /// 2. 附卡人
    /// </summary>
    public UserType CurrentUserType { get; set; }

    /// <summary>
    /// 檢核類型
    ///
    /// 1. 相同IP比對
    /// 2. 網路件 EMAIL 比對
    /// 3. 網路件手機號碼比對
    /// 4. 短時間頻繁ID比對
    /// </summary>
    public CheckTraceType CheckType { get; set; }
}
