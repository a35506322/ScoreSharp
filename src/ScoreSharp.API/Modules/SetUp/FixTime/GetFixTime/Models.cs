namespace ScoreSharp.API.Modules.SetUp.FixTime.GetFixTime;

public class GetFixTimeResponse
{
    /// <summary>
    /// 流水號
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// KYC是否維護 (Y/N)
    /// </summary>
    public string KYC_IsFix { get; set; } = null!;

    /// <summary>
    /// KYC維護開始時間
    /// </summary>
    public DateTime? KYC_StartTime { get; set; }

    /// <summary>
    /// KYC維護結束時間
    /// </summary>
    public DateTime? KYC_EndTime { get; set; }

    /// <summary>
    /// 更新人員
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
