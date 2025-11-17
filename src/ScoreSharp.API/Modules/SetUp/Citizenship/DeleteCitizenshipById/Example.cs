namespace ScoreSharp.API.Modules.SetUp.Citizenship.DeleteCitizenshipById;

[ExampleAnnotation(Name = "[2000]刪除國籍", ExampleType = ExampleType.Response)]
public class 刪除國籍_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("AE");
    }
}

[ExampleAnnotation(Name = "[4001]刪除國籍-查無資料", ExampleType = ExampleType.Response)]
public class 刪除國籍查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "TKG");
    }
}
