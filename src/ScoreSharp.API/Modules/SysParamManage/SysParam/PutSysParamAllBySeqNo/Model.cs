namespace ScoreSharp.API.Modules.SysParamManage.SysParam.PutSysParamAllBySeqNo;

public class PutSysParamAllBySeqNoRequest : IValidatableObject
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "PK")]
    [Required]
    public int SeqNo { get; set; }

    /// <summary>
    /// IP比對時間(小時)
    /// </summary>
    [Display(Name = "IP比對時間(小時)")]
    [Required]
    public int IPCompareHour { get; set; }

    /// <summary>
    /// IP比對吻合次數
    /// </summary>
    [Display(Name = "IP比對吻合次數")]
    [Required]
    public int IPMatchCount { get; set; }

    /// <summary>
    /// 查詢多久比對歷史資料
    /// </summary>
    [Display(Name = "查詢多久比對歷史資料")]
    [Required]
    public int QueryHisDataDayRange { get; set; }

    /// <summary>
    /// 網路件相同EMail比對時間(小時)
    /// </summary>
    [Display(Name = "網路件相同EMail比對時間(小時)")]
    [Required]
    public int WebCaseEmailCompareHour { get; set; }

    /// <summary>
    /// 網路件相同EMail比對吻合次數
    /// </summary>
    [Display(Name = "網路件相同EMail比對吻合次數")]
    [Required]
    public int WebCaseEmailMatchCount { get; set; }

    /// <summary>
    /// 網路件相同手機比對時間(小時)
    /// </summary>
    [Display(Name = "網路件相同手機比對時間(小時)")]
    [Required]
    public int WebCaseMobileCompareHour { get; set; }

    /// <summary>
    /// 網路件相同手機比對吻合次數
    /// </summary>
    [Display(Name = "網路件相同手機比對吻合次數")]
    [Required]
    public int WebCaseMobileMatchCount { get; set; }

    /// <summary>
    /// 短時間ID比對時間(小時)
    /// </summary>
    [Display(Name = "短時間ID比對時間(小時)")]
    [Required]
    public int ShortTimeIDCompareHour { get; set; }

    /// <summary>
    /// 短時間ID比對吻合次數
    /// </summary>
    [Display(Name = "短時間ID比對吻合次數")]
    [Required]
    public int ShortTimeIDMatchCount { get; set; }

    /// <summary>
    /// AML職業別版本
    /// </summary>
    [Display(Name = "AML職業別版本")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    [Required]
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
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    [Required]
    public string KYC_StrongReVersion { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (KYCFixStartTime.HasValue && KYCFixEndTime.HasValue && KYCFixStartTime.Value > KYCFixEndTime.Value)
        {
            yield return new ValidationResult("KYC 維護開始時間不能大於 KYC 維護結束時間", new[] { nameof(KYCFixStartTime) });
        }
    }
}
