namespace ScoreSharp.API.Modules.Auth.RouterCategory.DeleteRouterCategoryById;

[ExampleAnnotation(Name = "[4001]刪除路由類別-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除路由類別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除路由類別", ExampleType = ExampleType.Response)]
public class 刪除路由類別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}

[ExampleAnnotation(Name = "[4003]刪除路由類別-此資源已被使用", ExampleType = ExampleType.Response)]
public class 刪除路由類別此資源已被使用_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.此資源已被使用<string>(null, "SetUp");
    }
}
