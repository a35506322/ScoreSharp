namespace ScoreSharp.API.Modules.SetUp.InternalIP.GetInternalIPByQueryString;

[ExampleAnnotation(Name = "[2000]取得多筆行內IP", ExampleType = ExampleType.Response)]
public class 取得多筆行內IP_2000_ResEx : IExampleProvider<ResultResponse<List<GetInternalIPByQueryStringResponse>>>
{
    public ResultResponse<List<GetInternalIPByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetInternalIPByQueryStringResponse>
        {
            new GetInternalIPByQueryStringResponse
            {
                IP = "172.28.234.10",
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
