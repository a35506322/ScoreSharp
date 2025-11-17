namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementaryInfoByApplyNo;

public class UpdateSupplementaryInfoByApplyNoRequest
{
    /// <summary>
    ///  申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    [Required]
    public string ApplyNo { get; set; }

    /// <summary>
    ///  中文姓名
    /// </summary>
    [Display(Name = "中文姓名")]
    public string? CHName { get; set; }

    /// <summary>
    ///  身分證字號
    /// </summary>
    [Display(Name = "身分證字號")]
    public string? ID { get; set; }

    /// <summary>
    ///  性別
    /// </summary>
    [Display(Name = "性別")]
    public Sex? Sex { get; set; }

    /// <summary>
    ///  出生年月日
    /// </summary>
    [Display(Name = "出生年月日")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? BirthDay { get; set; }

    /// <summary>
    ///  英文姓名
    /// </summary>
    [Display(Name = "英文姓名")]
    public string? ENName { get; set; }

    /// <summary>
    ///  婚姻狀況
    /// </summary>
    [Display(Name = "婚姻狀況")]
    public MarriageState? MarriageState { get; set; }

    /// <summary>
    ///  與正卡人關係
    /// </summary>
    [Display(Name = "與正卡人關係")]
    public ApplicantRelationship? ApplicantRelationship { get; set; }

    /// <summary>
    ///  國籍
    /// </summary>
    [Display(Name = "國籍")]
    public string? CitizenshipCode { get; set; }

    /// <summary>
    ///  身分證發證日期
    /// </summary>
    [Display(Name = "身分證發證日期")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? IDIssueDate { get; set; }

    /// <summary>
    ///  身分證發證地點
    /// </summary>
    [Display(Name = "身分證發證地點")]
    public string? IDCardRenewalLocationCode { get; set; }

    /// <summary>
    ///  身分證請領狀態
    /// </summary>
    [Display(Name = "身分證請領狀態")]
    public IDTakeStatus? IDTakeStatus { get; set; }

    /// <summary>
    ///  出生地國籍
    /// </summary>
    [Display(Name = "出生地國籍")]
    public BirthCitizenshipCode? BirthCitizenshipCode { get; set; }

    /// <summary>
    ///  出生地其他
    /// </summary>
    [Display(Name = "出生地其他")]
    public string? BirthCitizenshipCodeOther { get; set; }

    /// <summary>
    ///  是否FATCA身份
    /// </summary>
    [Display(Name = "是否FATCA身份")]
    public string? IsFATCAIdentity { get; set; }

    /// <summary>
    ///  社會安全碼
    /// </summary>
    [Display(Name = "社會安全碼")]
    public string? SocialSecurityCode { get; set; }

    /// <summary>
    ///  是否為永久居留證
    /// </summary>
    [Display(Name = "是否為永久居留證")]
    public string? IsForeverResidencePermit { get; set; }

    /// <summary>
    ///  居留證發證日期
    /// </summary>
    [Display(Name = "居留證發證日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? ResidencePermitIssueDate { get; set; }

    /// <summary>
    ///  居留證期限
    /// </summary>
    [Display(Name = "居留證期限")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? ResidencePermitDeadline { get; set; }

    /// <summary>
    ///  居留證背面號碼
    /// </summary>
    [Display(Name = "居留證背面號碼")]
    public string? ResidencePermitBackendNum { get; set; }

    /// <summary>
    ///  護照號碼
    /// </summary>
    [Display(Name = "護照號碼")]
    public string? PassportNo { get; set; }

    /// <summary>
    ///  護照日期
    /// </summary>
    [Display(Name = "護照日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? PassportDate { get; set; }

    /// <summary>
    ///  外籍人士指定效期
    /// </summary>
    [Display(Name = "外籍人士指定效期")]
    [ValidDate(format: "yyyyMM", isROC: false)]
    public string? ExpatValidityPeriod { get; set; }

    /// <summary>
    ///  舊照查驗
    /// </summary>
    [Display(Name = "舊照查驗")]
    public string? OldCertificateVerified { get; set; }

    /// <summary>
    ///  公司名稱
    /// </summary>
    [Display(Name = "公司名稱")]
    public string? CompName { get; set; }

    /// <summary>
    ///  職稱
    /// </summary>
    [Display(Name = "職稱")]
    public string? CompJobTitle { get; set; }

    /// <summary>
    ///  公司電話
    /// </summary>
    [Display(Name = "公司電話")]
    public string? CompPhone { get; set; }

    /// <summary>
    ///  居住電話
    /// </summary>
    [Display(Name = "居住電話")]
    public string? LivePhone { get; set; }

    /// <summary>
    ///  行動電話
    /// </summary>
    [Display(Name = "行動電話")]
    public string? Mobile { get; set; }

    /// <summary>
    /// 居住地址類型
    /// </summary>
    [Display(Name = "居住地址類型")]
    public ResidenceType? ResidenceType { get; set; }

    #region 居住地址

    /// <summary>
    ///  居住_郵遞區號
    /// </summary>
    [Display(Name = "居住_郵遞區號")]
    public string? Live_ZipCode { get; set; }

    /// <summary>
    ///  居住_縣市
    /// </summary>
    [Display(Name = "居住_縣市")]
    public string? Live_City { get; set; }

    /// <summary>
    ///  居住_區域
    /// </summary>
    [Display(Name = "居住_區域")]
    public string? Live_District { get; set; }

    /// <summary>
    ///  居住_街道
    /// </summary>
    [Display(Name = "居住_街道")]
    public string? Live_Road { get; set; }

    /// <summary>
    ///  居住_巷
    /// </summary>
    [Display(Name = "居住_巷")]
    public string? Live_Lane { get; set; }

    /// <summary>
    ///  居住_弄
    /// </summary>
    [Display(Name = "居住_弄")]
    public string? Live_Alley { get; set; }

    /// <summary>
    ///  居住_號
    /// </summary>
    [Display(Name = "居住_號")]
    public string? Live_Number { get; set; }

    /// <summary>
    ///  居住_之號
    /// </summary>
    [Display(Name = "居住_之號")]
    public string? Live_SubNumber { get; set; }

    /// <summary>
    ///  居住_樓層
    /// </summary>
    [Display(Name = "居住_樓層")]
    public string? Live_Floor { get; set; }

    /// <summary>
    ///  居住_完整地址
    /// </summary>
    [Display(Name = "居住_完整地址")]
    public string? Live_FullAddr { get; set; }

    /// <summary>
    ///  居住_其他
    /// </summary>
    [Display(Name = "居住_其他")]
    public string? Live_Other { get; set; }

    #endregion

    /// <summary>
    /// 寄卡地址類型
    /// </summary>
    [Display(Name = "寄卡地址類型")]
    public ShippingCardAddressType? ShippingCardAddressType { get; set; }

    #region 寄卡地址

    /// <summary>
    ///  寄卡_郵遞區號
    /// </summary>
    [Display(Name = "寄卡_郵遞區號")]
    public string? SendCard_ZipCode { get; set; }

    /// <summary>
    ///  寄卡_縣市
    /// </summary>
    [Display(Name = "寄卡_縣市")]
    public string? SendCard_City { get; set; }

    /// <summary>
    ///  寄卡_區域
    /// </summary>
    [Display(Name = "寄卡_區域")]
    public string? SendCard_District { get; set; }

    /// <summary>
    ///  寄卡_街道
    /// </summary>
    [Display(Name = "寄卡_街道")]
    public string? SendCard_Road { get; set; }

    /// <summary>
    ///  寄卡_巷
    /// </summary>
    [Display(Name = "寄卡_巷")]
    public string? SendCard_Lane { get; set; }

    /// <summary>
    ///  寄卡_弄
    /// </summary>
    [Display(Name = "寄卡_弄")]
    public string? SendCard_Alley { get; set; }

    /// <summary>
    ///  寄卡_號
    /// </summary>
    [Display(Name = "寄卡_號")]
    public string? SendCard_Number { get; set; }

    /// <summary>
    ///  寄卡_之號
    /// </summary>
    [Display(Name = "寄卡_之號")]
    public string? SendCard_SubNumber { get; set; }

    /// <summary>
    ///  寄卡_樓層
    /// </summary>
    [Display(Name = "寄卡_樓層")]
    public string? SendCard_Floor { get; set; }

    /// <summary>
    ///  寄卡_完整地址
    /// </summary>
    [Display(Name = "寄卡_完整地址")]
    public string? SendCard_FullAddr { get; set; }

    /// <summary>
    ///  寄卡_其他
    /// </summary>
    [Display(Name = "寄卡_其他")]
    public string? SendCard_Other { get; set; }

    #endregion

    /// <summary>
    ///  敦陽系統黑名單是否相符
    /// </summary>
    [Display(Name = "敦陽系統黑名單是否相符")]
    public string? IsDunyangBlackList { get; set; }

    /// <summary>
    ///  姓名檢核理由碼
    /// </summary>
    [Display(Name = "姓名檢核理由碼")]
    public string? NameCheckedReasonCodes { get; set; }

    /// <summary>
    /// 當前或曾為PEP身分 (Y/N)，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "當前或曾為PEP身分")]
    public string? ISRCAForCurrentPEP { get; set; }

    /// <summary>
    /// 卸任PEP種類，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "卸任PEP種類")]
    public ResignPEPKind? ResignPEPKind { get; set; }

    /// <summary>
    /// 擔任PEP範圍，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "擔任PEP範圍")]
    public PEPRange? PEPRange { get; set; }

    /// <summary>
    /// 現任職位是否與PEP職位相關 (Y/N)，敦陽系統黑名單是否相符= Y 需填寫
    /// </summary>
    [Display(Name = "現任職位是否與PEP職位相關")]
    public string? IsCurrentPositionRelatedPEPPosition { get; set; }
}
