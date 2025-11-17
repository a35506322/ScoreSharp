namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.SignKYCStrongReview;

[ExampleAnnotation(Name = "[2000]簽核加強審查執行", ExampleType = ExampleType.Request)]
public class 簽核加強審查執行_2000_ReqEx : IExampleProvider<SignKYCStrongReviewRequest>
{
    public SignKYCStrongReviewRequest GetExample()
    {
        SignKYCStrongReviewRequest request = new()
        {
            ApplyNo = "20240803B0001",
            KYC_StrongReStatus = KYCStrongReStatusReq.送審中,
            KYC_Suggestion = "Y",
            KYC_StrongReDetailJson = "{\"KYC_StrongReDetailId\":\"1\",\"KYC_StrongReDetailName\":\"1\"}",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]簽核加強審查執行", ExampleType = ExampleType.Response)]
public class 簽核加強審查執行_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success("20240803B0001", "案件編號 20240803B0001 ，加強審核簽核完成");
    }
}

[ExampleAnnotation(Name = "[4001]簽核加強審查執行-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 簽核加強審查執行_查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "20240803B0001");
    }
}

[ExampleAnnotation(Name = "[4003]簽核加強審查執行-尚未進行KYC入檔", ExampleType = ExampleType.Response)]
public class 簽核加強審查執行_尚未進行KYC入檔_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "案件編號 20240803B0001 ，尚未進行KYC入檔");
    }
}

[ExampleAnnotation(Name = "[4003]簽核加強審查執行-加強審核中狀態未送審與駁回需由當前經辦處理", ExampleType = ExampleType.Response)]
public class 簽核加強審查執行_加強審核中狀態未送審與駁回需由當前經辦處理_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "案件編號 20240803B0001 ，加強審核中狀態未送審與駁回，需由當前經辦處理");
    }
}

[ExampleAnnotation(Name = "[4003]簽核加強審查執行-加強審核中狀態不允許變更", ExampleType = ExampleType.Response)]
public class 簽核加強審查執行_加強審核中狀態不允許變更_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "案件編號 20240803B0001 ，加強審核中狀態不允許變更為「駁回」");
    }
}

[ExampleAnnotation(Name = "[4003]簽核加強審查執行-加強審核中狀態送審中需由當前主管處理", ExampleType = ExampleType.Response)]
public class 簽核加強審查執行_加強審核中狀態送審中需由當前主管處理_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "案件編號 20240803B0001 ，加強審核中狀態送審中，需由當前主管處理");
    }
}
