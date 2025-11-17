namespace ScoreSharp.API.Modules.SetUp.ProjectCode.InsertProjectCode;

[ExampleAnnotation(Name = "[2000]新增專案代號", ExampleType = ExampleType.Request)]
public class 新增專案代號_2000_ReqEx : IExampleProvider<InsertProjectCodeRequest>
{
    public InsertProjectCodeRequest GetExample()
    {
        InsertProjectCodeRequest request = new()
        {
            ProjectCode = "201",
            ProjectName = "催收系統",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增專案代號", ExampleType = ExampleType.Response)]
public class 新增專案代號_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("201", "201");
    }
}

[ExampleAnnotation(Name = "[4002]新增專案代號-資料已存在", ExampleType = ExampleType.Response)]
public class 新增專案代號資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "307");
    }
}
