namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.UpdateIDCardRenewalLocationById;

[ExampleAnnotation(Name = "[2000]修改身分證換發地點", ExampleType = ExampleType.Request)]
public class 修改身分證換發地點_2000_ReqEx : IExampleProvider<UpdateIDCardRenewalLocationByIdRequest>
{
    public UpdateIDCardRenewalLocationByIdRequest GetExample()
    {
        UpdateIDCardRenewalLocationByIdRequest request = new()
        {
            IDCardRenewalLocationCode = "09020000",
            IDCardRenewalLocationName = "連江",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改身分證換發地點", ExampleType = ExampleType.Response)]
public class 修改身分證換發地點_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("65000000", "65000000");
    }
}

[ExampleAnnotation(Name = "[4001]修改身分證換發地點-查無資料", ExampleType = ExampleType.Response)]
public class 修改身分證換發地點查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "65000001");
    }
}

[ExampleAnnotation(Name = "[4003]修改身分證換發地點-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改身分證換發地點路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
