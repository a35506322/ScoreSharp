namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalIPCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得行內IP比對紀錄", ExampleType = ExampleType.Response)]
public class 取得行內IP比對紀錄_2000_ResEx : IExampleProvider<ResultResponse<GetInternalIPCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetInternalIPCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetInternalIPCheckLogByApplyNoResponse
            {
                ApplyNo = "20240803B0001",
                IsEqualInternalIP = "Y",
                CheckRecord = "確認是本行同仁申辦",
                UpdateUserId = "jerry",
                UpdateUserName = "傑瑞",
                UpdateTime = DateTime.Now,
                IsError = "Y",
            }
        );
    }
}

[ExampleAnnotation(Name = "[4001]取得行內IP比對紀錄-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得行內IP比對紀錄查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetInternalIPCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetInternalIPCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetInternalIPCheckLogByApplyNoResponse>(null, "20240803B0001");
    }
}
