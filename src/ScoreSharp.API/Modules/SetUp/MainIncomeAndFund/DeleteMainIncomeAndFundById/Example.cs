namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.DeleteMainIncomeAndFundById;

[ExampleAnnotation(Name = "[2000]刪除主要所得及資金來源", ExampleType = ExampleType.Response)]
public class 刪除主要所得及資金來源_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("2");
    }
}

[ExampleAnnotation(Name = "[4001]刪除主要所得及資金來源-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除主要所得及資金來源查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "13");
    }
}
