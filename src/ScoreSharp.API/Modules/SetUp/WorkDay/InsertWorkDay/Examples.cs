namespace ScoreSharp.API.Modules.SetUp.WorkDay.InsertWorkDay;

[ExampleAnnotation(Name = "[2000]新增工作日請求", ExampleType = ExampleType.Request)]
public class 新增工作日_2000_ReqEx : IExampleProvider<InsertWorkDayRequest>
{
    public InsertWorkDayRequest GetExample()
    {
        return new InsertWorkDayRequest
        {
            Date = "20250101",
            Year = "2025",
            Name = "中華民國開國紀念日",
            IsHoliday = "Y",
            HolidayCategory = "放假之紀念日及節日",
            Description = "全國各機關學校放假一日。",
        };
    }
}

[ExampleAnnotation(Name = "[2000]新增工作日成功", ExampleType = ExampleType.Response)]
public class 新增工作日_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("20250101", "20250101");
    }
}

[ExampleAnnotation(Name = "[4002]新增工作日資料已存在", ExampleType = ExampleType.Response)]
public class 新增工作日資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "20250101");
    }
}
