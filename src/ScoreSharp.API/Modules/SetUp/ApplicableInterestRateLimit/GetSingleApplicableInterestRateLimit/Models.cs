namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.GetSingleApplicableInterestRateLimit;

public class GetSingleApplicableInterestRateLimitResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public Guid SeqNo { get; set; }

    /// <summary>
    /// 判斷適用利率額度，預設一筆且只能有一筆
    /// </summary>
    public int ApplicableInterestRateLimit { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
