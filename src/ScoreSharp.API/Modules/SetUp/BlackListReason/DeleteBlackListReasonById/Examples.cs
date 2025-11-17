namespace ScoreSharp.API.Modules.SetUp.BlackListReason.DeleteBlackListReasonById;

[ExampleAnnotation(Name = "[2000]刪除黑名單理由", ExampleType = ExampleType.Response)]
public class 刪除黑名單理由_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("C7");
    }
}

[ExampleAnnotation(Name = "[4001]刪除黑名單理由-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除黑名單理由查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "C7");
    }
}
