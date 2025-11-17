namespace ScoreSharp.API.Modules.SetUp.CardPromotion.DeleteCardPromotionById;

[ExampleAnnotation(Name = "[2000]刪除信用卡優惠辦法", ExampleType = ExampleType.Response)]
public class 刪除信用卡優惠辦法_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("0001");
    }
}

[ExampleAnnotation(Name = "[4001]刪除信用卡優惠辦法-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除信用卡優惠辦法查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "0043");
    }
}

[ExampleAnnotation(Name = "[4003]刪除信用卡優惠辦法-此資源已被使用", ExampleType = ExampleType.Response)]
public class 刪除信用卡優惠辦法此資源已被使用_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.此資源已被使用<string>(null, "0002");
    }
}
