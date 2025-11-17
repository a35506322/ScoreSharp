using ScoreSharp.Common.Attributes;

namespace ScoreSharp.PaperMiddleware.Modules.Test.CreateSyncApplyInfoPaperReq;

public class CreateSyncApplyInfoPaperRequest : IValidatableObject
{
    public CardOwner CardOwner { get; set; }
    public int M_CardCount { get; set; }
    public int S_CardCount { get; set; }

    [ValidEnumValue]
    public CardStatus CardStatus { get; set; }

    [ValidEnumValue]
    public SyncStatus SyncStatus { get; set; }

    public bool M_IsTaiwanNationality { get; set; }
    public bool M_IsBornInTaiwan { get; set; }
    public bool S_IsTaiwanNationality { get; set; }
    public bool S_IsBornInTaiwan { get; set; }

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
        if (!allowedStatuses.Contains(CardStatus))
            yield return new ValidationResult(
                $"卡片狀態不在{CardStatus.紙本件_初始}、{CardStatus.紙本件_一次件檔中}、{CardStatus.紙本件_二次件檔中}、{CardStatus.紙本件_建檔審核中}的範圍內，請檢查",
                new[] { nameof(CardStatus) }
            );
    }
}

public class SyncApplyInfoPaperRequest : IValidatableObject
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    [MaxLength(14)]
    [Required]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 同步狀態
    /// 修改 = 1
    /// 完成 = 2
    /// </summary>
    [Display(Name = "同步狀態")]
    [ValidEnumValue]
    [Required]
    public SyncStatus SyncStatus { get; set; }

    /// <summary>
    /// 同步員工
    /// </summary>
    [Display(Name = "同步員工")]
    [MaxLength(30)]
    [Required]
    public string SyncUserId { get; set; }

    #region 正卡人申請書資料

    /// <summary>
    /// 正附卡別
    /// 正卡 = 1
    /// 附卡 = 2
    /// 正卡+附卡 = 3
    /// </summary>
    [Display(Name = "正附卡別")]
    [ValidEnumValue]
    public CardOwner? CardOwner { get; set; }

    /// <summary>
    /// 正卡_中文姓名
    /// </summary>
    [Display(Name = "正卡_中文姓名")]
    [MaxLength(30)]
    public string? M_CHName { get; set; }

    /// <summary>
    /// 正卡_身分證字號
    /// </summary>
    [Display(Name = "正卡_身分證字號")]
    [MaxLength(11)]
    [TWID]
    public string? M_ID { get; set; }

    /// <summary>
    /// 正卡_身分證發證日期
    /// 民國格式：YYYMMDD，範例：0761030
    /// </summary>
    [Display(Name = "正卡_身分證發證日期")]
    [MaxLength(7)]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? M_IDIssueDate { get; set; }

    /// <summary>
    /// 正卡_身分證發證地點
    /// 需符合徵審系統「身分證發證地點設定」
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

    /// <summary>
    /// 正卡_性別
    /// 男 = 1
    /// 女 = 2
    /// </summary>
    [Display(Name = "正卡_性別")]
    [ValidEnumValue]
    public Sex? M_Sex { get; set; }

    /// <summary>
    /// 正卡_婚姻狀況
    /// 已婚 = 1
    /// 未婚 = 2
    /// 其他 = 3
    /// </summary>
    [Display(Name = "正卡_婚姻狀況")]
    [ValidEnumValue]
    public MarriageState? M_MarriageState { get; set; }

    /// <summary>
    /// 正卡_子女人數
    /// </summary>
    [Display(Name = "正卡_子女人數")]
    public int? M_ChildrenCount { get; set; }

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
    /// 正卡_出生地
    /// 正卡_出生地 = 其他為必填
    /// 需符合徵審系統「國籍設定」。
    /// </summary>
    [Display(Name = "正卡_出生地其他")]
    [MaxLength(16)]
    public string? M_BirthCitizenshipCodeOther { get; set; }

    /// <summary>
    /// 正卡_國籍
    /// 需符合徵審系統「國籍設定」
    /// </summary>
    [Display(Name = "正卡_國籍")]
    [MaxLength(5)]
    public string? M_CitizenshipCode { get; set; }

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
    /// 正卡_畢業國小
    /// </summary>
    [Display(Name = "正卡_畢業國小")]
    [MaxLength(20)]
    public string? M_GraduatedElementarySchool { get; set; }

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
    /// 正卡_居住_其他
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
    /// 正卡_行動電話
    /// </summary>
    [Display(Name = "正卡_行動電話")]
    [MaxLength(10)]
    public string? M_Mobile { get; set; }

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
    public LiveOwner? M_LiveOwner { get; set; }

    /// <summary>
    /// 正卡_居住年數
    /// </summary>
    [Display(Name = "正卡_居住年數")]
    public int? M_LiveYear { get; set; }

    /// <summary>
    /// 正卡_E-MAIL
    /// </summary>
    [Display(Name = "正卡_E-MAIL")]
    [MaxLength(100)]
    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public string? M_EMail { get; set; }

    /// <summary>
    /// 正卡_公司名稱
    /// </summary>
    [Display(Name = "正卡_公司名稱")]
    [MaxLength(30)]
    public string? M_CompName { get; set; }

    /// <summary>
    /// 正卡_公司行業別
    /// 金融業 = 1
    /// 公務機關 = 2
    /// 營造/製造/運輸業 =3
    /// 一般商業 = 4
    /// 休閒 / 娛樂 / 服務業 = 5
    /// 軍警消防業 = 6
    /// 非營利團體 = 7
    /// 學生 = 8
    /// 自由業_其他 = 9
    /// </summary>
    [Display(Name = "正卡_公司行業別")]
    [ValidEnumValue]
    public CompTrade? M_CompTrade { get; set; }

    /// <summary>
    /// 正卡_AML職業別
    /// 需符合徵審系統「AML職業別」
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
    /// 需符合徵審系統「AML職級別」
    /// </summary>
    [Display(Name = "正卡_AML職級別")]
    [MaxLength(2)]
    public string? M_AMLJobLevelCode { get; set; }

    /// <summary>
    /// 正卡_公司職級別
    /// 駕駛人員 = 1
    /// 服務生/門市人員 = 2
    /// 專業人員 = 3
    /// 專業技工 = 4
    /// 業務人員 = 5
    /// 一般職員 = 6
    /// 主管階層 = 7
    /// 股東/董事/負責人 = 8
    /// 家管/其他 = 9
    /// </summary>
    [Display(Name = "正卡_公司職級別")]
    [ValidEnumValue]
    public CompJobLevel? M_CompJobLevel { get; set; }

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
    /// 正卡_公司電話
    /// </summary>
    [Display(Name = "正卡_公司電話")]
    [MaxLength(21)]
    public string? M_CompPhone { get; set; }

    /// <summary>
    /// 正卡_公司統一編號
    /// 8位數字
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
    /// 正卡_部門名稱
    /// 【商務卡】及【國旅卡】的紙本申請書有此欄位，新徵審系統需增加此欄位，欄位位置規劃中
    /// 自行填寫
    /// </summary>
    [Display(Name = "正卡_部門名稱")]
    [MaxLength(30)]
    public string? M_DepartmentName { get; set; }

    /// <summary>
    /// 現職月收入(元)
    /// 現行申請書欄位名稱為【現職月收入】，建議於紙本進件系統升級時，再調整掃描的欄位名稱
    /// </summary>
    [Display(Name = "現職月收入(元)")]
    public int? M_CurrentMonthIncome { get; set; }

    /// <summary>
    /// 正卡_到職日期
    /// 民國年 1090101
    /// </summary>
    [Display(Name = "正卡_到職日期")]
    [MaxLength(7)]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? M_EmploymentDate { get; set; }

    /// <summary>
    /// 正卡_年資
    /// </summary>
    [Display(Name = "正卡_年資")]
    public int? M_CompSeniority { get; set; }

    /// <summary>
    /// 正卡_是否同意轉換卡別
    /// Y 、N
    /// </summary>
    [Display(Name = "正卡_是否同意轉換卡別")]
    [MaxLength(1)]
    [RegularExpression("^[YN]$")]
    public string? M_ReissuedCardType { get; set; }

    /// <summary>
    /// 本人同意提供資料予聯名(認同)集團
    /// 定義值：Y/N
    /// </summary>
    [Display(Name = "本人同意提供資料予聯名(認同)集團")]
    [MaxLength(1)]
    [RegularExpression("^[YN]$")]
    public string? M_IsAgreeDataOpen { get; set; }

    /// <summary>
    /// 正卡_所得及資金來源
    /// 需符合徵審系統「主要所得及資金來源設定」。多選時中間以逗號(,)區隔
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
    /// 電子化約定條款
    /// 範例：202007
    /// </summary>
    [Display(Name = "電子化約定條款")]
    [MaxLength(10)]
    public string? ElecCodeId { get; set; }

    /// <summary>
    /// 正卡_是否持有本行卡
    /// Y 、N
    /// </summary>
    [Display(Name = "正卡_是否持有本行卡")]
    [MaxLength(1)]
    [RegularExpression("^[YN]$")]
    public string? M_IsHoldingBankCard { get; set; }

    /// <summary>
    /// 首刷禮代碼
    /// </summary>
    [Display(Name = "首刷禮代碼")]
    [MaxLength(10)]
    public string? FirstBrushingGiftCode { get; set; }

    /// <summary>
    /// 年費收取方式
    /// 關聯SetUp_AnnualFeeCollectionMethod
    /// </summary>
    [Display(Name = "年費收取方式")]
    [MaxLength(2)]
    public string? AnnualFeePaymentType { get; set; }

    /// <summary>
    /// 是否同意悠遊卡自動加值預設開啟
    /// Y/N
    /// </summary>
    [Display(Name = "是否同意悠遊卡自動加值預設開啟")]
    [MaxLength(1)]
    [RegularExpression("^[YN]$")]
    public string? M_IsAcceptEasyCardDefaultBonus { get; set; }

    /// <summary>
    /// 是否同意提供資料於第三人行銷
    /// Y/N
    /// </summary>
    [Display(Name = "是否同意提供資料於第三人行銷")]
    [MaxLength(1)]
    [RegularExpression("^[YN]$")]
    public string? IsAgreeMarketing { get; set; }

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

    #endregion

    #region 申請書其他相關資訊

    /// <summary>
    /// 專案代號
    /// 需符合徵審系統「專案設定」
    /// </summary>
    [Display(Name = "專案代號")]
    [MaxLength(20)]
    public string? ProjectCode { get; set; }

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
    /// 進件方式
    /// 親訪親簽 = 1
    /// 親訪未見親簽 = 2
    /// 設攤親簽 = 3
    /// 設為未見親簽 = 4
    /// 自來件 = 5
    /// 電話行銷 = 6
    /// </summary>
    [Display(Name = "進件方式")]
    [ValidEnumValue]
    public AcceptType? AcceptType { get; set; }

    /// <summary>
    /// 安麗直銷商編號
    /// </summary>
    [Display(Name = "安麗直銷商編號")]
    [MaxLength(30)]
    public string? AnliNo { get; set; }

    /// <summary>
    /// 案件種類
    /// 一般件 = 1
    /// 急件 = 2
    /// 緊急製卡 = 3
    /// </summary>
    [Display(Name = "案件種類")]
    [ValidEnumValue]
    public CaseType CaseType { get; set; }

    #endregion

    #region 附卡人申請書資料

    /// <summary>
    /// 附卡1_與正卡持有人關係
    /// 配偶 = 1
    /// 父母 = 2
    /// 子女 = 3
    /// 兄弟姊妹 = 4
    /// 配偶父母 = 5
    /// </summary>
    [Display(Name = "附卡1_與正卡持有人關係")]
    [ValidEnumValue]
    public ApplicantRelationship? S1_ApplicantRelationship { get; set; }

    /// <summary>
    /// 附卡1_中文姓名
    /// </summary>
    [Display(Name = "附卡1_中文姓名")]
    [MaxLength(30)]
    public string? S1_CHName { get; set; }

    /// <summary>
    /// 附卡1_身分證字號
    /// </summary>
    [Display(Name = "附卡1_身分證字號")]
    [MaxLength(11)]
    [TWID]
    public string? S1_ID { get; set; }

    /// <summary>
    /// 附卡1_身分證發證日期
    /// 民國格式：YYYMMDD，0761030
    /// </summary>
    [Display(Name = "附卡1_身分證發證日期")]
    [MaxLength(7)]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? S1_IDIssueDate { get; set; }

    /// <summary>
    /// 附卡1_身分證發證地點
    /// 需符合徵審系統「身分證發證地點設定」
    /// </summary>
    [Display(Name = "附卡1_身分證發證地點")]
    [MaxLength(20)]
    public string? S1_IDCardRenewalLocationCode { get; set; }

    /// <summary>
    /// 附卡1_身分證請領狀態
    /// 初發 = 1
    /// 補發 = 2
    /// 換發 = 3
    /// </summary>
    [Display(Name = "附卡1_身分證請領狀態")]
    [ValidEnumValue]
    public IDTakeStatus? S1_IDTakeStatus { get; set; }

    /// <summary>
    /// 附卡1_性別
    /// 男 = 1
    /// 女 = 2
    /// </summary>
    [Display(Name = "附卡1_性別")]
    [ValidEnumValue]
    public Sex? S1_Sex { get; set; }

    /// <summary>
    /// 附卡1_婚姻狀況
    /// 已婚 = 1
    /// 未婚 = 2
    /// 其他 = 3
    /// </summary>
    [Display(Name = "附卡1_婚姻狀況")]
    [ValidEnumValue]
    public MarriageState? S1_MarriageState { get; set; }

    /// <summary>
    /// 附卡1_出生日期
    /// 民國格式：YYYMMDD，範例：0761030
    /// </summary>
    [Display(Name = "附卡1_出生日期")]
    [MaxLength(7)]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? S1_BirthDay { get; set; }

    /// <summary>
    /// 附卡1_英文姓名
    /// </summary>
    [Display(Name = "附卡1_英文姓名")]
    [MaxLength(100)]
    public string? S1_ENName { get; set; }

    /// <summary>
    /// 附卡1_出生地
    /// 中華民國= 1
    /// 其他 = 2
    /// </summary>
    [Display(Name = "附卡1_出生地")]
    [ValidEnumValue]
    public BirthCitizenshipCode? S1_BirthCitizenshipCode { get; set; }

    /// <summary>
    /// 附卡1_出生地
    /// 附卡1_出生地 = 其他為必填
    /// 需符合徵審系統「國籍設定」。
    /// </summary>
    [Display(Name = "附卡1_出生地其他")]
    [MaxLength(16)]
    public string? S1_BirthCitizenshipCodeOther { get; set; }

    /// <summary>
    /// 附卡1_國籍
    /// 需符合徵審系統「國籍設定」。
    /// </summary>
    [Display(Name = "附卡1_國籍")]
    [MaxLength(5)]
    public string? S1_CitizenshipCode { get; set; }

    #region 附卡1_居住

    /// <summary>
    /// 附卡1居住地址_地址類型
    /// </summary>
    [Display(Name = "附卡1居住地址_地址類型")]
    [ValidEnumValue]
    public ResidenceType? S1_Live_AddressType { get; set; }

    /// <summary>
    /// 附卡1居住地址_郵遞區號
    /// </summary>
    [Display(Name = "附卡1居住地址_郵遞區號")]
    [MaxLength(30)]
    public string? S1_Live_ZipCode { get; set; }

    /// <summary>
    /// 附卡1居住地址_縣市
    /// </summary>
    [Display(Name = "附卡1居住地址_縣市")]
    [MaxLength(30)]
    public string? S1_Live_City { get; set; }

    /// <summary>
    /// 附卡1居住地址_區域
    /// </summary>
    [Display(Name = "附卡1居住地址_區域")]
    [MaxLength(30)]
    public string? S1_Live_District { get; set; }

    /// <summary>
    /// 附卡1居住地址_路
    /// </summary>
    [Display(Name = "附卡1居住地址_路")]
    [MaxLength(30)]
    public string? S1_Live_Road { get; set; }

    /// <summary>
    /// 附卡1居住地址_巷
    /// </summary>
    [Display(Name = "附卡1居住地址_巷")]
    [MaxLength(30)]
    public string? S1_Live_Lane { get; set; }

    /// <summary>
    /// 附卡1居住地址_弄
    /// </summary>
    [Display(Name = "附卡1居住地址_弄")]
    [MaxLength(30)]
    public string? S1_Live_Alley { get; set; }

    /// <summary>
    /// 附卡1居住地址_號
    /// </summary>
    [Display(Name = "附卡1居住地址_號")]
    [MaxLength(30)]
    public string? S1_Live_Number { get; set; }

    /// <summary>
    /// 附卡1居住地址_之號
    /// </summary>
    [Display(Name = "附卡1居住地址_之號")]
    [MaxLength(30)]
    public string? S1_Live_SubNumber { get; set; }

    /// <summary>
    /// 附卡1居住地址_樓
    /// </summary>
    [Display(Name = "附卡1居住地址_樓")]
    [MaxLength(30)]
    public string? S1_Live_Floor { get; set; }

    /// <summary>
    /// 附卡1居住地址_其他
    /// </summary>
    [Display(Name = "附卡1居住地址_其他")]
    [MaxLength(120)]
    public string? S1_Live_Other { get; set; }

    #endregion

    #region 附卡1_寄卡

    /// <summary>
    /// 附卡1寄卡地址_地址類型
    /// 同正卡寄卡地址 = 1
    /// 親領 = 2
    /// </summary>
    [Display(Name = "附卡1寄卡地址_地址類型")]
    [ValidEnumValue]
    public ShippingCardAddressType? S1_SendCard_AddressType { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_郵遞區號
    /// </summary>
    [Display(Name = "附卡1寄卡地址_郵遞區號")]
    [MaxLength(30)]
    public string? S1_SendCard_ZipCode { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_縣市
    /// </summary>
    [Display(Name = "附卡1寄卡地址_縣市")]
    [MaxLength(30)]
    public string? S1_SendCard_City { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_區域
    /// </summary>
    [Display(Name = "附卡1寄卡地址_區域")]
    [MaxLength(30)]
    public string? S1_SendCard_District { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_路
    /// </summary>
    [Display(Name = "附卡1寄卡地址_路")]
    [MaxLength(30)]
    public string? S1_SendCard_Road { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_巷
    /// </summary>
    [Display(Name = "附卡1寄卡地址_巷")]
    [MaxLength(30)]
    public string? S1_SendCard_Lane { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_弄
    /// </summary>
    [Display(Name = "附卡1寄卡地址_弄")]
    [MaxLength(30)]
    public string? S1_SendCard_Alley { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_號
    /// </summary>
    [Display(Name = "附卡1寄卡地址_號")]
    [MaxLength(30)]
    public string? S1_SendCard_Number { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_之號
    /// </summary>
    [Display(Name = "附卡1寄卡地址_之號")]
    [MaxLength(30)]
    public string? S1_SendCard_SubNumber { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_樓
    /// </summary>
    [Display(Name = "附卡1寄卡地址_樓")]
    [MaxLength(30)]
    public string? S1_SendCard_Floor { get; set; }

    /// <summary>
    /// 附卡1寄卡地址_其他
    /// </summary>
    [Display(Name = "附卡1寄卡地址_其他")]
    [MaxLength(120)]
    public string? S1_SendCard_Other { get; set; }

    #endregion

    /// <summary>
    /// 附卡1_居住電話
    /// </summary>
    [Display(Name = "附卡1_居住電話")]
    [MaxLength(18)]
    public string? S1_LivePhone { get; set; }

    /// <summary>
    /// 附卡1_行動電話
    /// </summary>
    [Display(Name = "附卡1_行動電話")]
    [MaxLength(10)]
    public string? S1_Mobile { get; set; }

    /// <summary>
    /// 附卡1_公司電話
    /// </summary>
    [Display(Name = "附卡1_公司電話")]
    [MaxLength(21)]
    public string? S1_CompPhone { get; set; }

    /// <summary>
    /// 附卡1_公司名稱
    /// </summary>
    [Display(Name = "附卡1_公司名稱")]
    [MaxLength(30)]
    public string? S1_CompName { get; set; }

    /// <summary>
    /// 附卡1_職稱
    /// </summary>
    [Display(Name = "附卡1_職稱")]
    [MaxLength(30)]
    public string? S1_CompJobTitle { get; set; }

    #endregion

    #region 卡片資訊
    /// <summary>
    /// 申請卡片
    /// </summary>
    public List<CardInfoDto> CardInfo { get; set; } = new List<CardInfoDto>();
    #endregion

    #region 申請書歷程紀錄

    /// <summary>
    /// 申請書歷程紀錄
    /// 如未有資料請給空陣列
    /// </summary>
    public List<ApplyProcessDto> ApplyProcess { get; set; } = new List<ApplyProcessDto>();

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

        if (CardOwner == ScoreSharp.Common.Enums.CardOwner.正卡_附卡)
        {
            if (S1_ID is null || S1_CHName is null)
            {
                yield return new ValidationResult("請填寫附卡人資訊", new[] { nameof(S1_CHName), nameof(S1_ID) });
            }
        }

        if (M_BirthCitizenshipCode == BirthCitizenshipCode.其他 && string.IsNullOrEmpty(M_BirthCitizenshipCodeOther))
            yield return new ValidationResult(
                "當「正卡_出生地」選擇「其他」時，請填寫「正卡_出生地其他」欄位。",
                new[] { nameof(M_BirthCitizenshipCodeOther) }
            );

        if (S1_BirthCitizenshipCode == BirthCitizenshipCode.其他 && string.IsNullOrEmpty(S1_BirthCitizenshipCodeOther))
            yield return new ValidationResult(
                "當「附卡1_出生地」選擇「其他」時，請填寫「附卡1_出生地其他」欄位。",
                new[] { nameof(S1_BirthCitizenshipCodeOther) }
            );
    }
}

public class ApplyProcessDto
{
    /// <summary>
    /// 進行動作
    /// 範例:一次建檔中
    /// 有資料情況下為必填
    /// 給中文即可
    /// </summary>
    [Display(Name = "進行動作")]
    [MaxLength(50)]
    [Required]
    public string Process { get; set; }

    /// <summary>
    /// 開始時間
    /// 有資料情況下為必填
    /// </summary>
    [Display(Name = "開始時間")]
    [Required]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// 有資料情況下為必填
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
    /// 有資料情況下為必填
    /// </summary>
    [Display(Name = "處理人員")]
    [MaxLength(30)]
    [Required]
    public string ProcessUserId { get; set; }
}

public class CardInfoDto
{
    /// <summary>
    /// 身分證
    /// </summary>
    [Display(Name = "身分證")]
    [MaxLength(11)]
    public string ID { get; set; }

    /// <summary>
    /// 正附卡人類型
    /// 正卡人 = 1
    /// 附卡人 = 2
    /// </summary>
    [Display(Name = "正附卡人類型")]
    [ValidEnumValue]
    public UserType UserType { get; set; }

    /// <summary>
    /// 卡片狀態
    /// 初始 = 1
    /// 一次件檔中 = 20002
    /// 二次件檔中= 20004
    /// 建檔審核中 = 20007
    /// </summary>
    [Display(Name = "卡片狀態")]
    [ValidEnumValue]
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 申請卡別
    /// 範例：JST59
    /// 需符合徵審系統「卡片設定」。
    /// 可多選欄位，如多選資料以/相隔，例如：JA00/JC00
    /// </summary>
    [Display(Name = "申請卡別")]
    [MaxLength(50)]
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡種
    /// 實體 = 1
    /// 數位 = 2
    /// 實體+數位 = 3
    /// </summary>
    [Display(Name = "申請卡種")]
    [ValidEnumValue]
    public ApplyCardKind ApplyCardKind { get; set; }
}
