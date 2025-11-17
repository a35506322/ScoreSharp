namespace ScoreSharp.API.Modules.Reviewer3rd.CheckInternalMobileByApplyNo;

[ExampleAnnotation(Name = "[2000]檢核行內手機業務狀況成功", ExampleType = ExampleType.Response)]
public class 檢核行內手機業務狀況成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號檢核行內手機業務狀況完畢");
    }
}

[ExampleAnnotation(Name = "[4001]檢核行內手機業務狀況-查無申請書編號", ExampleType = ExampleType.Response)]
public class 檢核行內手機業務狀況_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.NotFound<string>(checkApplyNo, "查無申請書編號");
    }
}

[ExampleAnnotation(Name = "[2000]檢核行內手機業務狀況-不符合檢核條件故清空紀錄", ExampleType = ExampleType.Response)]
public class 檢核行內手機業務狀況_不符合檢核條件故清空紀錄_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號檢核行內手機業務狀況完畢，不符合檢核條件故清空紀錄");
    }
}

[ExampleAnnotation(Name = "[5002]檢核行內手機業務狀況-發查第三方API失敗", ExampleType = ExampleType.Response)]
public class 檢核行內手機業務狀況失敗_5002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, "此申請書編號檢核行內手機失敗");
    }
}
