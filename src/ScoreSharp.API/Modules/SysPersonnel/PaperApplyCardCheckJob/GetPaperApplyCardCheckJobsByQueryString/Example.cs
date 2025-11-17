namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.GetPaperApplyCardCheckJobsByQueryString;

public class 查詢多筆紙本件案件檢核_2000_ResEx : IExampleProvider<ResultResponse<List<GetPaperApplyCardCheckJobsByQueryStringResponse>>>
{
    public ResultResponse<List<GetPaperApplyCardCheckJobsByQueryStringResponse>> GetExample()
    {
        var response = new ResultResponse<List<GetPaperApplyCardCheckJobsByQueryStringResponse>>()
        {
            ReturnCodeStatus = ReturnCodeStatus.成功,
            ReturnMessage = "",
            ReturnData = new List<GetPaperApplyCardCheckJobsByQueryStringResponse>()
            {
                new GetPaperApplyCardCheckJobsByQueryStringResponse()
                {
                    ApplyNo = "20250709770001",
                    IsQueryOriginalCardholderData = CaseCheckStatus.需檢核_成功,
                    QueryOriginalCardholderDataLastTime = DateTime.Parse("2025-07-09T16:19:12.51"),
                    IsCheckName = CaseCheckStatus.需檢核_成功,
                    CheckNameLastTime = DateTime.Parse("2025-07-09T16:19:13.273"),
                    IsCheck929 = CaseCheckStatus.需檢核_成功,
                    Check929LastTime = DateTime.Parse("2025-07-09T16:19:12.577"),
                    IsQueryBranchInfo = CaseCheckStatus.需檢核_成功,
                    QueryBranchInfoLastTime = DateTime.Parse("2025-07-09T16:19:12.587"),
                    IsCheckFocus = CaseCheckStatus.需檢核_成功,
                    CheckFocusLastTime = DateTime.Parse("2025-07-09T16:19:13.203"),
                    IsCheckShortTimeID = CaseCheckStatus.需檢核_成功,
                    CheckShortTimeIDLastTime = DateTime.Parse("2025-07-09T16:19:12.563"),
                    IsCheckInternalEmail = CaseCheckStatus.需檢核_成功,
                    CheckInternalEmailLastTime = DateTime.Parse("2025-07-09T16:19:12.547"),
                    IsCheckInternalMobile = CaseCheckStatus.需檢核_成功,
                    CheckInternalMobileLastTime = DateTime.Parse("2025-07-09T16:19:12.56"),
                    IsChecked = CaseCheckedStatus.完成,
                    ErrorCount = 0,
                    AddTime = DateTime.Parse("2025-07-09T16:18:07.673"),
                    IsCheckRepeatApply = CaseCheckStatus.需檢核_成功,
                    CheckRepeatApplyLastTime = DateTime.Parse("2025-07-09T16:19:12.51"),
                },
            },
        };
        return response;
    }
}
