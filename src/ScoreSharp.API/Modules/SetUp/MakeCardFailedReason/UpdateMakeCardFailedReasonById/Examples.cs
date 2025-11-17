namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.UpdateMakeCardFailedReasonById;

[ExampleAnnotation(Name = "[2000]修改製卡失敗原因", ExampleType = ExampleType.Request)]
public class 修改製卡失敗原因_2000_ReqEx : IExampleProvider<UpdateMakeCardFailedReasonByIdRequest>
{
    public UpdateMakeCardFailedReasonByIdRequest GetExample()
    {
        UpdateMakeCardFailedReasonByIdRequest request = new()
        {
            MakeCardFailedReasonCode = "11",
            MakeCardFailedReasonName = "總筆數與檔案(磁帶)內的筆數不合",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改製卡失敗原因", ExampleType = ExampleType.Response)]
public class 修改製卡失敗原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("11", "11");
    }
}

[ExampleAnnotation(Name = "[4001]修改製卡失敗原因-查無此資料", ExampleType = ExampleType.Response)]
public class 修改製卡失敗原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "18");
    }
}

[ExampleAnnotation(Name = "[4003]修改製卡失敗原因-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改製卡失敗原因路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
