namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCardStatus;

[ExampleAnnotation(Name = "[2000]取得卡別狀態", ExampleType = ExampleType.Response)]
public class 取得卡別狀態_2000_ResEx : IExampleProvider<ResultResponse<List<GetApplyCardStatusResponse>>>
{
    public ResultResponse<List<GetApplyCardStatusResponse>> GetExample()
    {
        var data = new List<GetApplyCardStatusResponse>
        {
            new GetApplyCardStatusResponse()
            {
                SeqNo = "1234567890",
                ApplyCardType = "JS59",
                ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                UserType = UserType.正卡人,
                CreditCheckCode = "A02",
                CreditCheckCodeName = "快發A02",
                CardPromotion = "1234567890",
                CardPromotionName = "1234567890",
                CardStatus = CardStatus.紙本件_待月收入預審,
                CardLimit = 1234567890,
                SupplementCount = 1234567890,
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
