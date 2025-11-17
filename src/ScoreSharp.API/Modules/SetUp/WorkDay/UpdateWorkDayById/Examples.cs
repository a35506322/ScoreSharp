namespace ScoreSharp.API.Modules.SetUp.WorkDay.UpdateWorkDayById;

[ExampleAnnotation(Name = "[2000]更新工作日請求", ExampleType = ExampleType.Request)]
public class 更新工作日_2000_ReqEx : IExampleProvider<UpdateWorkDayByIdRequest>
{
    public UpdateWorkDayByIdRequest GetExample()
    {
        return new UpdateWorkDayByIdRequest
        {
            Date = "20250101",
            Year = "2025",
            Name = "中華民國開國紀念日",
            IsHoliday = "N",
            HolidayCategory = "調整放假",
            Description = "因政策調整取消放假。",
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新工作日成功", ExampleType = ExampleType.Response)]
public class 更新工作日_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("20250101", "20250101");
    }
}

[ExampleAnnotation(Name = "[4001]更新工作日查無資料", ExampleType = ExampleType.Response)]
public class 更新工作日查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "20991231");
    }
}

[ExampleAnnotation(Name = "[4003]更新工作日路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 更新工作日路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
