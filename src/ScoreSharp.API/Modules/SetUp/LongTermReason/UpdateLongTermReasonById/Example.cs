namespace ScoreSharp.API.Modules.SetUp.LongTermReason.UpdateLongTermReasonById;

[ExampleAnnotation(Name = "[2000]修改長循分期戶理由碼", ExampleType = ExampleType.Request)]
public class 修改長循分期戶理由碼_2000_ReqEx : IExampleProvider<UpdateLongTermReasonByIdRequest>
{
    public UpdateLongTermReasonByIdRequest GetExample()
    {
        UpdateLongTermReasonByIdRequest request = new()
        {
            LongTermReasonCode = "ED",
            LongTermReasonName = "教育支出",
            ReasonStrength = 42,
            IsActive = "N",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改長循分期戶理由碼", ExampleType = ExampleType.Response)]
public class 修改長循分期戶理由碼_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("02", "02");
    }
}

[ExampleAnnotation(Name = "[4001]修改長循分期戶理由碼-查無資料", ExampleType = ExampleType.Response)]
public class 修改長循分期戶理由碼查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "FK");
    }
}

[ExampleAnnotation(Name = "[4003]修改長循分期戶理由碼-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改長循分期戶理由碼路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
