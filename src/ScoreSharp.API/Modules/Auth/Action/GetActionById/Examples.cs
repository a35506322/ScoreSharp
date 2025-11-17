namespace ScoreSharp.API.Modules.Auth.Action.GetActionById;

[ExampleAnnotation(Name = "[4001]取得操作-查無此資料", ExampleType = ExampleType.Response)]
public class 取得操作查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetActionByIdResponse>>
{
    public ResultResponse<GetActionByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetActionByIdResponse>(null, "顯示找不到的ID");
    }
}

[ExampleAnnotation(Name = "[2000]取得操作", ExampleType = ExampleType.Response)]
public class 取得操作_2000_ResEx : IExampleProvider<ResultResponse<GetActionByIdResponse>>
{
    public ResultResponse<GetActionByIdResponse> GetExample()
    {
        GetActionByIdResponse response = new GetActionByIdResponse
        {
            ActionId = "GetRouterCatregoriesByQueryString",
            ActionName = "查詢路由類別ByQueryString",
            IsCommon = "Y",
            RouterId = "SetUpBillDay",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateUserId = "ADMIN",
            UpdateTime = DateTime.Now,
        };

        return ApiResponseHelper.Success(response);
    }
}
