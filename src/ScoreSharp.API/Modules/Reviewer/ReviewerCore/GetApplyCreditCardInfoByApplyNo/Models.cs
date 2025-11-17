using System.Text.Json.Serialization;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoByApplyNo;

public record GetApplyCreditCardInfoByApplyNoResponse
{
    /// <summary>
    /// 主要資料，使用者稱為天
    /// </summary>
    public MainInfo MainInfo { get; init; }

    /// <summary>
    /// 正卡人申請基本資料
    /// </summary>
    public Primary_BasicInfo Primary_BasicInfo { get; init; }

    /// <summary>
    /// 職業資料
    /// </summary>
    public Primary_JobInfo Primary_JobInfo { get; init; }

    /// <summary>
    /// 學生資料
    /// </summary>
    public Primary_StudentInfo Primary_StudentInfo { get; init; }

    /// <summary>
    /// 網路進件有的欄位
    /// </summary>
    public Primary_WebCardInfo Primary_WebCardInfo { get; init; }

    /// <summary>
    /// 活動資料
    /// </summary>
    public Primary_ActivityInfo Primary_ActivityInfo { get; init; }

    /// <summary>
    /// 銀行查核資料
    /// </summary>
    public Primary_BankTraceInfo Primary_BankTraceInfo { get; init; }

    /// <summary>
    /// 附卡人資料，如果沒有申辦附卡，則為 null
    /// </summary>
    public Supplementary? Supplementary { get; init; }

    /// <summary>
    /// KYC加強審核資料，如果沒有進行KYC建檔，則為 null
    /// </summary>
    public KYCInfo KYCInfo { get; init; }

    public GetApplyCreditCardInfoByApplyNoResponse(
        MainInfo mainInfo,
        Primary_BasicInfo primary_BasicInfo,
        Primary_JobInfo primary_JobInfo,
        Primary_StudentInfo primary_StudentInfo,
        Primary_WebCardInfo primary_WebCardInfo,
        Primary_ActivityInfo primary_ActivityInfo,
        Primary_BankTraceInfo primary_BankTraceInfo,
        Supplementary supplementary,
        KYCInfo kycInfo
    )
    {
        MainInfo = mainInfo;
        Primary_BasicInfo = primary_BasicInfo;
        Primary_JobInfo = primary_JobInfo;
        Primary_StudentInfo = primary_StudentInfo;
        Primary_WebCardInfo = primary_WebCardInfo;
        Primary_ActivityInfo = primary_ActivityInfo;
        Primary_BankTraceInfo = primary_BankTraceInfo;
        Supplementary = supplementary;
        KYCInfo = kycInfo;
    }
}

public class MainInfo
{
    /// <summary>
    /// 中文姓名
    /// </summary>
    public string CHName { get; set; }

    /// <summary>
    /// 身份證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 申請日期
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 申請書編號
    ///
    /// 範例20180625A0001
    ///
    /// -網路件
    /// 1. IDType = 空白= 金融小白受理編號中會有 X
    /// 2. IDType = 存戶與卡友受理編號中會有 B， C D E F 等是進位，例如 20240901B9999下一個就是20240901C0001
    ///
    /// -紙本件
    /// 1. 中間案件編號58 08 00 48 是紙本進件的來源，如臨櫃之類的
    ///
    /// 國旅卡
    /// 1. 84、85為申請國旅卡
    ///
    /// </summary>s
    public string ApplyNo { get; set; }

    /// <summary>
    /// 卡片狀態 正附卡
    /// </summary>
    public List<CardStatusDto> CardStatusList { get; set; } = new();

    /// <summary>
    /// 家族訊息正/附卡
    /// 資料來源總行業管部自建資料庫 / Y 、N
    /// </summary>
    public List<FamilyMessageCheckedDto> FamilyMessageCheckedList { get; set; } = new();

    /// <summary>
    /// 是否重複申請件正卡
    /// 此欄位包含申請人是否曾有申請本行信用卡+現在是否為本行信用卡流通戶 / Y 、N
    /// </summary>
    public List<IsRepeatApplyDto> IsRepeatApplyList { get; set; } = new();

    /// <summary>
    /// 是否為原持卡人正/附卡
    /// </summary>
    public List<IsOriginalCardholderDto> IsOriginalCardholderList { get; set; } = new();

    /// <summary>
    /// 申請卡別
    /// 最多三張正卡,三張附卡
    /// </summary>
    public List<ApplyCardTypeDto> ApplyCardTypeList { get; set; } = new();

    /// <summary>
    /// 929-正/附卡
    /// 用API方式以身分證字號查詢資料庫，檢核是否命中情形，資料庫來源為業務管理部 / Y 、N
    /// </summary>
    public List<Checked929Dto> Checked929List { get; set; } = new();

    /// <summary>
    /// 客戶特殊註記
    /// </summary>
    public string? CustomerSpecialNotes { get; set; } = String.Empty;

    /// <summary>
    /// 身分證驗證結果-正/附卡
    /// 聯徵查回後會有相關資訊可入
    /// </summary>
    public List<IDCheckResultCheckedDto> IDCheckResultCheckedList { get; set; } = new();

    /// <summary>
    /// 關注名單1-正/附卡
    /// 命中 B 及 C 名單為 Y
    /// </summary>
    public List<Focus1CheckedDto> Focus1CheckedList { get; set; } = new();

    /// <summary>
    /// 黑名單註記
    /// </summary>
    public string? BlackListNote { get; set; } = String.Empty;

    /// <summary>
    /// 案件種類，如一般件
    /// </summary>
    public CaseType? CaseType { get; set; }
    public string CaseTypeName => CaseType is null ? String.Empty : CaseType.ToString();

    /// <summary>
    /// 原持卡人JCIC補充註記 (正/附卡)
    /// 現由文彬依聯徵查回結果提供
    /// </summary>
    public List<OriginCardholderJCICNotesDto> OriginCardholderJCICNotesList { get; set; } = new();

    /// <summary>
    /// 關注名單2-正/附卡
    /// 命中 A 及 D E F G 名單為 Y
    /// </summary>
    public List<Focus2CheckedDto> Focus2CheckedList { get; set; } = new();

    /// <summary>
    /// 月收入確認人員員編
    /// </summary>
    public string? MonthlyIncomeCheckUserId { get; set; } = String.Empty;
    public string? MonthlyIncomeCheckUserName { get; set; } = String.Empty;

    /// <summary>
    /// 正附卡：1. 正卡 2. 附卡 3. 正卡+附卡 4. 附卡2  5. 正卡+附卡2
    /// </summary>
    public CardOwner? CardOwner { get; set; }

    public string? CardOwnerName => CardOwner is null ? String.Empty : CardOwner.ToString();

    /// <summary>
    /// 長循分期戶 ： Y 、N
    /// </summary>
    public string? LongTerm { get; set; } = String.Empty;

    /// <summary>
    /// 姓名檢核 (正/附卡)
    /// 1. 於敦陽系統之姓名檢核
    /// </summary>
    public List<NameCheckedDto> NameCheckedList { get; set; } = new();

    /// <summary>
    /// 審查人員編號
    /// </summary>
    public List<ReviewerUserDto>? ReviewerUserList { get; set; }

    /// <summary>
    /// 評分結果：從授信政策科
    /// </summary>
    public List<CreditLimit_RatingAdviceDto> CreditLimit_RatingAdviceList { get; set; } = new();

    /// <summary>
    /// 分行客戶：用API方式以查詢分行平台資料，分行平台資訊來源為資訊部，Y 、N
    /// </summary>
    public string? IsBranchCustomer { get; set; } = String.Empty;

    /// <summary>
    /// 洗防風險等級：由AML-KYC提供風險等級
    /// </summary>
    public string? AMLRiskLevel { get; set; } = String.Empty;

    /// <summary>
    /// 核准人員員編
    /// </summary>
    public List<ApproveUserDto>? ApproveUserList { get; set; }

    /// <summary>
    /// 客服備註
    /// </summary>
    public string? CustomerServiceNotes { get; set; } = String.Empty;

    public List<CreditCheckCodeDto> CreditCheckCodeList { get; set; } = new();

    /// <summary>
    /// 當前經辦人員編號
    /// </summary>
    public string? CurrentHandleUserId { get; set; } = String.Empty;
    public string? CurrentHandleUserName { get; set; } = String.Empty;

    /// <summary>
    /// 是否為當前經辦
    /// </summary>
    public string? IsSelf { get; set; } = String.Empty;

    /// <summary>
    /// 原徵信人員
    /// </summary>
    public string? PreviousHandleUserId { get; set; }
    public string? PreviousHandleUserName { get; set; }

    /// <summary>
    /// 來源
    /// 1. ECARD
    /// 2. APP
    /// 3. 紙本
    /// </summary>
    public Source Source { get; set; }

    /// <summary>
    /// 來源
    /// 1. ECARD
    /// 2. APP
    /// 3. 紙本
    /// </summary>
    public string SourceName => Source.ToString();

    /// <summary>
    /// 卡片階段
    /// 1. 月收入確認
    /// 2. 人工徵審
    /// </summary>
    public CardStep? CardStep { get; set; }

    /// <summary>
    /// 卡片階段名稱
    /// </summary>
    public string CardStepName => CardStep is null ? String.Empty : CardStep.ToString();

    /// <summary>
    /// ECard申請書附件是否異常
    /// </summary>
    /// <value>
    /// Y：異常，N：無異常
    /// </value>
    /// <remarks>
    /// 紙本件固定為Null
    /// 原卡友會固定N
    /// </remarks>
    public string? ECard_AppendixIsException { get; set; }
}

public class Primary_BasicInfo
{
    /// <summary>
    /// 中文姓名
    /// </summary>
    public string CHName { get; set; }

    /// <summary>
    /// 身份證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 性別：1.男生 2. 女生
    /// </summary>
    public Sex? Sex { get; set; }

    public string? SexName => Sex is null ? String.Empty : Sex.ToString();

    /// <summary>
    /// 生日：民國格式為 YYYMMDD
    /// </summary>
    public string? BirthDay { get; set; }

    /// <summary>
    /// 是否學生身份 (Y: 是, N: 否)
    /// </summary>
    public string? IsStudent { get; set; }

    /// <summary>
    /// 英文名稱
    /// </summary>
    public string? ENName { get; set; }

    /// <summary>
    /// 身分證發證日期：民國格式為 YYYMMDD
    /// </summary>
    public string? IDIssueDate { get; set; }

    /// <summary>
    /// 身分證發證地點：關聯 SetUp_IDCardRenewalLocation
    /// </summary>
    public string? IDCardRenewalLocationCode { get; set; }

    public string? IDCardRenewalLocationName { get; set; }

    /// <summary>
    /// 身分證請領狀態：1. 初發, 2. 補發, 3. 換發
    /// </summary>
    public IDTakeStatus? IDTakeStatus { get; set; }

    public string IDTakeStatusName => IDTakeStatus is null ? String.Empty : IDTakeStatus.ToString();

    /// <summary>
    /// 國籍：關聯 SetUp_Citizenship
    /// </summary>
    public string? CitizenshipCode { get; set; }

    public string? CitizenshipName { get; set; }

    /// <summary>
    /// 出生地國籍：1. 中華民國 2. 其他
    /// </summary>
    public BirthCitizenshipCode? BirthCitizenshipCode { get; set; }

    public string? BirthCitizenshipName => BirthCitizenshipCode is null ? String.Empty : BirthCitizenshipCode.ToString();

    /// <summary>
    /// 出生地國籍_其他：當出生地國籍為其他時使用
    /// </summary>
    public string? BirthCitizenshipCodeOther { get; set; }

    /// <summary>
    /// 婚姻狀況：1. 已婚, 2. 未婚, 3. 其他
    /// </summary>
    public MarriageState? MarriageState { get; set; }
    public string? MarriageStateName => MarriageState is null ? String.Empty : MarriageState.ToString();

    /// <summary>
    /// 教育程度：1. 博士, 2. 碩士, 3. 大學, 4. 專科, 5. 高中職, 6. 其他
    /// </summary>
    public Education? Education { get; set; }
    public string? EducationName => Education is null ? String.Empty : Education.ToString();

    /// <summary>
    /// 畢業國小
    /// </summary>
    public string? GraduatedElementarySchool { get; set; }

    /// <summary>
    /// 是否申請數位卡
    /// 當 ApplyCardKind != 1 時，此欄位為N，反之為Y
    /// </summary>
    public string? IsApplyDigtalCard { get; set; }

    /// <summary>
    /// 是否轉換卡別，Y、N
    /// 紙本案件，申請書上可幫客人轉換卡別
    /// 對應紙本案件欄位正卡_換發別種卡別
    /// </summary>
    public string? IsConvertCard { get; set; }

    /// <summary>
    /// 行動電話
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// E-MAIL
    /// </summary>
    public string? EMail { get; set; }

    /// <summary>
    /// 帳單形式 (1：電子帳單、2：簡訊帳單、3：紙本帳單、4：LINE帳單
    /// )
    /// </summary>
    public BillType? BillType { get; set; }
    public string? BillTypeName => BillType is null ? String.Empty : BillType.ToString();

    #region 戶籍地址
    /// <summary>
    /// 戶籍_郵遞區號
    /// </summary>
    public string? Reg_ZipCode { get; set; }

    /// <summary>
    /// 戶籍_縣市
    /// </summary>
    public string? Reg_City { get; set; }

    /// <summary>
    /// 戶籍_區域
    /// </summary>
    public string? Reg_District { get; set; }

    /// <summary>
    /// 戶籍_街道
    /// </summary>
    public string? Reg_Road { get; set; }

    /// <summary>
    /// 戶籍_巷
    /// </summary>
    public string? Reg_Lane { get; set; }

    /// <summary>
    /// 戶籍_弄
    /// </summary>
    public string? Reg_Alley { get; set; }

    /// <summary>
    /// 戶籍_號
    /// </summary>
    public string? Reg_Number { get; set; }

    /// <summary>
    /// 戶籍_號2
    /// </summary>
    public string? Reg_SubNumber { get; set; }

    /// <summary>
    /// 戶籍_樓層
    /// </summary>
    public string? Reg_Floor { get; set; }

    /// <summary>
    /// 戶籍_完整地址
    /// </summary>
    public string? Reg_FullAddr { get; set; }

    /// <summary>
    /// 戶籍地址_其他
    /// </summary>
    public string? Reg_Other { get; set; }
    #endregion

    #region 居住地址
    /// <summary>
    /// 居住_郵遞區號
    /// </summary>
    public string? Live_ZipCode { get; set; }

    /// <summary>
    /// 居住_縣市
    /// </summary>
    public string? Live_City { get; set; }

    /// <summary>
    /// 居住_區域
    /// </summary>
    public string? Live_District { get; set; }

    /// <summary>
    /// 居住_街道
    /// </summary>
    public string? Live_Road { get; set; }

    /// <summary>
    /// 居住_巷
    /// </summary>
    public string? Live_Lane { get; set; }

    /// <summary>
    /// 居住_弄
    /// </summary>
    public string? Live_Alley { get; set; }

    /// <summary>
    /// 居住_號
    /// </summary>
    public string? Live_Number { get; set; }

    /// <summary>
    /// 居住_號2
    /// </summary>
    public string? Live_SubNumber { get; set; }

    /// <summary>
    /// 居住_樓層
    /// </summary>
    public string? Live_Floor { get; set; }

    /// <summary>
    /// 居住_完整地址
    /// </summary>
    public string? Live_FullAddr { get; set; }

    /// <summary>
    /// 居住地址_其他
    /// </summary>
    public string? Live_Other { get; set; }

    /// <summary>
    /// 居住地址
    /// </summary>
    public LiveAddressType? LiveAddressType { get; set; }
    public string? LiveAddressTypeName => LiveAddressType is null ? String.Empty : LiveAddressType.ToString();
    #endregion

    #region 帳單地址
    /// <summary>
    /// 帳單地址_郵遞區號
    /// </summary>
    public string? Bill_ZipCode { get; set; }

    /// <summary>
    /// 帳單地址_縣市
    /// </summary>
    public string? Bill_City { get; set; }

    /// <summary>
    /// 帳單地址_區域
    /// </summary>
    public string? Bill_District { get; set; }

    /// <summary>
    /// 帳單地址_街道
    /// </summary>
    public string? Bill_Road { get; set; }

    /// <summary>
    /// 帳單地址_巷
    /// </summary>
    public string? Bill_Lane { get; set; }

    /// <summary>
    /// 帳單地址_弄
    /// </summary>
    public string? Bill_Alley { get; set; }

    /// <summary>
    /// 帳單地址_號
    /// </summary>
    public string? Bill_Number { get; set; }

    /// <summary>
    /// 帳單地址_號2
    /// </summary>
    public string? Bill_SubNumber { get; set; }

    /// <summary>
    /// 帳單地址_樓層
    /// </summary>
    public string? Bill_Floor { get; set; }

    /// <summary>
    /// 帳單地址_完整地址
    /// </summary>
    public string? Bill_FullAddr { get; set; }

    /// <summary>
    /// 帳單地址_其他
    /// </summary>
    public string? Bill_Other { get; set; }

    /// <summary>
    /// 帳單地址
    /// </summary>
    public BillAddressType? BillAddressType { get; set; }
    public string? BillAddressTypeName => BillAddressType is null ? String.Empty : BillAddressType.ToString();
    #endregion

    #region 寄卡地址
    /// <summary>
    /// 寄卡地址_郵遞區號
    /// </summary>
    public string? SendCard_ZipCode { get; set; }

    /// <summary>
    /// 寄卡地址_縣市
    /// </summary>
    public string? SendCard_City { get; set; }

    /// <summary>
    /// 寄卡地址_區域
    /// </summary>
    public string? SendCard_District { get; set; }

    /// <summary>
    /// 寄卡地址_街道
    /// </summary>
    public string? SendCard_Road { get; set; }

    /// <summary>
    /// 寄卡地址_巷
    /// </summary>
    public string? SendCard_Lane { get; set; }

    /// <summary>
    /// 寄卡地址_弄
    /// </summary>
    public string? SendCard_Alley { get; set; }

    /// <summary>
    /// 寄卡地址_號
    /// </summary>
    public string? SendCard_Number { get; set; }

    /// <summary>
    /// 寄卡地址_號2
    /// </summary>
    public string? SendCard_SubNumber { get; set; }

    /// <summary>
    /// 寄卡地址_樓層
    /// </summary>
    public string? SendCard_Floor { get; set; }

    /// <summary>
    /// 寄卡地址_完整地址
    /// </summary>
    public string? SendCard_FullAddr { get; set; }

    /// <summary>
    /// 寄卡地址_其他
    /// </summary>
    public string? SendCard_Other { get; set; }

    /// <summary>
    /// 寄卡地址
    /// </summary>
    public SendCardAddressType? SendCardAddressType { get; set; }
    public string? SendCardAddressTypeName => SendCardAddressType is null ? String.Empty : SendCardAddressType.ToString();
    #endregion

    /// <summary>
    /// 戶籍電話
    /// </summary>
    public string? HouseRegPhone { get; set; }

    /// <summary>
    /// 居住電話
    /// </summary>
    public string? LivePhone { get; set; }

    /// <summary>
    /// 居住地所有權人：1. 本人, 2. 配偶, 3. 父母, 4. 子女, 5. 親戚, 6. 朋友, 7. 其他
    /// </summary>
    public LiveOwner? LiveOwner { get; set; }
    public string LiveOwnerName => LiveOwner is null ? String.Empty : LiveOwner.ToString();

    /// <summary>
    /// 居住年數
    /// </summary>
    public int? LiveYear { get; set; }

    /// <summary>
    /// 推廣單位
    /// </summary>
    public string? PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    public string? PromotionUser { get; set; }

    /// <summary>
    /// 進件方式，紙本案件才有
    /// </summary>
    public AcceptType? AcceptType { get; set; }
    public string? AcceptTypeName => AcceptType is null ? String.Empty : AcceptType.ToString();

    /// <summary>
    /// 安麗編號
    /// </summary>
    public string? AnliNo { get; set; }

    /// <summary>
    /// 居留證發證日期 (格式: YYYYMMDD)
    /// </summary>
    public string? ResidencePermitIssueDate { get; set; }

    /// <summary>
    /// 護照號碼
    /// </summary>
    public string? PassportNo { get; set; }

    /// <summary>
    /// 護照日期 (格式: YYYYMMDD)
    /// </summary>
    public string? PassportDate { get; set; }

    /// <summary>
    /// 外籍人士停留期限 (格式: YYYYMMDD)
    /// </summary>
    public string? ExpatValidityPeriod { get; set; }

    /// <summary>
    /// 敦陽系統黑名單查詢記錄 (Y/N)
    /// </summary>
    public string? IsDunyangBlackList { get; set; }

    /// <summary>
    /// 是否FATCA身份 (Y/N)，當國籍 = 美國時候預設為 Y
    /// </summary>
    public string? IsFATCAIdentity { get; set; }

    /// <summary>
    /// 社會安全號碼，FATCA身份=Y，徵審人員會去跟客人要此值
    /// </summary>
    public string? SocialSecurityCode { get; set; }

    /// <summary>
    /// 姓名檢核理由代碼，可為複數以「,」分割，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 要有理由碼
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
    public List<NameCheckdReasonDto>? NameCheckedReasonCodeList { get; set; }

    /// <summary>
    /// 當前或曾為PEP身分 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public string? ISRCAForCurrentPEP { get; set; }

    /// <summary>
    /// 卸任PEP種類：1. 國內 2.國外 3.國際組織 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public ResignPEPKind? ResignPEPKind { get; set; }

    public string? ResignPEPKindName => ResignPEPKind is null ? String.Empty : ResignPEPKind.ToString();

    /// <summary>
    /// 擔任PEP範圍:
    /// 不滿2年
    /// 滿兩年但不滿四年
    /// 滿4年 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public PEPRange? PEPRange { get; set; }

    public string? PEPRangeName => PEPRange is null ? String.Empty : PEPRange.ToString();

    /// <summary>
    /// 現任職位是否與PEP職位相關 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public string? IsCurrentPositionRelatedPEPPosition { get; set; }

    /// <summary>
    /// 居留證背面號碼：前兩碼大寫英文 + 8 碼數字，範例ＹＺ80000001
    /// </summary>
    public string? ResidencePermitBackendNum { get; set; }

    /// <summary>
    /// 是否為永久居留證
    /// 1. 配合法遵部政策，新增欄位。
    /// 2. 欄位用拉霸方式選擇，Y/N，預設為N。
    /// 3. 如為{N}，則須鍵入外籍人士指定效期。
    ///
    /// </summary>
    public string? IsForeverResidencePermit { get; set; }

    /// <summary>
    /// 居留證期限：格式 YYYYMMDD
    /// </summary>
    public string? ResidencePermitDeadline { get; set; }

    /// <summary>
    /// 舊照查驗
    /// </summary>
    public string? OldCertificateVerified { get; set; }

    /// <summary>
    /// 子女人數
    /// </summary>
    public int? ChildrenCount { get; set; }
}

public class Primary_JobInfo
{
    /// <summary>
    /// 公司名稱
    /// </summary>
    public string? CompName { get; set; }

    /// <summary>
    /// 公司統一編號
    /// </summary>
    public string? CompID { get; set; }

    /// <summary>
    /// 公司職稱
    /// </summary>
    public string? CompJobTitle { get; set; }

    /// <summary>
    /// 公司年資
    /// </summary>
    public int? CompSeniority { get; set; }

    /// <summary>
    /// 公司_郵遞區號
    /// </summary>
    public string? Comp_ZipCode { get; set; }

    /// <summary>
    /// 公司_縣市
    /// </summary>
    public string? Comp_City { get; set; }

    /// <summary>
    /// 公司_區域
    /// </summary>
    public string? Comp_District { get; set; }

    /// <summary>
    /// 公司_街道
    /// </summary>
    public string? Comp_Road { get; set; }

    /// <summary>
    /// 公司_巷
    /// </summary>
    public string? Comp_Lane { get; set; }

    /// <summary>
    /// 公司_弄
    /// </summary>
    public string? Comp_Alley { get; set; }

    /// <summary>
    /// 公司_號
    /// </summary>
    public string? Comp_Number { get; set; }

    /// <summary>
    /// 公司_號2
    /// </summary>
    public string? Comp_SubNumber { get; set; }

    /// <summary>
    /// 公司_樓層
    /// </summary>
    public string? Comp_Floor { get; set; }

    /// <summary>
    /// 公司_完整地址
    /// </summary>
    public string? Comp_FullAddr { get; set; }

    /// <summary>
    /// 公司_其他
    /// </summary>
    public string? Comp_Other { get; set; }

    /// <summary>
    /// 公司電話
    /// </summary>
    public string? CompPhone { get; set; }

    /// <summary>
    /// AML職業別_版本
    /// 方便前端版本來顯示AML職業別下拉選單的資料
    /// </summary>
    public string? AMLProfessionCode_Version { get; set; }

    /// <summary>
    /// AML職業別：關聯  SetUp_AMLProfession
    /// </summary>
    public string? AMLProfessionCode { get; set; }
    public string? AMLProfessionName { get; set; }

    /// <summary>
    /// AML職業別_其他
    /// </summary>
    public string? AMLProfessionOther { get; set; }

    /// <summary>
    /// AML職級別：關聯  SetUp_AMLJobLevel
    /// </summary>
    public string? AMLJobLevelCode { get; set; }
    public string? AMLJobLevelName { get; set; }

    /// <summary>
    /// 公司行業別
    /// 1. 營造/製造/運輸業
    /// 2. 一般商業
    /// 3. 休閒/娛樂/服務業
    /// 4. 軍警消防業
    /// 5. 非營利團體
    /// 6. 學生
    /// 7. 自由業/其他
    /// </summary>
    public CompTrade? CompTrade { get; set; }
    public string? CompTradeName => CompTrade.HasValue ? CompTrade.ToString() : String.Empty;

    /// <summary>
    /// 公司職級別
    /// 1. 駕駛人員
    /// 2. 服務生/門市人員
    /// 3. 專業人員
    /// 4. 專業技工
    /// 5. 業務人員
    /// 6. 一般職員
    /// 7. 主管階層
    /// 8. 股東/董事/負責人
    /// 9. 家管/其他
    /// </summary>
    public CompJobLevel? CompJobLevel { get; set; }
    public string? CompJobLevelName => CompJobLevel.HasValue ? CompJobLevel.ToString() : String.Empty;

    /// <summary>
    /// 所得及資金來源 (請參考Setup_MainIncomeAndFund表格、需符合Paperless「主要所得及資金來源勾選」，多選選項以逗號(,)區隔)
    /// </summary>
    public string? MainIncomeAndFundCodes { get; set; }
    public string? MainIncomeAndFundNames { get; set; }

    /// <summary>
    /// 主要收入_其他
    /// </summary>
    public string? MainIncomeAndFundOther { get; set; }

    /// <summary>
    /// 現職月收入(元)
    /// </summary>
    public int? CurrentMonthIncome { get; set; }

    /// <summary>
    /// 徵信代碼：如A02為原卡友
    /// </summary>
    public string? CreditCheckCode { get; set; }

    public string? CreditCheckName { get; set; }

    /// <summary>
    /// 部門名稱
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// 到職日期 (格式: YYYMMDD)
    /// </summary>
    public string? EmploymentDate { get; set; }
}

public class Primary_StudentInfo
{
    /// <summary>
    /// 是否學生身份 (Y: 是, N: 否)
    /// </summary>
    public string? IsStudent { get; set; }

    /// <summary>
    /// 學生就讀學校 (IsStudent = Y 才有值)
    /// </summary>
    public string? StudSchool { get; set; }

    /// <summary>
    /// 學生預定畢業日期 (IsStudent = Y 才有值, 格式: YYYYMMDD)
    /// </summary>
    public string? StudScheduledGraduationDate { get; set; }

    /// <summary>
    /// 家長姓名
    /// </summary>
    public string? ParentName { get; set; }

    /// <summary>
    /// 家長電話 (可以行動電話或者家電或公司電話)
    /// </summary>
    public string? ParentPhone { get; set; }

    /// <summary>
    /// 學生申請人與本人關係 (1: 父母, 2: 學生)
    /// </summary>
    public StudentApplicantRelationship? StudentApplicantRelationship { get; set; }
    public string? StudentApplicantRelationshipName => StudentApplicantRelationship is null ? String.Empty : StudentApplicantRelationship.ToString();

    #region 家長居住地址
    /// <summary>
    /// 家長居住_郵遞區號
    /// </summary>
    public string? ParentLive_ZipCode { get; set; }

    /// <summary>
    /// 家長居住_縣市
    /// </summary>
    public string? ParentLive_City { get; set; }

    /// <summary>
    /// 家長居住_區域
    /// </summary>
    public string? ParentLive_District { get; set; }

    /// <summary>
    /// 家長居住_街道
    /// </summary>
    public string? ParentLive_Road { get; set; }

    /// <summary>
    /// 家長居住_巷
    /// </summary>
    public string? ParentLive_Lane { get; set; }

    /// <summary>
    /// 家長居住_弄
    /// </summary>
    public string? ParentLive_Alley { get; set; }

    /// <summary>
    /// 家長居住_號
    /// </summary>
    public string? ParentLive_Number { get; set; }

    /// <summary>
    /// 家長居住_號2
    /// </summary>
    public string? ParentLive_SubNumber { get; set; }

    /// <summary>
    /// 家長居住_樓層
    /// </summary>
    public string? ParentLive_Floor { get; set; }

    /// <summary>
    /// 家長居住_完整地址
    ///  </summary>
    public string? ParentLive_FullAddr { get; set; }

    /// <summary>
    /// 家長居住_其他
    ///  </summary>
    public string? ParentLive_Other { get; set; }

    /// <summary>
    /// 家長地址類型
    /// </summary>
    public ParentLiveAddressType? ParentLiveAddressType { get; set; }
    public string? ParentLiveAddressTypeName => ParentLiveAddressType is null ? String.Empty : ParentLiveAddressType.ToString();
    #endregion
}

public class Primary_WebCardInfo
{
    /// <summary>
    /// 使用者來源IP位置 (E-CARD填寫)
    /// </summary>
    public string? UserSourceIP { get; set; }

    /// <summary>
    /// OTP時間 (E-CARD填寫)
    /// </summary>
    public DateTime? OTPTime { get; set; }

    /// <summary>
    /// OTP手機號碼 (E-CARD填寫)
    /// </summary>
    public string? OTPMobile { get; set; }

    /// <summary>
    /// 相同IP比對結果
    ///
    /// 1. (Y/N)
    /// 2. 比對 UserSourceIP 於24小時內是否重複申請案件
    /// </summary>
    public string? SameIPChecked { get; set; }

    /// <summary>
    /// 網路件手機號碼相同 (Y: 是, N: 否)
    /// </summary>
    public string? SameWebCaseMobileChecked { get; set; }

    /// <summary>
    /// 網路件E-Mail相同 (Y: 是, N: 否)
    /// </summary>
    public string? SameWebCaseEmailChecked { get; set; }

    /// <summary>
    /// 與行內IP相同 (Y: 是, N: 否)
    /// </summary>
    public string? IsEqualInternalIP { get; set; }
}

public class Primary_BankTraceInfo
{
    /// <summary>
    /// 短時間內ID重複申請 (Y: 是, N: 否)
    /// </summary>
    public string? ShortTimeIDChecked { get; set; }

    /// <summary>
    /// 行內Email相同 (Y: 是, N: 否)
    /// </summary>
    public string? InternalEmailSame_Flag { get; set; }

    /// <summary>
    /// 行內手機相同 (Y: 是, N: 否)
    /// </summary>
    public string? InternalMobileSame_Flag { get; set; }
}

public class Primary_ActivityInfo
{
    /// <summary>
    /// 首刷禮代碼 (E-CARD填寫)
    /// </summary>
    public string? FirstBrushingGiftCode { get; set; }

    /// <summary>
    /// 本人是否同意提供資料予聯名認同集團 (Y: 是, N: 否)
    /// </summary>
    public string? IsAgreeDataOpen { get; set; }

    /// <summary>
    /// 是否綁定消費通知 (Y: 是, N: 否)
    /// </summary>
    public string? IsPayNoticeBind { get; set; }

    /// <summary>
    /// 專案代號 (文彬組在使用的，對徵審系統無用意)
    /// </summary>
    public string? ProjectCode { get; set; }

    /// <summary>
    /// 是否同意提供資料於第三人行銷 (Y: 是, N: 否)
    /// </summary>
    public string? IsAgreeMarketing { get; set; }

    /// <summary>
    /// 是否同意悠遊卡自動加值預設開啟 (Y: 是, N: 否)
    /// </summary>
    public string? IsAcceptEasyCardDefaultBonus { get; set; }

    /// <summary>
    /// 電子化約定條款
    /// </summary>
    public string? ElecCodeId { get; set; }

    /// <summary>
    /// 年費收取方式代碼
    /// </summary>
    public string? AnnualFeePaymentType { get; set; }

    /// <summary>
    /// 年費收取方式名稱
    /// </summary>
    public string? AnnualFeePaymentTypeName { get; set; }
}

public class Supplementary
{
    /// <summary>
    /// 中文姓名
    /// </summary>
    public string? CHName { get; set; }

    /// <summary>
    /// 身分證字號
    /// PK
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// 性別
    /// 1 = 男
    /// 2 = 女
    /// </summary>
    public Sex? Sex { get; set; }
    public string? SexName => Sex is null ? String.Empty : Sex.ToString();

    /// <summary>
    /// 出生年月日
    /// 目前系統是請他提供YYYYMMDD
    /// </summary>
    public string? BirthDay { get; set; }

    /// <summary>
    /// 英文姓名
    /// </summary>
    public string? ENName { get; set; }

    /// <summary>
    /// 婚姻狀況
    /// 1.已婚
    /// 2.未婚
    /// 3.其他
    /// </summary>
    public MarriageState? MarriageState { get; set; }
    public string? MarriageStateName => MarriageState is null ? String.Empty : MarriageState.ToString();

    /// <summary>
    /// 與正卡人關係
    /// 1. 配偶
    /// 2. 父母
    /// 3. 子女
    /// 4. 兄弟姊妹
    /// 5. 配偶父母
    /// </summary>
    public ApplicantRelationship? ApplicantRelationship { get; set; }
    public string? ApplicantRelationshipName => ApplicantRelationship is null ? String.Empty : ApplicantRelationship.ToString();

    /// <summary>
    /// 國籍
    /// 由徵審系統提供值給E-CARD
    /// 關聯 SetUp_Citizenship
    /// </summary>
    public string? CitizenshipCode { get; set; }
    public string? CitizenshipName { get; set; }

    /// <summary>
    /// 身分證發證日期
    /// 民國YYYMMDD
    /// </summary>
    public string? IDIssueDate { get; set; }

    /// <summary>
    /// 身分證發證地點
    /// 關聯 SetUp_IDCardRenewalLocation
    /// </summary>
    public string? IDCardRenewalLocationCode { get; set; }
    public string? IDCardRenewalLocationName { get; set; }

    /// <summary>
    /// 身分證請領狀態
    /// 1.初發
    /// 2.補發
    /// 3.換發
    /// </summary>
    public IDTakeStatus? IDTakeStatus { get; set; }
    public string IDTakeStatusName => IDTakeStatus is null ? String.Empty : IDTakeStatus.ToString();

    /// <summary>
    /// 出生地國籍
    /// 1. 中華民國
    /// 2. 其他
    /// </summary>
    public BirthCitizenshipCode? BirthCitizenshipCode { get; set; }
    public string? BirthCitizenshipName => BirthCitizenshipCode is null ? String.Empty : BirthCitizenshipCode.ToString();

    /// <summary>
    /// 出生地國籍_其他
    /// 當出生地國及其他時候需要有值
    /// 關聯 SetUp_Citizenship
    /// </summary>
    public string? BirthCitizenshipCodeOther { get; set; }

    /// <summary>
    /// 出生地國籍_其他名稱
    /// 當出生地國及其他時候需要有值
    /// 關聯 SetUp_Citizenship
    /// </summary>
    public string? BirthCitizenshipCodeOtherName { get; set; }

    /// <summary>
    /// 是否FATCA身份
    /// Y/N
    /// </summary>
    public string? IsFATCAIdentity { get; set; }

    /// <summary>
    /// 社會安全碼
    /// FATCA身份=Y，徵審人員會去跟客人要此值
    /// </summary>
    public string? SocialSecurityCode { get; set; }

    /// <summary>
    /// 是否為永久居留證
    /// 配合法遵部政策，新增欄位。
    /// 欄位用拉霸方式選擇，Y/N，預設為N。
    /// 如為{N}，則須鍵入外籍人士指定效期。
    ///
    /// </summary>
    public string? IsForeverResidencePermit { get; set; }

    /// <summary>
    /// 居留證發證日期
    /// YYYYMMDD
    /// </summary>
    public string? ResidencePermitIssueDate { get; set; }

    /// <summary>
    /// 居留證期限
    /// 格式 YYYYMMDD
    /// </summary>
    public string? ResidencePermitDeadline { get; set; }

    /// <summary>
    /// 居留證背面號碼
    /// 前兩碼大寫英文 + 8 碼數字，範例ＹＺ80000001
    /// </summary>
    public string? ResidencePermitBackendNum { get; set; }

    /// <summary>
    /// 護照號碼
    /// </summary>
    public string? PassportNo { get; set; }

    /// <summary>
    /// 護照日期
    /// YYYYMMDD
    /// </summary>
    public string? PassportDate { get; set; }

    /// <summary>
    /// 外籍人士指定效期
    /// YYYYMMDD
    /// </summary>
    public string? ExpatValidityPeriod { get; set; }

    /// <summary>
    /// 舊照查驗
    /// Y：是、N：否
    /// </summary>
    public string? OldCertificateVerified { get; set; }

    /// <summary>
    /// 公司名稱
    /// </summary>
    public string? CompName { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    public string? CompJobTitle { get; set; }

    /// <summary>
    /// 公司電話
    /// </summary>
    public string? CompPhone { get; set; }

    /// <summary>
    /// 居住電話
    /// </summary>
    public string? LivePhone { get; set; }

    /// <summary>
    /// 行動電話
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 居住地址類型
    /// </summary>
    public ResidenceType? ResidenceType { get; set; }

    /// <summary>
    /// 居住地址類型名稱
    /// </summary>
    public string? ResidenceTypeName => ResidenceType.ToString() ?? string.Empty;

    /// <summary>
    /// 居住_郵遞區號
    /// </summary>
    public string? Live_ZipCode { get; set; }

    /// <summary>
    /// 居住_縣市
    /// </summary>
    public string? Live_City { get; set; }

    /// <summary>
    /// 居住_區域
    /// </summary>
    public string? Live_District { get; set; }

    /// <summary>
    /// 居住_街道
    /// </summary>
    public string? Live_Road { get; set; }

    /// <summary>
    /// 居住_巷
    /// </summary>
    public string? Live_Lane { get; set; }

    /// <summary>
    /// 居住_弄
    /// </summary>
    public string? Live_Alley { get; set; }

    /// <summary>
    /// 居住_號
    /// </summary>
    public string? Live_Number { get; set; }

    /// <summary>
    /// 居住_之號
    /// </summary>
    public string? Live_SubNumber { get; set; }

    /// <summary>
    /// 居住_樓層
    /// </summary>
    public string? Live_Floor { get; set; }

    /// <summary>
    /// 居住_完整地址
    /// </summary>
    public string? Live_FullAddr { get; set; }

    /// <summary>
    /// 居住_其他
    /// </summary>
    public string? Live_Other { get; set; }

    /// <summary>
    /// 寄卡地址類型
    /// </summary>
    public ShippingCardAddressType? ShippingCardAddressType { get; set; }

    /// <summary>
    /// 寄卡地址類型名稱
    /// </summary>
    public string? ShippingCardAddressTypeName => ShippingCardAddressType.ToString() ?? string.Empty;

    /// <summary>
    /// 寄卡_郵遞區號
    /// </summary>
    public string? SendCard_ZipCode { get; set; }

    /// <summary>
    /// 寄卡_縣市
    /// </summary>
    public string? SendCard_City { get; set; }

    /// <summary>
    /// 寄卡_區域
    /// </summary>
    public string? SendCard_District { get; set; }

    /// <summary>
    /// 寄卡_街道
    /// </summary>
    public string? SendCard_Road { get; set; }

    /// <summary>
    /// 寄卡_巷
    /// </summary>
    public string? SendCard_Lane { get; set; }

    /// <summary>
    /// 寄卡_弄
    /// </summary>
    public string? SendCard_Alley { get; set; }

    /// <summary>
    /// 寄卡_號
    /// </summary>
    public string? SendCard_Number { get; set; }

    /// <summary>
    /// 寄卡_之號
    /// </summary>
    public string? SendCard_SubNumber { get; set; }

    /// <summary>
    /// 寄卡_樓層
    /// </summary>
    public string? SendCard_Floor { get; set; }

    /// <summary>
    /// 寄卡_完整地址
    /// </summary>
    public string? SendCard_FullAddr { get; set; }

    /// <summary>
    /// 寄卡_其他
    /// </summary>
    public string? SendCard_Other { get; set; }

    /// <summary>
    /// 敦陽系統黑名單是否相符
    /// Y：是、N：否
    /// 由行員確認
    /// </summary>
    public string? IsDunyangBlackList { get; set; }

    /// <summary>
    /// 姓名檢核理由代碼
    /// 可為複數以「,」分割，姓名檢核 = Y 要有理由碼
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
    public string? NameCheckedReasonCodes { get; set; }

    /// <summary>
    /// 使用者類型
    /// PK
    /// 固定 2 = 附卡人
    /// </summary>
    public UserType? UserType { get; set; }

    /// <summary>
    /// 使用者類型
    /// PK
    /// 固定 2 = 附卡人
    /// </summary>
    public string? UserTypeName => UserType.ToString();

    /// <summary>
    /// 姓名檢核理由代碼，可為複數以「,」分割，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 要有理由碼
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
    public List<NameCheckdReasonDto>? NameCheckedReasonCodeList { get; set; }

    /// <summary>
    /// 當前或曾為PEP身分 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public string? ISRCAForCurrentPEP { get; set; }

    /// <summary>
    /// 卸任PEP種類：1. 國內 2.國外 3.國際組織 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public ResignPEPKind? ResignPEPKind { get; set; }

    public string? ResignPEPKindName => ResignPEPKind is null ? String.Empty : ResignPEPKind.ToString();

    /// <summary>
    /// 擔任PEP範圍:
    /// 不滿2年
    /// 滿兩年但不滿四年
    /// 滿4年 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public PEPRange? PEPRange { get; set; }

    public string? PEPRangeName => PEPRange is null ? String.Empty : PEPRange.ToString();

    /// <summary>
    /// 現任職位是否與PEP職位相關 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫
    /// </summary>
    public string? IsCurrentPositionRelatedPEPPosition { get; set; }
}

public class CardStatusDto
{
    /// <summary>
    /// 卡片狀態，查看附件-卡片狀態碼
    /// </summary>
    public CardStatus CardStatus { get; set; }

    public string CardStatusName => CardStatus.ToString();

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }
}

public class ApplyCardTypeDto
{
    /// <summary>
    /// 申請卡別：以 " / "串接，如JA00/JC00
    /// </summary>
    public string ApplyCardType { get; set; }
    public string ApplyCardTypeName { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }
}

public class Focus1CheckedDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 命中項目 以 " / "串接，如 B/C
    /// </summary>
    public string Focus1Hit { get; set; }

    /// <summary>
    /// 關注名單1 是否命中
    /// </summary>
    public string Focus1Checked { get; set; }
}

public class Focus2CheckedDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 命中項目 以 " / "串接，如 B/C
    /// </summary>
    public string Focus2Hit { get; set; }

    /// <summary>
    /// 關注名單2 是否命中
    /// </summary>
    public string Focus2Checked { get; set; }
}

public class CreditCheckCodeDto
{
    public string SeqNo { get; set; }

    /// <summary>
    /// 徵信代碼：如A02為原卡友
    /// </summary>
    public string? CreditCheckCode { get; set; }

    /// <summary>
    /// 徵信名稱
    /// </summary>
    public string? CreditCheckName { get; set; }
}

public class FamilyMessageCheckedDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 家族訊息正/附卡 (Y: 是, N: 否)
    /// </summary>
    public string FamilyMessageChecked { get; set; }
}

public class IsOriginalCardholderDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 是否為原持卡人 (Y: 是, N: 否)
    /// </summary>
    public string IsOriginalCardholder { get; set; }
}

public class Checked929Dto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 929-正/附卡 (Y: 是, N: 否)
    /// </summary>
    public string Checked929 { get; set; }
}

public class IDCheckResultCheckedDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 身分證驗證結果-正/附卡
    /// </summary>
    public string IDCheckResultChecked { get; set; }
}

public class OriginCardholderJCICNotesDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 原持卡人JCIC註記 Y 、N
    /// </summary>
    public string OriginCardholderJCICNotes { get; set; }
}

public class NameCheckedDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 姓名驗證結果-正/附卡  Y 、N
    /// </summary>
    public string NameChecked { get; set; }
}

public class CreditLimit_RatingAdviceDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 信用評分
    /// </summary>
    public int? CreditLimit_RatingAdvice { get; set; }
}

public class NameCheckdReasonDto
{
    public NameCheckedReasonCode NameCheckedReasonCode { get; set; }
    public string NameCheckdReasonName => NameCheckedReasonCode.ToString();
}

public class ApproveUserDto
{
    /// <summary>
    /// 核准人員ID
    /// </summary>
    public string? ApproveUserId { get; set; }

    /// <summary>
    /// 核准人員姓名
    /// </summary>
    public string? ApproveUserName { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 使用者類型名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();

    /// <summary>
    /// 申請卡別
    /// </summary>
    public string ApplyCardType { get; set; }
}

public class ReviewerUserDto
{
    /// <summary>
    /// 審查人員ID
    /// </summary>
    public string? ReviewerUserId { get; set; }

    /// <summary>
    /// 審查人員姓名
    /// </summary>
    public string? ReviewerUserName { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 使用者類型名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();

    /// <summary>
    /// 申請卡別
    /// </summary>
    public string ApplyCardType { get; set; }
}

public class KYCInfo
{
    /// <summary>
    /// 洗防風險等級
    /// </summary>
    public string? AMLRiskLevel { get; set; }

    /// <summary>
    /// KYC_回傳代碼
    /// </summary>
    [JsonPropertyName("kyc_RtnCode")]
    public string? KYC_RtnCode { get; set; }

    /// <summary>
    /// KYC_查詢時間
    /// </summary>
    [JsonPropertyName("kyc_QueryTime")]
    public DateTime? KYC_QueryTime { get; set; }

    /// <summary>
    /// KYC 回傳訊息
    /// </summary>
    [JsonPropertyName("kyc_Message")]
    public string? KYC_Message { get; set; }

    /// <summary>
    /// KYC加強審核版本
    ///
    /// - 利用此欄位決定KYC加強審核版本主要用於套印加強審核檔案
    /// - 由系統參數決定
    ///
    /// 目前版本列表:
    /// 20210501
    /// </summary>
    [JsonPropertyName("kyc_StrongReVersion")]
    public string? KYC_StrongReVersion { get; set; }

    /// <summary>
    /// 加強審核執行狀態
    ///
    /// - 狀態為 不需檢核 或 核准,才能核卡，否則就需要做加強審核簽核才行
    ///
    /// 定義值
    /// 1. 未送審 (經辦)
    /// 2. 送審中 (主管)
    /// 3. 核准 (可核卡) (經辦)
    /// 4. 駁回 (經辦)
    /// 5. 不需檢核
    ///
    /// </summary>
    [JsonPropertyName("kyc_StrongReStatus")]
    public KYCStrongReStatus? KYC_StrongReStatus { get; set; }

    /// <summary>
    /// 加強審核執行狀態
    ///
    /// - 狀態為 不需檢核 或 核准,才能核卡，否則就需要做加強審核簽核才行
    ///
    /// 定義值
    /// 1. 未送審 (經辦)
    /// 2. 送審中 (主管)
    /// 3. 核准 (可核卡) (經辦)
    /// 4. 駁回 (經辦)
    /// 5. 不需檢核
    ///
    /// </summary>
    [JsonPropertyName("kyc_StrongReStatusName")]
    public string? KYC_StrongReStatusName => KYC_StrongReStatus.HasValue ? KYC_StrongReStatus.Value.ToString() : null;

    /// <summary>
    /// KYC_經辦
    /// </summary>
    [JsonPropertyName("kyc_Handler")]
    public string? KYC_Handler { get; set; }

    /// <summary>
    /// KYC_經辦_簽核時間
    /// </summary>
    [JsonPropertyName("kyc_Handler_SignTime")]
    public DateTime? KYC_Handler_SignTime { get; set; }

    /// <summary>
    /// KYC_覆核
    /// </summary>
    [JsonPropertyName("kyc_Reviewer")]
    public string? KYC_Reviewer { get; set; }

    /// <summary>
    /// KYC_覆核_簽核時間
    /// </summary>
    [JsonPropertyName("kyc_Reviewer_SignTime")]
    public DateTime? KYC_Reviewer_SignTime { get; set; }

    /// <summary>
    /// KYC_防洗錢主管
    /// </summary>
    [JsonPropertyName("kyc_KYCManager")]
    public string? KYC_KYCManager { get; set; }

    /// <summary>
    /// KYC_防洗錢主管_簽核時間
    /// </summary>
    [JsonPropertyName("kyc_KYCManager_SignTime")]
    public DateTime? KYC_KYCManager_SignTime { get; set; }

    /// <summary>
    /// KYC_加強審核詳細資訊Json
    /// 如果不是假值，在顯示KYC加強審核區塊
    /// 前端自行 JSON.parse 解析
    /// </summary>
    [JsonPropertyName("kyc_StrongReDetailJson")]
    public string? KYC_StrongReDetailJson { get; set; }

    /// <summary>
    /// KYC_建議核准
    ///
    /// - Y/N
    /// </summary>
    [JsonPropertyName("kyc_Suggestion")]
    public string? KYC_Suggestion { get; set; }

    /// <summary>
    /// 是否顯示加強審核區塊
    /// </summary>
    public string? IsShowKYCStrongReview { get; set; }
}

public class IsRepeatApplyDto
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 是否為重覆進件 (Y: 是, N: 否)
    /// </summary>
    public string IsRepeatApply { get; set; }
}
