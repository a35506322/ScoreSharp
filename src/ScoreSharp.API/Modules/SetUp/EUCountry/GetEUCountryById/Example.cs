namespace ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountryById;

[ExampleAnnotation(Name = "[2000]取得EU國家", ExampleType = ExampleType.Response)]
public class 取得EU國家_2000_ResEx : IExampleProvider<ResultResponse<GetEUCountryByIdResponse>>
{
    public ResultResponse<GetEUCountryByIdResponse> GetExample()
    {
        GetEUCountryByIdResponse response = new()
        {
            EUCountryCode = "DE",
            EUCountryName = "德國",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得EU國家-查無此資料", ExampleType = ExampleType.Response)]
public class 取得EU國家查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "JP");
    }
}
