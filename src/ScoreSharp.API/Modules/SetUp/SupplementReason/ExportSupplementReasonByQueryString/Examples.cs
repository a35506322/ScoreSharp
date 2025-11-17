namespace ScoreSharp.API.Modules.SetUp.SupplementReason.ExportSupplementReasonByQueryString;

[ExampleAnnotation(Name = "[2000]匯出補件原因Excel", ExampleType = ExampleType.Response)]
public class 匯出補件原因Excel_2000_ResEx : IExampleProvider<ResultResponse<ExportSupplementReasonByQueryStringResponse>>
{
    public ResultResponse<ExportSupplementReasonByQueryStringResponse> GetExample()
    {
        return ApiResponseHelper.Success<ExportSupplementReasonByQueryStringResponse>(
            new ExportSupplementReasonByQueryStringResponse
            {
                FileName = "補件原因.xlsx",
                FileContent = Encoding.UTF8.GetBytes("補件代碼,補件名稱,是否啟用\n01,地址證明,Y\n02,收入證明,Y\n03,其他,N"),
            }
        );
    }
}
