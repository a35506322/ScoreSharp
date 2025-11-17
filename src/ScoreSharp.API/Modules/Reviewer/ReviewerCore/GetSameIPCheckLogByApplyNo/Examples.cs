namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameIPCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得相同IP檢核結果", ExampleType = ExampleType.Response)]
public class 取得相同IP檢核結果_2000_ResEx : IExampleProvider<ResultResponse<GetSameIPCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetSameIPCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetSameIPCheckLogByApplyNoResponse
            {
                ApplyNo = "20240803B0001",
                SameIPChecked = "Y",
                CheckRecord = "確認是本行同仁申辦",
                UpdateUserId = "jerry",
                UpdateUserName = "傑瑞",
                UpdateTime = DateTime.Now,
                IsError = "Y",
            }
        );
    }
}

[ExampleAnnotation(Name = "[4001]取得相同IP檢核結果-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得相同IP檢核結果查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetSameIPCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetSameIPCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetSameIPCheckLogByApplyNoResponse>(null, "20240803B0001");
    }
}
