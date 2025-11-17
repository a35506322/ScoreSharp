namespace ScoreSharp.API.Modules.SetUp.FixTime.UpdateFixTime;

[ExampleAnnotation(Name = "[2000]更新維護時段請求", ExampleType = ExampleType.Request)]
public class 更新維護時段_2000_ReqEx : IExampleProvider<UpdateFixTimeRequest>
{
    public UpdateFixTimeRequest GetExample()
    {
        return new UpdateFixTimeRequest
        {
            KYC_IsFix = "Y",
            KYC_StartTime = new DateTime(2025, 1, 1, 0, 0, 0),
            KYC_EndTime = new DateTime(2025, 1, 1, 6, 0, 0),
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新維護時段成功", ExampleType = ExampleType.Response)]
public class 更新維護時段_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success<string>(null, "維護時段設定已更新");
    }
}
