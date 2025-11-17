namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateReviewerSummaryBySeqNo;

[ExampleAnnotation(Name = "[2000]修改單筆照會摘要", ExampleType = ExampleType.Request)]
public class 修改單筆照會摘要_2000_ReqEx : IExampleProvider<UpdateReviewerSummaryBySeqNoRequest>
{
    public UpdateReviewerSummaryBySeqNoRequest GetExample()
    {
        UpdateReviewerSummaryBySeqNoRequest request = new()
        {
            SeqNo = 2,
            Record = "照會單位反饋客戶目前正在進行身份更新，需稍後重新提交申請",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改單筆照會摘要", ExampleType = ExampleType.Response)]
public class 修改單筆照會摘要_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("2", "2");
    }
}

[ExampleAnnotation(Name = "[4001]修改單筆照會摘要-查無此資料", ExampleType = ExampleType.Response)]
public class 修改單筆照會摘要查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound("100", "100");
    }
}

[ExampleAnnotation(Name = "[4003]修改單筆照會摘要-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改單筆照會摘要路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
