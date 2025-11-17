namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.UpdatePaperApplyCardCheckJobByApplyNo;

[ExampleAnnotation(Name = "[2000]更新單筆網路件卡友案件檢核", ExampleType = ExampleType.Request)]
public class 更新單筆紙本件案件檢核_2000_ReqEx : IExampleProvider<UpdatePaperApplyCardCheckJobByApplyNoRequest>
{
    public UpdatePaperApplyCardCheckJobByApplyNoRequest GetExample()
    {
        return new UpdatePaperApplyCardCheckJobByApplyNoRequest
        {
            ApplyNo = "20250709770001",
            IsQueryOriginalCardholderData = CaseCheckStatus.需檢核_成功,
            QueryOriginalCardholderDataLastTime = DateTime.Parse("2025-07-09 16:19:12.510"),
            IsCheckName = CaseCheckStatus.需檢核_成功,
            CheckNameLastTime = DateTime.Parse("2025-07-09 16:19:13.273"),
            IsCheck929 = CaseCheckStatus.需檢核_成功,
            Check929LastTime = DateTime.Parse("2025-07-09 16:19:12.577"),
            IsQueryBranchInfo = CaseCheckStatus.需檢核_成功,
            QueryBranchInfoLastTime = DateTime.Parse("2025-07-09 16:19:12.587"),
            IsCheckFocus = CaseCheckStatus.需檢核_成功,
            CheckFocusLastTime = DateTime.Parse("2025-07-09 16:19:13.203"),
            IsCheckShortTimeID = CaseCheckStatus.需檢核_成功,
            CheckShortTimeIDLastTime = DateTime.Parse("2025-07-09 16:19:12.563"),
            IsCheckInternalEmail = CaseCheckStatus.需檢核_成功,
            CheckInternalEmailLastTime = DateTime.Parse("2025-07-09 16:19:12.547"),
            IsCheckInternalMobile = CaseCheckStatus.需檢核_成功,
            CheckInternalMobileLastTime = DateTime.Parse("2025-07-09 16:19:12.560"),
            IsChecked = CaseCheckedStatus.完成,
            ErrorCount = 0,
            IsCheckRepeatApply = CaseCheckStatus.需檢核_成功,
            CheckRepeatApplyLastTime = DateTime.Parse("2025-07-09 16:19:12.51"),
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆紙本件案件檢核", ExampleType = ExampleType.Response)]
public class 更新單筆紙本件案件檢核_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("20250709770001", "20250709770001");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆紙本件案件檢核-查無資料", ExampleType = ExampleType.Response)]
public class 更新單筆紙本件案件檢核查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "20250709770001");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆紙本件案件檢核-呼叫有誤", ExampleType = ExampleType.Response)]
public class 更新單筆紙本件案件檢核呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
