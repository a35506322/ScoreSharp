namespace ScoreSharp.API.Modules.Auth.Router.DeleteRouterById;

[ExampleAnnotation(Name = "[4001]刪除路由-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除路由查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除路由", ExampleType = ExampleType.Response)]
public class 刪除路由_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}

[ExampleAnnotation(Name = "[4003]刪除路由-此資源已被使用", ExampleType = ExampleType.Response)]
public class 刪除路由此資源已被使用_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.此資源已被使用<string>(null, "SetUpBillDay");
    }
}
