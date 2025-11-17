namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetAllApplyCreditCardListByQueryString;

[ExampleAnnotation(Name = "[2000]查詢全域案件成功", ExampleType = ExampleType.Response)]
public class 查詢全域案件成功_2000_ResEx : IExampleProvider<ResultResponse<List<GetAllApplyCreditCardListByQueryStringResponse>>>
{
    public ResultResponse<List<GetAllApplyCreditCardListByQueryStringResponse>> GetExample()
    {
        var data = new List<GetAllApplyCreditCardListByQueryStringResponse>
        {
            new GetAllApplyCreditCardListByQueryStringResponse()
            {
                ApplyNo = "20240903X8997",
                CHName = "蔡弘文",
                ID = "J12698840",
                ApplyDate = DateTime.Parse("2023-04-02T11:48:51.63"),
                CaseType = CaseType.一般件,
                CurrentHandleUserId = "1001234",
                CurrentHandleUserName = "張三",
                IsQuery = "Y",
                CardStatusList = new List<CardStatusDto>()
                {
                    new CardStatusDto() { CardStatus = CardStatus.紙本件_初始, CardStatusName = "紙本件_初始" },
                },
                ApplyCardTypeList = new List<ApplyCardTypeDto>()
                {
                    new ApplyCardTypeDto() { ApplyCardType = "JS59", ApplyCardTypeName = "聯邦一卡通吉鶴卡" },
                    new ApplyCardTypeDto() { ApplyCardType = "JS60", ApplyCardTypeName = "聯邦一卡通吉鶴卡" },
                },
            },
            new GetAllApplyCreditCardListByQueryStringResponse()
            {
                ApplyNo = "20240903X4479",
                CHName = "宋明哲",
                ID = "W13289459",
                ApplyDate = DateTime.Parse("2023-09-15T12:05:51.52"),
                CaseType = CaseType.一般件,
                CurrentHandleUserId = String.Empty,
                CurrentHandleUserName = String.Empty,
                IsQuery = "N",
                CardStatusList = new List<CardStatusDto>()
                {
                    new CardStatusDto() { CardStatus = CardStatus.紙本件_初始, CardStatusName = "紙本件_初始" },
                },
                ApplyCardTypeList = new List<ApplyCardTypeDto>()
                {
                    new ApplyCardTypeDto() { ApplyCardType = "JS59", ApplyCardTypeName = "聯邦一卡通吉鶴卡" },
                    new ApplyCardTypeDto() { ApplyCardType = "JS60", ApplyCardTypeName = "聯邦一卡通吉鶴卡" },
                },
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
