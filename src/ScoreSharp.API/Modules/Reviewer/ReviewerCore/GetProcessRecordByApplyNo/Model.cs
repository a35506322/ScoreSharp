namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetProcessRecordByApplyNo;

public class GetProcessRecordByApplyNoResponse
{
    public List<ApplyCreditCardInfoProcess> ApplyCreditCardInfoProcess { get; set; }
    public List<ApplyFileLog> ApplyFileLog { get; set; }
}

public class ApplyCreditCardInfoProcess
{
    /// <summary>
    /// PK
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 案件狀態跟執行動作
    /// </summary>
    public string Process { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// 處理人員
    /// 系統人員 =  SYSTEM
    /// </summary>
    public string? ProcessUserId { get; set; }

    /// <summary>
    /// 處理人員姓名
    /// </summary>
    public string? ProcessUserName { get; set; }
}

public class ApplyFileLog
{
    /// <summary>
    /// PK
    /// </summary>
    public int SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 狀態或者動作
    /// </summary>
    public string Process { get; set; }

    /// <summary>
    /// 頁次
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 新增員工，系統執行為SYSTEM
    /// </summary>
    public string AddUserId { get; set; }

    /// <summary>
    /// 新增員工姓名
    /// </summary>
    public string AddUserName { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 檔案ID
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 資料庫名稱
    /// </summary>
    public string DBName { get; set; }

    /// <summary>
    /// 是否為歷史檔案
    /// </summary>
    public string IsHistory { get; set; }
}
