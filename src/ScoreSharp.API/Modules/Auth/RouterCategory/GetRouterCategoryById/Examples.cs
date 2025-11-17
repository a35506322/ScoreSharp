namespace ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoryById;

[ExampleAnnotation(Name = "[4001]取得路由類別-查無此資料", ExampleType = ExampleType.Response)]
public class 取得路由類別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetRouterCategoryByIdResponse>>
{
    public ResultResponse<GetRouterCategoryByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetRouterCategoryByIdResponse>(null, "顯示找不到的ID");
    }
}

[ExampleAnnotation(Name = "[2000]取得路由類別", ExampleType = ExampleType.Response)]
public class 取得路由類別_2000_ResEx : IExampleProvider<ResultResponse<GetRouterCategoryByIdResponse>>
{
    public ResultResponse<GetRouterCategoryByIdResponse> GetExample()
    {
        GetRouterCategoryByIdResponse response = new GetRouterCategoryByIdResponse
        {
            RouterCategoryId = "RouterCategory",
            RouterCategoryName = "路由類別",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateUserId = "ADMIN",
            UpdateTime = DateTime.Now,
            Icon = "pi pi-user",
            Sort = 99,
        };

        return ApiResponseHelper.Success(response);
    }
}
