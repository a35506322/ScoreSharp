namespace ScoreSharp.API.Modules.SetUp.WorkDay.DeleteWorkDayById;

[ExampleAnnotation(Name = "[2000]刪除工作日成功", ExampleType = ExampleType.Response)]
public class 刪除工作日_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess<string>("20250228");
    }
}

[ExampleAnnotation(Name = "[4001]刪除工作日查無資料", ExampleType = ExampleType.Response)]
public class 刪除工作日查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "20991231");
    }
}
