namespace ScoreSharp.API.Modules.Manage.Stakeholder.InsertStakeholder;

[ExampleAnnotation(Name = "[2000]新增利害關係人", ExampleType = ExampleType.Request)]
public class 新增利害關係人_2000_ReqEx : IExampleProvider<InsertStakeholderRequest>
{
    public InsertStakeholderRequest GetExample()
    {
        return new()
        {
            ID = "alexislee",
            UserId = "R268388828",
            IsActive = "Y",
        };
    }
}

[ExampleAnnotation(Name = "[2000]新增利害關係人", ExampleType = ExampleType.Response)]
public class 新增利害關係人_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("10", "10");
    }
}

[ExampleAnnotation(Name = "[4002]新增利害關係人-資料已存在", ExampleType = ExampleType.Response)]
public class 新增利害關係人資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "user123已存在ID：S170982120之利害關係人資料。");
    }
}
