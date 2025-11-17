namespace ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionById;

[ExampleAnnotation(Name = "[2000]取得信用卡優惠辦法", ExampleType = ExampleType.Response)]
public class 取得信用卡優惠辦法_2000_ResEx : IExampleProvider<ResultResponse<GetCardPromotionByIdResponse>>
{
    public ResultResponse<GetCardPromotionByIdResponse> GetExample()
    {
        GetCardPromotionByIdResponse response = new()
        {
            CardPromotionCode = "0001",
            CardPromotionName = "微風無限公關卡第一年免月費",
            PrimaryCardReservedPOT = "01",
            PrimaryCardUsedPOT = "01",
            SupplementaryCardUsedPOT = "01",
            UsedPOTExpiryMonth = "06",
            SupplementaryCardReservedPOT = "01",
            ReservePromotionPeriod = "01",
            InterestRate = Decimal.Parse("12.13"),
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得信用卡優惠辦法-查無此資料", ExampleType = ExampleType.Response)]
public class 取得信用卡優惠辦法查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "JP");
    }
}
