namespace ScoreSharp.API.Modules.Manage.TransferCase.GetTransferCasesDetailByQueryString;

public class GetTransferCasesDetailByQueryStringRequest
{
    [Required]
    [Display(Name = "徵審人員")]
    public string TransferredUserId { get; set; }

    [Required]
    [Display(Name = "案件類型")]
    [ValidEnumValue]
    public TransferCaseType TransferCaseType { get; set; }
}

public class GetTransferCasesDetailByQueryStringResponse
{
    /// <summary>
    /// 申請書編號：範例20180625A0001
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 中文姓名
    /// </summary>
    public string CHName { get; set; }

    /// <summary>
    /// 身份證字號
    /// </summary>
    public string ID { get; set; }

    public List<ApplyCardTypeDto> ApplyCardTypeList { get; set; }

    /// <summary>
    /// 案件種類，如一般件
    /// </summary>
    public CaseType? CaseType { get; set; }
    public string CaseTypeName => CaseType?.ToString() ?? string.Empty;

    /// <summary>
    /// 急件註記
    /// </summary>
    public string IsUrgent => CaseType == ScoreSharp.Common.Enums.CaseType.急件 ? "Y" : "N";

    /// <summary>
    /// 申請日期
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 最後處理時間
    /// </summary>
    public DateTime? LastUpdateTime { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string Notes { get; set; }
}

public class CaseStatusDto
{
    /// <summary>
    /// 卡片狀態，查看附件-卡片狀態碼
    /// </summary>
    public CardStatus CardStatus { get; set; }
    public string CardStatusName => CardStatus.ToString();
}

public class ApplyCardTypeDto
{
    /// <summary>
    /// 卡片狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 卡片狀態名稱
    /// </summary>
    public string CardStatusName { get; set; }

    /// <summary>
    /// 申請卡別：以"/"串接，如JA00/JC00
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡別
    ///
    /// (ApplyCardType)ApplyCardName
    ///
    /// (JA00)JCB 商務晶緻卡
    /// </summary>
    public string ApplyCardTypeName { get; set; }
}
