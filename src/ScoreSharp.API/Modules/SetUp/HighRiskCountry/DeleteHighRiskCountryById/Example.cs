namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.DeleteHighRiskCountryById;

[ExampleAnnotation(Name = "[2000]刪除洗錢及資恐高風險國家", ExampleType = ExampleType.Response)]
public class 刪除洗錢及資恐高風險國家_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("AE");
    }
}

[ExampleAnnotation(Name = "[4001]刪除洗錢及資恐高風險國家-查無資料", ExampleType = ExampleType.Response)]
public class 刪除洗錢及資恐高風險國家查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "TKG");
    }
}
