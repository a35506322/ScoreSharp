namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.UpdateAMLJobLevelById;

[ExampleAnnotation(Name = "[2000]修改AML職級別", ExampleType = ExampleType.Request)]
public class 修改AML職級別_2000_ReqEx : IExampleProvider<UpdateAMLJobLevelByIdRequest>
{
    public UpdateAMLJobLevelByIdRequest GetExample()
    {
        UpdateAMLJobLevelByIdRequest request = new()
        {
            AMLJobLevelCode = "1",
            AMLJobLevelName = "職員",
            IsSeniorManagers = "N",
            IsActive = "N",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改AML職級別", ExampleType = ExampleType.Response)]
public class 修改AML職級別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("1", "1");
    }
}

[ExampleAnnotation(Name = "[4001]修改AML職級別-查無此資料", ExampleType = ExampleType.Response)]
public class 修改AML職級別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("18", "18");
    }
}

[ExampleAnnotation(Name = "[4003]修改AML職級別-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改AML職級別路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>("18");
    }
}
