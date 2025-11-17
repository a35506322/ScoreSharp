namespace ScoreSharp.API.Modules.Reviewer3rd.GetNameCheck;

[ExampleAnnotation(Name = "[2000]查詢姓名檢核成功_正卡人", ExampleType = ExampleType.Response)]
public class 查詢姓名檢核成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.Success<string>("20240803B0001", "此申請書查詢姓名檢核成功");
}

[ExampleAnnotation(Name = "[2000]查詢姓名檢核成功_附卡人", ExampleType = ExampleType.Response)]
public class 查詢姓名檢核成功_附卡人_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.Success<string>("20240803B0001", "此申請書查詢姓名檢核成功");
}

[ExampleAnnotation(Name = "[5002]查詢姓名檢核_發查第三方API失敗", ExampleType = ExampleType.Response)]
public class 查詢姓名檢核失敗_5002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.CheckThirdPartyApiError<string>("20240803B0001", "20240803B0001");
}

[ExampleAnnotation(Name = "[4001]查詢姓名檢核_查無此ID", ExampleType = ExampleType.Response)]
public class 查詢姓名檢核失敗_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.NotFound<string>("找不到申請書編號 20250626990001 姓名為 蔣孝嚴 的附卡人資料", "20250626990001");
}
