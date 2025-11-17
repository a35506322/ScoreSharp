namespace ScoreSharp.API.Modules.Auth.Action.DeleteActionById;

[ExampleAnnotation(Name = "[4001]刪除操作-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除操作查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除操作", ExampleType = ExampleType.Response)]
public class 刪除操作_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}

[ExampleAnnotation(Name = "[4003]刪除操作-此資源已被使用", ExampleType = ExampleType.Response)]
public class 刪除操作此資源已被使用_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.此資源已被使用<string>(null, "GetBillDayByQueryString");
    }
}
