namespace ScoreSharp.API.Modules.SetUp.AddressInfo.UpdateAddressInfoById;

[ExampleAnnotation(Name = "[2000]修改地址資訊", ExampleType = ExampleType.Request)]
public class 修改地址資訊_2000_ReqEx : IExampleProvider<UpdateAddressInfoByIdRequest>
{
    public UpdateAddressInfoByIdRequest GetExample()
    {
        return new UpdateAddressInfoByIdRequest
        {
            SeqNo = "72424",
            ZIPCode = "247",
            City = "新北市",
            Area = "蘆洲區",
            Road = "長安街",
            Scope = "全部號",
            IsActive = "N",
        };
    }
}

[ExampleAnnotation(Name = "[2000]修改地址資訊", ExampleType = ExampleType.Response)]
public class 修改地址資訊_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("72424", "72424");
    }
}

[ExampleAnnotation(Name = "[4001]地址資訊-查無此資料", ExampleType = ExampleType.Response)]
public class 修改地址資訊查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "99999");
    }
}

[ExampleAnnotation(Name = "[4003]地址資訊-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改地址資訊路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
