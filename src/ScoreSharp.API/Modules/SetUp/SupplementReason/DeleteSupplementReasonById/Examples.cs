namespace ScoreSharp.API.Modules.SetUp.SupplementReason.DeleteSupplementReasonById;

[ExampleAnnotation(Name = "[4001]刪除補件原因-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除補件原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除補件原因", ExampleType = ExampleType.Response)]
public class 刪除補件原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}
