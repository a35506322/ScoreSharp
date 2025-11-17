namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountriesByQueryString;

[ExampleAnnotation(Name = "[2000]取得UN制裁國家", ExampleType = ExampleType.Response)]
public class 取得UN制裁國家_2000_ResEx : IExampleProvider<ResultResponse<List<GetUNSanctionCountriesByQueryStringResponse>>>
{
    public ResultResponse<List<GetUNSanctionCountriesByQueryStringResponse>> GetExample()
    {
        List<GetUNSanctionCountriesByQueryStringResponse> response = new()
        {
            new GetUNSanctionCountriesByQueryStringResponse
            {
                UNSanctionCountryCode = "TW",
                UNSanctionCountryName = "台灣",
                IsActive = "Y",
            },
            new GetUNSanctionCountriesByQueryStringResponse
            {
                UNSanctionCountryCode = "CH",
                UNSanctionCountryName = "瑞士",
                IsActive = "Y",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
