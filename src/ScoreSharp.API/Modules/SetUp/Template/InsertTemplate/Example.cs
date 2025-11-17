namespace ScoreSharp.API.Modules.SetUp.Template.InsertTemplate;

[ExampleAnnotation(Name = "[2000]新增樣板", ExampleType = ExampleType.Request)]
public class 新增樣板_2000_ReqEx : IExampleProvider<InsertTemplateRequest>
{
    public InsertTemplateRequest GetExample()
    {
        InsertTemplateRequest request = new()
        {
            TemplateId = "A01",
            TemplateName = "補件函",
            IsActive = "Y",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增樣板", ExampleType = ExampleType.Response)]
public class 新增樣板_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("A01", "A01");
    }
}

[ExampleAnnotation(Name = "[4002]樣板-資料已存在", ExampleType = ExampleType.Response)]
public class 新增樣板資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "Z99");
    }
}
