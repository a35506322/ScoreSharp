namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.InsertAMLJobLevel;

[ExampleAnnotation(Name = "[2000]新增AML職級別", ExampleType = ExampleType.Request)]
public class 新增AML職級別_2000_ReqEx : IExampleProvider<InsertAMLJobLevelRequest>
{
    public InsertAMLJobLevelRequest GetExample()
    {
        InsertAMLJobLevelRequest request = new()
        {
            AMLJobLevelCode = "4",
            AMLJobLevelName = "董事長或相當職位",
            IsSeniorManagers = "Y",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增AML職級別", ExampleType = ExampleType.Response)]
public class 新增AML職級別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("4", "4");
    }
}

[ExampleAnnotation(Name = "[4002]新增AML職級別-資料已存在", ExampleType = ExampleType.Response)]
public class 新增AML職級別資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>("1", "1");
    }
}
