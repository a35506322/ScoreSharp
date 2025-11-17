namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalMobileCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得相同行內手機檢核結果", ExampleType = ExampleType.Response)]
public class 取得相同行內手機檢核結果_2000_ResEx : IExampleProvider<ResultResponse<GetInternalMobileCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetInternalMobileCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetInternalMobileCheckLogByApplyNoResponse
            {
                ApplyNo = "20240803B0001",
                SameInternalMobileChecked = "Y",
                CheckRecord = "確認是本行同仁申辦",
                UpdateUserId = "tom",
                UpdateUserName = "湯姆",
                UpdateTime = DateTime.Now,
                IsError = "N",
                Relation = SameDataRelation.兄弟姊妹,
                SameInternalMobileCheckDetails = new List<SameInternalMobileCheckDetailDto>
                {
                    new SameInternalMobileCheckDetailDto
                    {
                        CurrentApplyNo = "20240803B0001",
                        CurrentCardStatusList = new List<CardStatusDto>()
                        {
                            new CardStatusDto { SeqNo = "01K0P6QEG7XBMA92PHG1H9M6D1", CardStatus = CardStatus.完成黑名單查詢 },
                        },
                        CurrentID = "A123456789",
                        CurrentName = "王小明",
                        CurrentMobile = "測試手機號碼",
                        CurrentPromotionUnit = "推廣單位",
                        CurrentPromotionUser = "推廣人員",
                        SameID = "B987654321",
                        SameName = "李連結",
                        SameBillAddr = "台北市內湖區瑞光路399號",
                    },
                },
            }
        );
    }
}

[ExampleAnnotation(Name = "[4001]取得相同行內手機檢核結果-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得相同行內手機檢核結果_查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetInternalMobileCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetInternalMobileCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetInternalMobileCheckLogByApplyNoResponse>(null, "20240803B0001");
    }
}
