namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.InsertMakeCardFailedReason;

[ExampleAnnotation(Name = "[2000]新增製卡失敗原因", ExampleType = ExampleType.Request)]
public class 新增製卡失敗原因_2000_ReqEx : IExampleProvider<InsertMakeCardFailedReasonRequest>
{
    public InsertMakeCardFailedReasonRequest GetExample()
    {
        InsertMakeCardFailedReasonRequest request = new()
        {
            MakeCardFailedReasonCode = "YE",
            MakeCardFailedReasonName = "緊急製卡讀不到主機對應卡號",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增製卡失敗原因", ExampleType = ExampleType.Response)]
public class 新增製卡失敗原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("YE", "YE");
    }
}

[ExampleAnnotation(Name = "[4002]新增製卡失敗原因-資料已存在", ExampleType = ExampleType.Response)]
public class 新增製卡失敗原因資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "11");
    }
}
