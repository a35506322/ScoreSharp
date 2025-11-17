namespace ScoreSharp.API.Modules.SetUp.Citizenship.InsertCitizenship;

[ExampleAnnotation(Name = "[2000]新增國籍", ExampleType = ExampleType.Request)]
public class 新增國籍_2000_ReqEx : IExampleProvider<InsertCitizenshipRequest>
{
    public InsertCitizenshipRequest GetExample()
    {
        InsertCitizenshipRequest request = new()
        {
            CitizenshipCode = "BH",
            CitizenshipName = "巴林",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增國籍", ExampleType = ExampleType.Response)]
public class 新增國籍_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("BH", "BH");
    }
}

[ExampleAnnotation(Name = "[4002]新增國籍-資料已存在", ExampleType = ExampleType.Response)]
public class 新增國籍資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "TW");
    }
}
