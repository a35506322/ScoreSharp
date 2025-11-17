namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.CheckSameMobileByApplyNo;

[ExampleAnnotation(Name = "[2000]比對相同手機號碼成功", ExampleType = ExampleType.Response)]
public class 比對相同手機號碼成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.Success("20240803B0001", "申請書編號 20240803B0001 比對相同手機號碼成功，結果為 true");
}

[ExampleAnnotation(Name = "[4001]比對相同手機號碼失敗-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 比對相同手機號碼失敗查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240803B0001");
}

[ExampleAnnotation(Name = "[5000]比對相同手機號碼失敗-內部程式失敗", ExampleType = ExampleType.Response)]
public class 比對相同手機號碼失敗內部程式失敗_5000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.InternalServerErrorByException<string>(null, "申請書編號 20240803B0001 比對相同手機號碼失敗");
}
