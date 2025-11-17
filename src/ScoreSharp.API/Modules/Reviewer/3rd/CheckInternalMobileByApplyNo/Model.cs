namespace ScoreSharp.API.Modules.Reviewer3rd.CheckInternalMobileByApplyNo;

public class QueryOriginalCardholderData
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 英文姓名
    /// </summary>
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 中文姓名
    /// </summary>
    public string ChineseName { get; set; } = string.Empty;

    /// <summary>
    /// 生日
    /// </summary>
    public string BirthDate { get; set; } = string.Empty;

    /// <summary>
    /// 帳單郵遞區號
    /// </summary>
    public string BillZip { get; set; } = string.Empty;

    /// <summary>
    /// 住宅電話
    /// </summary>
    public string HomeTel { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡電話
    /// </summary>
    public string ContactTel { get; set; } = string.Empty;

    /// <summary>
    /// 公司電話
    /// </summary>
    public string CompanyTel { get; set; } = string.Empty;

    /// <summary>
    /// 手機號碼
    /// </summary>
    public string CellTel { get; set; } = string.Empty;

    /// <summary>
    /// 信用額度
    /// </summary>
    public float CRCrlimit { get; set; } = 0;

    /// <summary>
    /// 統一編號
    /// </summary>
    public string UniformNumber { get; set; } = string.Empty;

    /// <summary>
    /// 電子郵件
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 性別
    /// </summary>
    public Sex? Sex { get; set; }

    /// <summary>
    /// 帳單日  (日期01~31)
    /// </summary>
    public string CycleDD { get; set; } = string.Empty;

    /// <summary>
    /// 帳單地址
    /// </summary>
    public string BillAddr { get; set; } = string.Empty;

    /// <summary>
    /// 戶籍地址
    /// </summary>
    public string HomeAddr { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡人姓名
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡人關係
    /// </summary>
    public string ContactRel { get; set; } = string.Empty;

    /// <summary>
    /// 公司地址
    /// </summary>
    public string CompanyAddr { get; set; } = string.Empty;

    /// <summary>
    /// 寄送地址
    /// </summary>
    public string SendAddr { get; set; } = string.Empty;

    /// <summary>
    /// 公司名稱
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 職稱
    /// </summary>
    public string CompanyTitle { get; set; } = string.Empty;

    /// <summary>
    /// 教育程度碼
    /// </summary>
    public Education? EducateCode { get; set; }

    /// <summary>
    /// 婚姻狀況
    /// </summary>
    public MarriageState? MarriageCode { get; set; }

    /// <summary>
    /// 職業碼
    /// </summary>
    public string ProfessionCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作年資
    /// </summary>
    public int ProfessionPeriod { get; set; } = 0;

    /// <summary>
    /// 月收入
    /// </summary>
    public int MonthlySalary { get; set; } = 0;

    /// <summary>
    /// 外國人國籍
    /// </summary>
    public string National { get; set; } = string.Empty;

    /// <summary>
    /// 外國人護照號碼
    /// </summary>
    public string Passport { get; set; } = string.Empty;

    /// <summary>
    /// 外國人護照有效日期
    /// </summary>
    public string PassportDate { get; set; } = string.Empty;

    /// <summary>
    /// 外國人護照發照日期
    /// </summary>
    public string ForeignerIssueDate { get; set; } = string.Empty;

    /// <summary>
    /// 畢業國小
    /// </summary>
    public string SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 居住地址
    /// </summary>
    public string ContactAddr { get; set; } = string.Empty;

    /// <summary>
    /// 居住年限
    /// </summary>
    public int ResideNBR { get; set; } = 0;

    /// <summary>
    /// 公司行業
    /// </summary>
    public CompTrade? CompTrade { get; set; }

    /// <summary>
    /// 公司職等
    /// </summary>
    public CompJobLevel? CompJobLevel { get; set; }
}
