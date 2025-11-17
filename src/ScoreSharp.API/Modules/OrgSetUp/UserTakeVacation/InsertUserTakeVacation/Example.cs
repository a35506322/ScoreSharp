namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.InsertUserTakeVacation;

[ExampleAnnotation(Name = "[2000]新增單筆員工休假", ExampleType = ExampleType.Request)]
public class 新增單筆員工休假_2000_ReqEx : IExampleProvider<InsertUserTakeVacationRequest>
{
    public InsertUserTakeVacationRequest GetExample()
    {
        InsertUserTakeVacationRequest request = new()
        {
            UserId = "abbylin",
            StartTime = DateTime.Now.AddMinutes(10),
            EndTime = DateTime.Now.AddHours(8),
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增單筆員工休假", ExampleType = ExampleType.Response)]
public class 新增單筆員工休假_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("abbylin", "abbylin");
    }
}

[ExampleAnnotation(Name = "[4003]新增單筆員工休假-時間有誤", ExampleType = ExampleType.Response)]
public class 新增單筆員工休假時間有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "請確認開始時間不晚於結束時間");
    }
}

[ExampleAnnotation(Name = "[4001]新增單筆員工休假-查無定義值", ExampleType = ExampleType.Response)]
public class 新增單筆員工休假查無定義值_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string userId = "kevin";

        return ApiResponseHelper.NotFound<string>(null, userId);
    }
}
