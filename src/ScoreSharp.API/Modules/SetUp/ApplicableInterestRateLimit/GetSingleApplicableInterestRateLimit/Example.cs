namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.GetSingleApplicableInterestRateLimit;

[ExampleAnnotation(Name = "[2000]取得判斷適用利率額度", ExampleType = ExampleType.Response)]
public class 取得判斷適用利率額度_2000_ResEx : IExampleProvider<ResultResponse<GetSingleApplicableInterestRateLimitResponse>>
{
    public ResultResponse<GetSingleApplicableInterestRateLimitResponse> GetExample()
    {
        GetSingleApplicableInterestRateLimitResponse response = new()
        {
            SeqNo = Guid.Parse("E35252D6-0FD1-4AE1-89F1-DDF65F80F1DC"),
            ApplicableInterestRateLimit = 200000,
            UpdateTime = DateTime.Now,
            UpdateUserId = "Admin",
        };
        return ApiResponseHelper.Success(response);
    }
}
