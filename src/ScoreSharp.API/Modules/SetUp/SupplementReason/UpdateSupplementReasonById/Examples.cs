namespace ScoreSharp.API.Modules.SetUp.SupplementReason.UpdateSupplementReasonById;

[ExampleAnnotation(Name = "[2000]修改補件原因", ExampleType = ExampleType.Request)]
public class 修改補件原因_2000_ReqEx : IExampleProvider<UpdateSupplementReasonByIdRequest>
{
    public UpdateSupplementReasonByIdRequest GetExample()
    {
        UpdateSupplementReasonByIdRequest request = new()
        {
            SupplementReasonName = "地址證明",
            SupplementReasonCode = "01",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改補件原因", ExampleType = ExampleType.Response)]
public class 修改補件原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("01", "01");
    }
}

[ExampleAnnotation(Name = "[4001]修改補件原因-查無此資料", ExampleType = ExampleType.Response)]
public class 修改補件原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "05");
    }
}

[ExampleAnnotation(Name = "[4003]修改補件原因-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改補件原因路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
