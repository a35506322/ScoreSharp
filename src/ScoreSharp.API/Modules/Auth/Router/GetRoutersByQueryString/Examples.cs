namespace ScoreSharp.API.Modules.Auth.Router.GetRoutersByQueryString;

[ExampleAnnotation(Name = "[2000]取得路由", ExampleType = ExampleType.Response)]
public class 取得路由_2000_ResEx : IExampleProvider<ResultResponse<List<GetRoutersByQueryStringResponse>>>
{
    public ResultResponse<List<GetRoutersByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetRoutersByQueryStringResponse>
        {
            new GetRoutersByQueryStringResponse
            {
                RouterCategoryId = "SetUp",
                RouterId = "SetUpInternalIP",
                RouterName = "行內IP設定",
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
