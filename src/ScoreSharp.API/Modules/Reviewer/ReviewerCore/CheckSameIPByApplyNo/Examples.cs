namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.CheckSameIPByApplyNo;

[ExampleAnnotation(Name = "[2000]比對相同IP成功", ExampleType = ExampleType.Response)]
public class 比對相同IP成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.Success("20240803B0001", "申請書編號 20240803B0001 比對相同IP成功，結果為 Y");
}

[ExampleAnnotation(Name = "[4001]比對相同IP失敗-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 比對相同IP失敗查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240803B0001");
}

[ExampleAnnotation(Name = "[4003]比對相同IP失敗-非 Web 案件", ExampleType = ExampleType.Response)]
public class 比對相同IP失敗非Web案件_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號 20240803B0001 不是 Web 案件");
}

[ExampleAnnotation(Name = "[4003]比對相同IP失敗-沒有 IP 紀錄", ExampleType = ExampleType.Response)]
public class 比對相同IP失敗沒有IP紀錄_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號 20240803B0001 沒有 IP 紀錄");
}
