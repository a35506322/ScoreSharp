namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetMonthlyIncomeInfoByApplyNo;

[ExampleAnnotation(Name = "[2000]查詢月收入簽核顯示資料", ExampleType = ExampleType.Response)]
public class 查詢月收入簽核顯示資料_2000_ResEx : IExampleProvider<ResultResponse<GetMonthlyIncomeInfoByApplyNoResponse>>
{
    public ResultResponse<GetMonthlyIncomeInfoByApplyNoResponse> GetExample()
    {
        var data = new GetMonthlyIncomeInfoByApplyNoResponse()
        {
            ApplyNo = "20240903X8997",
            CardOwner = CardOwner.正卡_附卡,
            CurrentMonthIncome = 100000,
            CreditCheckCode = "",
            CardInfoList = new List<CardInfo>()
            {
                new CardInfo()
                {
                    CardStatus = CardStatus.網路件_待月收入預審,
                    ApplyCardType = "JS59",
                    ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                    UserType = UserType.正卡人,
                },
                new CardInfo()
                {
                    CardStatus = CardStatus.網路件_待月收入預審,
                    ApplyCardType = "JS59",
                    ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                    UserType = UserType.附卡人,
                },
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
