namespace ScoreSharp.API.Modules.SetUp.BlackListReason.InsertBlackListReason;

[ExampleAnnotation(Name = "[2000]新增黑名單理由")]
public class 新增黑名單理由_2000_ReqEx : IExampleProvider<InsertBlackListReasonRequest>
{
    public InsertBlackListReasonRequest GetExample()
    {
        InsertBlackListReasonRequest request = new()
        {
            BlackListReasonCode = "C7",
            BlackListReasonName = "資料外流-電話",
            ReasonStrength = 4,
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增黑名單理由")]
public class 新增黑名單理由_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("C7", "C7");
    }
}

[ExampleAnnotation(Name = "[4002]新增黑名單理由-資料已存在")]
public class 新增黑名單理由資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "01");
    }
}
