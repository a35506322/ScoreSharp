namespace ScoreSharp.API.Modules.SetUp.BlackListReason.UpdateBlackListReasonById;

[ExampleAnnotation(Name = "[2000]修改黑名單理由", ExampleType = ExampleType.Request)]
public class 修改黑名單理由_2000_ReqEx : IExampleProvider<UpdateBlackListReasonByIdRequest>
{
    public UpdateBlackListReasonByIdRequest GetExample()
    {
        UpdateBlackListReasonByIdRequest request = new()
        {
            BlackListReasonCode = "C7",
            BlackListReasonName = "資料外流-電話",
            IsActive = "Y",
            ReasonStrength = 4,
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改黑名單理由", ExampleType = ExampleType.Response)]
public class 修改黑名單理由_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("C7", "C7");
    }
}

[ExampleAnnotation(Name = "[2000]修改黑名單理由-查無此資料", ExampleType = ExampleType.Response)]
public class 修改黑名單理由查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "07");
    }
}

[ExampleAnnotation(Name = "[2000]修改黑名單理由-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改黑名單理由路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
