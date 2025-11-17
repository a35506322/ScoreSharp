namespace ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonByQueryString;

[ExampleAnnotation(Name = "[2000]取得退件原因", ExampleType = ExampleType.Response)]
public class 取得退件原因_2000_ResEx : IExampleProvider<ResultResponse<List<GetRejectionReasonByQueryStringResponse>>>
{
    public ResultResponse<List<GetRejectionReasonByQueryStringResponse>> GetExample()
    {
        var data = new List<GetRejectionReasonByQueryStringResponse>
        {
            new GetRejectionReasonByQueryStringResponse
            {
                RejectionReasonCode = "01",
                RejectionReasonName = "信用評分不足",
                IsActive = "Y",
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
