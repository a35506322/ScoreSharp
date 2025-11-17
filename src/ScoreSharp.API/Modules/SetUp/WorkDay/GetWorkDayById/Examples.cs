namespace ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDayById;

[ExampleAnnotation(Name = "[2000]取得單筆工作日資料", ExampleType = ExampleType.Response)]
public class 取得單筆工作日資料_2000_ResEx : IExampleProvider<ResultResponse<GetWorkDayByIdResponse>>
{
    public ResultResponse<GetWorkDayByIdResponse> GetExample()
    {
        var data = new GetWorkDayByIdResponse
        {
            Date = "20250101",
            Year = "2025",
            Name = "中華民國開國紀念日",
            IsHoliday = "Y",
            HolidayCategory = "放假之紀念日及節日",
            Description = "全國各機關學校放假一日。",
            AddUserId = "SYSTEM",
            AddTime = new DateTime(2025, 6, 19, 11, 14, 3),
            UpdateUserId = null,
            UpdateTime = null,
        };

        return ApiResponseHelper.Success(data);
    }
}

[ExampleAnnotation(Name = "[4001]查無工作日資料", ExampleType = ExampleType.Response)]
public class 查無工作日資料_4001_ResEx : IExampleProvider<ResultResponse<GetWorkDayByIdResponse>>
{
    public ResultResponse<GetWorkDayByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetWorkDayByIdResponse>(null, "20991231");
    }
}
