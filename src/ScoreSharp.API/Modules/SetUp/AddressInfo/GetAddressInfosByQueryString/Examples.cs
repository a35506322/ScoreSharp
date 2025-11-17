namespace ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfosByQueryString;

[ExampleAnnotation(Name = "[2000]取得全部地址資訊", ExampleType = ExampleType.Response)]
public class 取得多筆地址資訊_2000_ResEx : IExampleProvider<ResultResponse<List<GetAddressInfosByQueryStringResponse>>>
{
    public ResultResponse<List<GetAddressInfosByQueryStringResponse>> GetExample()
    {
        var data = new List<GetAddressInfosByQueryStringResponse>
        {
            new GetAddressInfosByQueryStringResponse
            {
                SeqNo = "72424",
                ZIPCode = "247",
                City = "新北市",
                Area = "蘆洲區",
                Road = "長安街",
                Scope = "[\"all\"]",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = new DateTime(2025, 1, 15, 10, 30, 0),
                UpdateUserId = "admin",
                UpdateTime = new DateTime(2025, 1, 15, 14, 20, 0),
            },
            new GetAddressInfosByQueryStringResponse
            {
                SeqNo = "72425",
                ZIPCode = "247",
                City = "新北市",
                Area = "蘆洲區",
                Road = "長安街",
                Scope = "[\"all\"]",
                IsActive = "Y",
                AddUserId = "user123",
                AddTime = new DateTime(2025, 1, 14, 09, 15, 0),
                UpdateUserId = "user123",
                UpdateTime = new DateTime(2025, 1, 14, 16, 45, 0),
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
