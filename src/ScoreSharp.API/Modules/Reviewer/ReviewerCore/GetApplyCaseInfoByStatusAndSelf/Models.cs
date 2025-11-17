namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCaseInfoByStatusAndSelf;

public class GetApplyCaseInfoByStatusAndSelfResponse
{
    /// <summary>
    /// 案件數量統計
    /// </summary>
    public List<CaseCountStatisticDto> CaseCountStatistic { get; set; } = new();

    /// <summary>
    /// 查詢案件資訊
    /// </summary>
    public List<BaseData> BaseDataList { get; set; } = new();
}

public class ApplyCardListDto
{
    /// <summary>
    /// 卡片處理序號
    /// </summary>
    public string HandleSeqNo { get; set; }

    /// <summary>
    /// 卡片階段
    /// </summary>
    public CardStep? CardStep { get; set; }

    /// <summary>
    /// 卡片階段名稱
    /// </summary>
    public string CardStepName => CardStep is not null ? CardStep.ToString() : string.Empty;

    /// <summary>
    /// 正附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();

    /// <summary>
    /// 申請卡別代碼
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡別名稱
    /// </summary>
    public string ApplyCardName { get; set; }

    /// <summary>
    /// 該卡片的狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 卡片狀態名稱
    /// </summary>
    public string CardStatusName => CardStatus.ToString();

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }
}

public class NameCheckDto
{
    public string NameChecked { get; set; }
    public UserType UserType { get; set; }
    public string UserTypeName => UserType.ToString();
}

public class BaseData
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
    /// 姓名檢核-正/附卡
    /// </summary>
    public List<NameCheckDto> NameCheckList { get; set; } = new();

    /// <summary>
    /// 身份證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 申請卡別
    /// </summary>
    public List<ApplyCardListDto> ApplyCardTypeList { get; set; } = new();

    /// <summary>
    /// 申請日期
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 案件種類，如一般件
    /// </summary>
    public CaseType? CaseType { get; set; }
    public string CaseTypeName => CaseType?.ToString() ?? string.Empty;

    /// <summary>
    /// 最後處理時間
    /// </summary>
    public DateTime? LastUpdateTime { get; set; }

    /// <summary>
    /// 推廣單位
    /// </summary>
    public string? PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    public string? PromotionUser { get; set; }

    /// <summary>
    /// 來源
    /// </summary>
    public Source? Source { get; set; }
}

public class CaseCountStatisticDto
{
    public CaseStatus CaseStatus { get; set; }
    public string CaseStatusName => CaseStatus.ToString();
    public int Count { get; set; }
}
