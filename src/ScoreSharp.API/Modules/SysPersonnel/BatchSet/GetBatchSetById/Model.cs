namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet.GetBatchSetById;

public class GetBatchSetByIdResponse
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "序號")]
    public int SeqNo { get; set; }

    /// <summary>
    /// 網路件申請書檔案異常重新抓取_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "網路件申請書檔案異常重新抓取_是否啟動")]
    public string RetryWebCaseFileErrorJob_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 網路件申請書檔案異常重新抓取_執行數量
    /// </summary>
    [Display(Name = "網路件申請書檔案異常重新抓取_執行數量")]
    public int RetryWebCaseFileErrorJob_BatchSize { get; set; }

    /// <summary>
    /// A02KYC同步排程_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "A02KYC同步排程_是否啟動")]
    public string A02KYCSyncJob_IsEnabled { get; set; } = null!;

    /// <summary>
    /// A02KYC同步排程_執行數量
    /// </summary>
    [Display(Name = "A02KYC同步排程_執行數量")]
    public int A02KYCSyncJob_BatchSize { get; set; }

    /// <summary>
    /// 每2小時比對漏進案件批次_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "每2小時比對漏進案件批次_是否啟動")]
    public string CompareMissingCases_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 網路進件卡友檢核新案件_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "網路進件卡友檢核新案件_是否啟動")]
    public string EcardA02CheckNewCase_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 網路進件卡友檢核新案件_執行數量
    /// </summary>
    [Display(Name = "網路進件卡友檢核新案件_執行數量")]
    public int EcardA02CheckNewCase_BatchSize { get; set; }

    /// <summary>
    /// 網路進件非卡友檢核新案件_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "網路進件非卡友檢核新案件_是否啟動")]
    public string EcardNotA02CheckNewCase_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 網路進件非卡友檢核新案件_執行數量
    /// </summary>
    [Display(Name = "網路進件非卡友檢核新案件_執行數量")]
    public int EcardNotA02CheckNewCase_BatchSize { get; set; }

    /// <summary>
    /// 國旅卡人士資料檢核排程_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "國旅卡人士資料檢核排程_是否啟動")]
    public string GuoLuKaCheck_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 國旅卡人士資料檢核排程_執行數量
    /// </summary>
    [Display(Name = "國旅卡人士資料檢核排程_執行數量")]
    public int GuoLuKaCheck_BatchSize { get; set; }

    /// <summary>
    /// 國旅卡人士資料檢核排程_國旅卡撤件天數
    /// </summary>
    [Display(Name = "國旅卡人士資料檢核排程_國旅卡撤件天數")]
    public int GuoLuKaCaseWithdrawDays { get; set; }

    /// <summary>
    /// 紙本件檢核新案件排程_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "紙本件檢核新案件排程_是否啟動")]
    public string PaperCheckNewCase_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 紙本件檢核新案件排程_查詢數量
    /// </summary>
    [Display(Name = "紙本件檢核新案件排程_查詢數量")]
    public int PaperCheckNewCase_BatchSize { get; set; }

    /// <summary>
    /// 重試KYC入檔作業排程_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "重試KYC入檔作業排程_是否啟動")]
    public string RetryKYCSync_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 重試KYC入檔作業排程_執行數量
    /// </summary>
    [Display(Name = "重試KYC入檔作業排程_執行數量")]
    public int RetryKYCSync_BatchSize { get; set; }

    /// <summary>
    /// 寄信KYC錯誤_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "寄信KYC錯誤_是否啟動")]
    public string SendKYCErrorLog_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 寄信KYC錯誤_寄信數量
    /// </summary>
    [Display(Name = "寄信KYC錯誤_寄信數量")]
    public int SendKYCErrorLog_BatchSize { get; set; }

    /// <summary>
    /// 寄信系統錯誤_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "寄信系統錯誤_是否啟動")]
    public string SendSystemErrorLog_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 寄信系統錯誤_寄信數量
    /// </summary>
    [Display(Name = "寄信系統錯誤_寄信數量")]
    public int SendSystemErrorLog_BatchSize { get; set; }

    /// <summary>
    /// 報表作業補件函批次_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "報表作業補件函批次_是否啟動")]
    public string SupplementTemplateReport_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 系統派案_網路件待月收入預審_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "系統派案_網路件待月收入預審_是否啟動")]
    public string SystemAssignment_WebCase_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 系統派案_紙本件待月收入預審_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "系統派案_紙本件待月收入預審_是否啟動")]
    public string SystemAssignment_Paper_IsEnabled { get; set; } = null!;

    /// <summary>
    /// 系統派案人工徵信中_是否啟動
    /// Y / N
    /// </summary>
    [Display(Name = "系統派案人工徵信中_是否啟動")]
    public string SystemAssignment_ReviewManual_IsEnabled { get; set; } = null!;
}
