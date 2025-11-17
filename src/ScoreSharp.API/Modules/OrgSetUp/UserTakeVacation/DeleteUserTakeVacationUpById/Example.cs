namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.DeleteUserTakeVacationById;

[ExampleAnnotation(Name = "[4001]刪除單筆員工休假-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除單筆員工休假查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "100");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆員工休假", ExampleType = ExampleType.Response)]
public class 刪除單筆員工休假_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("2");
    }
}
