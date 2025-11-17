namespace ScoreSharp.API.Modules.Manage.CaseStatistics.GetCaseStatisticsByQueryString;

[ExampleAnnotation(Name = "[2000]取得派案統計並匯出-查詢資料", ExampleType = ExampleType.Request)]
public class 取得派案統計並匯出_查詢資料_2000_ReqEx : IExampleProvider<GetCaseStatisticsByQueryStringRequest>
{
    public GetCaseStatisticsByQueryStringRequest GetExample() =>
        new GetCaseStatisticsByQueryStringRequest
        {
            UserId = "arthurlin",
            Addtime = new DateTime(2025, 9, 3),
            Type = "Query",
        };
}

[ExampleAnnotation(Name = "[2000]取得派案統計並匯出-匯出資料", ExampleType = ExampleType.Request)]
public class 取得派案統計並匯出_匯出資料_2000_ReqEx : IExampleProvider<GetCaseStatisticsByQueryStringRequest>
{
    public GetCaseStatisticsByQueryStringRequest GetExample() => new GetCaseStatisticsByQueryStringRequest { Type = "Export" };
}

[ExampleAnnotation(Name = "[2000]取得派案統計並匯出-查無資料", ExampleType = ExampleType.Response)]
public class 取得派案統計並匯出_查無資料_2000_ResEx : IExampleProvider<ResultResponse<GetCaseStatisticsByQueryStringResponse>>
{
    public ResultResponse<GetCaseStatisticsByQueryStringResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetCaseStatisticsByQueryStringResponse
            {
                QueryData = [],
                ExportData = null,
                Type = "Query",
            }
        );
    }
}

[ExampleAnnotation(Name = "[2000]取得派案統計並匯出-查詢成功", ExampleType = ExampleType.Response)]
public class 取得派案統計並匯出_查詢成功_2000_ResEx : IExampleProvider<ResultResponse<GetCaseStatisticsByQueryStringResponse>>
{
    public ResultResponse<GetCaseStatisticsByQueryStringResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetCaseStatisticsByQueryStringResponse
            {
                QueryData =
                [
                    new GetCaseStatisticsDto
                    {
                        UserId = "arthurlin",
                        UserName = "林芃均",
                        AddTime = new DateTime(2025, 9, 3, 10, 0, 0),
                        CaseType = "系統派案",
                        ApplyNo = "20250903B4108",
                    },
                ],
                ExportData = null,
                Type = "Query",
            }
        );
    }
}

[ExampleAnnotation(Name = "[2000]取得派案統計並匯出-匯出成功", ExampleType = ExampleType.Response)]
public class 取得派案統計並匯出_匯出成功_2000_ResEx : IExampleProvider<ResultResponse<GetCaseStatisticsByQueryStringResponse>>
{
    public ResultResponse<GetCaseStatisticsByQueryStringResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetCaseStatisticsByQueryStringResponse
            {
                QueryData = [],
                ExportData = new ExportGetCaseStatisticsToExcelDto
                {
                    FileName = "查詢派案統計.xlsx",
                    FileContent = Encoding.UTF8.GetBytes(
                        "員工編號,員工姓名,派案日期,派案類型,申請書編號\n" + "arthurlin,林芃均,2025-09-03 10:00:00,系統派案,20250903B4108,"
                    ),
                },
                Type = "Query",
            }
        );
    }
}
