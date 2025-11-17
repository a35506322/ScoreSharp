namespace ScoreSharp.API.Modules.SetUp.Template.UpdateTemplateById;

[ExampleAnnotation(Name = "[2000]修改單筆樣板", ExampleType = ExampleType.Request)]
public class 修改單筆樣板_2000_ReqEx : IExampleProvider<UpdateTemplateByIdRequest>
{
    public UpdateTemplateByIdRequest GetExample()
    {
        UpdateTemplateByIdRequest request = new()
        {
            TemplateName = "Test通知函_範本",
            TemplateId = "Z99",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改單筆樣板", ExampleType = ExampleType.Response)]
public class 修改單筆樣板_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("Z99", "Z99");
    }
}

[ExampleAnnotation(Name = "[4001]修改單筆樣板-查無此資料", ExampleType = ExampleType.Response)]
public class 修改單筆樣板查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "A05");
    }
}

[ExampleAnnotation(Name = "[4003]修改單筆樣板-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改單筆樣板路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
