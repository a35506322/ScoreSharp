namespace ScoreSharp.API.Modules.SetUp.RejectionReason.UpdateRejectionReasonById;

[ExampleAnnotation(Name = "[2000]修改退件原因", ExampleType = ExampleType.Request)]
public class 修改退件原因_2000_ReqEx : IExampleProvider<UpdateRejectionReasonByIdRequest>
{
    public UpdateRejectionReasonByIdRequest GetExample()
    {
        UpdateRejectionReasonByIdRequest request = new()
        {
            RejectionReasonCode = "01",
            RejectionReasonName = "信用評分不足",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改退件原因", ExampleType = ExampleType.Response)]
public class 修改退件原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("01", "01");
    }
}

[ExampleAnnotation(Name = "[4001]修改退件原因-查無此資料", ExampleType = ExampleType.Response)]
public class 修改退件原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "05");
    }
}

[ExampleAnnotation(Name = "[4003]修改退件原因-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改退件原因路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
