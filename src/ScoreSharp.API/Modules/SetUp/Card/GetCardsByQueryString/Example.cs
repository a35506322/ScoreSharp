namespace ScoreSharp.API.Modules.SetUp.Card.GetCardsByQueryString;

[ExampleAnnotation(Name = "[2000]取得信用卡卡片種類", ExampleType = ExampleType.Response)]
public class 取得信用卡卡片種類_2000_ResEx : IExampleProvider<ResultResponse<List<GetCardsByQueryStringResponse>>>
{
    public ResultResponse<List<GetCardsByQueryStringResponse>> GetExample()
    {
        List<GetCardsByQueryStringResponse> response = new()
        {
            new GetCardsByQueryStringResponse
            {
                BINCode = "35665779",
                CardCode = "JST79",
                CardName = "國民旅遊悠遊晶緻卡",
                CardCategory = CardCategory.一般發卡,
                CardCategoryName = CardCategory.一般發卡.ToString(),
                SampleRejectionLetter = SampleRejectionLetter.拒件函_信用卡,
                SampleRejectionLetterName = SampleRejectionLetter.拒件函_信用卡.ToString(),
                DefaultBillDay = "01",
                SaleLoanCategory = SaleLoanCategory.其他,
                SaleLoanCategoryName = SaleLoanCategory.其他.ToString(),
                DefaultDiscount = new DefaultCardPromotionDto() { CardPromotionCode = "1999", CardPromotionName = "不收年費卡別-13.88%" },
                IsActive = "Y",
                PrimaryCardQuotaUpperlimit = 8000000,
                PrimaryCardQuotaLowerlimit = 10000,
                PrimaryCardYearUpperlimit = 99,
                PrimaryCardYearLowerlimit = 18,
                SupplementaryCardQuotaUpperlimit = 8000000,
                SupplementaryCardQuotaLowerlimit = 10000,
                SupplementaryCardYearUpperlimit = 99,
                SupplementaryCardYearLowerlimit = 15,
                IsCARDPAUnderLimit = "N",
                CARDPACQuotaLimit = 20,
                IsApplyAdditionalCard = "Y",
                IsIndependentCard = "Y",
                IsIVRvCTIQuery = "Y",
                IsCITSCard = "N",
                IsQuickCardIssuance = "Y",
                IsTicket = "Y",
                IsJointGroup = "Y",
                OptionalCardPromotions =
                [
                    new OptionalCardPromotionsDto() { CardPromotionCode = "1991", CardPromotionName = "收年費商務差旅卡-15%" },
                ],
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
