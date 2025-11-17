namespace ScoreSharp.API.Modules.Auth.Router.GetRouterById;

[ExampleAnnotation(Name = "[4001]取得路由-查無此資料", ExampleType = ExampleType.Response)]
public class 取得路由查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetRouterByIdResponse>>
{
    public ResultResponse<GetRouterByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetRouterByIdResponse>(null, "顯示找不到的ID");
    }
}

[ExampleAnnotation(Name = "[2000]取得路由", ExampleType = ExampleType.Response)]
public class 取得路由_2000_ResEx : IExampleProvider<ResultResponse<GetRouterByIdResponse>>
{
    public ResultResponse<GetRouterByIdResponse> GetExample()
    {
        GetRouterByIdResponse response = new GetRouterByIdResponse
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
        };

        return ApiResponseHelper.Success(response);
    }
}
