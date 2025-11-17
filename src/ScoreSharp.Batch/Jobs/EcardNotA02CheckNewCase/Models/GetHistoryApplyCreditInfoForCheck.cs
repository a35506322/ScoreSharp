namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class GetHistoryApplyCreditInfoForCheck
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// IP 位址
    /// </summary>
    public string UserSourceIP { get; set; }

    /// <summary>
    /// 手機
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 信箱
    /// </summary>
    public string EMail { get; set; }

    /// <summary>
    /// 申請日期
    /// </summary>
    public DateTime ApplyDate { get; set; }
}
