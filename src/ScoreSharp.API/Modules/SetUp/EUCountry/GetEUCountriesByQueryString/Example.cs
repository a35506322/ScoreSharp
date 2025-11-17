namespace ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountriesByQueryString;

[ExampleAnnotation(Name = "[2000]取得EU國家", ExampleType = ExampleType.Response)]
public class 取得EU國家_2000_ResEx : IExampleProvider<ResultResponse<List<GetEUCountriesByQueryStringResponse>>>
{
    public ResultResponse<List<GetEUCountriesByQueryStringResponse>> GetExample()
    {
        List<GetEUCountriesByQueryStringResponse> response = new()
        {
            new GetEUCountriesByQueryStringResponse
            {
                EUCountryCode = "DE",
                EUCountryName = "德國",
                IsActive = "Y",
            },
            new GetEUCountriesByQueryStringResponse
            {
                EUCountryCode = "DK",
                EUCountryName = "丹麥",
                IsActive = "N",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
