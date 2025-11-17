namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForA02.UpdateWebApplyCardCheckJobForA02ByApplyNo;

[ExampleAnnotation(Name = "[2000]更新單筆網路件卡友案件檢核", ExampleType = ExampleType.Request)]
public class 更新單筆網路件卡友案件檢核_2000_ReqEx : IExampleProvider<UpdateWebApplyCardCheckJobForA02ByApplyNoRequest>
{
    public UpdateWebApplyCardCheckJobForA02ByApplyNoRequest GetExample()
    {
        return new UpdateWebApplyCardCheckJobForA02ByApplyNoRequest()
        {
            ApplyNo = "20250716B9211",
            IsQueryOriginalCardholderData = CaseCheckStatus.需檢核_成功,
            QueryOriginalCardholderDataLastTime = DateTime.Parse("2025-07-10T14:05:24.267"),
            IsCheck929 = CaseCheckStatus.需檢核_成功,
            Check929LastTime = DateTime.Parse("2025-07-10T14:01:00.647"),
            IsCheckInternalEmail = CaseCheckStatus.需檢核_成功,
            CheckInternalEmailLastTime = DateTime.Parse("2025-07-10T14:01:00.667"),
            IsCheckInternalMobile = CaseCheckStatus.需檢核_成功,
            CheckInternalMobileLastTime = DateTime.Parse("2025-07-10T14:01:00.657"),
            IsCheckSameIP = CaseCheckStatus.需檢核_成功,
            CheckSameIPLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckEqualInternalIP = CaseCheckStatus.需檢核_成功,
            CheckEqualInternalIPLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckSameWebCaseEmail = CaseCheckStatus.需檢核_成功,
            CheckSameWebCaseEmailLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckSameWebCaseMobile = CaseCheckStatus.需檢核_成功,
            CheckSameWebCaseMobileLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsCheckFocus = CaseCheckStatus.需檢核_成功,
            CheckFocusLastTime = DateTime.Parse("2025-07-10T14:01:00.84"),
            IsCheckShortTimeID = CaseCheckStatus.需檢核_成功,
            CheckShortTimeIDLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsBlackList = CaseCheckStatus.需檢核_成功,
            BlackListLastTime = DateTime.Parse("2025-07-10T14:01:00.62"),
            IsChecked = CaseCheckedStatus.完成,
            ErrorCount = 0,
            IsCheckRepeatApply = CaseCheckStatus.不需檢核,
            CheckRepeatApplyLastTime = null,
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆網路件卡友案件檢核", ExampleType = ExampleType.Response)]
public class 更新單筆網路件卡友案件檢核_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("20250716B9211", "20250716B9211");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆網路件卡友案件檢核-查無資料", ExampleType = ExampleType.Response)]
public class 更新單筆網路件卡友案件檢核查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "20250716B9211");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆網路件卡友案件檢核-呼叫有誤", ExampleType = ExampleType.Response)]
public class 更新單筆網路件卡友案件檢核呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
