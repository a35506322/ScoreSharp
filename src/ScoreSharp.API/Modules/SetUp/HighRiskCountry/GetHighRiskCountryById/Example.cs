namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountryById;

[ExampleAnnotation(Name = "[2000]查詢單筆洗錢及資恐高風險國家", ExampleType = ExampleType.Response)]
public class 查詢單筆洗錢及資恐高風險國家_2000_ResEx : IExampleProvider<ResultResponse<GetHighRiskCountryByIdResponse>>
{
    public ResultResponse<GetHighRiskCountryByIdResponse> GetExample()
    {
        GetHighRiskCountryByIdResponse response = new()
        {
            HighRiskCountryCode = "TW",
            HighRiskCountryName = "台灣",
            IsActive = "Y",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]查詢單筆洗錢及資恐高風險國家-查無資料", ExampleType = ExampleType.Response)]
public class 查詢單筆洗錢及資恐高風險國家查無資料_4001_ResEx : IExampleProvider<ResultResponse<GetHighRiskCountryByIdResponse>>
{
    public ResultResponse<GetHighRiskCountryByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetHighRiskCountryByIdResponse>(null, "TKG");
    }
}
