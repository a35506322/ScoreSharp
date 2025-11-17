namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryOriginalCardholderDataResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = null!;

    [JsonPropertyName("rtnCode")]
    public string RtnCode { get; set; } = null!;

    [JsonPropertyName("info")]
    public Info Info { get; set; } = null!;
}

public class Info
{
    [JsonPropertyName("Table")]
    public List<QueryOriginalCardholderInfo> Table { get; set; } = new();
}

public class QueryOriginalCardholderInfo
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    [JsonPropertyName("ID")]
    public string? ID { get; set; } = string.Empty;

    /// <summary>
    /// 英文姓名
    /// </summary>
    [JsonPropertyName("ENGLISH_NAME")]
    public string? EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 中文姓名
    /// </summary>
    [JsonPropertyName("CHINESE_NAME")]
    public string? ChineseName { get; set; } = string.Empty;

    /// <summary>
    /// 生日
    /// </summary>
    [JsonPropertyName("BIRTH_DATE")]
    public string? BirthDate { get; set; } = string.Empty;

    /// <summary>
    /// 帳單郵遞區號
    /// </summary>
    [JsonPropertyName("BILL_ZIP")]
    public string? BillZip { get; set; } = string.Empty;

    /// <summary>
    /// 住宅電話
    /// </summary>
    [JsonPropertyName("HOME_TEL")]
    public string? HomeTel { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡電話
    /// </summary>
    [JsonPropertyName("CONTACT_TEL")]
    public string? ContactTel { get; set; } = string.Empty;

    /// <summary>
    /// 公司電話
    /// </summary>
    [JsonPropertyName("COMPANY_TEL")]
    public string? CompanyTel { get; set; } = string.Empty;

    /// <summary>
    /// 手機號碼
    /// </summary>
    [JsonPropertyName("CELL_TEL")]
    public string? CellTel { get; set; } = string.Empty;

    /// <summary>
    /// 信用額度
    /// </summary>
    [JsonPropertyName("CR_CRLIMIT")]
    public float? CRCrlimit { get; set; } = 0;

    /// <summary>
    /// 統一編號
    /// </summary>
    [JsonPropertyName("UNIFORM_NUMBER")]
    public string? UniformNumber { get; set; } = string.Empty;

    /// <summary>
    /// 電子郵件
    /// </summary>
    [JsonPropertyName("EMAIL")]
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 性別
    /// </summary>
    [JsonPropertyName("SEX")]
    public string? Sex { get; set; } = string.Empty;

    /// <summary>
    /// 帳單日  (日期01~31)
    /// </summary>
    [JsonPropertyName("CYCLE_DD")]
    public string? CycleDD { get; set; } = string.Empty;

    /// <summary>
    /// 帳單地址
    /// </summary>
    [JsonPropertyName("BILL_ADDR")]
    public string? BillAddr { get; set; } = string.Empty;

    /// <summary>
    /// 戶籍地址
    /// </summary>
    [JsonPropertyName("HOME_ADDR")]
    public string? HomeAddr { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡人姓名
    /// </summary>
    [JsonPropertyName("CONTACT_NAME")]
    public string? ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡人關係
    /// </summary>
    [JsonPropertyName("CONTACT_REL")]
    public string? ContactRel { get; set; } = string.Empty;

    /// <summary>
    /// 公司地址
    /// </summary>
    [JsonPropertyName("COMPANY_ADDR")]
    public string? CompanyAddr { get; set; } = string.Empty;

    /// <summary>
    /// 寄送地址
    /// </summary>
    [JsonPropertyName("SEND_ADDR")]
    public string? SendAddr { get; set; } = string.Empty;

    /// <summary>
    /// 公司名稱
    /// </summary>
    [JsonPropertyName("COMPANY_NAME")]
    public string? CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 職稱
    /// </summary>
    [JsonPropertyName("COMPANY_TITLE")]
    public string? CompanyTitle { get; set; } = string.Empty;

    /// <summary>
    /// 教育程度碼
    /// </summary>
    [JsonPropertyName("EDUCATE_CODE")]
    public string? EducateCode { get; set; } = string.Empty;

    /// <summary>
    /// 婚姻狀況
    /// </summary>
    [JsonPropertyName("MARRIAGE_CODE")]
    public string? MarriageCode { get; set; } = string.Empty;

    /// <summary>
    /// 職業碼
    /// </summary>
    [JsonPropertyName("PROFESSION_CODE")]
    public string? ProfessionCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作年資
    /// </summary>
    [JsonPropertyName("PROFESSION_PERIOD")]
    public string? ProfessionPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 薪資
    /// </summary>
    [JsonPropertyName("SALARY")]
    public string? Salary { get; set; } = string.Empty;

    /// <summary>
    /// 外國人國籍
    /// </summary>
    [JsonPropertyName("NATIONAL")]
    public string? National { get; set; } = string.Empty;

    /// <summary>
    /// 外國人護照號碼
    /// </summary>
    [JsonPropertyName("PASSPORT")]
    public string? Passport { get; set; } = string.Empty;

    /// <summary>
    /// 外國人護照有效日期
    /// </summary>
    [JsonPropertyName("PASSPORT_DATE")]
    public string? PassportDate { get; set; } = string.Empty;

    /// <summary>
    /// 外國人護照發照日期
    /// </summary>
    [JsonPropertyName("FOREIGNER_ISSUE_DATE")]
    public string? ForeignerIssueDate { get; set; } = string.Empty;

    /// <summary>
    /// 畢業國小
    /// </summary>
    [JsonPropertyName("SCHOOL_NAME")]
    public string? SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 居住地址
    /// </summary>
    [JsonPropertyName("CONTACT_ADDR")]
    public string? ContactAddr { get; set; } = string.Empty;

    /// <summary>
    /// 居住年限
    /// </summary>
    [JsonPropertyName("RESIDE_NBR")]
    public string? ResideNBR { get; set; } = string.Empty;
}
