namespace ScoreSharp.API.Modules.Auth.Role.GetRoleAuthById;

[ExampleAnnotation(Name = "[2000]取得角色權限", ExampleType = ExampleType.Response)]
public class 取得角色權限_2000_ResEx : IExampleProvider<ResultResponse<List<GetRoleAuthByIdResponse>>>
{
    public ResultResponse<List<GetRoleAuthByIdResponse>> GetExample()
    {
        List<GetRoleAuthByIdResponse> response = new()
        {
            new GetRoleAuthByIdResponse
            {
                RouterCategoryId = "SetUp",
                RouterCategoryName = "設定作業",
                Routers = new List<Router>
                {
                    new Router
                    {
                        RouterId = "SetUpBlackListReason",
                        RouterName = "取有單筆黑名單理由",

                        Actions = new List<Action>
                        {
                            new Action
                            {
                                ActionId = "GetBlackListReasonById",
                                ActionName = "取有單筆黑名單理由",
                                HasPermission = "Y",
                            },
                        },
                    },
                    new Router
                    {
                        RouterId = "SetUpBillDay",
                        RouterName = "帳單日期",
                        Actions = new List<Action>
                        {
                            new Action
                            {
                                ActionId = "GetBillDayByQueryString",
                                ActionName = "取帳單日期",
                                HasPermission = "N",
                            },
                        },
                    },
                },
            },
        };
        return ApiResponseHelper.Success(response);
    }
}
