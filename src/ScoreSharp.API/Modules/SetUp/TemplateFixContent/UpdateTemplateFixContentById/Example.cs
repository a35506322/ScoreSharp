namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.UpdateTemplateFixContentById;

[ExampleAnnotation(Name = "[2000]修改單筆樣板固定值", ExampleType = ExampleType.Request)]
public class 修改單筆樣板固定值_2000_ReqEx : IExampleProvider<UpdateTemplateFixContentByIdRequest>
{
    public UpdateTemplateFixContentByIdRequest GetExample()
    {
        UpdateTemplateFixContentByIdRequest request = new()
        {
            SeqNo = 1,
            TemplateId = "A01",
            TemplateKey = "Phone",
            TemplateValue = "02-24567891",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改單筆樣板固定值", ExampleType = ExampleType.Response)]
public class 修改單筆樣板固定值_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("1", "1");
    }
}

[ExampleAnnotation(Name = "[4001]修改單筆樣板固定值-查無此資料", ExampleType = ExampleType.Response)]
public class 修改單筆樣板固定值查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "A05");
    }
}

[ExampleAnnotation(Name = "[4003]修改單筆樣板固定值-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改單筆樣板固定值路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改單筆樣板固定值-無效的樣板ID", ExampleType = ExampleType.Response)]
public class 修改單筆樣板固定值無效的樣板ID_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "無效的樣板ID，請重新確認。");
    }
}
