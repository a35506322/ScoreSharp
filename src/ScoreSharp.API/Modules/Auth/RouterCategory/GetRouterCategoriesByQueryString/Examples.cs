namespace ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoriesByQueryString;

[ExampleAnnotation(Name = "[2000]取得路由類別", ExampleType = ExampleType.Response)]
public class 取得路由類別_2000_ResEx : IExampleProvider<ResultResponse<List<GetRouterCategoriesByQueryStringResponse>>>
{
    public ResultResponse<List<GetRouterCategoriesByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetRouterCategoriesByQueryStringResponse>
        {
            new GetRouterCategoriesByQueryStringResponse
            {
                RouterCategoryId = "RouterCategory",
                RouterCategoryName = "路由類別",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
                UpdateUserId = "admin",
                UpdateTime = DateTime.Now,
                Icon = "pi pi-user",
                Sort = 99,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
