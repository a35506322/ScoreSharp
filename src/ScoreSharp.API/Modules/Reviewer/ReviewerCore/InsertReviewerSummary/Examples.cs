using ScoreSharp.API.Modules.SetUp.BlackListReason.InsertBlackListReason;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertReviewerSummary;

[ExampleAnnotation(Name = "[2000]新增單筆照會摘要", ExampleType = ExampleType.Request)]
public class 新增單筆照會摘要_2000_ReqEx : IExampleProvider<InsertReviewerSummaryRequest>
{
    public InsertReviewerSummaryRequest GetExample()
    {
        InsertReviewerSummaryRequest request = new() { ApplyNo = "20241126F2347", Record = "(預)活存明細無帳號且餘額未達" };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增單筆照會摘要")]
public class 新增單筆照會摘要_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("20241126F2347", "20241126F2347");
    }
}

[ExampleAnnotation(Name = "[4001]新增單筆照會摘要-查無資料")]
public class 新增單筆照會摘要查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("20241107B3463", "20241107B3463");
    }
}
