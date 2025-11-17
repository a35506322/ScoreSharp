namespace ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonByQueryString;

[ExampleAnnotation(Name = "[2000]取得補件原因", ExampleType = ExampleType.Response)]
public class 取得補件原因_2000_ResEx : IExampleProvider<ResultResponse<List<GetSupplementReasonByQueryStringResponse>>>
{
    public ResultResponse<List<GetSupplementReasonByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetSupplementReasonByQueryStringResponse>
        {
            new GetSupplementReasonByQueryStringResponse
            {
                SupplementReasonCode = "03",
                SupplementReasonName = "收入證明",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
                UpdateUserId = "admin",
                UpdateTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
