namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationByQueryString;

[ExampleAnnotation(Name = "[2000]取得身分證換發地點", ExampleType = ExampleType.Response)]
public class 取得身分證換發地點_2000_ResEx : IExampleProvider<ResultResponse<List<GetIDCardRenewalLocationByQueryStringResponse>>>
{
    public ResultResponse<List<GetIDCardRenewalLocationByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetIDCardRenewalLocationByQueryStringResponse>
        {
            new GetIDCardRenewalLocationByQueryStringResponse
            {
                IDCardRenewalLocationCode = "09007000",
                IDCardRenewalLocationName = "連江",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
                UpdateUserId = "admin",
                UpdateTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
