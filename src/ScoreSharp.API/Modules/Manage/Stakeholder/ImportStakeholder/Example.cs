namespace ScoreSharp.API.Modules.Manage.Stakeholder.ImportStakeholder;

[ExampleAnnotation(Name = "[2000]匯入利害關係人", ExampleType = ExampleType.Response)]
public class 匯入利害關係人_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("匯入 [檔案名稱] 成功", null);
    }
}

[ExampleAnnotation(Name = "[4003]匯入利害關係人-檔案格式錯誤", ExampleType = ExampleType.Response)]
public class 匯入利害關係人_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "檔案格式錯誤，請上傳 .txt 檔案。");
    }
}
