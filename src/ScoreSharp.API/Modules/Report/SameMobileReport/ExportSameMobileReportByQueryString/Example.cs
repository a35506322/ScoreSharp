namespace ScoreSharp.API.Modules.Report.SameMobileReport.ExportSameMobileReportByQueryString;

[ExampleAnnotation(Name = "[2000]匯出線上辦卡手機號碼比對相同報表Excel", ExampleType = ExampleType.Response)]
public class 匯出線上辦卡手機號碼比對相同報表Excel_2000_ResEx
    : IExampleProvider<ResultResponse<ExportSameMobileReportByQueryStringResponse>>
{
    public ResultResponse<ExportSameMobileReportByQueryStringResponse> GetExample()
    {
        return ApiResponseHelper.Success<ExportSameMobileReportByQueryStringResponse>(
            new ExportSameMobileReportByQueryStringResponse
            {
                FileName = "線上辦卡手機號碼比對相同報表_202503071519.xlsx",
                FileContent = Encoding.UTF8.GetBytes(
                    "申請書編號,身分證字號,姓名,申請書狀態,公司名稱,手機號碼,OTP手機,推廣單位,推廣人員,比對結果,同手機號碼之申請書編號,同手機號碼之身分證字號,同手機號碼之姓名,同手機號碼之申請書狀態,同手機號碼之公司名稱,同手機號碼之OTP手機,是否異常,確認紀錄,確認人員,確認時間\n"
                        + "20250230X3101,A110025358,孟哲瀚,待月收入預審,聯邦測試公司,0905020225,0969791957,911T,1001234,Y,202502304B4994,A110035356,段立果,待月收入預審,聯邦測試公司,0929132920,N,此紀錄沒問題,arthurlin,2025/03/05 14:28:05"
                ),
            }
        );
    }
}
