namespace ScoreSharp.API.Modules.SetUp.EUCountry.InsertEUCountry;

[ExampleAnnotation(Name = "[2000]新增EU國家", ExampleType = ExampleType.Request)]
public class 新增EU國家_2000_ReqEx : IExampleProvider<InsertEUCountryRequest>
{
    public InsertEUCountryRequest GetExample()
    {
        InsertEUCountryRequest request = new()
        {
            EUCountryCode = "TH",
            EUCountryName = "泰國",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增EU國家", ExampleType = ExampleType.Response)]
public class 新增EU國家_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("TH", "TH");
    }
}

[ExampleAnnotation(Name = "[4002]新增EU國家-資料已存在", ExampleType = ExampleType.Response)]
public class 新增EU國家資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "TW");
    }
}

[ExampleAnnotation(Name = "[4003]新增EU國家-查無國籍設定資料", ExampleType = ExampleType.Response)]
public class 新增EU國家查無國籍設定資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");
    }
}
