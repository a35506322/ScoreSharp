namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.UpdateApplicableInterestRateLimitById;

public class UpdateApplicableInterestRateLimitByIdRequest
{
    /// <summary>
    /// PK
    /// </summary>
    public Guid SeqNo { get; set; }

    /// <summary>
    /// 判斷適用利率額度，預設一筆且只能有一筆
    /// </summary>
    public int ApplicableInterestRateLimit { get; set; }
}
