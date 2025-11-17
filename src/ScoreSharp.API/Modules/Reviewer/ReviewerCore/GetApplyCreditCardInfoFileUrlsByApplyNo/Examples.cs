namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoFileUrlsByApplyNo;

[ExampleAnnotation(Name = "[2000]取得申請書_附件URL", ExampleType = ExampleType.Response)]
public class 取得申請書_附件URL_2000_ResEx : IExampleProvider<ResultResponse<GetApplyCreditCardInfoFileUrlsByApplyNoResponse>>
{
    public ResultResponse<GetApplyCreditCardInfoFileUrlsByApplyNoResponse> GetExample() =>
        ApiResponseHelper.Success(
            new GetApplyCreditCardInfoFileUrlsByApplyNoResponse
            {
                FileUrls = new List<string>
                {
                    "/GetApplyCreditCardInfoFileByApplyNoAndFileId/20241127X8342/F05A94A1-9FE4-4B2D-B002-96AAC9AA3923",
                    "/GetApplyCreditCardInfoFileByApplyNoAndFileId/20241127X8342/F05A94A1-9FE4-4B2D-B002-96AAC9AA3924",
                },
            },
            "取得申請書附件URL成功"
        );
};
