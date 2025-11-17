namespace ScoreSharp.API.Modules.Reviewer3rd.Get929ByApplyNo;

[ExampleAnnotation(Name = "[2000]查詢929業務狀況成功", ExampleType = ExampleType.Response)]
public class 查詢929業務狀況成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號查詢929業務狀況完畢");
    }
}

[ExampleAnnotation(Name = "[4003]查詢929業務狀況-查無申請書編號", ExampleType = ExampleType.Response)]
public class 查詢929業務狀況_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.BusinessLogicFailed<string>(checkApplyNo, "查無申請書編號");
    }
}

[ExampleAnnotation(Name = "[5002]查詢929業務狀況-發查第三方API失敗", ExampleType = ExampleType.Response)]
public class 查詢929業務狀況失敗_5002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, checkApplyNo);
    }
}
