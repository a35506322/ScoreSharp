namespace ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonByQueryString;

[ExampleAnnotation(Name = "[2000]取得黑名單理由", ExampleType = ExampleType.Response)]
public class 取得黑名單理由_2000_ResEx : IExampleProvider<ResultResponse<List<GetBlackListReasonByQueryStringResponse>>>
{
    public ResultResponse<List<GetBlackListReasonByQueryStringResponse>> GetExample()
    {
        var data = new List<GetBlackListReasonByQueryStringResponse>
        {
            new GetBlackListReasonByQueryStringResponse
            {
                BlackListReasonCode = "01",
                BlackListReasonName = "催收退件",
                IsActive = "Y",
                ReasonStrength = 9,
                AddUserId = "ADMIN",
                AddTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
