namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountriesByQueryString;

[ExampleAnnotation(Name = "[2000]查詢多筆高金融保密國家", ExampleType = ExampleType.Response)]
public class 查詢多筆高金融保密國家_2000_ResEx
    : IExampleProvider<ResultResponse<List<GetHighFinancialSecrecyCountriesByQueryStringResponse>>>
{
    public ResultResponse<List<GetHighFinancialSecrecyCountriesByQueryStringResponse>> GetExample()
    {
        List<GetHighFinancialSecrecyCountriesByQueryStringResponse> response = new()
        {
            new GetHighFinancialSecrecyCountriesByQueryStringResponse
            {
                HighFinancialSecrecyCountryCode = "TW",
                HighFinancialSecrecyCountryName = "台灣",
                IsActive = "Y",
            },
            new GetHighFinancialSecrecyCountriesByQueryStringResponse
            {
                HighFinancialSecrecyCountryCode = "JP",
                HighFinancialSecrecyCountryName = "日本",
                IsActive = "Y",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
