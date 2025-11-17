namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountryById;

[ExampleAnnotation(Name = "[2000]取得UN制裁國家", ExampleType = ExampleType.Response)]
public class 取得UN制裁國家_2000_ResEx : IExampleProvider<ResultResponse<GetUNSanctionCountryByIdResponse>>
{
    public ResultResponse<GetUNSanctionCountryByIdResponse> GetExample()
    {
        GetUNSanctionCountryByIdResponse response = new()
        {
            UNSanctionCountryCode = "TW",
            UNSanctionCountryName = "台灣",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得UN制裁國家-查無此資料", ExampleType = ExampleType.Response)]
public class 取得UN制裁國家查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "JP");
    }
}
