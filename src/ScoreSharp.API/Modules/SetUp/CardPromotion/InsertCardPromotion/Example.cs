namespace ScoreSharp.API.Modules.SetUp.CardPromotion.InsertCardPromotion;

[ExampleAnnotation(Name = "[2000]新增信用卡優惠辦法", ExampleType = ExampleType.Request)]
public class 新增信用卡優惠辦法_2000_ReqEx : IExampleProvider<InsertCardPromotionRequest>
{
    public InsertCardPromotionRequest GetExample()
    {
        InsertCardPromotionRequest request = new()
        {
            CardPromotionCode = "0003",
            CardPromotionName = "聯邦LINE Bank聯名卡第一年免月費",
            PrimaryCardReservedPOT = "03",
            PrimaryCardUsedPOT = "03",
            SupplementaryCardUsedPOT = "03",
            UsedPOTExpiryMonth = "03",
            SupplementaryCardReservedPOT = "03",
            ReservePromotionPeriod = "03",
            IsActive = "Y",
            InterestRate = Decimal.Parse("12.13"),
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增信用卡優惠辦法", ExampleType = ExampleType.Response)]
public class 新增信用卡優惠辦法_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("0003", "0003");
    }
}

[ExampleAnnotation(Name = "[4002]新增信用卡優惠辦法-資料已存在", ExampleType = ExampleType.Response)]
public class 新增信用卡優惠辦法資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "0001");
    }
}
