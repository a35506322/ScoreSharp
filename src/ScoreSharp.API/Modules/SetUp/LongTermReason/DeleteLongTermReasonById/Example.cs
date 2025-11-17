namespace ScoreSharp.API.Modules.SetUp.LongTermReason.DeleteLongTermReasonById;

[ExampleAnnotation(Name = "[2000]刪除長循分期戶理由碼", ExampleType = ExampleType.Response)]
public class 刪除長循分期戶理由碼_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("FD");
    }
}

[ExampleAnnotation(Name = "[4001]刪除長循分期戶理由碼-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除長循分期戶理由碼查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "43");
    }
}
