namespace ScoreSharp.API.Modules.Report.BatchReportDownload.GetReportFileBySeqNo;

[ExampleAnnotation(Name = "[2000]下載報表-成功", ExampleType = ExampleType.Response)]
public class 下載報表成功_2000_ResEx : IExampleProvider<ResultResponse<GetReportFileBySeqNoResponse>>
{
    public ResultResponse<GetReportFileBySeqNoResponse> GetExample()
    {
        var fileContent = new byte[]
        { /* example file content */
        };
        return ApiResponseHelper.Success(
            new GetReportFileBySeqNoResponse
            {
                FileContent = fileContent,
                FileName = "example.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            },
            "下載報表成功"
        );
    }
}

[ExampleAnnotation(Name = "[4001]下載報表-查無此資料", ExampleType = ExampleType.Response)]
public class 下載報表查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetReportFileBySeqNoResponse>>
{
    public ResultResponse<GetReportFileBySeqNoResponse> GetExample() =>
        ApiResponseHelper.NotFound<GetReportFileBySeqNoResponse>(null, "01HRHK6NMVT1234567890");
}
