namespace ScoreSharp.API.Modules.Auth.Action.InsertAction;

[ExampleAnnotation(Name = "[2000]新增操作", ExampleType = ExampleType.Request)]
public class 新增操作_2000_ReqEx : IExampleProvider<InsertActionRequest>
{
    public InsertActionRequest GetExample()
    {
        InsertActionRequest request = new()
        {
            ActionId = "GetBillDayByQueryString",
            ActionName = "查詢多筆帳單日",
            IsCommon = "Y",
            IsActive = "Y",
            RouterId = "SetUpBillDay",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增操作", ExampleType = ExampleType.Response)]
public class 新增操作_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("GetBillDayByQueryString", "GetBillDayByQueryString");
    }
}

[ExampleAnnotation(Name = "[4002]新增操作-資料已存在", ExampleType = ExampleType.Response)]
public class 新增操作資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "GetBillDayByQueryString");
    }
}

[ExampleAnnotation(Name = "[4003]新增操作-查無路由", ExampleType = ExampleType.Response)]
public class 新增操作查無路由_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由Id", "InsertAction");
    }
}
