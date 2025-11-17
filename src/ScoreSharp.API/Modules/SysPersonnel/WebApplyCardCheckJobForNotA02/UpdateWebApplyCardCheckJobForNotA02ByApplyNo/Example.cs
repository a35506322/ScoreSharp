namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02.UpdateWebApplyCardCheckJobForNotA02ByApplyNo;

[ExampleAnnotation(Name = "[2000]更新單筆網路件非卡友案件檢核", ExampleType = ExampleType.Request)]
public class 更新單筆網路件非卡友案件檢核_2000_ReqEx : IExampleProvider<UpdateWebApplyCardCheckJobForNotA02ByApplyNoRequest>
{
    public UpdateWebApplyCardCheckJobForNotA02ByApplyNoRequest GetExample()
    {
        return new UpdateWebApplyCardCheckJobForNotA02ByApplyNoRequest()
        {
            ApplyNo = "20250710B1124",
            IsCheckName = CaseCheckStatus.需檢核_成功,
            CheckNameLastTime = DateTime.Parse("2025-07-10T14:05:24.267"),
            IsQueryBranchInfo = CaseCheckStatus.需檢核_成功,
            QueryBranchInfoLastTime = DateTime.Parse("2025-07-10T14:01:00.673"),
            IsCheck929 = CaseCheckStatus.需檢核_成功,
            Check929LastTime = DateTime.Parse("2025-07-10T14:01:00.647"),
            IsCheckSameIP = CaseCheckStatus.需檢核_成功,
            CheckSameIPLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckEqualInternalIP = CaseCheckStatus.需檢核_成功,
            CheckEqualInternalIPLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsChecked = CaseCheckedStatus.完成,
            ErrorCount = 0,
            IsCheckSameWebCaseEmail = CaseCheckStatus.需檢核_成功,
            CheckSameWebCaseEmailLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckSameWebCaseMobile = CaseCheckStatus.需檢核_成功,
            CheckSameWebCaseMobileLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckFocus = CaseCheckStatus.需檢核_成功,
            CheckFocusLastTime = DateTime.Parse("2025-07-10T14:01:00.84"),
            IsCheckShortTimeID = CaseCheckStatus.需檢核_成功,
            CheckShortTimeIDLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckInternalEmail = CaseCheckStatus.需檢核_成功,
            CheckInternalEmailLastTime = DateTime.Parse("2025-07-10T14:01:00.667"),
            IsCheckInternalMobile = CaseCheckStatus.需檢核_成功,
            CheckInternalMobileLastTime = DateTime.Parse("2025-07-10T14:01:00.657"),
            IsCheckRepeatApply = CaseCheckStatus.需檢核_成功,
            CheckRepeatApplyLastTime = DateTime.Parse("2025-07-10T14:01:00.657"),
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆網路件非卡友案件檢核", ExampleType = ExampleType.Response)]
public class 更新單筆網路件非卡友案件檢核_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("20250710B1124", "20250710B1124");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆網路件非卡友案件檢核-查無資料", ExampleType = ExampleType.Response)]
public class 更新單筆網路件非卡友案件檢核查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "20250710B1124");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆網路件非卡友案件檢核-呼叫有誤", ExampleType = ExampleType.Response)]
public class 更新單筆網路件非卡友案件檢核呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
