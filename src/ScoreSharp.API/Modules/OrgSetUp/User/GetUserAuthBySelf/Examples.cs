namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUserAuthBySelf;

[ExampleAnnotation(Name = "[2000]取得使用者權限", ExampleType = ExampleType.Response)]
public class 取得使用者權限_2000_ResEx : IExampleProvider<ResultResponse<List<GetUserAuthByIdResponse>>>
{
    public ResultResponse<List<GetUserAuthByIdResponse>> GetExample()
    {
        List<GetUserAuthByIdResponse> response = new()
        {
            new GetUserAuthByIdResponse
            {
                routerCategoryId = "SetUp",
                routerCategoryName = "設定作業",
                icon = "pi pi-user",
                routers = new List<Router>
                {
                    new Router
                    {
                        routerId = "SetUpBlackListReason",
                        routerName = "取有單筆黑名單理由",
                        icon = "pi pi-user",
                        actions = new List<Action>
                        {
                            new Action { actionId = "GetBlackListReasonById", actionName = "取有單筆黑名單理由" },
                        },
                    },
                    new Router
                    {
                        routerId = "SetUpBillDay",
                        routerName = "帳單日期",
                        icon = "pi pi-user",
                        actions = new List<Action>
                        {
                            new Action { actionId = "GetBillDayByQueryString", actionName = "取帳單日期" },
                        },
                    },
                },
            },
        };
        return ApiResponseHelper.Success(response);
    }
}
