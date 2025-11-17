namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.DeleteAnnualFeeCollectionMethodById;

[ExampleAnnotation(Name = "[2000]刪除年費收取方式", ExampleType = ExampleType.Response)]
public class 刪除年費收取方式_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("01");
    }
}

[ExampleAnnotation(Name = "[4001]刪除年費收取方式-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除年費收取方式查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "12");
    }
}
