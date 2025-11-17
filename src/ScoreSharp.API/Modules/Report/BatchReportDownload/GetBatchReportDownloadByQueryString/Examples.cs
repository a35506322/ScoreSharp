namespace ScoreSharp.API.Modules.Report.GetBatchReportDownloadByQueryString;

[ExampleAnnotation(Name = "[2000]查詢批次報表下載", ExampleType = ExampleType.Response)]
public class 查詢批次報表下載_2000_ResEx : IExampleProvider<ResultResponse<List<GetBatchReportDownloadByQueryStringResponse>>>
{
    public ResultResponse<List<GetBatchReportDownloadByQueryStringResponse>> GetExample()
    {
        return ApiResponseHelper.Success(
            new List<GetBatchReportDownloadByQueryStringResponse>
            {
                new GetBatchReportDownloadByQueryStringResponse
                {
                    SeqNo = Ulid.NewUlid().ToString(),
                    ReportName = "報表1",
                    ReportType = ReportType.補件函_包含簽名函,
                    ReportFullAddr = "/path/to/report1",
                    AddTime = DateTime.Now,
                    LastDownloadUserId = "user01",
                    LastDownloadUserName = "使用者01",
                },
            }
        );
    }
}
