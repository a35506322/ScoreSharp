namespace ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonById;

[ExampleAnnotation(Name = "[2000]取得長循分期戶理由碼", ExampleType = ExampleType.Response)]
public class 取得長循分期戶理由碼_2000_ResEx : IExampleProvider<ResultResponse<GetLongTermReasonByIdResponse>>
{
    public ResultResponse<GetLongTermReasonByIdResponse> GetExample()
    {
        GetLongTermReasonByIdResponse response = new()
        {
            LongTermReasonCode = "02",
            LongTermReasonName = "結婚和婚禮支出",
            ReasonStrength = 45,
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得長循分期戶理由碼-查無此資料", ExampleType = ExampleType.Response)]
public class 取得長循分期戶理由碼查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "JP");
    }
}
