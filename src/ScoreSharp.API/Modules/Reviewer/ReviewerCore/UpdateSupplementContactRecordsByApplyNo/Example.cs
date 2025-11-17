namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementContactRecordsByApplyNo;

[ExampleAnnotation(Name = "[2000]修改補聯繫紀錄-成功", ExampleType = ExampleType.Request)]
public class 修改補聯繫紀錄成功_2000_ReqEx : IExampleProvider<UpdateSupplementContactRecordsByApplyNoRequest>
{
    public UpdateSupplementContactRecordsByApplyNoRequest GetExample() =>
        new()
        {
            ApplyNo = "20250321G7943",
            SupplementContactRecords_Type = SupplementContactRecordsType.人員通知,
            SupplementContactRecords_Result = SupplementContactRecordsResult.客戶考慮中,
            SupplementContactRecords_Summary = "補聯繫紀錄-其他-摘要",
        };
}

[ExampleAnnotation(Name = "[4001]修改補聯繫紀錄-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 修改補聯繫紀錄查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20250101G3234");
}

[ExampleAnnotation(Name = "[4003]修改補聯繫紀錄-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改補聯繫紀錄路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.路由與Req比對錯誤<string>("2");
}

[ExampleAnnotation(Name = "[2000]修改補聯繫紀錄-成功", ExampleType = ExampleType.Response)]
public class 修改補聯繫紀錄成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20250321G7943", "20250321G7943");
}
