using ScoreSharp.API.Modules.Auth.RouterCategory.UpdateRouterCategoryById;

[ExampleAnnotation(Name = "[2000]修改路由類別", ExampleType = ExampleType.Request)]
public class 修改路由類別_2000_ReqEx : IExampleProvider<UpdateRouterCategoryByIdRequest>
{
    public UpdateRouterCategoryByIdRequest GetExample()
    {
        UpdateRouterCategoryByIdRequest request = new()
        {
            RouterCategoryId = "Auth",
            RouterCategoryName = "權限類別",
            IsActive = "Y",
            Icon = "pi pi-user",
            Sort = 9,
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改路由類別", ExampleType = ExampleType.Response)]
public class 修改路由類別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("Auth", "Auth");
    }
}

[ExampleAnnotation(Name = "[4001]修改路由類別-查無此資料", ExampleType = ExampleType.Response)]
public class 修改路由類別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "Auth");
    }
}

[ExampleAnnotation(Name = "[4003]修改路由類別-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改路由類別路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
