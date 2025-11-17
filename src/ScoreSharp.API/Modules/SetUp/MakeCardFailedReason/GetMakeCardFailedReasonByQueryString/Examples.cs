namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonByQueryString;

[ExampleAnnotation(Name = "[2000]取得製卡失敗原因", ExampleType = ExampleType.Response)]
public class 取得製卡失敗原因_2000_ResEx : IExampleProvider<ResultResponse<List<GetMakeCardFailedReasonByQueryStringResponse>>>
{
    public ResultResponse<List<GetMakeCardFailedReasonByQueryStringResponse>> GetExample()
    {
        var data = new List<GetMakeCardFailedReasonByQueryStringResponse>
        {
            new GetMakeCardFailedReasonByQueryStringResponse
            {
                MakeCardFailedReasonCode = "11",
                MakeCardFailedReasonName = "總筆數與檔案(磁帶)內的筆數不合",
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
