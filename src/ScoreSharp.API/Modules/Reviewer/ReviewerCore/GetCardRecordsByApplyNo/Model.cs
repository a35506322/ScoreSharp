namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetCardRecordsByApplyNo;

public class GetCardRecordsByApplyNoResponse
{
    /// <summary>
    /// PK
    /// 自增
    /// </summary>
    [Display(Name = "PK")]
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// 非叢集索引
    /// </summary>
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    [Display(Name = "卡片狀態")]
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 卡片狀態名稱
    /// </summary>
    [Display(Name = "卡片狀態名稱")]
    public string CardStatusName => this.CardStatus.ToString();

    /// <summary>
    /// 核卡額度
    /// </summary>
    [Display(Name = "核卡額度")]
    public int? CardLimit { get; set; }

    /// <summary>
    /// 核准員工
    /// </summary>
    [Display(Name = "核准員工")]
    public string ApproveUserId { get; set; }

    /// <summary>
    /// 核准員工姓名
    /// </summary>
    [Display(Name = "核准員工姓名")]
    public string ApproveUserName { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    [Display(Name = "新增時間")]
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 處理備註
    /// </summary>
    [Display(Name = "處理備註")]
    public string? HandleNote { get; set; }

    /// <summary>
    /// FK
    /// 關聯 Reviewer_ApplyCreditCardInfoHandle
    /// ULID
    /// </summary>
    [Display(Name = "FK")]
    public string HandleSeqNo { get; set; }
}
