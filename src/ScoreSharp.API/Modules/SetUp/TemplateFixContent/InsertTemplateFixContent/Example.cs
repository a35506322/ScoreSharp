namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.InsertTemplateFixContent;

[ExampleAnnotation(Name = "[2000]新增樣板固定值", ExampleType = ExampleType.Request)]
public class 新增樣板固定值_2000_ReqEx : IExampleProvider<InsertTemplateFixContentRequest>
{
    public InsertTemplateFixContentRequest GetExample()
    {
        InsertTemplateFixContentRequest request = new()
        {
            TemplateId = "A01",
            TemplateKey = "FaxNumber",
            TemplateValue = "02-87526169　07-3302783",
            IsActive = "Y",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增樣板固定值", ExampleType = ExampleType.Response)]
public class 新增樣板固定值_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("5", "5");
    }
}

[ExampleAnnotation(Name = "[4002]樣板固定值-資料已存在", ExampleType = ExampleType.Response)]
public class 新增樣板固定值資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "TemplateId = 已存在的樣板ID & TemplateKey = 已存在的樣板Key值");
    }
}

[ExampleAnnotation(Name = "[4003]樣板固定值-無效的樣板ID", ExampleType = ExampleType.Response)]
public class 新增樣板固定值無效的樣板ID_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "無效的樣板ID，請重新確認。");
    }
}
