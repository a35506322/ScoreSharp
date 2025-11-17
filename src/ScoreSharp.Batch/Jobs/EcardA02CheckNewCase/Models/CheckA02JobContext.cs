namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class CheckA02JobContext
{
    /// <summary>
    /// E-CARD申請書編號
    /// - 對應 E-CARD = APPLY_NO
    /// - IDType = 空白= 金融小白，受理編號中會有 X
    /// - IDType = 存戶與卡友，受理編號中會有 B，如果當B9999就進位為C0001以此類推
    /// - 徵信代碼 = A02
    ///
    ///
    ///
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 是否發查原持卡人
    /// </summary>
    public CaseCheckStatus IsQueryOriginalCardholderData { get; set; }

    /// <summary>
    /// 發查原持卡人最後時間
    /// </summary>
    public DateTime? QueryOriginalCardholderDataLastTime { get; set; }

    /// <summary>
    /// 是否發查929
    /// </summary>
    public CaseCheckStatus IsCheck929 { get; set; }

    /// <summary>
    /// 發查929最後時間
    /// </summary>
    public DateTime? Check929LastTime { get; set; }

    /// <summary>
    /// 是否檢核行內Email
    /// </summary>
    public CaseCheckStatus IsCheckInternalEmail { get; set; }

    /// <summary>
    /// 檢核行內Email最後時間
    /// </summary>
    public DateTime? CheckInternalEmailLastTime { get; set; }

    /// <summary>
    /// 是否發查行內手機重複
    /// </summary>
    public CaseCheckStatus IsCheckInternalMobile { get; set; }

    /// <summary>
    /// 發查行內手機重複最後時間
    /// </summary>
    public DateTime? CheckInternalMobileLastTime { get; set; }

    /// <summary>
    /// 是否檢查相同IP
    /// </summary>
    public CaseCheckStatus IsCheckSameIP { get; set; }

    /// <summary>
    /// 檢查相同IP最 後時間
    /// </summary>
    public DateTime? CheckSameIPLastTime { get; set; }

    /// <summary>
    /// 是否檢查行內IP
    /// </summary>
    public CaseCheckStatus IsCheckEqualInternalIP { get; set; }

    /// <summary>
    /// 檢查行內IP最後時間
    /// </summary>
    public DateTime? CheckEqualInternalIPLastTime { get; set; }

    /// <summary>
    /// 是否檢查相同網路件Email
    /// </summary>
    public CaseCheckStatus IsCheckSameWebCaseEmail { get; set; }

    /// <summary>
    /// 檢查相同網路件Email最後時間
    /// </summary>
    public DateTime? CheckSameWebCaseEmailLastTime { get; set; }

    /// <summary>
    /// 是否檢查相同網路件手機
    /// </summary>
    public CaseCheckStatus IsCheckSameWebCaseMobile { get; set; }

    /// <summary>
    /// 檢查相同網路件手機最後時間
    /// </summary>
    public DateTime? CheckSameWebCaseMobileLastTime { get; set; }

    /// <summary>
    /// 是否發查關注名單
    /// </summary>
    public CaseCheckStatus IsCheckFocus { get; set; }

    /// <summary>
    /// 發查關注名單最後時間
    /// </summary>
    public DateTime? CheckFocusLastTime { get; set; }

    /// <summary>
    /// 是否檢查短時間ID相同
    /// </summary>
    public CaseCheckStatus IsCheckShortTimeID { get; set; }

    /// <summary>
    /// 檢查短時間ID相同最後時間
    /// </summary>
    public DateTime? CheckShortTimeIDLastTime { get; set; }

    /// <summary>
    /// 是否黑名單查詢
    /// </summary>
    public CaseCheckStatus IsBlackList { get; set; }

    /// <summary>
    /// 查黑名單最後時間
    /// </summary>
    public DateTime? BlackListLastTime { get; set; }

    /// <summary>
    /// 是否檢驗完畢
    /// </summary>
    public CaseCheckedStatus IsChecked { get; set; }

    /// <summary>
    /// 錯誤次數
    /// 預設 0
    /// 上述檢核有錯誤記一次
    /// 如果成功則變為0
    ///
    /// </summary>
    public int ErrorCount { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime? AddTime { get; set; }

    public string UserSourceIP { get; set; }
    public DateTime ApplyDate { get; set; }
    public string ID { get; set; }
    public string CHName { get; set; }
    public string EMail { get; set; }
    public string Mobile { get; set; }
    public CardStatus CardStatus { get; set; }
    public UserType UserType { get; set; }
    public string HandleSeqNo { get; set; }

    /// <summary>
    /// 是否檢查重覆進件
    /// </summary>
    public CaseCheckStatus IsCheckRepeatApply { get; set; }

    /// <summary>
    /// 檢查重覆進件最後時間
    /// </summary>
    public DateTime? CheckRepeatApplyLastTime { get; set; }
}
