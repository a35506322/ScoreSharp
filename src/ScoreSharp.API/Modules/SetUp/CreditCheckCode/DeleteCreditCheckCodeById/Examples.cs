namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.DeleteCreditCheckCodeById;

[ExampleAnnotation(Name = "[2000]刪除徵信代碼", ExampleType = ExampleType.Response)]
public class 刪除徵信代碼_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("A05");
    }
}

[ExampleAnnotation(Name = "[4001]刪除徵信代碼-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除徵信代碼查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(string.Empty, "A06");
    }
}
