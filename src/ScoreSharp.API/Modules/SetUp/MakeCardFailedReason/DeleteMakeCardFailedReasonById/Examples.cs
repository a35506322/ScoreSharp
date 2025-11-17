namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.DeleteMakeCardFailedReasonById;

[ExampleAnnotation(Name = "[2000]刪除製卡失敗原因", ExampleType = ExampleType.Response)]
public class 刪除製卡失敗原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("13");
    }
}

[ExampleAnnotation(Name = "[4001]刪除製卡失敗原因-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除製卡失敗原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "18");
    }
}
