namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.DeletePromotionUnitById;

[ExampleAnnotation(Name = "[2000]刪除推廣單位", ExampleType = ExampleType.Response)]
public class 刪除推廣單位_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("200");
    }
}

[ExampleAnnotation(Name = "[4001]刪除推廣單位-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除推廣單位查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("150", "150");
    }
}
