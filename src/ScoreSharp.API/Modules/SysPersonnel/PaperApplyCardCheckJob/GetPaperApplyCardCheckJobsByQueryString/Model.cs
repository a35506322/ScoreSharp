namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.GetPaperApplyCardCheckJobsByQueryString;

public class GetPaperApplyCardCheckJobsByQueryStringRequest
{
    /// <summary>
    /// 起始日期 (區間查詢)
    /// </summary>
    [Display(Name = "起始日期")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 結束日期 (區間查詢)
    /// </summary>
    [Display(Name = "結束日期")]
    public DateTime EndDate { get; set; }
}

public class GetPaperApplyCardCheckJobsByQueryStringResponse
{
    /// <summary>
    /// 申請書編號
    /// YYYYMMDD+1-2碼數字為信用卡+4碼流水號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 是否查詢原持卡人
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsQueryOriginalCardholderData { get; set; }

    /// <summary>
    /// 是否查詢原持卡人名稱
    /// </summary>
    public string IsQueryOriginalCardholderDataName => this.IsQueryOriginalCardholderData.ToString();

    /// <summary>
    /// 查詢原持卡人最後時間
    /// </summary>
    public DateTime? QueryOriginalCardholderDataLastTime { get; set; }

    /// <summary>
    /// 是否發查敦陽姓名檢核
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsCheckName { get; set; }

    /// <summary>
    /// 是否發查敦陽姓名檢核名稱
    /// </summary>
    public string IsCheckNameName => this.IsCheckName.ToString();

    /// <summary>
    /// 查敦陽姓名檢核最後時間
    /// </summary>
    public DateTime? CheckNameLastTime { get; set; }

    /// <summary>
    /// 是否發查929
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsCheck929 { get; set; }

    /// <summary>
    /// 是否發查929名稱
    /// </summary>
    public string IsCheck929Name => this.IsCheck929.ToString();

    /// <summary>
    /// 查敦陽姓名檢核最後時間
    /// </summary>
    public DateTime? Check929LastTime { get; set; }

    /// <summary>
    /// 是否查詢分行資訊
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsQueryBranchInfo { get; set; }

    /// <summary>
    /// 是否查詢分行資訊名稱
    /// </summary>
    public string IsQueryBranchInfoName => this.IsQueryBranchInfo.ToString();

    /// <summary>
    /// 查詢分行資訊最後時間
    /// </summary>
    public DateTime? QueryBranchInfoLastTime { get; set; }

    /// <summary>
    /// 是否發查關注名單
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsCheckFocus { get; set; }

    /// <summary>
    /// 是否發查關注名單名稱
    /// </summary>
    public string IsCheckFocusName => this.IsCheckFocus.ToString();

    /// <summary>
    /// 發查關注名單最後時間
    /// </summary>
    public DateTime? CheckFocusLastTime { get; set; }

    /// <summary>
    /// 是否檢查短時間ID相同
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsCheckShortTimeID { get; set; }

    /// <summary>
    /// 是否檢查短時間ID相同名稱
    /// </summary>
    public string IsCheckShortTimeIDName => this.IsCheckShortTimeID.ToString();

    /// <summary>
    /// 檢查短時間ID相同最後時間
    /// </summary>
    public DateTime? CheckShortTimeIDLastTime { get; set; }

    /// <summary>
    /// 是否發查行內Email重複
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsCheckInternalEmail { get; set; }

    public string IsCheckInternalEmailName => this.IsCheckInternalEmail.ToString();

    /// <summary>
    /// 發查行內Email重複最後時間
    /// </summary>
    public DateTime? CheckInternalEmailLastTime { get; set; }

    /// <summary>
    /// 是否發查行內手機重複
    /// 需檢核_未完成 = 1
    /// 需檢核_成功 = 2
    /// 需檢核_失敗 = 3
    /// 不需檢核 = 4
    /// </summary>
    public CaseCheckStatus IsCheckInternalMobile { get; set; }

    /// <summary>
    /// 是否發查行內手機重複名稱
    /// </summary>
    public string IsCheckInternalMobileName => this.IsCheckInternalMobile.ToString();

    /// <summary>
    /// 發查行內手機重複最後時間
    /// </summary>
    public DateTime? CheckInternalMobileLastTime { get; set; }

    /// <summary>
    /// 是否檢驗完畢
    /// 完成 = 1
    /// 未完成 = 2
    /// </summary>
    public CaseCheckedStatus IsChecked { get; set; }

    /// <summary>
    /// 是否檢驗完畢名稱
    /// </summary>
    public string IsCheckedName => this.IsChecked.ToString();

    /// <summary>
    /// 錯誤次數
    /// 預設 0
    /// 上述檢核有錯誤記一次
    /// 如果成功則變為0
    /// </summary>
    public int ErrorCount { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 是否檢查重覆進件
    /// </summary>
    public CaseCheckStatus IsCheckRepeatApply { get; set; }

    /// <summary>
    /// 檢查重覆進件最後時間
    /// </summary>
    public DateTime? CheckRepeatApplyLastTime { get; set; }

    /// <summary>
    /// 是否檢查重覆進件名稱
    /// </summary>
    public string IsCheckRepeatApplyName => this.IsCheckRepeatApply.ToString();
}
