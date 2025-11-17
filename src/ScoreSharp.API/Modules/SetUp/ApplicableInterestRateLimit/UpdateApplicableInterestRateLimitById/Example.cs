namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.UpdateApplicableInterestRateLimitById;

[ExampleAnnotation(Name = "[2000]修改判斷適用利率額度", ExampleType = ExampleType.Request)]
public class 修改判斷適用利率額度_2000_ReqEx : IExampleProvider<UpdateApplicableInterestRateLimitByIdRequest>
{
    public UpdateApplicableInterestRateLimitByIdRequest GetExample()
    {
        UpdateApplicableInterestRateLimitByIdRequest request = new()
        {
            SeqNo = Guid.Parse("E35252D6-0FD1-4AE1-89F1-DDF65F80F1DC"),
            ApplicableInterestRateLimit = 250000,
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改判斷適用利率額度", ExampleType = ExampleType.Response)]
public class 修改判斷適用利率額度_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("E35252D6-0FD1-4AE1-89F1-DDF65F80F1DC", "E35252D6-0FD1-4AE1-89F1-DDF65F80F1DC");
    }
}

[ExampleAnnotation(Name = "[4003]修改判斷適用利率額度-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改判斷適用利率額度路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
