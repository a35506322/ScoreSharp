namespace ScoreSharp.Batch.Jobs.SupplementTemplateReport;

public class PeddingSupplementCaseDto
{
    /// <summary>
    /// 申請編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 批次補卡狀態
    /// </summary>
    public string BatchSupplementStatus { get; set; }

    /// <summary>
    /// 批次補卡時間
    /// </summary>
    public DateTime? BatchSupplementTime { get; set; }

    /// <summary>
    /// 補卡原因代碼
    /// </summary>
    public string SupplementReasonCode { get; set; }

    /// <summary>
    /// 補卡寄送地址類型
    /// </summary>
    public MailingAddressType SupplementSendCardAddr { get; set; }

    /// <summary>
    /// 中文姓名
    /// </summary>
    public string CHName { get; set; }

    /// <summary>
    /// 手機
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 郵遞區號
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 區域
    /// </summary>
    public string? District { get; set; }

    /// <summary>
    /// 路名
    /// </summary>
    public string? Road { get; set; }

    /// <summary>
    /// 巷
    /// </summary>
    public string? Lane { get; set; }

    /// <summary>
    /// 弄
    /// </summary>
    public string? Alley { get; set; }

    /// <summary>
    /// 號
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// 之號
    /// </summary>
    public string? SubNumber { get; set; }

    /// <summary>
    /// 樓層
    /// </summary>
    public string? Floor { get; set; }

    /// <summary>
    /// 其他地址資訊
    /// </summary>
    public string? Other { get; set; }

    /// <summary>
    /// 補卡原因 (中文)
    /// </summary>
    public string[] SupplementReasonNames { get; set; }

    /// <summary>
    /// 處理序號
    /// </summary>
    public string HandleSeqNo { get; set; }
}
