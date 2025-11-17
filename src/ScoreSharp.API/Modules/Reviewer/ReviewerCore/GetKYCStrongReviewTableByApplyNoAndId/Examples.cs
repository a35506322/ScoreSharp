namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetKYCStrongReviewTableByApplyNoAndId;

[ExampleAnnotation(Name = "[4001]取得KYC加強審核表格-查無此申請書編號及ID", ExampleType = ExampleType.Response)]
public class 取得KYC加強審核表格_查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse>>
{
    public ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetKYCStrongReviewTableByApplyNoAndIdResponse>(null, "20240803B0001-A123456789");
    }
}

[ExampleAnnotation(Name = "[4003]取得KYC加強審核表格-未產生KYC加強審核表格", ExampleType = ExampleType.Response)]
public class 取得KYC加強審核表格_未產生KYC加強審核表格_4003_ResEx : IExampleProvider<ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse>>
{
    public ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<GetKYCStrongReviewTableByApplyNoAndIdResponse>(
            null,
            "此申請書編號 20240803B0001及ID A123456789，未產生KYC加強審核表格"
        );
    }
}

[ExampleAnnotation(Name = "[2000]取得KYC加強審核表格-成功", ExampleType = ExampleType.Response)]
public class 取得KYC加強審核表格_成功_2000_ResEx : IExampleProvider<ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse>>
{
    public ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse> GetExample()
    {
        var response = new GetKYCStrongReviewTableByApplyNoAndIdResponse
        {
            ApplyNo = "20240803B0001",
            ID = "A123456789",
            KYCStrongReviewTable = new byte[] { },
            FileName = "KYC加強審核執行表_20240803B0001_A123456789_20240803123456.pdf",
            ContentType = "application/pdf",
        };
        return ApiResponseHelper.Success(response, "取得KYC加強審核表格成功");
    }
}
