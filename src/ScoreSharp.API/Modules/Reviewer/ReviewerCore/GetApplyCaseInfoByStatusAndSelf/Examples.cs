namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCaseInfoByStatusAndSelf;

[ExampleAnnotation(Name = "[2000]查詢行員自身案件", ExampleType = ExampleType.Response)]
public class 查詢行員自身案件_2000_ResEx : IExampleProvider<ResultResponse<GetApplyCaseInfoByStatusAndSelfResponse>>
{
    public ResultResponse<GetApplyCaseInfoByStatusAndSelfResponse> GetExample()
    {
        var data = new GetApplyCaseInfoByStatusAndSelfResponse
        {
            CaseCountStatistic = new List<CaseCountStatisticDto>()
            {
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.網路件月收入確認, Count = 9 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.網路件人工審查, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.緊急製卡, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.補回件, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.拒件_撤件重審, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.網路件製卡失敗, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.紙本件月收入確認, Count = 2 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.紙本件人工審查, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.急件, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.退回重審, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.未補回, Count = 0 },
                new CaseCountStatisticDto() { CaseStatus = CaseStatus.紙本件製卡失敗, Count = 0 },
            },
            BaseDataList = new List<BaseData>()
            {
                new BaseData()
                {
                    ApplyNo = "20240903X8997",
                    CHName = "蔡弘文",
                    ID = "J12698840",
                    ApplyCardTypeList = new List<ApplyCardListDto>
                    {
                        new ApplyCardListDto()
                        {
                            ApplyCardType = "JS59",
                            ApplyCardName = "聯邦一卡通吉鶴卡",
                            CardStatus = CardStatus.網路件_待月收入預審,
                            HandleSeqNo = "1234567890",
                            CardStep = CardStep.月收入確認,
                            UserType = UserType.正卡人,
                            ID = "J12698840",
                        },
                    },
                    ApplyDate = DateTime.Parse("2023-04-02T11:48:51.63"),
                    CaseType = CaseType.一般件,
                    LastUpdateTime = DateTime.Parse("2024-09-03T17:40:13.543"),
                    PromotionUnit = "911T",
                    PromotionUser = "1001234",
                },
                new BaseData()
                {
                    ApplyNo = "20240903X4479",
                    CHName = "宋明哲",
                    ID = "W13289459",
                    ApplyCardTypeList = new List<ApplyCardListDto>
                    {
                        new ApplyCardListDto()
                        {
                            ApplyCardType = "JS59",
                            ApplyCardName = "聯邦一卡通吉鶴卡",
                            CardStatus = CardStatus.紙本件_初始,
                            HandleSeqNo = "1234567890",
                            CardStep = CardStep.月收入確認,
                            UserType = UserType.正卡人,
                            ID = "J12698840",
                        },
                    },
                    ApplyDate = DateTime.Parse("2023-09-15T12:05:51.52"),
                    CaseType = CaseType.急件,
                    LastUpdateTime = DateTime.Parse("2024-09-03T17:40:13.543"),
                    PromotionUnit = "911T",
                    PromotionUser = "1001234",
                },
            },
        };
        return ApiResponseHelper.Success(data);
    }
}

[ExampleAnnotation(Name = "[4000]查詢行員自身案件-查詢值無效", ExampleType = ExampleType.Response)]
public class 查詢行員自身案件查詢值無效_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var errors = new Dictionary<string, IEnumerable<string>>
        {
            {
                "caseStatus",
                new List<string> { "值 13 無效。" }
            },
        };

        return ApiResponseHelper.BadRequest(errors);
    }
}
