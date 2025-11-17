namespace ScoreSharp.API.Modules.Test.Models;

public class GetApplicationInfoReqDataResponse
{
    /// <summary>
    /// 申請書編號：範例20180625A0001
    /// </summary>
    [Display(Name = "申請書編號")]
    [Required]
    public string ApplyNo { get; set; }

    #region 個人基本資料

    /// <summary>
    /// 中文姓名
    /// </summary>
    [Display(Name = "中文姓名")]
    [Required]
    public string CHName { get; set; }

    /// <summary>
    /// 身份證字號
    /// </summary>
    [Display(Name = "身份證字號")]
    [Required]
    [RegularExpression("^[A-Z]{1}[1-2]{1}[0-9]{8}$")]
    public string ID { get; set; }

    /// <summary>
    /// 國籍：關聯 SetUp_Citizenship
    /// </summary>
    [Display(Name = "國籍")]
    [Required]
    public string CitizenshipCode { get; set; }

    /// <summary>
    /// 出生地國籍
    /// </summary>
    [Display(Name = "出生地國籍")]
    [Required]
    [ValidEnumValue]
    public BirthCitizenshipCode BirthCitizenshipCode { get; set; }

    /// <summary>
    /// 行動電話
    /// </summary>
    [Display(Name = "行動電話")]
    [Required]
    [RegularExpression("^09\\d{8}$")]
    public string Mobile { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    [Display(Name = "性別")]
    [Required]
    [ValidEnumValue]
    public Sex Sex { get; set; }

    /// <summary>
    /// 生日：民國格式為 YYYMMDD
    /// </summary>
    [Display(Name = "生日")]
    [Required]
    [RegularExpression("^[0-9]{3}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
    public string BirthDay { get; set; }

    /// <summary>
    /// 英文姓名
    /// </summary>
    [Display(Name = "英文姓名")]
    [Required]
    public string ENName { get; set; }

    /// <summary>
    /// 身分證發證地點：關聯 SetUp_IDCardRenewalLocation
    /// </summary>
    [Display(Name = "身分證發證地點")]
    [Required]
    public string IDCardRenewalLocationCode { get; set; }

    /// <summary>
    /// 身分證請領狀態
    /// </summary>
    [Display(Name = "身分證請領狀態")]
    [Required]
    [ValidEnumValue]
    public IDTakeStatus IDTakeStatus { get; set; }

    /// <summary>
    /// 出生地國籍_其他：當出生地國籍為其他時使用
    /// </summary>
    [Display(Name = "出生地國籍_其他")]
    public string? BirthCitizenshipCodeOther { get; set; }

    /// <summary>
    /// 婚姻狀況
    /// </summary>
    [Display(Name = "婚姻狀況")]
    [ValidEnumValue]
    public MarriageState? MarriageState { get; set; }

    /// <summary>
    /// 教育程度
    /// </summary>
    [Display(Name = "教育程度")]
    [ValidEnumValue]
    public Education? Education { get; set; }

    /// <summary>
    /// 畢業國小
    /// </summary>
    [Display(Name = "畢業國小")]
    [Required]
    public string GraduatedElementarySchool { get; set; }

    /// <summary>
    /// E-MAIL
    /// </summary>
    [Display(Name = "E-MAIL")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public string? EMail { get; set; }

    /// <summary>
    /// 戶籍電話
    /// </summary>
    [Display(Name = "戶籍電話")]
    [RegularExpression(@"^[\d\s\+\-\(\)]+$")]
    public string? HouseRegPhone { get; set; }

    /// <summary>
    /// 居住電話
    /// </summary>
    [Display(Name = "居住電話")]
    [RegularExpression(@"^[\d\s\+\-\(\)]+$")]
    public string? LivePhone { get; set; }

    /// <summary>
    /// 居住地所有權人
    /// </summary>
    [Display(Name = "居住地所有權人")]
    [ValidEnumValue]
    public LiveOwner? LiveOwner { get; set; }

    /// <summary>
    /// 居住年數
    /// </summary>
    [Display(Name = "居住年數")]
    public int? LiveYear { get; set; }

    /// <summary>
    /// 推廣單位
    /// </summary>
    [Display(Name = "推廣單位")]
    public string? PromotionUnit { get; set; }

    /// <summary>
    /// 身分證發證日期：民國格式為 YYYMMDD
    /// </summary>
    [Display(Name = "身分證發證日期")]
    [Required]
    [RegularExpression("^[0-9]{3}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
    public string IDIssueDate { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    [Display(Name = "推廣人員")]
    public string? PromotionUser { get; set; }

    /// <summary>
    /// 安麗編號
    /// </summary>
    [Display(Name = "安麗編號")]
    public string? AnliNo { get; set; }

    /// <summary>
    /// 居留證發證日期 (格式: YYYYMMDD)
    /// </summary>
    [Display(Name = "居留證發證日期")]
    [RegularExpression(@"^\d{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
    public string? ResidencePermitIssueDate { get; set; }

    /// <summary>
    /// 護照號碼
    /// </summary>
    [Display(Name = "護照號碼")]
    public string? PassportNo { get; set; }

    /// <summary>
    /// 護照日期 (格式: YYYYMMDD) / 待確認
    /// </summary>
    [Display(Name = "護照日期")]
    [RegularExpression(@"^\d{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
    public string? PassportDate { get; set; }

    /// <summary>
    /// 外籍人士指定期限 (格式: YYYYMMDD) / 待確認
    /// </summary>
    [Display(Name = "外籍人士指定期限")]
    [RegularExpression(@"^\d{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
    public string? ExpatValidityPeriod { get; set; }

    /// <summary>
    /// 是否申請數位卡(Y：是、N：否)
    /// </summary>
    [Display(Name = "是否申請數位卡")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsApplyDigtalCard { get; set; }

    /// <summary>
    /// 是否FATCA身份 (Y/N)，當國籍 = 美國時候預設為 Y
    /// </summary>
    [Display(Name = "是否FATCA身份")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsFATCAIdentity { get; set; }

    /// <summary>
    /// 社會安全碼，FATCA身份=Y，徵審人員會去跟客人要此值
    /// </summary>
    [Display(Name = "社會安全碼")]
    public string? SocialSecurityCode { get; set; }

    /// <summary>
    /// 敦陽系統黑名單是否相符（Y：是、N：否）
    /// 由行員確認
    /// </summary>
    [Display(Name = "敦陽系統黑名單是否相符")]
    [RegularExpression("[YN]")]
    public string? IsDunyangBlackList { get; set; }

    /// <summary>
    /// 姓名檢核理由代碼，可為複數以「,」分割，姓名檢核 = Y 要有理由碼
    /// 1. PEP
    /// 2. 要再問
    /// 3. 黑名單
    /// 4. 負面新聞
    /// 5. 無
    /// 6. RCA
    /// 7. 國內PEP
    /// 8. 國外PEP
    /// 9. 國際組織PEP
    /// 10. 卸任PEP
    /// </summary>
    [Display(Name = "姓名檢核理由代碼")]
    public string? NameCheckedReasonCodes { get; set; }

    /// <summary>
    /// 當前或曾為PEP身分 (Y/N)，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "當前或曾為PEP身分")]
    [RegularExpression("[YN]")]
    public string? ISRCAForCurrentPEP { get; set; }

    /// <summary>
    /// 卸任PEP種類，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "卸任PEP種類")]
    [ValidEnumValue]
    public ResignPEPKind? ResignPEPKind { get; set; }

    /// <summary>
    /// 擔任PEP範圍，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "擔任PEP範圍")]
    [ValidEnumValue]
    public PEPRange? PEPRange { get; set; }

    /// <summary>
    /// 現任職位是否與PEP職位相關 (Y/N)，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "現任職位是否與PEP職位相關")]
    [RegularExpression("[YN]")]
    public string? IsCurrentPositionRelatedPEPPosition { get; set; }

    /// <summary>
    /// 帳單形式
    /// </summary>
    [Display(Name = "帳單形式")]
    [Required]
    [ValidEnumValue]
    public BillType BillType { get; set; }

    /// <summary>
    /// 進件方式，紙本才有
    /// </summary>
    [Display(Name = "進件方式")]
    [ValidEnumValue]
    public AcceptType? AcceptType { get; set; }

    /// <summary>
    /// 是否轉換卡別：Y、N，紙本案件，申請書上可幫客人轉換卡別
    /// </summary>
    [Display(Name = "是否轉換卡別")]
    [RegularExpression("[YN]")]
    public string? IsConvertCard { get; set; }

    /// <summary>
    /// 居留證背面號碼：前兩碼大寫英文 + 8 碼數字，範例ＹＺ80000001
    /// </summary>
    [Display(Name = "居留證背面號碼")]
    [RegularExpression(@"^[A-Z]{2}\d{8}$")]
    public string? ResidencePermitBackendNum { get; set; }

    /// <summary>
    /// 是否為永久居留證
    /// 1. 配合法遵部政策，新增欄位。
    /// 2. 欄位用拉霸方式選擇，Y/N，預設為N。
    /// 3. 如為{N}，則須鍵入外籍人士指定效期。
    /// </summary>
    [Display(Name = "是否為永久居留證")]
    [RegularExpression("[YN]")]
    public string? IsForeverResidencePermit { get; set; }

    /// <summary>
    /// 居留證期限
    /// </summary>
    [Display(Name = "居留證期限")]
    public string? ResidencePermitDeadline { get; set; }

    /// <summary>
    /// 舊照查驗
    /// Y:是,N:否
    /// </summary>
    [Display(Name = "舊照查驗")]
    [RegularExpression("[YN]")]
    public string? OldCertificateVerified { get; set; }

    #endregion

    #region 戶籍地址

    /// <summary>
    /// 郵遞區號(戶籍)
    /// </summary>
    [Display(Name = "郵遞區號(戶籍)")]
    [Required]
    public string Reg_ZipCode { get; set; }

    /// <summary>
    /// 正卡戶藉地址_縣市
    /// </summary>
    [Display(Name = "正卡戶藉地址_縣市")]
    [Required]
    public string Reg_City { get; set; }

    /// <summary>
    /// 正卡戶藉地址_鄉鎮市區
    /// </summary>
    [Display(Name = "正卡戶藉地址_鄉鎮市區")]
    [Required]
    public string Reg_District { get; set; }

    /// <summary>
    /// 正卡戶藉地址_路
    /// </summary>
    [Display(Name = "正卡戶藉地址_路")]
    [Required]
    public string Reg_Road { get; set; }

    /// <summary>
    /// 正卡戶藉地址_巷
    /// </summary>
    [Display(Name = "正卡戶藉地址_巷")]
    public string? Reg_Lane { get; set; }

    /// <summary>
    /// 正卡戶藉地址_弄
    /// </summary>
    [Display(Name = "正卡戶藉地址_弄")]
    public string? Reg_Alley { get; set; }

    /// <summary>
    /// 正卡戶藉地址_號
    /// </summary>
    [Display(Name = "正卡戶藉地址_號")]
    public string? Reg_Number { get; set; }

    /// <summary>
    /// 正卡戶藉地址_之號
    /// </summary>
    [Display(Name = "正卡戶藉地址_之號")]
    public string? Reg_SubNumber { get; set; }

    /// <summary>
    /// 正卡戶藉地址_樓
    /// </summary>
    [Display(Name = "正卡戶藉地址_樓")]
    public string? Reg_Floor { get; set; }

    /// <summary>
    /// 正卡戶藉地址_完整地址
    /// </summary>
    [Display(Name = "正卡戶藉地址_完整地址")]
    public string? Reg_FullAddr { get; set; }

    /// <summary>
    /// 正卡戶藉地址_其他
    /// </summary>
    [Display(Name = "正卡戶藉地址_其他")]
    public string? Reg_Other { get; set; }

    #endregion

    #region 居住地址

    //  居住地址
    /// <summary>
    /// 郵遞區號(居住)
    /// </summary>
    [Display(Name = "郵遞區號(居住)")]
    [Required]
    public string Live_ZipCode { get; set; }

    /// <summary>
    /// 正卡居住地址_縣市
    /// </summary>
    [Display(Name = "正卡居住地址_縣市")]
    [Required]
    public string Live_City { get; set; }

    /// <summary>
    /// 正卡居住地址_鄉鎮市區
    /// </summary>
    [Display(Name = "正卡居住地址_鄉鎮市區")]
    [Required]
    public string Live_District { get; set; }

    /// <summary>
    /// 正卡居住地址_路
    /// </summary>
    [Display(Name = "正卡居住地址_路")]
    [Required]
    public string Live_Road { get; set; }

    /// <summary>
    /// 正卡居住地址_巷
    /// </summary>
    [Display(Name = "正卡居住地址_巷")]
    public string? Live_Lane { get; set; }

    /// <summary>
    /// 正卡居住地址_弄
    /// </summary>
    [Display(Name = "正卡居住地址_弄")]
    public string? Live_Alley { get; set; }

    /// <summary>
    /// 正卡居住地址_號
    /// </summary>
    [Display(Name = "正卡居住地址_號")]
    public string? Live_Number { get; set; }

    /// <summary>
    /// 正卡居住地址_之號
    /// </summary>
    [Display(Name = "正卡居住地址_之號")]
    public string? Live_SubNumber { get; set; }

    /// <summary>
    /// 正卡居住地址_樓
    /// </summary>
    [Display(Name = "正卡居住地址_樓")]
    public string? Live_Floor { get; set; }

    /// <summary>
    /// 正卡居住地址_完整地址
    /// </summary>
    [Display(Name = "正卡居住地址_完整地址")]
    public string? Live_FullAddr { get; set; }

    /// <summary>
    /// 正卡居住地址_其他
    /// </summary>
    [Display(Name = "正卡居住地址_其他")]
    public string? Live_Other { get; set; }

    /// <summary>
    /// 居住地址類型
    /// </summary>
    [Display(Name = "居住地址類型")]
    [ValidEnumValue]
    public LiveAddressType? LiveAddressType { get; set; }

    #endregion

    #region 寄卡地址

    /// <summary>
    /// 郵遞區號(寄卡)
    /// </summary>
    [Display(Name = "郵遞區號(寄卡)")]
    [Required]
    public string SendCard_ZipCode { get; set; }

    /// <summary>
    /// 正卡寄卡地址_縣市
    /// </summary>
    [Display(Name = "正卡寄卡地址_縣市")]
    [Required]
    public string SendCard_City { get; set; }

    /// <summary>
    /// 正卡寄卡地址_鄉鎮市區
    /// </summary>
    [Display(Name = "正卡寄卡地址_鄉鎮市區")]
    [Required]
    public string SendCard_District { get; set; }

    /// <summary>
    /// 正卡寄卡地址_路
    /// </summary>
    [Display(Name = "正卡寄卡地址_路")]
    [Required]
    public string SendCard_Road { get; set; }

    /// <summary>
    /// 正卡寄卡地址_巷
    /// </summary>
    [Display(Name = "正卡寄卡地址_巷")]
    public string? SendCard_Lane { get; set; }

    /// <summary>
    /// 正卡寄卡地址_弄
    /// </summary>
    [Display(Name = "正卡寄卡地址_弄")]
    public string? SendCard_Alley { get; set; }

    /// <summary>
    /// 正卡寄卡地址_號
    /// </summary>
    [Display(Name = "正卡寄卡地址_號")]
    public string? SendCard_Number { get; set; }

    /// <summary>
    /// 正卡寄卡地址_之號
    /// </summary>
    [Display(Name = "正卡寄卡地址_之號")]
    public string? SendCard_SubNumber { get; set; }

    /// <summary>
    /// 正卡寄卡地址_樓
    /// </summary>
    [Display(Name = "正卡寄卡地址_樓")]
    public string? SendCard_Floor { get; set; }

    /// <summary>
    /// 正卡寄卡地址_完整地址
    /// </summary>
    [Display(Name = "正卡寄卡地址_完整地址")]
    public string? SendCard_FullAddr { get; set; }

    /// <summary>
    /// 正卡寄卡地址_其他
    /// </summary>
    [Display(Name = "正卡寄卡地址_其他")]
    public string? SendCard_Other { get; set; }

    /// <summary>
    /// 寄卡地址類型
    /// </summary>
    [Display(Name = "寄卡地址類型")]
    [ValidEnumValue]
    public SendCardAddressType? SendCardAddressType { get; set; }

    #endregion

    #region 帳單地址

    /// <summary>
    /// 郵遞區號(帳單)
    /// </summary>
    [Display(Name = "郵遞區號(帳單)")]
    [Required]
    public string Bill_ZipCode { get; set; }

    /// <summary>
    /// 正卡帳單地址_縣市
    /// </summary>
    [Display(Name = "正卡帳單地址_縣市")]
    [Required]
    public string Bill_City { get; set; }

    /// <summary>
    /// 正卡帳單地址_鄉鎮市區
    /// </summary>
    [Display(Name = "正卡帳單地址_鄉鎮市區")]
    [Required]
    public string Bill_District { get; set; }

    /// <summary>
    /// 正卡帳單地址_路
    /// </summary>
    [Display(Name = "正卡帳單地址_路")]
    [Required]
    public string Bill_Road { get; set; }

    /// <summary>
    /// 正卡帳單地址_巷
    /// </summary>
    [Display(Name = "正卡帳單地址_巷")]
    public string? Bill_Lane { get; set; }

    /// <summary>
    /// 正卡帳單地址_弄
    /// </summary>
    [Display(Name = "正卡帳單地址_弄")]
    public string? Bill_Alley { get; set; }

    /// <summary>
    /// 正卡帳單地址_號
    /// </summary>
    [Display(Name = "正卡帳單地址_號")]
    public string? Bill_Number { get; set; }

    /// <summary>
    /// 正卡帳單地址_之號
    /// </summary>
    [Display(Name = "正卡帳單地址_之號")]
    public string? Bill_SubNumber { get; set; }

    /// <summary>
    /// 正卡帳單地址_樓
    /// </summary>
    [Display(Name = "正卡帳單地址_樓")]
    public string? Bill_Floor { get; set; }

    /// <summary>
    /// 正卡帳單地址_完整地址
    /// </summary>
    [Display(Name = "正卡帳單地址_完整地址")]
    public string? Bill_FullAddr { get; set; }

    /// <summary>
    /// 正卡帳單地址_其他
    /// </summary>
    [Display(Name = "正卡帳單地址_其他")]
    public string? Bill_Other { get; set; }

    /// <summary>
    /// 帳單地址類型
    /// </summary>
    [Display(Name = "帳單地址類型")]
    [ValidEnumValue]
    public BillAddressType? BillAddressType { get; set; }

    #endregion

    #region 職業資料

    /// <summary>
    /// 公司名稱
    /// </summary>
    [Display(Name = "公司名稱")]
    public string? CompName { get; set; }

    /// <summary>
    /// 公司統一編號
    /// </summary>
    [Display(Name = "公司統一編號")]
    [RegularExpression("^\\d{8}$")]
    public string? CompID { get; set; }

    /// <summary>
    /// 公司職稱
    /// </summary>
    [Display(Name = "公司職稱")]
    public string? CompJobTitle { get; set; }

    /// <summary>
    /// 公司年資
    /// </summary>
    [Display(Name = "公司年資")]
    public int? CompSeniority { get; set; }

    /// <summary>
    /// 公司電話
    /// </summary>
    [Display(Name = "公司電話")]
    public string? CompPhone { get; set; }

    /// <summary>
    /// AML職業別：關聯  SetUp_AMLProfession
    /// </summary>
    [Display(Name = "AML職業別")]
    [Required]
    public string AMLProfessionCode { get; set; }

    /// <summary>
    /// AML職業別_其他
    /// </summary>
    [Display(Name = "AML職業別_其他")]
    public string? AMLProfessionOther { get; set; }

    /// <summary>
    /// AML職級別：關聯 SetUp_AMLJobLevel
    /// </summary>
    [Display(Name = "AML職級別")]
    [Required]
    public string AMLJobLevelCode { get; set; }

    /// <summary>
    /// 公司行業別
    /// </summary>
    [Display(Name = "公司行業別")]
    [ValidEnumValue]
    public CompTrade? CompTrade { get; set; }

    /// <summary>
    /// 公司職級別
    /// </summary>
    [Display(Name = "公司職級別")]
    [ValidEnumValue]
    public CompJobLevel? CompJobLevel { get; set; }

    /// <summary>
    /// 現職月收入(元)
    /// </summary>
    [Display(Name = "現職月收入(元)")]
    public int? CurrentMonthIncome { get; set; }

    /// <summary>
    /// 所得及資金來源 (請參考Setup_MainIncomeAndFund表格、需符合Paperless「主要所得及資金來源勾選」，多選選項以逗號(,)區隔)
    /// </summary>
    [Display(Name = "所得及資金來源")]
    public string? MainIncomeAndFundCodes { get; set; }

    /// <summary>
    /// 主要收入_其他
    /// </summary>
    [Display(Name = "主要收入_其他")]
    public string? MainIncomeAndFundOther { get; set; }

    #endregion

    #region 公司地址

    /// <summary>
    /// 郵遞區號(公司)
    /// </summary>
    [Display(Name = "郵遞區號(公司)")]
    public string? Comp_ZipCode { get; set; }

    /// <summary>
    /// 正卡公司地址_縣市
    /// </summary>
    [Display(Name = "正卡公司地址_縣市")]
    public string? Comp_City { get; set; }

    /// <summary>
    /// 正卡公司地址_鄉鎮市區
    /// </summary>
    [Display(Name = "正卡公司地址_鄉鎮市區")]
    public string? Comp_District { get; set; }

    /// <summary>
    /// 正卡公司地址_路
    /// </summary>
    [Display(Name = "正卡公司地址_路")]
    public string? Comp_Road { get; set; }

    /// <summary>
    /// 正卡公司地址_巷
    /// </summary>
    [Display(Name = "正卡公司地址_巷")]
    public string? Comp_Lane { get; set; }

    /// <summary>
    /// 正卡公司地址_弄
    /// </summary>
    [Display(Name = "正卡公司地址_弄")]
    public string? Comp_Alley { get; set; }

    /// <summary>
    /// 正卡公司地址_號
    /// </summary>
    [Display(Name = "正卡公司地址_號")]
    public string? Comp_Number { get; set; }

    /// <summary>
    /// 正卡公司地址_之號
    /// </summary>
    [Display(Name = "正卡公司地址_之號")]
    public string? Comp_SubNumber { get; set; }

    /// <summary>
    /// 正卡公司地址_樓
    /// </summary>
    [Display(Name = "正卡公司地址_樓")]
    public string? Comp_Floor { get; set; }

    /// <summary>
    /// 正卡公司地址_完整地址
    /// </summary>
    [Display(Name = "正卡公司地址_完整地址")]
    public string? Comp_FullAddr { get; set; }

    /// <summary>
    /// 正卡公司地址_其他
    /// </summary>
    [Display(Name = "正卡公司地址_其他")]
    public string? Comp_Other { get; set; }

    #endregion

    #region 學生申請人 IsStudent = Y 才有值

    /// <summary>
    /// 是否學生身份 (Y: 是, N: 否)
    /// </summary>
    [Display(Name = "是否學生身份")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsStudent { get; set; }

    /// <summary>
    /// 家長姓名
    /// </summary>
    [Display(Name = "家長姓名")]
    public string? ParentName { get; set; }

    /// <summary>
    /// 學生就讀學校
    /// </summary>
    [Display(Name = "學生就讀學校")]
    public string? StudSchool { get; set; }

    /// <summary>
    /// 家長電話 (可以行動電話或家電電話)
    /// </summary>
    [Display(Name = "家長電話")]
    public string? ParentPhone { get; set; }

    /// <summary>
    /// 學生預定畢業日期 (IsStudent = Y 才有值, 民國格式為 YYYMMDD)
    /// </summary>
    [Display(Name = "學生預定畢業日期")]
    [RegularExpression("^[0-9]{3}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
    public string? StudScheduledGraduationDate { get; set; }

    /// <summary>
    /// 學生申請人與本人關係
    /// </summary>
    [Display(Name = "學生申請人與本人關係")]
    [ValidEnumValue]
    public StudentApplicantRelationship? StudentApplicantRelationship { get; set; }

    #endregion

    #region 家長居住地址 IsStudent = Y 才有值

    /// <summary>
    /// 郵遞區號(家長居住)
    /// </summary>
    [Display(Name = "郵遞區號(家長居住)")]
    public string? ParentLive_ZipCode { get; set; }

    /// <summary>
    /// 正卡家長居住地址_縣市
    /// </summary>
    [Display(Name = "正卡家長居住地址_縣市")]
    public string? ParentLive_City { get; set; }

    /// <summary>
    /// 正卡家長居住地址_鄉鎮市區
    /// </summary>
    [Display(Name = "正卡家長居住地址_鄉鎮市區")]
    public string? ParentLive_District { get; set; }

    /// <summary>
    /// 正卡家長居住地址_路
    /// </summary>
    [Display(Name = "正卡家長居住地址_路")]
    public string? ParentLive_Road { get; set; }

    /// <summary>
    /// 正卡家長居住地址_巷
    /// </summary>
    [Display(Name = "正卡家長居住地址_巷")]
    public string? ParentLive_Lane { get; set; }

    /// <summary>
    /// 正卡家長居住地址_弄
    /// </summary>
    [Display(Name = "正卡家長居住地址_弄")]
    public string? ParentLive_Alley { get; set; }

    /// <summary>
    /// 正卡家長居住地址_號
    /// </summary>
    [Display(Name = "正卡家長居住地址_號")]
    public string? ParentLive_Number { get; set; }

    /// <summary>
    /// 正卡家長居住地址_之號
    /// </summary>
    [Display(Name = "正卡家長居住地址_之號")]
    public string? ParentLive_SubNumber { get; set; }

    /// <summary>
    /// 正卡家長居住地址_樓
    /// </summary>
    [Display(Name = "正卡家長居住地址_樓")]
    public string? ParentLive_Floor { get; set; }

    /// <summary>
    /// 正卡家長居住地址_完整地址
    /// </summary>
    [Display(Name = "正卡家長居住地址_完整地址")]
    public string? ParentLive_FullAddr { get; set; }

    /// <summary>
    /// 正卡家長居住地址_其他
    /// </summary>
    [Display(Name = "正卡家長居住地址_其他")]
    public string? ParentLive_Other { get; set; }

    /// <summary>
    /// 正卡家長居住地址類型
    /// </summary>
    [Display(Name = "正卡家長居住地址類型")]
    [ValidEnumValue]
    public ParentLiveAddressType? ParentLiveAddressType { get; set; }

    #endregion

    #region 活動資料

    /// <summary>
    /// 首刷禮代碼 (E-CARD填寫)
    /// </summary>
    [Display(Name = "首刷禮代碼")]
    public string? FirstBrushingGiftCode { get; set; }

    /// <summary>
    /// 專案代號 (請參考Setup_ProjectCode)
    /// </summary>
    [Display(Name = "專案代號")]
    [Required]
    public string ProjectCode { get; set; }

    /// <summary>
    /// 本人是否同意提供資料予聯名認同集團 (Y: 是, N: 否)
    /// </summary>
    [Display(Name = "本人是否同意提供資料予聯名認同集團")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsAgreeDataOpen { get; set; }

    /// <summary>
    /// 是否同意提供資料供本行進行行銷 (Y: 是, N: 否)
    /// </summary>
    [Display(Name = "是否同意提供資料供本行進行行銷")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsAgreeMarketing { get; set; }

    /// <summary>
    /// 是否綁定消費通知 (Y: 是, N: 否)
    /// </summary>
    [Display(Name = "是否綁定消費通知")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsPayNoticeBind { get; set; }

    /// <summary>
    /// 是否同意悠遊卡自動加值預設開啟 (Y: 是, N: 否)
    /// </summary>
    [Display(Name = "是否同意悠遊卡自動加值預設開啟")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsAcceptEasyCardDefaultBonus { get; set; }

    #endregion

    #region 簽核

    /// <summary>
    /// 徵信代碼
    /// 1. 當前最新徵信代碼
    /// 2. 關聯 SetUp_CreditCheckCode
    /// 3. 提供進行月收入確認時使用，是否僅能在月收入確認
    /// </summary>
    [Display(Name = "徵信代碼")]
    public string? CreditCheckCode { get; set; }

    #endregion
}
