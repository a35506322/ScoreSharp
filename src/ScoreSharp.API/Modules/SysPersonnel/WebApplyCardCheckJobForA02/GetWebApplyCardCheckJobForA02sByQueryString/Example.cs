namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForA02.GetWebApplyCardCheckJobForA02sByQueryString;

public class 查詢多筆網路件卡友案件檢核_2000_ResEx : IExampleProvider<ResultResponse<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>>>
{
    public ResultResponse<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\":2000,\"returnMessage\":\"\",\"returnData\":[{\"applyNo\":\"20250716B9211\",\"isQueryOriginalCardholderData\":1,\"isQueryOriginalCardholderDataName\":\"需檢核_未完成\",\"queryOriginalCardholderDataLastTime\":null,\"isCheck929\":1,\"isCheck929Name\":\"需檢核_未完成\",\"check929LastTime\":null,\"isCheckInternalEmail\":1,\"isCheckInternalEmailName\":\"需檢核_未完成\",\"checkInternalEmailLastTime\":null,\"isCheckInternalMobile\":1,\"isCheckInternalMobileName\":\"需檢核_未完成\",\"checkInternalMobileLastTime\":null,\"isCheckSameIP\":1,\"isCheckSameIPName\":\"需檢核_未完成\",\"checkSameIPLastTime\":null,\"isCheckEqualInternalIP\":1,\"isCheckEqualInternalIPName\":\"需檢核_未完成\",\"checkEqualInternalIPLastTime\":null,\"isCheckSameWebCaseEmail\":1,\"isCheckSameWebCaseEmailName\":\"需檢核_未完成\",\"checkSameWebCaseEmailLastTime\":null,\"isCheckSameWebCaseMobile\":1,\"isCheckSameWebCaseMobileName\":\"需檢核_未完成\",\"checkSameWebCaseMobileLastTime\":null,\"isCheckFocus\":1,\"isCheckFocusName\":\"需檢核_未完成\",\"checkFocusLastTime\":null,\"isCheckShortTimeID\":1,\"isCheckShortTimeIDName\":\"需檢核_未完成\",\"checkShortTimeIDLastTime\":null,\"isBlackList\":1,\"isBlackListName\":\"需檢核_未完成\",\"blackListLastTime\":null,\"isChecked\":2,\"isCheckedName\":\"未完成\",\"errorCount\":0,\"addTime\":\"2025-07-16T15:38:10.763\",\"isCheckRepeatApply\":1,\"isCheckRepeatApplyName\":\"需檢核_未完成\",\"checkRepeatApplyLastTime\":null}]}";

        var response = JsonHelper.反序列化物件不分大小寫<ResultResponse<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>>>(jsonString);
        return response;
    }
}
