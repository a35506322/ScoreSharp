namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.DeleteApplyFileAttachmentBySeqNo;

[ExampleAnnotation(Name = "[4001]刪除徵審行員附件-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除徵審行員附件查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除徵審行員附件", ExampleType = ExampleType.Response)]
public class 刪除徵審行員附件_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}
