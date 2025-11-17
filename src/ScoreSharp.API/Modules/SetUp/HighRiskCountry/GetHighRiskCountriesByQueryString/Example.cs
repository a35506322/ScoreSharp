namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountriesByQueryString;

[ExampleAnnotation(Name = "[2000]取得洗錢及資恐高風險國家", ExampleType = ExampleType.Response)]
public class 取得洗錢及資恐高風險國家_2000_ResEx : IExampleProvider<ResultResponse<List<GetHighRiskCountriesByQueryStringResponse>>>
{
    public ResultResponse<List<GetHighRiskCountriesByQueryStringResponse>> GetExample()
    {
        List<GetHighRiskCountriesByQueryStringResponse> response = new()
        {
            new GetHighRiskCountriesByQueryStringResponse
            {
                HighRiskCountryCode = "TW",
                HighRiskCountryName = "台灣",
                IsActive = "Y",
            },
            new GetHighRiskCountriesByQueryStringResponse
            {
                HighRiskCountryCode = "JP",
                HighRiskCountryName = "日本",
                IsActive = "Y",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
