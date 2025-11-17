namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryConcernDetailResponse
{
    /// <summary>
    /// 交易序號，String　服務成功處理請求後(HTTP回應狀態碼200)
    /// 會提供20碼交易序號用於識別,請紀錄Log
    /// </summary>
    [JsonPropertyName("traceId")]
    public string? TraceId { get; set; }

    /// <summary>
    /// 狀態碼
    /// </summary>
    [JsonPropertyName("rtnCode")]
    public string? RtnCode { get; set; }

    /// <summary>
    /// 狀態訊息
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    /// <summary>
    /// 回傳值
    /// </summary>
    [JsonPropertyName("info")]
    public QueryConcernDetailInfo? Info { get; set; }
}

public class QueryConcernDetailInfo
{
    /// <summary>
    /// 告誡名單 (A)
    /// </summary>
    [JsonPropertyName("restriction")]
    public List<Restriction> Restriction { get; set; } = new();

    /// <summary>
    /// 受警示企業戶之負責人 (B)
    /// </summary>
    [JsonPropertyName("warningCompany")]
    public List<WarningCompany> WarningCompany { get; set; } = new();

    /// <summary>
    /// 風險帳戶 (C)
    /// </summary>
    [JsonPropertyName("riskAccount")]
    public List<RiskAccount> RiskAccount { get; set; } = new();

    /// <summary>
    /// 聯徵資料─行方不明 (D)
    /// </summary>
    [JsonPropertyName("fled")]
    public List<Fled> Fled { get; set; } = new();

    /// <summary>
    /// 聯徵資料─收容遣返 (E)
    /// </summary>
    [JsonPropertyName("punish")]
    public List<Punish> Punish { get; set; } = new();

    /// <summary>
    /// 聯徵資料─出境 (F)
    /// </summary>
    [JsonPropertyName("immi")]
    public List<Immi> Immi { get; set; } = new();

    /// <summary>
    /// 失蹤人口 (G)
    /// </summary>
    [JsonPropertyName("missingPersons")]
    public MissingPersons MissingPersons { get; set; } = new();

    /// <summary>
    /// 疑似涉詐境內帳戶 (H)
    /// </summary>
    [JsonPropertyName("frdId")]
    public List<FrdId> FrdId { get; set; } = new();

    /// <summary>
    /// 聯徵資料─解聘 (I)
    /// </summary>
    [JsonPropertyName("layOff")]
    public List<LayOff> LayOff { get; set; } = new();
}

/// <summary>
/// 告誡名單 (A)
/// </summary>
public class Restriction
{
    /// <summary>
    /// 資料類型 A : 本日新增 U : 本日告誡資料異動
    /// </summary>
    [JsonPropertyName("dataType")]
    public string DataType { get; set; } = string.Empty;

    /// <summary>
    /// 受處分人身分證號
    /// </summary>
    [JsonPropertyName("pid")]
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 告誡日期 民國年七碼
    /// </summary>
    [JsonPropertyName("warningDate")]
    public string WarningDate { get; set; } = string.Empty;

    /// <summary>
    /// 告誡期限 民國年七碼
    /// </summary>
    [JsonPropertyName("expireDate")]
    public string ExpireDate { get; set; } = string.Empty;

    /// <summary>
    /// 告誡分局中文全名
    /// </summary>
    [JsonPropertyName("issuer")]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// 資料匯入日
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}

/// <summary>
/// 受警示企業戶之負責人 (B)
/// </summary>
public class WarningCompany
{
    /// <summary>
    /// 帳號
    /// </summary>
    [JsonPropertyName("account")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 公司統編 統編+A
    /// </summary>
    [JsonPropertyName("corporateID")]
    public string CorporateID { get; set; } = string.Empty;

    /// <summary>
    /// 企業戶負責人ID ID+A
    /// </summary>
    [JsonPropertyName("pid")]
    public string PID { get; set; } = string.Empty;

    /// <summary>
    /// 開戶日期 八碼
    /// </summary>
    [JsonPropertyName("accountDate")]
    public string AccountDate { get; set; } = string.Empty;

    /// <summary>
    /// 事故代號 29：受警示之人 39：延伸之人 00：刪除名單
    /// </summary>
    [JsonPropertyName("accidentCode")]
    public string? AccidentCode { get; set; }

    /// <summary>
    /// 事故設定日期
    /// </summary>
    [JsonPropertyName("accidentDate")]
    public string? AccidentDate { get; set; }

    /// <summary>
    /// 事故解除日期
    /// </summary>
    [JsonPropertyName("accidentCancelDate")]
    public string? AccidentCancelDate { get; set; }

    /// <summary>
    /// 資料日期
    /// </summary>
    [JsonPropertyName("createDate")]
    public string? CreateDate { get; set; }
}

/// <summary>
/// 風險帳戶 (C)
/// </summary>
public class RiskAccount
{
    /// <summary>
    /// 帳號
    /// </summary>
    [JsonPropertyName("account")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 身分證號 ID+A
    /// </summary>
    [JsonPropertyName("pid")]
    public string PID { get; set; } = string.Empty;

    /// <summary>
    /// 開戶日期 八碼
    /// </summary>
    [JsonPropertyName("accountDate")]
    public string AccountDate { get; set; } = string.Empty;

    /// <summary>
    /// 事故設定日期 八碼
    /// </summary>
    [JsonPropertyName("accidentDate")]
    public string AccidentDate { get; set; } = string.Empty;

    /// <summary>
    /// 事故解除日期
    /// </summary>
    [JsonPropertyName("accidentCancelDate")]
    public string AccidentCancelDate { get; set; } = string.Empty;

    /// <summary>
    /// 摘要 1：新增   0：刪除
    /// </summary>
    [JsonPropertyName("memo")]
    public string Memo { get; set; } = string.Empty;

    /// <summary>
    /// 資料更新日期
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}

/// <summary>
/// 聯徵資料─行方不明 (D)
/// </summary>
public class Fled
{
    /// <summary>
    /// 統一證號
    /// </summary>
    [JsonPropertyName("residentIdNo")]
    public string ResidentIdNo { get; set; } = string.Empty;

    /// <summary>
    /// 英文姓名
    /// </summary>
    [JsonPropertyName("enName")]
    public string ENName { get; set; } = string.Empty;

    /// <summary>
    /// 護照號碼
    /// </summary>
    [JsonPropertyName("passportNo")]
    public string PassportNo { get; set; } = string.Empty;

    /// <summary>
    /// 國籍  (菲律賓 24、印尼 9、泰國 30、越南 34、馬來西亞 19、蒙古 21，前方不補 0)
    /// </summary>
    [JsonPropertyName("nationality")]
    public string Nationality { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期 八碼
    /// </summary>
    [JsonPropertyName("birthdate")]
    public string BirthDate { get; set; } = string.Empty;

    /// <summary>
    /// 性別 0:男性，1:女性，可能為空值
    /// </summary>
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// 失聯日期 八碼
    /// </summary>
    [JsonPropertyName("fledDate")]
    public string FledDate { get; set; } = string.Empty;

    /// <summary>
    /// 查獲日期 八碼
    /// </summary>
    [JsonPropertyName("catchingDate")]
    public string CatchingDate { get; set; } = string.Empty;

    /// <summary>
    /// 資料更新日期 八碼
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}

/// <summary>
/// 聯徵資料─收容遣返 (E)
/// </summary>
public class Punish
{
    /// <summary>
    /// 統一證號
    /// </summary>
    [JsonPropertyName("residentIdNo")]
    public string ResidentIdNo { get; set; } = string.Empty;

    /// <summary>
    /// 英文姓名
    /// </summary>
    [JsonPropertyName("enName")]
    public string ENName { get; set; } = string.Empty;

    /// <summary>
    /// 護照號碼
    /// </summary>
    [JsonPropertyName("passportNo")]
    public string PassportNo { get; set; } = string.Empty;

    /// <summary>
    /// 國籍 (菲律賓 24、印尼 9、泰國 30、越南 34、馬來西 亞 19、蒙古 21，前方不補 0)
    /// </summary>
    [JsonPropertyName("nationality")]
    public string Nationality { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期 八碼
    /// </summary>
    [JsonPropertyName("birthdate")]
    public string BirthDate { get; set; } = string.Empty;

    /// <summary>
    /// 性別 0:男性，1:女性，可能為空值
    /// </summary>
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// 查獲日期 八碼
    /// </summary>
    [JsonPropertyName("catchingDate")]
    public string CatchingDate { get; set; } = string.Empty;

    /// <summary>
    /// 出境日期 八碼
    /// </summary>
    [JsonPropertyName("immigrateDate")]
    public string ImmigrateDate { get; set; } = string.Empty;

    /// <summary>
    /// 資料更新日期 八碼
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}

/// <summary>
/// F.聯徵資料─出境
/// </summary>
public class Immi
{
    /// <summary>
    /// 統一證號
    /// </summary>
    [JsonPropertyName("residentIdNo")]
    public string ResidentIdNo { get; set; } = string.Empty;

    /// <summary>
    /// 英文姓名
    /// </summary>
    [JsonPropertyName("enName")]
    public string ENName { get; set; } = string.Empty;

    /// <summary>
    /// 護照號碼
    /// </summary>
    [JsonPropertyName("passportNo")]
    public string PassportNo { get; set; } = string.Empty;

    /// <summary>
    /// 國籍 (菲律賓 24、印尼 9、泰國 30、越南 34、馬來西 亞 19、蒙古 21，前方不補 0)
    /// </summary>
    [JsonPropertyName("nationality")]
    public string Nationality { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期 八碼
    /// </summary>
    [JsonPropertyName("birthdate")]
    public string BirthDate { get; set; } = string.Empty;

    /// <summary>
    /// 性別 0:男性，1:女性，可能為空值
    /// </summary>
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// 出境日期
    /// </summary>
    [JsonPropertyName("immigrateDate")]
    public string ImmigrateDate { get; set; } = string.Empty;

    /// <summary>
    /// 在臺狀態
    /// </summary>
    [JsonPropertyName("inTW")]
    public string InTW { get; set; } = string.Empty;

    /// <summary>
    /// 資料更新日期 八碼
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}

/// <summary>
/// 失蹤人口 (G)
/// </summary>
public class MissingPersons
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    [JsonPropertyName("pid")]
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 是否為失蹤人口 (Y:符合 N:不符合)
    /// </summary>
    [JsonPropertyName("ynmpInfo")]
    public string YnmpInfo { get; set; } = string.Empty;

    /// <summary>
    /// 資料更新日期 八碼
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}

public class FrdId
{
    /// <summary>
    /// 身分證字號
    /// </summary>
    [JsonPropertyName("pid")]
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 帳號
    /// </summary>
    [JsonPropertyName("account")]
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 通報日期
    /// </summary>
    [JsonPropertyName("notifyDate")]
    public string NotifyDate { get; set; } = string.Empty;

    /// <summary>
    /// 資料更新日期
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}

public class LayOff
{
    /// <summary>
    /// 傳輸日期
    /// </summary>
    [JsonPropertyName("transDate")]
    public string TransDate { get; set; } = string.Empty;

    /// <summary>
    /// 異動代碼
    ///
    /// I = 新增
    /// U = 修改
    /// D = 刪除
    /// </summary>
    [JsonPropertyName("chngId")]
    public string ChngId { get; set; } = string.Empty;

    /// <summary>
    /// 國籍代碼
    /// </summary>
    [JsonPropertyName("natCode")]
    public string NatCode { get; set; } = string.Empty;

    /// <summary>
    /// 護照號碼
    /// </summary>
    [JsonPropertyName("passno")]
    public string PassNo { get; set; } = string.Empty;

    /// <summary>
    /// 撤銷函文號
    /// </summary>
    [JsonPropertyName("expirwkno")]
    public string ExpirWkNo { get; set; } = string.Empty;

    /// <summary>
    /// 雇主通報日期
    /// </summary>
    [JsonPropertyName("knowdate")]
    public string KnowDate { get; set; } = string.Empty;

    /// <summary>
    /// 移工解聘日期
    /// </summary>
    [JsonPropertyName("dynadate")]
    public string DynaDate { get; set; } = string.Empty;

    /// <summary>
    /// 狀況代碼
    /// </summary>
    [JsonPropertyName("happcode")]
    public string HappCode { get; set; } = string.Empty;

    /// <summary>
    /// 雇主處分代碼
    /// </summary>
    [JsonPropertyName("vendcode")]
    public string VendCode { get; set; } = string.Empty;

    /// <summary>
    /// 外國人處分代碼
    /// </summary>
    [JsonPropertyName("laborcode")]
    public string LaborCode { get; set; } = string.Empty;

    /// <summary>
    /// 撤銷函日期
    /// </summary>
    [JsonPropertyName("expiredate")]
    public string ExpireDate { get; set; } = string.Empty;

    /// <summary>
    /// 打詐通報種類代碼
    /// </summary>
    [JsonPropertyName("immiType")]
    public string ImmiType { get; set; } = string.Empty;

    /// <summary>
    /// 打詐通報種類代碼中文說明
    /// </summary>
    [JsonPropertyName("immiTypeDesc")]
    public string ImmiTypeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 狀況代碼中文說明
    /// </summary>
    [JsonPropertyName("happCodeDesc")]
    public string HappCodeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 雇主處分代碼中文說明
    /// </summary>
    [JsonPropertyName("vendCodeDesc")]
    public string VendCodeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 外國人處分代碼中文說明
    /// </summary>
    [JsonPropertyName("laborCodeDesc")]
    public string LaborCodeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 定型搞代碼
    /// </summary>
    [JsonPropertyName("wpCode")]
    public string WpCode { get; set; } = string.Empty;

    /// <summary>
    /// 定型搞代碼中文說明
    /// </summary>
    [JsonPropertyName("desc")]
    public string WpCodeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 勞發署居留證號
    /// </summary>
    [JsonPropertyName("resnum")]
    public string Resnum { get; set; } = string.Empty;

    /// <summary>
    /// 居留截止日
    /// </summary>
    [JsonPropertyName("immigartionDate")]
    public string ImmigartionDate { get; set; } = string.Empty;

    /// <summary>
    /// 移民署傳檔日
    /// </summary>
    [JsonPropertyName("systemDate")]
    public string SystemDate { get; set; } = string.Empty;

    /// <summary>
    /// 移民署居留證號
    /// </summary>
    [JsonPropertyName("niaResidenceNo")]
    public string NiaResidenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 資料更新日期
    /// </summary>
    [JsonPropertyName("createDate")]
    public string CreateDate { get; set; } = string.Empty;
}
