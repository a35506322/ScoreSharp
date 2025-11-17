namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalEmailCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]更新相同行內Email比對紀錄-成功", ExampleType = ExampleType.Request)]
public class 更新相同行內Email比對紀錄成功_2000_ReqEx : IExampleProvider<UpdateInternalEmailCheckLogByApplyNoRequest>
{
    public UpdateInternalEmailCheckLogByApplyNoRequest GetExample() =>
        new()
        {
            ApplyNo = "20240803B0001",
            CheckRecord = "確認後無誤",
            IsError = "N",
            Relation = SameDataRelation.子女,
        };
}

[ExampleAnnotation(Name = "[4001]更新相同行內Email比對紀錄-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 更新相同行內Email比對紀錄查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240803B0001");
}

[ExampleAnnotation(Name = "[4003]更新相同行內Email比對紀錄-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 更新相同行內Email比對紀錄路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.路由與Req比對錯誤<string>("20240803B0001");
}

[ExampleAnnotation(Name = "[4003]更新相同行內Email比對紀錄-比對紀錄為N", ExampleType = ExampleType.Response)]
public class 更新相同行內Email比對紀錄_比對紀錄為N_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號 20240803B0001 的行內Email比對紀錄為 N，無法更新");
}

[ExampleAnnotation(Name = "[2000]更新相同行內Email比對紀錄-成功", ExampleType = ExampleType.Response)]
public class 更新相同行內Email比對紀錄成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20240803B0001", "20240803B0001");
}
