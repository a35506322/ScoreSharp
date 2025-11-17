namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.DeleteHighFinancialSecrecyCountryById;

[ExampleAnnotation(Name = "[2000]刪除單筆高金融保密國家", ExampleType = ExampleType.Response)]
public class 刪除單筆高金融保密國家_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("AE");
    }
}

[ExampleAnnotation(Name = "[4001]刪除單筆高金融保密國家-查無資料", ExampleType = ExampleType.Response)]
public class 刪除單筆高金融保密國家_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "TKG");
    }
}
