namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.DeleteApplicationCategoryById;

[ExampleAnnotation(Name = "[2000]刪除申請書類別", ExampleType = ExampleType.Response)]
public class 刪除申請書類別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("AP0008");
    }
}

[ExampleAnnotation(Name = "[4001]刪除申請書類別-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除申請書類別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "AP0005");
    }
}
