namespace ScoreSharp.API.Modules.Manage.Stakeholder.UpdateStakeholderById;

[ExampleAnnotation(Name = "[2000]更新單筆利害關係人", ExampleType = ExampleType.Request)]
public class 更新單筆利害關係人_2000_ReqEx : IExampleProvider<UpdateStakeholderByIdRequest>
{
    public UpdateStakeholderByIdRequest GetExample()
    {
        UpdateStakeholderByIdRequest request = new()
        {
            SeqNo = 2,
            UserId = "user123",
            ID = "W259630351",
            IsActive = "N",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆利害關係人", ExampleType = ExampleType.Response)]
public class 更新單筆利害關係人_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("1", "1");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆利害關係人-查無此資料", ExampleType = ExampleType.Response)]
public class 更新單筆利害關係人查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("50", "50");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆利害關係人-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 更新單筆利害關係人路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
