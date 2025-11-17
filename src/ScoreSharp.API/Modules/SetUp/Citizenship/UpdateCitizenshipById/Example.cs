namespace ScoreSharp.API.Modules.SetUp.Citizenship.UpdateCitizenshipById;

[ExampleAnnotation(Name = "[2000]修改國籍", ExampleType = ExampleType.Request)]
public class 修改國籍_2000_ReqEx : IExampleProvider<UpdateCitizenshipByIdRequest>
{
    public UpdateCitizenshipByIdRequest GetExample()
    {
        UpdateCitizenshipByIdRequest request = new()
        {
            CitizenshipCode = "TW",
            CitizenshipName = "呆灣",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改國籍", ExampleType = ExampleType.Response)]
public class 修改國籍_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("TW", "TW");
    }
}

[ExampleAnnotation(Name = "[4001]修改國籍-查無資料", ExampleType = ExampleType.Response)]
public class 修改國籍查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "TKG");
    }
}

[ExampleAnnotation(Name = "[4003]修改國籍-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改國籍路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
