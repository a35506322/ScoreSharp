namespace ScoreSharp.API.Modules.SetUp.LongTermReason.InsertLongTermReason;

[ExampleAnnotation(Name = "[2000]新增長循分期戶理由碼", ExampleType = ExampleType.Request)]
public class 新增長循分期戶理由碼_2000_ReqEx : IExampleProvider<InsertLongTermReasonRequest>
{
    public InsertLongTermReasonRequest GetExample()
    {
        InsertLongTermReasonRequest request = new()
        {
            LongTermReasonCode = "01",
            LongTermReasonName = "購買高價電子產品",
            ReasonStrength = 45,
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增長循分期戶理由碼", ExampleType = ExampleType.Response)]
public class 新增長循分期戶理由碼_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("01", "01");
    }
}

[ExampleAnnotation(Name = "[4002]新增長循分期戶理由碼-資料已存在", ExampleType = ExampleType.Response)]
public class 新增長循分期戶理由碼資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "05");
    }
}
