namespace ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionsByQueryString;

[ExampleAnnotation(Name = "[2000]取得信用卡優惠辦法", ExampleType = ExampleType.Response)]
public class 取得信用卡優惠辦法_2000_ResEx : IExampleProvider<ResultResponse<List<GetCardPromotionsByQueryStringResponse>>>
{
    public ResultResponse<List<GetCardPromotionsByQueryStringResponse>> GetExample()
    {
        List<GetCardPromotionsByQueryStringResponse> response = new()
        {
            new GetCardPromotionsByQueryStringResponse
            {
                CardPromotionCode = "0001",
                CardPromotionName = "微風無限公關卡第一年免月費",
                PrimaryCardReservedPOT = "01",
                PrimaryCardUsedPOT = "01",
                SupplementaryCardUsedPOT = "01",
                UsedPOTExpiryMonth = "06",
                SupplementaryCardReservedPOT = "01",
                ReservePromotionPeriod = "01",
                InterestRate = Decimal.Parse("12.12"),
                IsActive = "Y",
                AddUserId = "ADMIN",
                AddTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
            new GetCardPromotionsByQueryStringResponse
            {
                CardPromotionCode = "0002",
                CardPromotionName = "超吉鶴卡第一年免年費",
                PrimaryCardReservedPOT = "02",
                PrimaryCardUsedPOT = "02",
                SupplementaryCardUsedPOT = "02",
                UsedPOTExpiryMonth = "06",
                SupplementaryCardReservedPOT = "02",
                ReservePromotionPeriod = "02",
                InterestRate = Decimal.Parse("10.40"),
                IsActive = "Y",
                AddUserId = "ADMIN",
                AddTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
