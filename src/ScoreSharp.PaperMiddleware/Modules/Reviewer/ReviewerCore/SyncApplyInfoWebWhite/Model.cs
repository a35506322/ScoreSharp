using ScoreSharp.Common.Attributes;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoWebWhite;

/// <summary>
/// 網路件小白同步案件資料請求
/// </summary>
public class SyncApplyInfoWebWhiteRequest : IValidatableObject
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    [MaxLength(14)]
    [Required]
    public string ApplyNo { get; set; } = string.Empty;

    /// <summary>
    /// 同步員編
    /// </summary>
    [Display(Name = "同步員編")]
    [MaxLength(30)]
    [Required]
    public string SyncUserId { get; set; } = string.Empty;

    #region 正卡人申請書資料

    /// <summary>
    /// 正附卡別
    /// 正卡 = 1
    /// 附卡 = 2
    /// 正卡+附卡 = 3
    /// </summary>
    [Display(Name = "正附卡別")]
    [Required]
    [ValidEnumValue]
    public CardOwner CardOwner { get; set; }

    /// <summary>
    /// 正卡_中文姓名
    /// </summary>
    [Display(Name = "正卡_中文姓名")]
    [MaxLength(30)]
    [Required]
    public string M_CHName { get; set; } = string.Empty;

    /// <summary>
    /// 正卡_性別
    /// 男 = 1
    /// 女 = 2
    /// </summary>
    [Display(Name = "正卡_性別")]
    [ValidEnumValue]
    public Sex? M_Sex { get; set; }

    /// <summary>
    /// 正卡_出生日期
    /// 民國格式：YYYMMDD，範例：0761030
    /// </summary>
    [Display(Name = "正卡_出生日期")]
    [MaxLength(7)]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? M_BirthDay { get; set; }

    /// <summary>
    /// 正卡_英文姓名
    /// </summary>
    [Display(Name = "正卡_英文姓名")]
    [MaxLength(100)]
    public string? M_ENName { get; set; }

    /// <summary>
    /// 正卡_出生地
    /// 中華民國= 1
    /// 其他 = 2
    /// </summary>
    [Display(Name = "正卡_出生地")]
    [ValidEnumValue]
    public BirthCitizenshipCode? M_BirthCitizenshipCode { get; set; }

    /// <summary>
    /// 正卡_出生地其他
    /// 當正卡_出生地 = 其他必填
    /// 需符合徵審系統「國籍設定」。
    /// </summary>
    [Display(Name = "正卡_出生地其他")]
    [MaxLength(16)]
    public string? M_BirthCitizenshipCodeOther { get; set; }

    /// <summary>
    /// 正卡_國籍
    /// 需符合徵審系統「國籍設定」。
    /// </summary>
    [Display(Name = "正卡_國籍")]
    [MaxLength(5)]
    public string? M_CitizenshipCode { get; set; }

    /// <summary>
    /// 正卡_身分證字號
    /// </summary>
    [Display(Name = "正卡_身分證字號")]
    [MaxLength(11)]
    [TWID]
    [Required]
    public string M_ID { get; set; } = string.Empty;

    /// <summary>
    /// 正卡_身分證發證日期
    /// 民國格式：YYYMMDD，0761030
    /// </summary>
    [Display(Name = "正卡_身分證發證日期")]
    [MaxLength(7)]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? M_IDIssueDate { get; set; }

    /// <summary>
    /// 正卡_身分證發證地點
    /// 需符合徵審系統「身分證發證地點設定」。
    /// </summary>
    [Display(Name = "正卡_身分證發證地點")]
    [MaxLength(20)]
    public string? M_IDCardRenewalLocationCode { get; set; }

    /// <summary>
    /// 正卡_身分證請領狀態
    /// 初發 = 1
    /// 補發 = 2
    /// 換發 = 3
    /// </summary>
    [Display(Name = "正卡_身分證請領狀態")]
    [ValidEnumValue]
    public IDTakeStatus? M_IDTakeStatus { get; set; }

    #region 正卡_戶籍
    /// <summary>
    /// 正卡_戶籍_郵遞區號
    /// </summary>
    [Display(Name = "正卡_戶籍_郵遞區號")]
    [MaxLength(30)]
    public string? M_Reg_ZipCode { get; set; }

    /// <summary>
    /// 正卡_戶籍_縣市
    /// </summary>
    [Display(Name = "正卡_戶籍_縣市")]
    [MaxLength(30)]
    public string? M_Reg_City { get; set; }

    /// <summary>
    /// 正卡_戶籍_區域
    /// </summary>
    [Display(Name = "正卡_戶籍_區域")]
    [MaxLength(30)]
    public string? M_Reg_District { get; set; }

    /// <summary>
    /// 正卡_戶籍_路
    /// </summary>
    [Display(Name = "正卡_戶籍_路")]
    [MaxLength(30)]
    public string? M_Reg_Road { get; set; }

    /// <summary>
    /// 正卡_戶籍_巷
    /// </summary>
    [Display(Name = "正卡_戶籍_巷")]
    [MaxLength(30)]
    public string? M_Reg_Lane { get; set; }

    /// <summary>
    /// 正卡_戶籍_弄
    /// </summary>
    [Display(Name = "正卡_戶籍_弄")]
    [MaxLength(30)]
    public string? M_Reg_Alley { get; set; }

    /// <summary>
    /// 正卡_戶籍_號
    /// </summary>
    [Display(Name = "正卡_戶籍_號")]
    [MaxLength(30)]
    public string? M_Reg_Number { get; set; }

    /// <summary>
    /// 正卡_戶籍_之號
    /// </summary>
    [Display(Name = "正卡_戶籍_之號")]
    [MaxLength(30)]
    public string? M_Reg_SubNumber { get; set; }

    /// <summary>
    /// 正卡_戶籍_樓層
    /// </summary>
    [Display(Name = "正卡_戶籍_樓層")]
    [MaxLength(30)]
    public string? M_Reg_Floor { get; set; }

    /// <summary>
    /// 正卡_戶籍_其他
    /// </summary>
    [Display(Name = "正卡_戶籍_其他")]
    [MaxLength(120)]
    public string? M_Reg_Other { get; set; }
    #endregion

    #region 正卡_居住

    /// <summary>
    /// 正卡_居住_地址類型
    /// 同戶籍地址 = 1
    /// 同帳單地址 = 2
    /// 同寄卡地址 = 3
    /// 同公司地址 = 4
    /// 其他 = 5
    /// </summary>
    [Display(Name = "正卡_居住_地址類型")]
    [ValidEnumValue]
    public LiveAddressType? M_Live_AddressType { get; set; }

    /// <summary>
    /// 正卡_居住_郵遞區號
    /// </summary>
    [Display(Name = "正卡_居住_郵遞區號")]
    [MaxLength(30)]
    public string? M_Live_ZipCode { get; set; }

    /// <summary>
    /// 正卡_居住_縣市
    /// </summary>
    [Display(Name = "正卡_居住_縣市")]
    [MaxLength(30)]
    public string? M_Live_City { get; set; }

    /// <summary>
    /// 正卡_居住_區域
    /// </summary>
    [Display(Name = "正卡_居住_區域")]
    [MaxLength(30)]
    public string? M_Live_District { get; set; }

    /// <summary>
    /// 正卡_居住_路
    /// </summary>
    [Display(Name = "正卡_居住_路")]
    [MaxLength(30)]
    public string? M_Live_Road { get; set; }

    /// <summary>
    /// 正卡_居住_巷
    /// </summary>
    [Display(Name = "正卡_居住_巷")]
    [MaxLength(30)]
    public string? M_Live_Lane { get; set; }

    /// <summary>
    /// 正卡_居住_弄
    /// </summary>
    [Display(Name = "正卡_居住_弄")]
    [MaxLength(30)]
    public string? M_Live_Alley { get; set; }

    /// <summary>
    /// 正卡_居住_號
    /// </summary>
    [Display(Name = "正卡_居住_號")]
    [MaxLength(30)]
    public string? M_Live_Number { get; set; }

    /// <summary>
    /// 正卡_居住_之號
    /// </summary>
    [Display(Name = "正卡_居住_之號")]
    [MaxLength(30)]
    public string? M_Live_SubNumber { get; set; }

    /// <summary>
    /// 正卡_居住_樓層
    /// </summary>
    [Display(Name = "正卡_居住_樓層")]
    [MaxLength(30)]
    public string? M_Live_Floor { get; set; }

    /// <summary>
    /// 正卡_居住_其他
    /// </summary>
    [Display(Name = "正卡_居住_其他")]
    [MaxLength(120)]
    public string? M_Live_Other { get; set; }
    #endregion

    #region 正卡_帳單

    /// <summary>
    /// 正卡_帳單_地址類型
    /// 同戶籍地址 = 1
    /// 同居住地址 = 2
    /// 同寄卡地址 = 3
    /// 同公司地址 = 4
    /// 其他 = 5
    /// </summary>
    [Display(Name = "正卡_帳單_地址類型")]
    [ValidEnumValue]
    public BillAddressType? M_Bill_AddressType { get; set; }

    /// <summary>
    /// 正卡_帳單_郵遞區號
    /// </summary>
    [Display(Name = "正卡_帳單_郵遞區號")]
    [MaxLength(30)]
    public string? M_Bill_ZipCode { get; set; }

    /// <summary>
    /// 正卡_帳單_縣市
    /// </summary>
    [Display(Name = "正卡_帳單_縣市")]
    [MaxLength(30)]
    public string? M_Bill_City { get; set; }

    /// <summary>
    /// 正卡_帳單_區域
    /// </summary>
    [Display(Name = "正卡_帳單_區域")]
    [MaxLength(30)]
    public string? M_Bill_District { get; set; }

    /// <summary>
    /// 正卡_帳單_路
    /// </summary>
    [Display(Name = "正卡_帳單_路")]
    [MaxLength(30)]
    public string? M_Bill_Road { get; set; }

    /// <summary>
    /// 正卡_帳單_巷
    /// </summary>
    [Display(Name = "正卡_帳單_巷")]
    [MaxLength(30)]
    public string? M_Bill_Lane { get; set; }

    /// <summary>
    /// 正卡_帳單_弄
    /// </summary>
    [Display(Name = "正卡_帳單_弄")]
    [MaxLength(30)]
    public string? M_Bill_Alley { get; set; }

    /// <summary>
    /// 正卡_帳單_號
    /// </summary>
    [Display(Name = "正卡_帳單_號")]
    [MaxLength(30)]
    public string? M_Bill_Number { get; set; }

    /// <summary>
    /// 正卡_帳單_之號
    /// </summary>
    [Display(Name = "正卡_帳單_之號")]
    [MaxLength(30)]
    public string? M_Bill_SubNumber { get; set; }

    /// <summary>
    /// 正卡_帳單_樓層
    /// </summary>
    [Display(Name = "正卡_帳單_樓層")]
    [MaxLength(30)]
    public string? M_Bill_Floor { get; set; }

    /// <summary>
    /// 正卡_帳單_其他
    /// </summary>
    [Display(Name = "正卡_帳單_其他")]
    [MaxLength(120)]
    public string? M_Bill_Other { get; set; }
    #endregion

    #region 正卡_寄卡

    /// <summary>
    /// 正卡_寄卡_地址類型
    /// 同戶籍地址 = 1
    /// 同居住地址 = 2
    /// 同帳單地址 = 3
    /// 同公司地址 = 4
    /// 親領 = 5
    /// 其他 = 6
    /// </summary>
    [Display(Name = "正卡_寄卡_地址類型")]
    [ValidEnumValue]
    public SendCardAddressType? M_SendCard_AddressType { get; set; }

    /// <summary>
    /// 正卡_寄卡_郵遞區號
    /// </summary>
    [Display(Name = "正卡_寄卡_郵遞區號")]
    [MaxLength(30)]
    public string? M_SendCard_ZipCode { get; set; }

    /// <summary>
    /// 正卡_寄卡_縣市
    /// </summary>
    [Display(Name = "正卡_寄卡_縣市")]
    [MaxLength(30)]
    public string? M_SendCard_City { get; set; }

    /// <summary>
    /// 正卡_寄卡_區域
    /// </summary>
    [Display(Name = "正卡_寄卡_區域")]
    [MaxLength(30)]
    public string? M_SendCard_District { get; set; }

    /// <summary>
    /// 正卡_寄卡_路
    /// </summary>
    [Display(Name = "正卡_寄卡_路")]
    [MaxLength(30)]
    public string? M_SendCard_Road { get; set; }

    /// <summary>
    /// 正卡_寄卡_巷
    /// </summary>
    [Display(Name = "正卡_寄卡_巷")]
    [MaxLength(30)]
    public string? M_SendCard_Lane { get; set; }

    /// <summary>
    /// 正卡_寄卡_弄
    /// </summary>
    [Display(Name = "正卡_寄卡_弄")]
    [MaxLength(30)]
    public string? M_SendCard_Alley { get; set; }

    /// <summary>
    /// 正卡_寄卡_號
    /// </summary>
    [Display(Name = "正卡_寄卡_號")]
    [MaxLength(30)]
    public string? M_SendCard_Number { get; set; }

    /// <summary>
    /// 正卡_寄卡_之號
    /// </summary>
    [Display(Name = "正卡_寄卡_之號")]
    [MaxLength(30)]
    public string? M_SendCard_SubNumber { get; set; }

    /// <summary>
    /// 正卡_寄卡_樓層
    /// </summary>
    [Display(Name = "正卡_寄卡_樓層")]
    [MaxLength(30)]
    public string? M_SendCard_Floor { get; set; }

    /// <summary>
    /// 正卡_寄卡_其他
    /// </summary>
    [Display(Name = "正卡_寄卡_其他")]
    [MaxLength(120)]
    public string? M_SendCard_Other { get; set; }
    #endregion

    /// <summary>
    /// 正卡_行動電話
    /// </summary>
    [Display(Name = "正卡_行動電話")]
    [MaxLength(10)]
    public string? M_Mobile { get; set; }

    /// <summary>
    /// 正卡_E-MAIL
    /// </summary>
    [Display(Name = "正卡_E-MAIL")]
    [MaxLength(100)]
    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email 格式錯誤")]
    public string? M_EMail { get; set; }

    /// <summary>
    /// 正卡_AML職業別
    /// 需符合徵審系統「AML職業別」。
    /// </summary>
    [Display(Name = "正卡_AML職業別")]
    [MaxLength(2)]
    public string? M_AMLProfessionCode { get; set; }

    /// <summary>
    /// 正卡_AML職業類別其他
    /// </summary>
    [Display(Name = "正卡_AML職業類別其他")]
    [MaxLength(50)]
    public string? M_AMLProfessionOther { get; set; }

    /// <summary>
    /// 正卡_AML職級別
    /// 需符合徵審系統「AML職級別」。
    /// </summary>
    [Display(Name = "正卡_AML職級別")]
    [MaxLength(2)]
    public string? M_AMLJobLevelCode { get; set; }

    /// <summary>
    /// 正卡_公司名稱
    /// </summary>
    [Display(Name = "正卡_公司名稱")]
    [MaxLength(30)]
    public string? M_CompName { get; set; }

    /// <summary>
    /// 正卡_公司電話
    /// </summary>
    [Display(Name = "正卡_公司電話")]
    [MaxLength(21)]
    public string? M_CompPhone { get; set; }

    #region 正卡_公司
    /// <summary>
    /// 正卡公司地址_郵遞區號
    /// </summary>
    [Display(Name = "正卡公司地址_郵遞區號")]
    [MaxLength(30)]
    public string? M_Comp_ZipCode { get; set; }

    /// <summary>
    /// 正卡公司地址_縣市
    /// </summary>
    [Display(Name = "正卡公司地址_縣市")]
    [MaxLength(30)]
    public string? M_Comp_City { get; set; }

    /// <summary>
    /// 正卡公司地址_區域
    /// </summary>
    [Display(Name = "正卡公司地址_區域")]
    [MaxLength(30)]
    public string? M_Comp_District { get; set; }

    /// <summary>
    /// 正卡公司地址_路
    /// </summary>
    [Display(Name = "正卡公司地址_路")]
    [MaxLength(30)]
    public string? M_Comp_Road { get; set; }

    /// <summary>
    /// 正卡公司地址_巷
    /// </summary>
    [Display(Name = "正卡公司地址_巷")]
    [MaxLength(30)]
    public string? M_Comp_Lane { get; set; }

    /// <summary>
    /// 正卡公司地址_弄
    /// </summary>
    [Display(Name = "正卡公司地址_弄")]
    [MaxLength(30)]
    public string? M_Comp_Alley { get; set; }

    /// <summary>
    /// 正卡公司地址_號
    /// </summary>
    [Display(Name = "正卡公司地址_號")]
    [MaxLength(30)]
    public string? M_Comp_Number { get; set; }

    /// <summary>
    /// 正卡公司地址_之號
    /// </summary>
    [Display(Name = "正卡公司地址_之號")]
    [MaxLength(30)]
    public string? M_Comp_SubNumber { get; set; }

    /// <summary>
    /// 正卡公司地址_樓
    /// </summary>
    [Display(Name = "正卡公司地址_樓")]
    [MaxLength(30)]
    public string? M_Comp_Floor { get; set; }

    /// <summary>
    /// 正卡公司地址_其他
    /// </summary>
    [Display(Name = "正卡公司地址_其他")]
    [MaxLength(120)]
    public string? M_Comp_Other { get; set; }
    #endregion

    /// <summary>
    /// 正卡_現職月收入(元)
    /// </summary>
    [Display(Name = "正卡_現職月收入(元)")]
    public int? M_CurrentMonthIncome { get; set; }

    /// <summary>
    /// 正卡_所得及資金來源
    /// 需符合徵審系統「主要所得及資金來源設定」。多選時中間以逗號(,)區隔。
    /// 範例:1,2,3,4,5
    /// </summary>
    [Display(Name = "正卡_所得及資金來源")]
    [MaxLength(50)]
    public string? M_MainIncomeAndFundCodes { get; set; }

    /// <summary>
    /// 正卡_所得及資金來源其他
    /// </summary>
    [Display(Name = "正卡_所得及資金來源其他")]
    [MaxLength(30)]
    public string? M_MainIncomeAndFundOther { get; set; }

    /// <summary>
    /// 本人同意提供資料予聯名(認同)集團
    /// </summary>
    [Display(Name = "本人同意提供資料予聯名(認同)集團")]
    [MaxLength(1)]
    [RegularExpression(@"^[YN]$", ErrorMessage = "只能輸入Y或N")]
    public string? M_IsAgreeDataOpen { get; set; }

    /// <summary>
    /// 推廣單位代號
    /// </summary>
    [Display(Name = "推廣單位代號")]
    [MaxLength(30)]
    public string? PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員編號
    /// </summary>
    [Display(Name = "推廣人員編號")]
    [MaxLength(30)]
    public string? PromotionUser { get; set; }

    /// <summary>
    /// 是否同意提供資料於第三人行銷
    /// </summary>
    [Display(Name = "是否同意提供資料於第三人行銷")]
    [MaxLength(1)]
    [RegularExpression(@"^[YN]$", ErrorMessage = "只能輸入Y或N")]
    public string? IsAgreeMarketing { get; set; }

    /// <summary>
    /// 是否同意悠遊卡自動加值預設開啟
    /// </summary>
    [Display(Name = "是否同意悠遊卡自動加值預設開啟")]
    [MaxLength(1)]
    [RegularExpression(@"^[YN]$", ErrorMessage = "只能輸入Y或N")]
    public string? M_IsAcceptEasyCardDefaultBonus { get; set; }

    /// <summary>
    /// 帳單形式
    /// 電子帳單 = 1
    /// 簡訊帳單 = 2
    /// 紙本帳單 = 3
    /// LINE帳單 = 4
    /// </summary>
    [Display(Name = "帳單形式")]
    [ValidEnumValue]
    public BillType? BillType { get; set; }

    /// <summary>
    /// 正卡_教育程度
    /// 博士 = 1
    /// 碩士 = 2
    /// 大學 = 3
    /// 專科 = 4
    /// 高中高職 = 5
    /// 其他 = 6
    /// </summary>
    [Display(Name = "正卡_教育程度")]
    [ValidEnumValue]
    public Education? M_Education { get; set; }

    /// <summary>
    /// 正卡_居住電話
    /// </summary>
    [Display(Name = "正卡_居住電話")]
    [MaxLength(18)]
    public string? M_HouseRegPhone { get; set; }

    /// <summary>
    /// 正卡_戶籍電話
    /// </summary>
    [Display(Name = "正卡_戶籍電話")]
    [MaxLength(18)]
    public string? M_LivePhone { get; set; }

    /// <summary>
    /// 正卡_畢業國小
    /// </summary>
    [Display(Name = "正卡_畢業國小")]
    [MaxLength(20)]
    public string? M_GraduatedElementarySchool { get; set; }

    /// <summary>
    /// 正卡_公司統一編號
    /// </summary>
    [Display(Name = "正卡_公司統一編號")]
    [MaxLength(8)]
    [RegularExpression(@"^\d{8}$")]
    public string? M_CompID { get; set; }

    /// <summary>
    /// 正卡_職稱
    /// </summary>
    [Display(Name = "正卡_職稱")]
    [MaxLength(30)]
    public string? M_CompJobTitle { get; set; }

    /// <summary>
    /// 正卡_年資
    /// </summary>
    [Display(Name = "正卡_年資")]
    public int? M_CompSeniority { get; set; }
    #endregion

    /// <summary>
    /// 正卡_居住地所有權人
    /// 本人 = 1
    /// 配偶 = 2
    /// 父母親 = 3
    /// 親屬 = 4
    /// 宿舍 = 5
    /// 租貸 = 6
    /// 其他 = 7
    /// </summary>
    [Display(Name = "正卡_居住地所有權人")]
    [ValidEnumValue]
    public LiveOwner? LiveOwner { get; set; }

    /// <summary>
    /// 安麗直銷商編號
    /// </summary>
    [Display(Name = "安麗直銷商編號")]
    [MaxLength(30)]
    public string? AnliNo { get; set; }

    /// <summary>
    /// 首刷禮代碼
    /// </summary>
    /// <remarks>
    /// 為借用欄位：實際使用名稱為【首刷禮代號】，現行新徵審系統已有此欄位可對應
    /// </remarks>
    [Display(Name = "首刷禮代碼")]
    [MaxLength(10)]
    public string? FirstBrushingGiftCode { get; set; }

    /// <summary>
    /// 專案代號
    /// 需符合徵審系統「專案設定」
    /// </summary>
    [Display(Name = "專案代號")]
    [MaxLength(20)]
    public string? ProjectCode { get; set; }

    #region 卡片資訊

    /// <summary>
    /// 申請卡片
    /// </summary>
    [Display(Name = "卡片資訊")]
    public List<CardInfo> CardInfo { get; set; } = new List<CardInfo>();

    #endregion

    #region 申請書歷程

    /// <summary>
    /// 申請歷程
    /// </summary>
    [Display(Name = "申請流程")]
    public List<ApplyProcess> ApplyProcess { get; set; } = new List<ApplyProcess>();

    #endregion

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // 卡片狀態要在紙本件的狀態範圍內
        var allowedStatuses = new[]
        {
            CardStatus.紙本件_初始,
            CardStatus.紙本件_一次件檔中,
            CardStatus.紙本件_二次件檔中,
            CardStatus.紙本件_建檔審核中,
        };
        if (CardInfo.Any(card => !allowedStatuses.Contains(card.CardStatus)))
            yield return new ValidationResult(
                $"卡片狀態不在{CardStatus.紙本件_初始}、{CardStatus.紙本件_一次件檔中}、{CardStatus.紙本件_二次件檔中}、{CardStatus.紙本件_建檔審核中}的範圍內，請檢查",
                new[] { nameof(CardStatus) }
            );

        if (M_BirthCitizenshipCode == BirthCitizenshipCode.其他 && string.IsNullOrEmpty(M_BirthCitizenshipCodeOther))
        {
            yield return new ValidationResult("當「正卡_出生地」選擇「其他」時，請填寫「正卡_出生地其他」欄位", new[] { nameof(ApplyProcess) });
        }
    }
}

/// <summary>
/// 卡片資訊
/// </summary>
public class CardInfo
{
    /// <summary>
    /// 身分證
    /// </summary>
    [Display(Name = "身分證")]
    [Required]
    [MaxLength(11)]
    public string ID { get; set; }

    /// <summary>
    /// 正附卡人類型
    /// </summary>
    [Display(Name = "正附卡人類型")]
    [Required]
    [ValidEnumValue]
    public UserType UserType { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    [Display(Name = "卡片狀態")]
    [Required]
    [ValidEnumValue]
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 申請卡別
    /// </summary>
    [Display(Name = "申請卡別")]
    [MaxLength(50)]
    [Required]
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡種
    /// </summary>
    [Display(Name = "申請卡種")]
    [Required]
    [ValidEnumValue]
    public ApplyCardKind ApplyCardKind { get; set; }
}

/// <summary>
/// 申請流程
/// </summary>
public class ApplyProcess
{
    /// <summary>
    /// 進行動作
    /// </summary>
    [Display(Name = "進行動作")]
    [MaxLength(50)]
    [Required]
    public string Process { get; set; } = string.Empty;

    /// <summary>
    /// 開始時間
    /// </summary>
    [Display(Name = "開始時間")]
    [Required]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    [Display(Name = "結束時間")]
    [Required]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Display(Name = "備註")]
    [MaxLength(200)]
    public string? Notes { get; set; }

    /// <summary>
    /// 處理人員
    /// </summary>
    [Display(Name = "處理人員")]
    [MaxLength(30)]
    [Required]
    public string ProcessUserId { get; set; } = string.Empty;
}

public class CardStausChangeResult
{
    public CardStatus BeforeCardStatus { get; set; }
    public CardStatus AfterCardStatus { get; set; }
}

public static class 參數類別
{
    public static readonly string 卡片種類 = "Card";
    public static readonly string 身分證換發地點 = "IDCardRenewalLocation";
    public static readonly string 國籍 = "Citizenship";
    public static readonly string AML職業別 = "AMLProfession";
    public static readonly string AML職級別 = "AMLJobLevel";
    public static readonly string 徵信代碼 = "CreditCheckCode";
    public static readonly string 主要收入來源 = "MainIncomeAndFund";
    public static readonly string 推廣單位 = "PromotionUnit";
    public static readonly string 縣市 = "City";
}
