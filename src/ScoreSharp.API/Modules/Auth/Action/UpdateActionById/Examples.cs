namespace ScoreSharp.API.Modules.Auth.Action.UpdateActionById;

[ExampleAnnotation(Name = "[2000]更新操作", ExampleType = ExampleType.Request)]
public class 修改操作_2000_ReqEx : IExampleProvider<UpdateActionByIdRequest>
{
    public UpdateActionByIdRequest GetExample()
    {
        UpdateActionByIdRequest request = new()
        {
            ActionId = "GetBillDayById",
            ActionName = "查詢單筆帳單日",
            RouterId = "SetUpBillDay",
            IsCommon = "Y",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]更新操作", ExampleType = ExampleType.Response)]
public class 修改操作_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("SetUpBillDay", "SetUpBillDay");
    }
}

[ExampleAnnotation(Name = "[4001]更新操作-查無此資料", ExampleType = ExampleType.Response)]
public class 修改操作查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "SetUpBill");
    }
}

[ExampleAnnotation(Name = "[4003]更新操作-查無路由", ExampleType = ExampleType.Response)]
public class 修改操作查無路由_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由Id", "InsertAction");
    }
}

[ExampleAnnotation(Name = "[4003]更新操作-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改操作路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
