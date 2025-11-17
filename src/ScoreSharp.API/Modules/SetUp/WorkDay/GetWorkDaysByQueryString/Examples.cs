namespace ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDaysByQueryString;

[ExampleAnnotation(Name = "[2000]取得多筆工作日資料", ExampleType = ExampleType.Response)]
public class 取得多筆工作日資料_2000_ResEx : IExampleProvider<ResultResponse<List<GetWorkDaysByQueryStringResponse>>>
{
    public ResultResponse<List<GetWorkDaysByQueryStringResponse>> GetExample()
    {
        var data = new List<GetWorkDaysByQueryStringResponse>
        {
            new GetWorkDaysByQueryStringResponse
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
            },
            new GetWorkDaysByQueryStringResponse
            {
                Date = "20250228",
                Year = "2025",
                Name = "和平紀念日",
                IsHoliday = "Y",
                HolidayCategory = "放假之紀念日及節日",
                Description = "例假日調整放假。",
                AddUserId = "SYSTEM",
                AddTime = new DateTime(2025, 6, 19, 11, 14, 3),
                UpdateUserId = null,
                UpdateTime = null,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
