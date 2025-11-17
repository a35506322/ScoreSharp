namespace ScoreSharp.API.Modules.Report.SameMobileReport.GetSameMobileReportByQueryString;

[ExampleAnnotation(Name = "[2000]查詢線上辦卡手機號碼比對相同報表", ExampleType = ExampleType.Response)]
public class 查詢線上辦卡手機號碼比對相同報表_2000_ResEx : IExampleProvider<ResultResponse<List<GetSameMobileReportByQueryStringResponse>>>
{
    public ResultResponse<List<GetSameMobileReportByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetSameMobileReportByQueryStringResponse>
        {
            new GetSameMobileReportByQueryStringResponse
            {
                ApplyNo = "20250303X3101",
                ID = "A110035356",
                CHName = "孟哲瀚",
                CardStatus = CardStatus.網路件_待月收入預審,
                Mobile = "0905020755",
                OTPMobile = "0969791957",
                SameWebCaseMobileChecked = "Y",
                SameApplyNo = "20250304B4994",
                SameID = "A110035356",
                SameName = "段立果",
                SameCardStatus = CardStatus.網路件_待月收入預審,
                SameOTPMobile = "0929132920",
                IsError = "N",
                CheckRecord = "此紀錄沒問題",
                UpdateUserId = "arthurlin",
                UpdateTime = DateTime.Now,
            },
            new GetSameMobileReportByQueryStringResponse
            {
                ApplyNo = "20250306X2197",
                ID = "A110035377",
                CHName = "趙燁華",
                CardStatus = CardStatus.網路件_待月收入預審,
                Mobile = "0997797134",
                OTPMobile = "0996600871",
                SameWebCaseMobileChecked = "N",
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
