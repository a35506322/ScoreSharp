namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameMobileCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得相同手機號碼檢核結果", ExampleType = ExampleType.Response)]
public class 取得相同手機號碼檢核結果_2000_ResEx : IExampleProvider<ResultResponse<GetSameMobileCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetSameMobileCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetSameMobileCheckLogByApplyNoResponse
            {
                ApplyNo = "20240803B0001",
                SameWebCaseMobileChecked = "Y",
                CheckRecord = "確認是本行同仁申辦",
                UpdateUserId = "jerry",
                UpdateUserName = "傑瑞",
                UpdateTime = DateTime.Now,
                IsError = "Y",
                SameMobileCheckDetails = new List<SameMobileCheckDetailDto>
                {
                    new SameMobileCheckDetailDto
                    {
                        SeqNo = 1,
                        CurrentApplyNo = "20240803B0001",
                        CurrentCardStatus = CardStatus.完成月收入確認,
                        CurrentID = "A123456789",
                        CurrentName = "王小明",
                        CurrentCompName = "測試公司",
                        CurrentOTPMobile = "0912345678",
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

[ExampleAnnotation(Name = "[4001]取得相同手機號碼檢核結果-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得相同手機號碼檢核結果查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetSameMobileCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetSameMobileCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetSameMobileCheckLogByApplyNoResponse>(null, "20240803B0001");
    }
}
