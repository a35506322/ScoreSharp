namespace ScoreSharp.API.Modules.Auth.Action.GetActionsByQueryString;

[ExampleAnnotation(Name = "[2000]取得操作", ExampleType = ExampleType.Response)]
public class 取得操作_2000_ResEx : IExampleProvider<ResultResponse<List<GetActionsByQueryStringResponse>>>
{
    public ResultResponse<List<GetActionsByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetActionsByQueryStringResponse>
        {
            new GetActionsByQueryStringResponse
            {
                ActionName = "查詢路由類別ByQueryString",
                ActionId = "GetRouterCatregoriesByQueryString",
                RouterId = "SetUpRouterCategory",
                IsCommon = "Y",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
                UpdateUserId = "admin",
                UpdateTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
