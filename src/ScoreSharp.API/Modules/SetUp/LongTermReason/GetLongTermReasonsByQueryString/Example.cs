namespace ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonsByQueryString;

[ExampleAnnotation(Name = "[2000]取得長循分期戶理由碼", ExampleType = ExampleType.Response)]
public class 取得長循分期戶理由碼_2000_ResEx : IExampleProvider<ResultResponse<List<GetLongTermReasonsByQueryStringResponse>>>
{
    public ResultResponse<List<GetLongTermReasonsByQueryStringResponse>> GetExample()
    {
        List<GetLongTermReasonsByQueryStringResponse> response = new()
        {
            new GetLongTermReasonsByQueryStringResponse
            {
                LongTermReasonCode = "05",
                LongTermReasonName = "大額消費分期付款",
                ReasonStrength = 50,
                IsActive = "Y",
            },
            new GetLongTermReasonsByQueryStringResponse
            {
                LongTermReasonCode = "ED",
                LongTermReasonName = "教育支出",
                ReasonStrength = 52,
                IsActive = "Y",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
