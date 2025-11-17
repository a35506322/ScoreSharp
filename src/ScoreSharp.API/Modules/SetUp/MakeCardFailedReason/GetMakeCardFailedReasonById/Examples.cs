namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonById;

[ExampleAnnotation(Name = "[2000]取得製卡失敗原因", ExampleType = ExampleType.Response)]
public class 取得製卡失敗原因_2000_ResEx : IExampleProvider<ResultResponse<GetMakeCardFailedReasonByIdResponse>>
{
    public ResultResponse<GetMakeCardFailedReasonByIdResponse> GetExample()
    {
        GetMakeCardFailedReasonByIdResponse response = new()
        {
            MakeCardFailedReasonCode = "11",
            MakeCardFailedReasonName = "總筆數與檔案(磁帶)內的筆數不合",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得製卡失敗原因-查無此資料", ExampleType = ExampleType.Response)]
public class 取得製卡失敗原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetMakeCardFailedReasonByIdResponse>>
{
    public ResultResponse<GetMakeCardFailedReasonByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetMakeCardFailedReasonByIdResponse>(null, "13");
    }
}
