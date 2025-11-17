namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.DeleteAMLJobLevelById;

[ExampleAnnotation(Name = "[2000]刪除AML職級別", ExampleType = ExampleType.Response)]
public class 刪除AML職級別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("2");
    }
}

[ExampleAnnotation(Name = "[4001]刪除AML職級別-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除AML職級別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("13", "13");
    }
}
