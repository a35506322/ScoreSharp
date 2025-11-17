namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.DeleteIDCardRenewalLocationById;

[ExampleAnnotation(Name = "[2000]刪除身分證換發地點", ExampleType = ExampleType.Response)]
public class 刪除身分證換發地點_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("65000000");
    }
}

[ExampleAnnotation(Name = "[4001]刪除身分證換發地點-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除身分證換發地點查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "65000001");
    }
}
