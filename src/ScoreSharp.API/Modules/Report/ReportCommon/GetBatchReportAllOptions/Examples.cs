namespace ScoreSharp.API.Modules.Report.ReportCommon.GetBatchReportAllOptions;

[ExampleAnnotation(Name = "[2000]取得全部報表相關下拉選單", ExampleType = ExampleType.Response)]
public class 取得全部報表相關下拉選單_2000_ResEx : IExampleProvider<ResultResponse<GetBatchReportAllOptionsResponse>>
{
    public ResultResponse<GetBatchReportAllOptionsResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetBatchReportAllOptionsResponse
            {
                ReportType = new List<OptionsDtoTypeInt>
                {
                    new OptionsDtoTypeInt
                    {
                        Name = "信用卡申請進度查詢",
                        Value = 1,
                        IsActive = "Y",
                    },
                    new OptionsDtoTypeInt
                    {
                        Name = "信用卡申請進度明細",
                        Value = 2,
                        IsActive = "Y",
                    },
                },
            }
        );
    }
}
