namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry.UpdateEUSanctionCountryById;

[ExampleAnnotation(Name = "[2000]修改EU制裁國家", ExampleType = ExampleType.Request)]
public class 修改EU制裁國家_2000_ReqEx : IExampleProvider<UpdateEUSanctionCountryByIdRequest>
{
    public UpdateEUSanctionCountryByIdRequest GetExample()
    {
        UpdateEUSanctionCountryByIdRequest request = new()
        {
            EUSanctionCountryCode = "TW",
            EUSanctionCountryName = "呆灣",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改EU制裁國家", ExampleType = ExampleType.Response)]
public class 修改EU制裁國家_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("TW", "TW");
    }
}

[ExampleAnnotation(Name = "[4001]修改EU制裁國家-查無資料", ExampleType = ExampleType.Response)]
public class 修改EU制裁國家查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "TKG");
    }
}

[ExampleAnnotation(Name = "[4003]修改EU制裁國家-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改EU制裁國家路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改EU制裁國家-查無國籍設定資料", ExampleType = ExampleType.Response)]
public class 修改EU制裁國家查無國籍設定資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, $"查無國籍設定資料，請檢查");
    }
}

[ExampleAnnotation(Name = "[4003]修改EU制裁國家-名稱與國籍設定不相同", ExampleType = ExampleType.Response)]
public class 修改EU制裁國家名稱與國籍設定不相同_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "名稱與國籍設定不相同，請檢查");
    }
}
