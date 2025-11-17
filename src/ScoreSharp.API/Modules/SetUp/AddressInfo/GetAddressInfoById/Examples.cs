namespace ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfoById;

[ExampleAnnotation(Name = "[2000]取得單筆地址資訊", ExampleType = ExampleType.Response)]
public class 取得單筆地址資訊_2000_ResEx : IExampleProvider<ResultResponse<GetAddressInfoByIdResponse>>
{
    public ResultResponse<GetAddressInfoByIdResponse> GetExample()
    {
        var data = new GetAddressInfoByIdResponse
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
        };

        return ApiResponseHelper.Success(data);
    }
}

[ExampleAnnotation(Name = "[4001]地址資訊-查無此資料", ExampleType = ExampleType.Response)]
public class 取得單筆地址資訊查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetAddressInfoByIdResponse>>
{
    public ResultResponse<GetAddressInfoByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetAddressInfoByIdResponse>(null, "99999");
    }
}
