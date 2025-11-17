namespace ScoreSharp.API.Modules.SetUp.RejectionReason.ExportRejectionReasonByQueryString;

[ExampleAnnotation(Name = "[2000]匯出退件原因Excel", ExampleType = ExampleType.Response)]
public class 匯出退件原因Excel_2000_ResEx : IExampleProvider<ResultResponse<ExportRejectionReasonByQueryStringResponse>>
{
    public ResultResponse<ExportRejectionReasonByQueryStringResponse> GetExample()
    {
        return ApiResponseHelper.Success<ExportRejectionReasonByQueryStringResponse>(
            new ExportRejectionReasonByQueryStringResponse
            {
                FileName = "退件原因.xlsx",
                FileContent = Encoding.UTF8.GetBytes("退件代碼,退件名稱,是否啟用\n01,地址證明,Y\n02,收入證明,Y\n03,其他,N"),
            }
        );
    }
}
