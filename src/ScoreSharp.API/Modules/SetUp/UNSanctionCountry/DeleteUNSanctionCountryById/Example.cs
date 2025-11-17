namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.DeleteUNSanctionCountryById;

[ExampleAnnotation(Name = "[2000]刪除UN制裁國家", ExampleType = ExampleType.Response)]
public class 刪除UN制裁國家_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("TW");
    }
}

[ExampleAnnotation(Name = "[4001]刪除UN制裁國家-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除UN制裁國家查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "JP");
    }
}
