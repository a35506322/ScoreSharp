namespace ScoreSharp.API.Modules.SetUp.CardPromotion.UpdateCardPromotionById;

[ExampleAnnotation(Name = "[2000]修改信用卡優惠辦法", ExampleType = ExampleType.Request)]
public class 修改信用卡優惠辦法_2000_ReqEx : IExampleProvider<UpdateCardPromotionByIdRequest>
{
    public UpdateCardPromotionByIdRequest GetExample()
    {
        UpdateCardPromotionByIdRequest request = new()
        {
            CardPromotionCode = "0003",
            CardPromotionName = "聯邦LINE Bank聯名卡第一年免月費",
            PrimaryCardReservedPOT = "03",
            PrimaryCardUsedPOT = "03",
            SupplementaryCardUsedPOT = "03",
            UsedPOTExpiryMonth = "03",
            SupplementaryCardReservedPOT = "03",
            ReservePromotionPeriod = "03",
            IsActive = "N",
            InterestRate = Decimal.Parse("13.14"),
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改信用卡優惠辦法", ExampleType = ExampleType.Response)]
public class 修改信用卡優惠辦法_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("0003", "0003");
    }
}

[ExampleAnnotation(Name = "[4001]修改信用卡優惠辦法-查無資料", ExampleType = ExampleType.Response)]
public class 修改信用卡優惠辦法查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "0043");
    }
}

[ExampleAnnotation(Name = "[4003]修改信用卡優惠辦法-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改信用卡優惠辦法路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
