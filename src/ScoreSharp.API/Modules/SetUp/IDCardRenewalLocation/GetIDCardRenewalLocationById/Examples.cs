namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationById;

[ExampleAnnotation(Name = "[2000]取得身分證換發地點", ExampleType = ExampleType.Response)]
public class 取得身分證換發地點_2000_ResEx : IExampleProvider<ResultResponse<GetIDCardRenewalLocationByIdResponse>>
{
    public ResultResponse<GetIDCardRenewalLocationByIdResponse> GetExample()
    {
        GetIDCardRenewalLocationByIdResponse response = new()
        {
            IDCardRenewalLocationCode = "09007000",
            IDCardRenewalLocationName = "連江",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得身分證換發地點-查無此資料", ExampleType = ExampleType.Response)]
public class 取得身分證換發地點查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "65000001");
    }
}
