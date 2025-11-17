namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.InsertIDCardRenewalLocation;

[ExampleAnnotation(Name = "[2000]新增身分證換發地點", ExampleType = ExampleType.Request)]
public class 新增身分證換發地點_2000_ReqEx : IExampleProvider<InsertIDCardRenewalLocationRequest>
{
    public InsertIDCardRenewalLocationRequest GetExample()
    {
        InsertIDCardRenewalLocationRequest request = new()
        {
            IDCardRenewalLocationCode = "65000000",
            IDCardRenewalLocationName = "新北市",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增身分證換發地點", ExampleType = ExampleType.Response)]
public class 新增身分證換發地點_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("65000000", "65000000");
    }
}

[ExampleAnnotation(Name = "[4002]新增身分證換發地點-資料已存在", ExampleType = ExampleType.Response)]
public class 新增身分證換發地點資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "09007000");
    }
}
