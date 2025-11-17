namespace ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonById;

[ExampleAnnotation(Name = "[2000]取得黑名單理由", ExampleType = ExampleType.Response)]
public class 取得黑名單理由_2000_ResEx : IExampleProvider<ResultResponse<GetBlackListReasonByIdResponse>>
{
    public ResultResponse<GetBlackListReasonByIdResponse> GetExample()
    {
        GetBlackListReasonByIdResponse response = new()
        {
            BlackListReasonCode = "01",
            BlackListReasonName = "催收退件",
            IsActive = "Y",
            ReasonStrength = 9,
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得黑名單理由-查無此資料", ExampleType = ExampleType.Response)]
public class 取得黑名單理由查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetBlackListReasonByIdResponse>>
{
    public ResultResponse<GetBlackListReasonByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetBlackListReasonByIdResponse>(null, "05");
    }
}
