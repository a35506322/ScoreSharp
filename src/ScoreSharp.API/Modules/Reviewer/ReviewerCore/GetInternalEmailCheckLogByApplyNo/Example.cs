namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalEmailCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得相同行內Email檢核結果", ExampleType = ExampleType.Response)]
public class 取得相同行內Email檢核結果_2000_ResEx : IExampleProvider<ResultResponse<GetInternalEmailCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetInternalEmailCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetInternalEmailCheckLogByApplyNoResponse
            {
                ApplyNo = "20240803B0001",
                SameInternalEmailChecked = "Y",
                CheckRecord = "確認是本行同仁申辦",
                UpdateUserId = "jerry",
                UpdateUserName = "傑瑞",
                UpdateTime = DateTime.Now,
                IsError = "N",
                Relation = SameDataRelation.配偶,
                SameInternalEmailCheckDetails = new List<SameInternalEmailCheckDetailDto>
                {
                    new SameInternalEmailCheckDetailDto
                    {
                        CurrentApplyNo = "20240803B0001",
                        CurrentCardStatusList = new List<CardStatusDto>()
                        {
                            new CardStatusDto { SeqNo = "01K0P6QEG7XBMA92PHG1H9M6D1", CardStatus = CardStatus.完成月收入確認 },
                        },
                        CurrentID = "A123456789",
                        CurrentName = "王小明",
                        CurrentEmail = "測試Email",
                        CurrentPromotionUnit = "推廣單位",
                        CurrentPromotionUser = "推廣人員",
                        SameID = "B987654321",
                        SameName = "李大華",
                        SameBillAddr = "台北市內湖區瑞光路399號",
                    },
                },
            }
        );
    }
}

[ExampleAnnotation(Name = "[4001]取得相同行內Email檢核結果-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得相同行內Email檢核結果_查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetInternalEmailCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetInternalEmailCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetInternalEmailCheckLogByApplyNoResponse>(null, "20240803B0001");
    }
}
