namespace ScoreSharp.API.Modules.SetUp.SupplementReason.InsertSupplementReason;

[ExampleAnnotation(Name = "[2000]新增補件原因", ExampleType = ExampleType.Request)]
public class 新增補件原因_2000_ReqEx : IExampleProvider<InsertSupplementReasonRequest>
{
    public InsertSupplementReasonRequest GetExample()
    {
        InsertSupplementReasonRequest request = new()
        {
            SupplementReasonCode = "03",
            SupplementReasonName = "收入證明",
            IsActive = "Y",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增補件原因", ExampleType = ExampleType.Response)]
public class 新增補件原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("01", "01");
    }
}

[ExampleAnnotation(Name = "[4002]補件原因-資料已存在", ExampleType = ExampleType.Response)]
public class 新增補件原因資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "01");
    }
}
