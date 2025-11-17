namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetLatestBankRecordByApplyNo;

public class GetLatestBankRecordByApplyNoResponse
{
    /// <summary>
    /// 分行資訊
    /// </summary>
    public BranchCusInfoResponse BranchCusInfo { get; set; } = new();

    /// <summary>
    /// 929
    /// </summary>
    public Query929InfoResponse Query929Info { get; set; } = new();

    /// <summary>
    /// 行外餘額資訊
    /// </summary>
    public OutsideBankInfoResponse? OutsideBankInfo { get; set; } = null;

    /// <summary>
    /// 關注名單1
    ///     	- WarningCompany B.受警示企業戶之負責人
    ///     	- RiskAccount C.風險帳戶
    /// </summary>
    public Focus1InfoResponse Focus1Info { get; set; } = new();

    /// <summary>
    /// 關注名單2
    ///     	- WarnLog A.告誡名單
    ///     	- FledLog D.聯徵資料─行方不明
    ///     	- PunishLog E.聯徵資料─收容遣返
    ///     	- ImmiLog F.聯徵資料─出境
    ///     	- MissingPersonsLog G.失蹤人口
    /// </summary>
    public Focus2InfoResponse Focus2Info { get; set; } = new();
}

public class BranchCusInfoResponse
{
    /// <summary>
    /// API 回覆訊息
    /// </summary>
    public TxnResponse BranchCusRes { get; set; } = new();

    /// <summary>
    /// 客戶資訊
    /// </summary>
    public List<BranchCusCusInfoResponse> BranchCusCusInfo { get; set; } = new();

    /// <summary>
    /// 定存明細資訊
    /// </summary>
    public List<BranchCusCDResponse> BranchCusCD { get; set; } = new();

    /// <summary>
    /// 活期存款明細資訊
    /// </summary>
    public List<BranchCusDDResponse> BranchCusDD { get; set; } = new();

    /// <summary>
    /// 支票存款明細資訊
    /// </summary>
    public List<BranchCusCADResponse> BranchCusCAD { get; set; } = new();

    /// <summary>
    /// 財富管理客戶
    /// </summary>
    public List<BranchCusWMCustResponse> BranchCusWMCust { get; set; } = new();

    /// <summary>
    /// 授信逾期狀況
    /// </summary>
    public List<BranchCusCreditOverResponse> BranchCusCreditOver { get; set; } = new();

    /// <summary>
    /// 彙總資訊
    /// </summary>
    public BrachCusSummaryResponse BranchCusSummary { get; set; } = new();
}

public class BranchCusCusInfoResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 識別碼
    ///
    /// 支票平均存款近6個月平均餘額均達10萬元以上,
    /// 房屋貸款客戶-排除逾期繳款,
    /// 薪轉戶,
    /// 財富管理戶-尊榮級,
    /// 財富管理戶-潛力級
    /// </summary>
    public string SN { get; set; } = null!;

    /// <summary>
    /// 分行代碼
    /// </summary>
    public string Cate { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class BranchCusCDResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 幣別
    ///
    /// 範例: TWD
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    /// 起息日
    ///
    /// 範例: 01000101
    /// </summary>
    public string InterestD { get; set; } = null!;

    /// <summary>
    /// 降息日
    ///
    /// 範例: 01000101
    /// </summary>
    public string ExpirationD { get; set; } = null!;

    /// <summary>
    /// 金額
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// 分行代碼
    /// </summary>
    public string Cate { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class BranchCusDDResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 分行代碼
    /// </summary>
    public string Cate { get; set; } = null!;

    /// <summary>
    /// 幣別
    ///
    /// 範例: TWD
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    /// 帳戶別
    /// </summary>
    public string Account { get; set; } = null!;

    /// <summary>
    /// 開戶日
    ///
    /// 範例: 01000101
    /// </summary>
    public string OpenAccountD { get; set; } = null!;

    /// <summary>
    /// 目前餘額基準日
    ///
    /// 範例: 01000101
    /// </summary>
    public string CreditD { get; set; } = null!;

    /// <summary>
    /// 近三個月平均餘額
    /// </summary>
    public int Last3MavgCredit { get; set; }

    /// <summary>
    /// 三個月平均餘額
    /// </summary>
    public int ThreeMavgCredit { get; set; }

    /// <summary>
    /// 兩個月平均餘額
    /// </summary>
    public int TwoMavgCredit { get; set; }

    /// <summary>
    /// 一個月平均餘額
    /// </summary>
    public int OneMavgCredit { get; set; }

    /// <summary>
    /// 目前餘額
    /// </summary>
    public int Credit { get; set; }

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class BranchCusCADResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 分行代碼
    /// </summary>
    public string Cate { get; set; } = null!;

    /// <summary>
    /// 帳戶別
    /// </summary>
    public string Account { get; set; } = null!;

    /// <summary>
    /// 開戶日
    ///
    /// 範例: 01000101
    /// </summary>
    public string OpenAccountD { get; set; } = null!;

    /// <summary>
    /// 目前餘額基準日
    ///
    /// 範例: 01000101
    /// </summary>
    public string CreditD { get; set; } = null!;

    /// <summary>
    /// 近三個月平均餘額
    /// </summary>
    public int Last3MavgCredit { get; set; }

    /// <summary>
    /// 三個月平均餘額
    /// </summary>
    public int ThreeMavgCredit { get; set; }

    /// <summary>
    /// 兩個月平均餘額
    /// </summary>
    public int TwoMavgCredit { get; set; }

    /// <summary>
    /// 一個月平均餘額
    /// </summary>
    public int OneMavgCredit { get; set; }

    /// <summary>
    /// 目前餘額
    /// </summary>
    public int Credit { get; set; }

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class BranchCusWMCustResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 是否為財富管理
    /// </summary>
    public string ICountFlag { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class BranchCusCreditOverResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 分行授信帳號
    ///
    /// 範例: 000000000001
    /// </summary>
    public string Account { get; set; } = null!;

    /// <summary>
    /// 逾期狀態
    ///
    /// 範例: M1
    /// </summary>
    public string OverStatus { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class BrachCusSummaryResponse
{
    /// <summary>
    /// 定期明細總額
    /// </summary>
    public int BranchCusCDTotal { get; set; }

    /// <summary>
    /// 活期存款明細總額
    /// </summary>
    public int BranchCusDDTotal { get; set; }

    /// <summary>
    /// 支票存款明細總額
    /// </summary>
    public int BranchCusCADTotal { get; set; }

    /// <summary>
    /// 定期明細總額+活期存款明細總額+支票存款明細總額
    /// </summary>
    public int SummaryTotal { get; set; }

    /// <summary>
    /// 備註，填寫那些資料表有資料
    /// </summary>
    public string Note { get; set; } = "";
}

public class Query929InfoResponse
{
    public TxnResponse Query929Res { get; set; } = new();

    /// <summary>
    /// 備註，填寫那些資料表有資料
    /// </summary>
    public string Note { get; set; }

    public List<Query929LogResponse> Query929Logs { get; set; } = new();
}

public class Query929LogResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 交易日期
    /// </summary>
    public string? TxnDate { get; set; }

    /// <summary>
    /// 歸屬分行
    /// </summary>
    public string? BrachCode { get; set; }

    /// <summary>
    /// 交易櫃員
    /// </summary>
    public string? BrachEmp { get; set; }

    /// <summary>
    /// 業務來源
    /// </summary>
    public string? BusinessCode { get; set; }

    /// <summary>
    /// 戶名
    /// </summary>
    public string? ChName { get; set; }

    /// <summary>
    /// 登錄日期
    /// </summary>
    public string? LoginDate { get; set; }

    /// <summary>
    /// 拒絕原因
    /// </summary>
    public string? ApplyCause { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? ApplyReMark { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class OutsideBankInfoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 活存目前餘額
    /// </summary>
    public int? HUOCUN_Balance { get; set; }

    /// <summary>
    /// 定存目前餘額
    /// </summary>
    public int? DINGCUN_Balance { get; set; }

    /// <summary>
    /// 活存90天平均餘額
    /// </summary>
    public int? HUOCUN_Balance_90 { get; set; }

    /// <summary>
    /// 定存90天平均餘額
    /// </summary>
    public int? DINGCUN_Balance_90 { get; set; }

    /// <summary>
    /// 餘額更新日期
    /// </summary>
    public DateTime? BalanceUpdateDate { get; set; }

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class Focus1InfoResponse
{
    /// <summary>
    /// 備註，填寫那些資料表有資料
    /// </summary>
    public string Note { get; set; }

    public TxnResponse QueryFocus1Res { get; set; } = new();

    /// <summary>
    /// B. 受警示企業戶之負責人
    /// </summary>
    public List<WarnCompLogResponse> WarnCompLogs { get; set; } = new();

    /// <summary>
    /// C. 風險帳戶
    /// </summary>
    public List<RiskAccountLogResponse> RiskAccountLogs { get; set; } = new();

    /// <summary>
    /// H. 疑似涉詐境內帳戶
    /// </summary>
    public List<FrdIdLogResponse> FrdIdLogs { get; set; } = new();
}

public class Focus2InfoResponse
{
    /// <summary>
    /// 備註，填寫那些資料表有資料
    /// </summary>
    public string Note { get; set; }

    public TxnResponse QueryFocus2Res { get; set; } = new();

    /// <summary>
    /// A. 告誡名單
    /// </summary>
    public List<WarnLogResponse> WarnLogs { get; set; } = new();

    /// <summary>
    /// D. 聯徵資料─行方不明
    /// </summary>
    public List<FledLogResponse> FledLogs { get; set; } = new();

    /// <summary>
    /// E. 聯徵資料─收容遣返
    /// </summary>
    public List<PunishLogResponse> PunishLogs { get; set; } = new();

    /// <summary>
    /// F. 聯徵資料─出境
    /// </summary>
    public List<ImmiLogResponse> ImmiLogs { get; set; } = new();

    /// <summary>
    /// G. 失蹤人口
    /// </summary>
    public List<MissingPersonsLogResponse> MissingPersonsLogs { get; set; } = new();

    /// <summary>
    /// I. 聯徵資料─解聘
    /// </summary>
    public List<LayOffLogResponse> LayOffLogs { get; set; } = new();
}

public class WarningInfoResponse
{
    /// <summary>
    /// 備註，填寫那些資料表有資料
    /// </summary>
    public string Note { get; private set; }

    /// <summary>
    /// 最新查詢日期
    /// </summary>
    public string LatestQueryDate { get; private set; }

    public List<WarnLogResponse> WarnLogs { get; set; } = new();

    public void SetNote()
    {
        Note = "";
        if (!WarnLogs.Any())
        {
            Note += "查無資料";
        }
        else
        {
            Note += string.Join("／", WarnLogs.Select(x => $"{x.UserType.ToString()}:{x.ID}:有資料"));
        }
    }

    public void SetLatestQueryDate()
    {
        LatestQueryDate = WarnLogs.Max(x => x.AddTime).ToString("yyyy/MM/dd");
    }
}

public class WarnLogResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 資料類型
    /// </summary>
    public string? DataType { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// 告誡日期
    /// </summary>
    public string? WarningDate { get; set; }

    /// <summary>
    /// 告誡期限
    /// </summary>
    public string? ExpireDate { get; set; }

    /// <summary>
    /// 告誡分局
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// 資料匯入日
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 回傳代碼
    /// </summary>
    public string RtnCode { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class WarnCompLogResponse
{
    /// <summary>
    /// PK，自增
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 公司統編
    /// 統編+A
    /// </summary>
    public string? CorporateID { get; set; }

    /// <summary>
    /// 企業戶負責人ID
    /// ID+A
    /// </summary>
    public string? PID { get; set; }

    /// <summary>
    /// 開戶日期，八碼
    /// </summary>
    public string? AccountDate { get; set; }

    /// <summary>
    /// 事故代號
    /// 29：受警示之人 39：延伸之人 00：刪除名單
    /// </summary>
    public string? AccidentCode { get; set; }

    /// <summary>
    /// 事故設定日期
    /// </summary>
    public string? AccidentDate { get; set; }

    /// <summary>
    /// 事故解除日期
    /// </summary>
    public string? AccidentCancelDate { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class RiskAccountLogResponse
{
    /// <summary>
    /// PK，自增
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// ID，ID+A
    /// </summary>
    public string? PID { get; set; }

    /// <summary>
    /// 開戶日期
    /// </summary>
    public string? AccountDate { get; set; }

    /// <summary>
    /// 事故設定日期
    /// </summary>
    public string? AccidentDate { get; set; }

    /// <summary>
    /// 事故解除日期
    /// </summary>
    public string? AccidentCancelDate { get; set; }

    /// <summary>
    /// 摘要
    /// 1：新增　0：刪除
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class FledLogResponse
{
    /// <summary>
    /// PK，自增
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 統一證號
    /// </summary>
    public string? ResidentIdNo { get; set; }

    /// <summary>
    /// 英文姓名
    /// </summary>
    public string? ENName { get; set; }

    /// <summary>
    /// 護照號碼
    /// </summary>
    public string? PassportNo { get; set; }

    /// <summary>
    /// 國籍
    /// 菲律賓 24、印尼 9、泰國 30、越南 34、馬來西亞 19、蒙古 21，前方不補 0
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    public string? BirthDate { get; set; }

    /// <summary>
    /// 性別
    /// 0:男性，1:女性，可能為空值
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// 失聯日期
    /// </summary>
    public string? FledDate { get; set; }

    /// <summary>
    /// 查獲日期
    /// </summary>
    public string? CatchingDate { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class PunishLogResponse
{
    /// <summary>
    /// PK，自增
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 統一證號
    /// </summary>
    public string? ResidentIdNo { get; set; }

    /// <summary>
    /// 英文姓名
    /// </summary>
    public string? ENName { get; set; }

    /// <summary>
    /// 護照號碼
    /// </summary>
    public string? PassportNo { get; set; }

    /// <summary>
    /// 國籍
    /// 菲律賓 24、印尼 9、泰國 30、越南 34、馬來西亞 19、蒙古 21，前方不補 0
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    public string? BirthDate { get; set; }

    /// <summary>
    /// 性別
    /// 0:男性，1:女性，可能為空值
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// 查獲日期
    /// </summary>
    public string? CatchingDate { get; set; }

    /// <summary>
    /// 出境日期
    /// </summary>
    public string? ImmigrateDate { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class ImmiLogResponse
{
    /// <summary>
    /// PK，自增
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 統一證號
    /// </summary>
    public string? ResidentIdNo { get; set; }

    /// <summary>
    /// 英文姓名
    /// </summary>
    public string? ENName { get; set; }

    /// <summary>
    /// 護照號碼
    /// </summary>
    public string? PassportNo { get; set; }

    /// <summary>
    /// 國籍
    /// 菲律賓 24、印尼 9、泰國 30、越南 34、馬來西亞 19、蒙古 21，前方不補 0
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    public string? BirthDate { get; set; }

    /// <summary>
    /// 性別
    /// 0:男性，1:女性，可能為空值
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// 出境日期
    /// </summary>
    public string? ImmigrateDate { get; set; }

    /// <summary>
    /// 在臺狀態
    /// </summary>
    public string? InTW { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class MissingPersonsLogResponse
{
    /// <summary>
    /// PK，自增
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 是否為失蹤人口
    /// Y:符合 N:不符合
    /// </summary>
    public string? YnmpInfo { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 正附卡人
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}

public class TxnResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 系統回覆訊息
    ///
    /// 授權成功
    /// </summary>
    public string Response { get; set; } = null!;

    /// <summary>
    /// 系統回覆代碼
    ///
    /// 0000
    /// </summary>
    public string RtnCode { get; set; } = null!;

    /// <summary>
    /// 查詢時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 交易序號
    /// </summary>
    public string? TraceId { get; set; }
}

public class FrdIdLogResponse
{
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 帳號
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 通報日期
    /// </summary>
    public string? NotifyDate { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }
}

public class LayOffLogResponse
{
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; } = null!;

    /// <summary>
    /// 傳輸日期
    /// </summary>
    public string? TransDate { get; set; }

    /// <summary>
    /// 異動代碼
    ///
    /// I = 新增
    /// U = 修改
    /// D = 刪除
    /// </summary>
    public string? ChngId { get; set; }

    /// <summary>
    /// 國籍代碼
    /// </summary>
    public string? NatCode { get; set; }

    /// <summary>
    /// 護照號碼
    /// </summary>
    public string? PassNo { get; set; }

    /// <summary>
    /// 撤銷函文號
    /// </summary>
    public string? ExpirWkNo { get; set; }

    /// <summary>
    /// 雇主通報日期
    /// </summary>
    public string? KnowDate { get; set; }

    /// <summary>
    /// 移工解聘日期
    /// </summary>
    public string? DynaDate { get; set; }

    /// <summary>
    /// 狀況代碼
    /// </summary>
    public string? HappCode { get; set; }

    /// <summary>
    /// 雇主處分代碼
    /// </summary>
    public string? VendCode { get; set; }

    /// <summary>
    /// 外國人處分代碼
    /// </summary>
    public string? LaborCode { get; set; }

    /// <summary>
    /// 撤銷函日期
    /// </summary>
    public string? ExpireDate { get; set; }

    /// <summary>
    /// 打詐通報種類代碼
    /// </summary>
    public string? ImmiType { get; set; }

    /// <summary>
    /// 打詐通報種類代碼中文說明
    /// </summary>
    public string? ImmiTypeDesc { get; set; }

    /// <summary>
    /// 狀況代碼中文說明
    /// </summary>
    public string? HappCodeDesc { get; set; }

    /// <summary>
    /// 雇主處分代碼中文說明
    /// </summary>
    public string? VendCodeDesc { get; set; }

    /// <summary>
    /// 外國人處分代碼中文說明
    /// </summary>
    public string? LaborCodeDesc { get; set; }

    /// <summary>
    /// 定型搞代碼
    /// </summary>
    public string? WpCode { get; set; }

    /// <summary>
    /// 定型搞代碼中文說明
    /// </summary>
    public string? WpCodeDesc { get; set; }

    /// <summary>
    /// 勞發署居留證號
    /// </summary>
    public string? Resnum { get; set; }

    /// <summary>
    /// 居留截止日
    /// </summary>
    public string? ImmigartionDate { get; set; }

    /// <summary>
    /// 移民署傳檔日
    /// </summary>
    public string? SystemDate { get; set; }

    /// <summary>
    /// 移民署居留證號
    /// </summary>
    public string? NiaResidenceNo { get; set; }

    /// <summary>
    /// 資料更新日期
    /// </summary>
    public string? CreateDate { get; set; }

    /// <summary>
    /// 正附卡人名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();
}
