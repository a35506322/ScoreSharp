namespace ScoreSharp.API.Modules.SysParamManage.SysParam.GetSysParamAll;

public class GetSysParamAllResponse
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "PK")]
    public int SeqNo { get; set; }

    /// <summary>
    /// IP比對時間(小時)
    /// </summary>
    [Display(Name = "IP比對時間(小時)")]
    public int IPCompareHour { get; set; }

    /// <summary>
    /// IP比對吻合次數
    /// </summary>
    [Display(Name = "IP比對吻合次數")]
    public int IPMatchCount { get; set; }

    /// <summary>
    /// 查詢多久比對歷史資料
    /// </summary>
    [Display(Name = "查詢多久比對歷史資料")]
    public int QueryHisDataDayRange { get; set; }

    /// <summary>
    /// 網路件相同EMail比對時間(小時)
    /// </summary>
    [Display(Name = "網路件相同EMail比對時間(小時)")]
    public int WebCaseEmailCompareHour { get; set; }

    /// <summary>
    /// 網路件相同EMail比對吻合次數
    /// </summary>
    [Display(Name = "網路件相同EMail比對吻合次數")]
    public int WebCaseEmailMatchCount { get; set; }

    /// <summary>
    /// 網路件相同手機比對時間(小時)
    /// </summary>
    [Display(Name = "網路件相同手機比對時間(小時)")]
    public int WebCaseMobileCompareHour { get; set; }

    /// <summary>
    /// 網路件相同手機比對吻合次數
    /// </summary>
    [Display(Name = "網路件相同手機比對吻合次數")]
    public int WebCaseMobileMatchCount { get; set; }

    /// <summary>
    /// 短時間ID比對時間(小時)
    /// </summary>
    [Display(Name = "短時間ID比對時間(小時)")]
    public int ShortTimeIDCompareHour { get; set; }

    /// <summary>
    /// 短時間ID比對吻合次數
    /// </summary>
    [Display(Name = "短時間ID比對吻合次數")]
    public int ShortTimeIDMatchCount { get; set; }

    /// <summary>
    /// 國旅卡案件系統撤件天數
    /// </summary>
    [Display(Name = "國旅卡案件系統撤件天數")]
    public int GuoLuKaCaseWithdrawDays { get; set; }

    /// <summary>
    /// 國旅卡客戶檢核排程檢核案件數
    /// </summary>
    [Display(Name = "國旅卡客戶檢核排程檢核案件數")]
    public int GuoLuKaCheckBatchCaseCount { get; set; }

    /// <summary>
    /// AML職業別版本
    /// </summary>
    [Display(Name = "AML職業別版本")]
    public string AMLProfessionCode_Version { get; set; }

    /// <summary>
    /// KYC 維護開始時間
    /// </summary>
    [Display(Name = "KYC 維護開始時間")]
    public DateTime? KYCFixStartTime { get; set; }

    /// <summary>
    /// KYC 維護結束時間
    /// </summary>
    [Display(Name = "KYC 維護結束時間")]
    public DateTime? KYCFixEndTime { get; set; }

    /// <summary>
    /// KYC加強審核版本
    /// </summary>
    [Display(Name = "KYC加強審核版本")]
    public string KYC_StrongReVersion { get; set; }
}
