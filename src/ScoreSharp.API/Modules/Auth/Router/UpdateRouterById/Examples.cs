namespace ScoreSharp.API.Modules.Auth.Router.UpdateRouterById;

[ExampleAnnotation(Name = "[2000]更新路由", ExampleType = ExampleType.Request)]
public class 修改路由_2000_ReqEx : IExampleProvider<UpdateRouterByIdRequest>
{
    public UpdateRouterByIdRequest GetExample()
    {
        UpdateRouterByIdRequest request = new()
        {
            RouterId = "SetUpBillDay",
            RouterName = "帳單日設定",
            DynamicParams = null,
            RouterCategoryId = "SetUp",
            IsActive = "Y",
            Icon = null,
            Sort = 99,
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]更新路由", ExampleType = ExampleType.Response)]
public class 修改路由_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("SetUpBillDay", "SetUpBillDay");
    }
}

[ExampleAnnotation(Name = "[4001]更新路由-查無此資料", ExampleType = ExampleType.Response)]
public class 修改路由查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "SetUpBill");
    }
}

[ExampleAnnotation(Name = "[4003]更新路由-查無路由類別", ExampleType = ExampleType.Response)]
public class 修改路由查無路由類別_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由類別Id", "SetUpBill");
    }
}

[ExampleAnnotation(Name = "[4003]更新路由-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改路由路由與Req比對錯誤4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
