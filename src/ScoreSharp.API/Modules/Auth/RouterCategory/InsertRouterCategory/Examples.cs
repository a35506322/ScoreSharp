namespace ScoreSharp.API.Modules.Auth.RouterCategory.InsertRouterCategory;

[ExampleAnnotation(Name = "[2000]新增路由類別", ExampleType = ExampleType.Request)]
public class 新增路由類別_2000_ReqEx : IExampleProvider<InsertRouterCategoryRequest>
{
    public InsertRouterCategoryRequest GetExample()
    {
        InsertRouterCategoryRequest request = new()
        {
            RouterCategoryId = "Auth",
            RouterCategoryName = "權限類別",
            IsActive = "Y",
            Icon = "pi pi-user",
            Sort = 99,
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增路由類別", ExampleType = ExampleType.Response)]
public class 新增路由類別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("Auth", "Auth");
    }
}

[ExampleAnnotation(Name = "[4002]路由類別-資料已存在", ExampleType = ExampleType.Response)]
public class 新增路由類別資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists("", "Auth");
    }
}
