namespace ScoreSharp.API.Modules.SetUp.RejectionReason.DeleteRejectionReasonById;

[ExampleAnnotation(Name = "[2000]刪除退件原因", ExampleType = ExampleType.Response)]
public class 刪除退件原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}

[ExampleAnnotation(Name = "[4001]刪除退件原因-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除退件原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}
