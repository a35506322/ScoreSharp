namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.InsertReviewerPermission;

public class InsertReviewerPermissionRequest
{
    /// <summary>
    /// 案件狀態
    /// PK
    /// </summary>
    [Display(Name = "案件狀態")]
    [Required]
    [ValidEnumValue]
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 月收入確認_是否顯示變更案件種類
    /// Y / N
    /// </summary>
    [Display(Name = "月收入確認_是否顯示變更案件種類")]
    [Required]
    [RegularExpression("[YN]")]
    public string MonthlyIncome_IsShowChangeCaseType { get; set; }

    /// <summary>
    /// 月收入確認_是否顯示變更卡別僅國旅卡
    /// Y / N
    /// </summary>
    [Display(Name = "月收入確認_是否顯示變更卡別僅國旅卡")]
    [Required]
    [RegularExpression("[YN]")]
    public string MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard { get; set; }

    /// <summary>
    /// 月收入確認_是否顯示權限內
    /// Y / N
    /// </summary>
    [Display(Name = "月收入確認_是否顯示權限內")]
    [Required]
    [RegularExpression("[YN]")]
    public string MonthlyIncome_IsShowInPermission { get; set; }

    /// <summary>
    /// 月收入確認_是否顯示月收入確認
    /// Y / N
    /// </summary>
    [Display(Name = "月收入確認_是否顯示月收入確認")]
    [Required]
    [RegularExpression("[YN]")]
    public string MonthlyIncome_IsShowMonthlyIncome { get; set; }

    /// <summary>
    /// 姓名檢核
    /// Y / N
    /// </summary>
    [Display(Name = "姓名檢核")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsShowNameCheck { get; set; }

    /// <summary>
    /// 更新正卡人基本資料
    /// Y / N
    /// </summary>
    [Display(Name = "更新正卡人基本資料")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsShowUpdatePrimaryInfo { get; set; }

    /// <summary>
    /// 查詢分行資訊
    /// Y / N
    /// </summary>
    [Display(Name = "查詢分行資訊")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsShowQueryBranchInfo { get; set; }

    /// <summary>
    /// 查詢929
    /// Y / N
    /// </summary>
    [Display(Name = "查詢929")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsShowQuery929 { get; set; }

    /// <summary>
    /// 新增圖檔
    /// Y / N
    /// </summary>
    [Display(Name = "新增圖檔")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsShowInsertFileAttachment { get; set; }

    /// <summary>
    /// 編輯附件備註_備註資料
    /// Y / N
    /// </summary>
    [Display(Name = "編輯附件備註_備註資料")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsShowUpdateApplyNote { get; set; }

    /// <summary>
    /// 是否為當前經辦
    /// Y / N
    /// </summary>
    [Display(Name = "是否為當前經辦")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsCurrentHandleUserId { get; set; }

    /// <summary>
    /// 新增照會摘要
    /// Y / N
    /// </summary>
    [Display(Name = "新增照會摘要")]
    [Required]
    [RegularExpression("[YN]")]
    public string InsertReviewerSummary { get; set; }

    /// <summary>
    /// 再查詢關注名單1
    /// Y / N
    /// </summary>
    [Display(Name = "再查詢關注名單1")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowFocus1 { get; set; }

    /// <summary>
    /// 再查詢關注名單2
    /// Y / N
    /// </summary>
    [Display(Name = "再查詢關注名單2")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowFocus2 { get; set; }

    /// <summary>
    /// 再查詢 檢驗手機號碼相同
    /// Y / N
    /// </summary>
    [Display(Name = "再查詢檢驗手機號碼相同")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowWebMobileRequery { get; set; }

    /// <summary>
    /// 再查詢檢驗電子信箱相同
    /// Y / N
    /// </summary>
    [Display(Name = "再查詢檢驗電子信箱相同")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowWebEmailRequery { get; set; }

    /// <summary>
    /// 徵審照會摘要-編輯
    /// Y / N
    /// </summary>
    [Display(Name = "徵審照會摘要-編輯")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateReviewerSummary { get; set; }

    /// <summary>
    /// 徵審照會摘要-刪除
    /// Y / N
    /// </summary>
    [Display(Name = "徵審照會摘要-刪除")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowDeleteReviewerSummary { get; set; }

    /// <summary>
    /// 圖檔刪除
    /// Y / N
    /// </summary>
    [Display(Name = "圖檔刪除")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowDeleteApplyFileAttachment { get; set; }

    /// <summary>
    /// 溝通備註
    /// Y / N
    /// </summary>
    [Display(Name = "溝通備註")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowCommunicationNotes { get; set; }

    /// <summary>
    /// 卡片階段
    /// 1.月收入確認
    /// 2.人工徵審
    /// 主因原廠商的有些狀態有重疊導致但權限跟流程又要不同,例如補件作業中,人工徵信的補件作業件要把狀態改為補回件
    ///
    /// </summary>
    [Display(Name = "卡片階段")]
    public CardStep? CardStep { get; set; }

    /// <summary>
    /// 人工徵審_是否顯示變更案件種類
    /// Y / N
    /// </summary>
    [Display(Name = "人工徵審_是否顯示變更案件種類")]
    [RegularExpression("[YN]")]
    [Required]
    public string ManualReview_IsShowChangeCaseType { get; set; }

    /// <summary>
    /// 人工徵審_是否顯示權限內
    /// Y / N
    /// </summary>
    [Display(Name = "人工徵審_是否顯示權限內")]
    [RegularExpression("[YN]")]
    [Required]
    public string ManualReview_IsShowInPermission { get; set; }

    /// <summary>
    /// 人工徵審_是否顯示權限外
    /// Y / N
    /// </summary>
    [Display(Name = "人工徵審_是否顯示權限外")]
    [RegularExpression("[YN]")]
    [Required]
    public string ManualReview_IsShowOutPermission { get; set; }

    /// <summary>
    /// 人工徵審_是否顯示退回重審
    /// Y / N
    /// </summary>
    [Display(Name = "人工徵審_是否顯示退回重審")]
    [RegularExpression("[YN]")]
    [Required]
    public string ManualReview_IsShowReturnReview { get; set; }

    /// <summary>
    /// 儲存相同 IP確認紀錄
    /// Y / N
    /// </summary>
    [Display(Name = "儲存相同 IP確認紀錄")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateSameIPCheckRecord { get; set; }

    /// <summary>
    /// 儲存網路手機號碼確認紀錄
    /// Y / N
    /// </summary>
    [Display(Name = "儲存網路手機號碼確認紀錄")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateWebMobileCheckRecord { get; set; }

    /// <summary>
    /// 儲存網路電子信箱確認紀錄
    /// Y / N
    /// </summary>
    [Display(Name = "儲存網路電子信箱確認紀錄")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateWebEmailCheckRecord { get; set; }

    /// <summary>
    /// 儲存行內 IP確認紀錄
    /// Y / N
    /// </summary>
    [Display(Name = "儲存行內 IP確認紀錄")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateInternalIPCheckRecord { get; set; }

    /// <summary>
    /// 儲存短時間頻繁確認紀錄
    /// Y / N
    /// </summary>
    [Display(Name = "儲存短時間頻繁確認紀錄")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateShortTimeIDCheckRecord { get; set; }

    /// <summary>
    /// 再查詢行內手機
    /// Y / N
    /// </summary>
    [Display(Name = "再查詢行內手機")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowInternalMobile { get; set; }

    /// <summary>
    /// 再查詢行內Email
    /// Y / N
    /// </summary>
    [Display(Name = "再查詢行內Email")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowInternalEmail { get; set; }

    /// <summary>
    /// 儲存行內手機確認紀錄
    /// Y / N
    /// </summary>
    [Display(Name = "儲存行內手機確認紀錄")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateInternalMobileCheckRecord { get; set; }

    /// <summary>
    /// 儲存行內Email確認紀錄
    /// Y / N
    /// </summary>
    [Display(Name = "儲存行內Email確認紀錄")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateInternalEmailCheckRecord { get; set; }

    /// <summary>
    /// 更新附卡人基本資料
    /// Y/ N
    /// </summary>
    [Display(Name = "更新附卡人基本資料")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowUpdateSupplementaryInfo { get; set; }

    /// <summary>
    /// KYC入檔
    /// Y / N
    /// 在人工徵信中顯示
    /// </summary>
    [Display(Name = "KYC入檔")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsShowKYCSync { get; set; }
}
