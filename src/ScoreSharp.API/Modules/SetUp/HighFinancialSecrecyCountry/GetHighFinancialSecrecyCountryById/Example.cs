namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountryById;

[ExampleAnnotation(Name = "[2000]查詢單筆高金融保密國家", ExampleType = ExampleType.Response)]
public class 查詢單筆高金融保密國家_2000_ResEx : IExampleProvider<ResultResponse<GetHighFinancialSecrecyCountryByIdResponse>>
{
    public ResultResponse<GetHighFinancialSecrecyCountryByIdResponse> GetExample()
    {
        GetHighFinancialSecrecyCountryByIdResponse response = new()
        {
            HighFinancialSecrecyCountryCode = "TW",
            HighFinancialSecrecyCountryName = "台灣",
            IsActive = "Y",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]查詢單筆高金融保密國家-查無資料", ExampleType = ExampleType.Response)]
public class 查詢單筆高金融保密國家查無資料_4001_ResEx : IExampleProvider<ResultResponse<GetHighFinancialSecrecyCountryByIdResponse>>
{
    public ResultResponse<GetHighFinancialSecrecyCountryByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetHighFinancialSecrecyCountryByIdResponse>(null, "TKG");
    }
}
