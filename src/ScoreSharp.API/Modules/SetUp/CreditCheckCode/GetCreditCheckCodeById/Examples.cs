namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodeById;

[ExampleAnnotation(Name = "[2000]取得徵信代碼", ExampleType = ExampleType.Response)]
public class 取得徵信代碼_2000_ResEx : IExampleProvider<ResultResponse<GetCreditCheckCodeByIdResponse>>
{
    public ResultResponse<GetCreditCheckCodeByIdResponse> GetExample()
    {
        GetCreditCheckCodeByIdResponse response = new GetCreditCheckCodeByIdResponse
        {
            CreditCheckCode = "A03",
            CreditCheckCodeName = "女性憑ID簡易辦卡",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateUserId = "ADMIN",
            UpdateTime = DateTime.Now,
        };

        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得徵信代碼-查無此資料", ExampleType = ExampleType.Response)]
public class 取得徵信代碼查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetCreditCheckCodeByIdResponse>>
{
    public ResultResponse<GetCreditCheckCodeByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetCreditCheckCodeByIdResponse>(null, "顯示找不到的ID");
    }
}
