namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class ConcernDetailInfo : BaseResDto
{
    /// <summary>
    /// 告誡名單 (A)
    /// </summary>
    public List<Reviewer3rd_WarnLog> WarnLogs { get; set; } = new();

    /// <summary>
    /// 受警示企業戶之負責人 (B)
    /// </summary>
    public List<Reviewer3rd_WarnCompLog> WarningCompanyLogs { get; set; } = new();

    /// <summary>
    /// 風險帳戶 (C)
    /// </summary>
    public List<Reviewer3rd_RiskAccountLog> RiskAccountLogs { get; set; } = new();

    /// <summary>
    /// 聯徵資料─行方不明 (D)
    /// </summary>
    public List<Reviewer3rd_FledLog> FledLogs { get; set; } = new();

    /// <summary>
    /// 聯徵資料─收容遣返 (E)
    /// </summary>
    public List<Reviewer3rd_PunishLog> PunishLogs { get; set; } = new();

    /// <summary>
    /// 聯徵資料─出境 (F)
    /// </summary>
    public List<Reviewer3rd_ImmiLog> ImmiLogs { get; set; } = new();

    /// <summary>
    /// 失蹤人口 (G)
    /// </summary>
    public Reviewer3rd_MissingPersonsLog? MissingPersonsLogs { get; set; } = new();

    /// <summary>
    /// 疑似涉詐境內帳戶 (H)
    /// </summary>
    public List<Reviewer3rd_FrdIdLog> FrdIdLogs { get; set; } = new();

    /// <summary>
    /// 聯徵資料─解聘 (I)
    /// </summary>
    public List<Reviewer3rd_LayOffLog> LayOffLogs { get; set; } = new();

    /// <summary>
    /// 關注名單1
    /// </summary>
    public List<string> Focus1HitList { get; set; } = new();

    /// <summary>
    /// 關注名單1 是否命中
    /// </summary>
    public string Focus1Checked => Focus1HitList.Any() ? "Y" : "N";

    /// <summary>
    /// 關注名單2
    /// </summary>
    public List<string> Focus2HitList { get; set; } = new();

    /// <summary>
    /// 關注名單2 是否命中
    /// </summary>
    public string Focus2Checked => Focus2HitList.Any() ? "Y" : "N";
}
