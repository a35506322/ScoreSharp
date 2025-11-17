namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.DeleteReviewerSummaryBySeqNo;

[ExampleAnnotation(Name = "[4001]刪除單筆照會摘要-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除照會摘要查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆照會摘要", ExampleType = ExampleType.Response)]
public class 刪除照會摘要_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}
