namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameEmailCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]更新相同Email比對紀錄-成功", ExampleType = ExampleType.Request)]
public class 更新相同Email比對紀錄成功_2000_ReqEx : IExampleProvider<UpdateSameEmailCheckLogByApplyNoRequest>
{
    public UpdateSameEmailCheckLogByApplyNoRequest GetExample() =>
        new()
        {
            ApplyNo = "20240803B0001",
            CheckRecord = "確認後無誤",
            IsError = "N",
        };
}

[ExampleAnnotation(Name = "[4001]更新相同Email比對紀錄-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 更新相同Email比對紀錄查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240803B0001");
}

[ExampleAnnotation(Name = "[4003]更新相同Email比對紀錄-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 更新相同Email比對紀錄路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.路由與Req比對錯誤<string>("20240803B0001");
}

[ExampleAnnotation(Name = "[2000]更新相同Email比對紀錄-成功", ExampleType = ExampleType.Response)]
public class 更新相同Email比對紀錄成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20240803B0001", "20240803B0001");
}

[ExampleAnnotation(Name = "[4004]更新相同Email比對紀錄-申請書編號與Req不一致", ExampleType = ExampleType.Response)]
public class 更新相同Email比對紀錄申請書編號與Req不一致_4004_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.路由與Req比對錯誤<string>("20240803B0001");
}
