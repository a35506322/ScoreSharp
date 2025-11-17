namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.DeleteReviewerPermissionById;

[ExampleAnnotation(Name = "[4001]刪除單筆徵審權限-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除單筆徵審權限查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "30010");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆徵審權限", ExampleType = ExampleType.Response)]
public class 刪除單筆徵審權限_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("2");
    }
}
