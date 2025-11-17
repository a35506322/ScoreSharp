namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameEmailCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得相同電子信箱檢核結果", ExampleType = ExampleType.Response)]
public class 取得相同電子信箱檢核結果_2000_ResEx : IExampleProvider<ResultResponse<GetSameEmailCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetSameEmailCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetSameEmailCheckLogByApplyNoResponse
            {
                ApplyNo = "20240803B0001",
                SameWebCaseEmailChecked = "Y",
                CheckRecord = "確認是本行同仁申辦",
                UpdateUserId = "jerry",
                UpdateUserName = "傑瑞",
                UpdateTime = DateTime.Now,
                IsError = "Y",
                SameEmailCheckDetails = new List<SameEmailCheckDetailDto>
                {
                    new SameEmailCheckDetailDto
                    {
                        SeqNo = 1,
                        CurrentApplyNo = "20240803B0001",
                        CurrentCardStatus = CardStatus.完成月收入確認,
                        CurrentID = "A123456789",
                        CurrentName = "王小明",
                        CurrentCompName = "測試公司",
                        CurrentEmail = "test@example.com",
                        CurrentOTPMobile = "0912345678",
                        CurrentPromotionUnit = "台北分行",
                        CurrentPromotionUser = "張經理",
                        SameApplyNo = "20240801B0002",
                        SameID = "B987654321",
                        SameName = "李大華",
                        SameCardStatus = CardStatus.完成月收入確認,
                        SameCompName = "另一家公司",
                        SameOTPMobile = "0912345678",
                    },
                },
            }
        );
    }
}

[ExampleAnnotation(Name = "[4001]取得相同電子信箱檢核結果-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得相同電子信箱檢核結果查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetSameEmailCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetSameEmailCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetSameEmailCheckLogByApplyNoResponse>(null, "20240803B0001");
    }
}
