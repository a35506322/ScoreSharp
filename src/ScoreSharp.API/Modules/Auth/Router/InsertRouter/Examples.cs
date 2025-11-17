using ScoreSharp.API.Modules.Auth.Router.InsertRouter;

[ExampleAnnotation(Name = "[2000]新增路由", ExampleType = ExampleType.Request)]
public class 新增路由_2000_ReqEx : IExampleProvider<InsertRouterRequest>
{
    public InsertRouterRequest GetExample()
    {
        InsertRouterRequest request = new()
        {
            RouterId = "SetUpBillDay",
            RouterName = "帳單日設定",
            DynamicParams = null,
            IsActive = "Y",
            RouterCategoryId = "SetUp",
            Icon = null,
            Sort = 99,
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增路由", ExampleType = ExampleType.Response)]
public class 新增路由_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("SetUpBillDay", "SetUpBillDay");
    }
}

[ExampleAnnotation(Name = "[4002]新增路由-資料已存在", ExampleType = ExampleType.Response)]
public class 新增路由資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "SetUpBillDay");
    }
}

[ExampleAnnotation(Name = "[4003]新增路由-查無路由類別", ExampleType = ExampleType.Response)]
public class 新增路由查無路由類別_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由類別Id", "Action");
    }
}
