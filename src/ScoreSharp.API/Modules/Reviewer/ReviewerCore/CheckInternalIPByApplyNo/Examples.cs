namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.CheckInternalIPByApplyNo;

[ExampleAnnotation(Name = "[2000]比對行內IP成功", ExampleType = ExampleType.Response)]
public class 比對行內IP成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.Success("20240803B0001", "申請書編號 20240803B0001 比對行內IP成功，結果為 Y");
}

[ExampleAnnotation(Name = "[4001]比對行內IP失敗-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 比對行內IP失敗查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240803B0001");
}
