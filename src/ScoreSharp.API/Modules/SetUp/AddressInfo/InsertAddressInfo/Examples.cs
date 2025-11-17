namespace ScoreSharp.API.Modules.SetUp.AddressInfo.InsertAddressInfo;

[ExampleAnnotation(Name = "[2000]新增地址資訊", ExampleType = ExampleType.Request)]
public class 新增地址資訊_2000_ReqEx : IExampleProvider<InsertAddressInfoRequest>
{
    public InsertAddressInfoRequest GetExample()
    {
        return new InsertAddressInfoRequest
        {
            ZIPCode = "247",
            City = "新北市",
            Area = "蘆洲區",
            Road = "長安街",
            Scope = "[\"all\"]",
            IsActive = "Y",
        };
    }
}

[ExampleAnnotation(Name = "[2000]新增地址資訊", ExampleType = ExampleType.Response)]
public class 新增地址資訊_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("72424", "72424");
    }
}

[ExampleAnnotation(Name = "[4002]地址資訊-資料已存在", ExampleType = ExampleType.Response)]
public class 新增地址資訊資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "247-新北市-蘆洲區-長安街");
    }
}
