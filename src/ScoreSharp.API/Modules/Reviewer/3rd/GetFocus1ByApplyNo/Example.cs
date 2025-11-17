namespace ScoreSharp.API.Modules.Reviewer3rd.GetFocus1ByApplyNo;

[ExampleAnnotation(Name = "[2000]查詢關注名單1成功", ExampleType = ExampleType.Response)]
public class 查詢關注名單1成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號查詢關注名單1完畢");
    }
}

[ExampleAnnotation(Name = "[4003]查詢關注名單1-查無申請書編號", ExampleType = ExampleType.Response)]
public class 查詢關注名單1失敗_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.BusinessLogicFailed<string>(checkApplyNo, "查無申請書編號");
    }
}

[ExampleAnnotation(Name = "[5002]查詢關注名單1-發查第三方API失敗", ExampleType = ExampleType.Response)]
public class 查詢關注名單1失敗_5002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string checkApplyNo = "20241203E2074";
        return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, checkApplyNo);
    }
}
