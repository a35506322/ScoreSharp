namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateShortTimeIDLogByApplyNo;

[ExampleAnnotation(Name = "[2000]回覆頻繁申請ID確認紀錄-成功", ExampleType = ExampleType.Request)]
public class 回覆頻繁申請ID確認紀錄成功_2000_ReqEx : IExampleProvider<UpdateShortTimeIDLogByApplyNoRequest>
{
    public UpdateShortTimeIDLogByApplyNoRequest GetExample()
    {
        var request = new UpdateShortTimeIDLogByApplyNoRequest()
        {
            ApplyNo = "20240803B0001",
            CheckRecord = "確認後無誤",
            IsError = "N",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]回覆頻繁申請ID確認紀錄-成功", ExampleType = ExampleType.Response)]
public class 回覆頻繁申請ID確認紀錄成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20240803B0001", "20240803B0001");
}

[ExampleAnnotation(Name = "[4001]回覆頻繁申請ID確認紀錄-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 回覆頻繁申請ID確認紀錄查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240803B0001");
}

[ExampleAnnotation(Name = "[4003]回覆頻繁申請ID確認紀錄-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 回覆頻繁申請ID確認紀錄路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.路由與Req比對錯誤<string>("20240803B0001");
}

[ExampleAnnotation(Name = "[4003]回覆頻繁申請ID確認紀錄-商業邏輯驗證失敗", ExampleType = ExampleType.Response)]
public class 回覆頻繁申請ID確認紀錄商業邏輯驗證失敗_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.BusinessLogicFailed<string>(null, $"申請書編號 20240803B0001 的短時間頻繁申請紀錄為 N，無法更新");
}
