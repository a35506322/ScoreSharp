namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountriesByQueryString;

[ExampleAnnotation(Name = "[2000]取得EU制裁國家", ExampleType = ExampleType.Response)]
public class 取得EU制裁國家_2000_ResEx : IExampleProvider<ResultResponse<List<GetEUSanctionCountriesByQueryStringResponse>>>
{
    public ResultResponse<List<GetEUSanctionCountriesByQueryStringResponse>> GetExample()
    {
        List<GetEUSanctionCountriesByQueryStringResponse> response = new()
        {
            new GetEUSanctionCountriesByQueryStringResponse
            {
                EUSanctionCountryCode = "TW",
                EUSanctionCountryName = "台灣",
                IsActive = "Y",
            },
            new GetEUSanctionCountriesByQueryStringResponse
            {
                EUSanctionCountryCode = "CH",
                EUSanctionCountryName = "瑞士",
                IsActive = "Y",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
