namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodesByQueryString;

[ExampleAnnotation(Name = "[2000]取得徵信代碼", ExampleType = ExampleType.Response)]
public class 取得徵信代碼_2000_ResEx : IExampleProvider<ResultResponse<List<GetCreditCheckCodesByQueryStringResponse>>>
{
    public ResultResponse<List<GetCreditCheckCodesByQueryStringResponse>> GetExample()
    {
        var data = new List<GetCreditCheckCodesByQueryStringResponse>()
        {
            new GetCreditCheckCodesByQueryStringResponse
            {
                CreditCheckCode = "A01",
                CreditCheckCodeName = "聯徵辦卡",
                IsActive = "Y",
            },
            new GetCreditCheckCodesByQueryStringResponse
            {
                CreditCheckCode = "A03",
                CreditCheckCodeName = "女性憑ID簡易辦卡",
                IsActive = "Y",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
