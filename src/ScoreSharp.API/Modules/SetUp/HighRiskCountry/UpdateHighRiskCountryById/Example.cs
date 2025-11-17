namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.UpdateHighRiskCountryById;

[ExampleAnnotation(Name = "[2000]單筆修改洗錢及資恐高風險國家", ExampleType = ExampleType.Request)]
public class 單筆修改洗錢及資恐高風險國家_2000_ReqEx : IExampleProvider<UpdateHighRiskCountryByIdRequest>
{
    public UpdateHighRiskCountryByIdRequest GetExample()
    {
        UpdateHighRiskCountryByIdRequest request = new()
        {
            HighRiskCountryCode = "TW",
            HighRiskCountryName = "呆灣",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改洗錢及資恐高風險國家", ExampleType = ExampleType.Response)]
public class 單筆修改洗錢及資恐高風險國家_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("TW", "TW");
    }
}

[ExampleAnnotation(Name = "[4001]修改洗錢及資恐高風險國家-查無資料", ExampleType = ExampleType.Response)]
public class 修改洗錢及資恐高風險國家查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "TKG");
    }
}

[ExampleAnnotation(Name = "[4003]修改洗錢及資恐高風險國家-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改洗錢及資恐高風險國家路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改洗錢及資恐高風險國家-查無國籍設定資料", ExampleType = ExampleType.Response)]
public class 修改洗錢及資恐高風險國家查無國籍設定資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, $"查無國籍設定資料，請檢查");
    }
}

[ExampleAnnotation(Name = "[4003]修改洗錢及資恐高風險國家-名稱與國籍設定不相同", ExampleType = ExampleType.Response)]
public class 修改洗錢及資恐高風險國家名稱與國籍設定不相同_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "名稱與國籍設定不相同，請檢查");
    }
}
