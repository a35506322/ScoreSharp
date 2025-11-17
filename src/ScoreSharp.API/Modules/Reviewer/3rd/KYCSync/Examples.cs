namespace ScoreSharp.API.Modules.Reviewer3rd.KYCSync;

[ExampleAnnotation(Name = "[2000]入檔KYC成功", ExampleType = ExampleType.Response)]
public class 入檔KYC成功_2000_ResEx : IExampleProvider<ResultResponse<KYCSyncResponse>>
{
    public ResultResponse<KYCSyncResponse> GetExample()
    {
        KYCSyncResponse response = new()
        {
            ApplyNo = "20250804B0453",
            KYC_RtnCode = "K0000",
            KYC_Message = "",
            KYC_RiskLevel = "L",
            KYC_QueryTime = DateTime.Now,
            KYC_Exception = "",
        };

        return ApiResponseHelper.Success<KYCSyncResponse>(response, "KYC入檔");
    }
}

[ExampleAnnotation(Name = "[4001]查無資料", ExampleType = ExampleType.Response)]
public class 查無資料_4001_ResEx : IExampleProvider<ResultResponse<KYCSyncResponse>>
{
    public ResultResponse<KYCSyncResponse> GetExample() => ApiResponseHelper.NotFound<KYCSyncResponse>(null, "20250804B0453");
}

[ExampleAnnotation(Name = "[4003]商業邏輯驗證失敗_系統維護", ExampleType = ExampleType.Response)]
public class 系統維護_4003_ResEx : IExampleProvider<ResultResponse<KYCSyncResponse>>
{
    public ResultResponse<KYCSyncResponse> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<KYCSyncResponse>(null, "入檔KYC系統維護中，請稍後再試。");
}

[ExampleAnnotation(Name = "[5002]發查第三方API失敗", ExampleType = ExampleType.Response)]
public class 發查第三方API失敗_5002_ResEx : IExampleProvider<ResultResponse<KYCSyncResponse>>
{
    public ResultResponse<KYCSyncResponse> GetExample()
    {
        KYCSyncResponse response = new()
        {
            ApplyNo = "20250804B0453",
            KYC_RtnCode = "KD003",
            KYC_Message = "cIAMFlag/主要所得及資金來源代碼有誤,",
            KYC_RiskLevel = "",
            KYC_QueryTime = DateTime.Now,
            KYC_Exception = "",
        };
        return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName<KYCSyncResponse>(data: response, checkThirdPartyApiName: "入檔KYC");
    }
}
