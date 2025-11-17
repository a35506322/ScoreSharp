using ScoreSharp.API.Modules.SetUp.InternalIP.UpdateInternalIPForIsActiveById;

[ExampleAnnotation(Name = "[2000]修改行內IP狀態", ExampleType = ExampleType.Request)]
public class 修改行內IP狀態_2000_ReqEx : IExampleProvider<UpdateInternalIPForIsActiveByIdRequest>
{
    public UpdateInternalIPForIsActiveByIdRequest GetExample()
    {
        UpdateInternalIPForIsActiveByIdRequest request = new() { IP = "172.28.234.10", IsActive = "Y" };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改行內IP狀態", ExampleType = ExampleType.Response)]
public class 修改行內IP狀態_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("172.28.234.10", "172.28.234.10");
    }
}

[ExampleAnnotation(Name = "[4001]修改行內IP狀態-查無此資料", ExampleType = ExampleType.Response)]
public class 修改行內IP狀態查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "172.28.234.10");
    }
}

[ExampleAnnotation(Name = "[4003]修改行內IP狀態-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改行內IP狀態路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
