namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

public class GetApplyCreditCardBaseDataResult
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 正卡_身份證字號
    /// </summary>
    public string M_ID { get; set; }

    /// <summary>
    /// 正卡_中文姓名
    /// </summary>
    public string M_CHName { get; set; }

    /// <summary>
    /// 正卡_姓名檢核
    /// </summary>
    public string M_NameChecked { get; set; }

    /// <summary>
    /// 申請日期
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 案件種類
    /// </summary>
    public CaseType? CaseType { get; set; }

    /// <summary>
    /// 案件種類名稱
    /// </summary>
    public string CaseTypeName => CaseType is not null ? CaseType.ToString() : string.Empty;

    /// <summary>
    /// 正卡_是否重複進件
    /// </summary>
    public string M_IsRepeatApply { get; set; }

    /// <summary>
    /// 正附卡
    /// </summary>
    public CardOwner CardOwner { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    public string CardOwnerName => CardOwner.ToString();

    /// <summary>
    /// 推廣單位
    /// </summary>
    public string PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    public string PromotionUser { get; set; }

    /// <summary>
    /// 當前處理人員
    /// </summary>
    public string CurrentHandleUserId { get; set; }

    /// <summary>
    /// 當前處理人員姓名
    /// </summary>
    public string CurrentHandleUserName { get; set; }

    /// <summary>
    /// 最後更新人員 ID
    /// </summary>
    public string LastUpdateUserId { get; set; }

    /// <summary>
    /// 最後更新人員姓名
    /// </summary>
    public string LastUpdateUserName { get; set; }

    /// <summary>
    /// 最後更新時間
    /// </summary>
    public DateTime? LastUpdateTime { get; set; }

    /// <summary>
    /// 申請卡片資訊列表
    /// </summary>
    public List<ApplyCardInfoDto> ApplyCardList { get; set; } = new();

    /// <summary>
    /// 附卡身分證字號
    /// </summary>
    public string S1_ID { get; set; }

    /// <summary>
    /// 附卡中文姓名
    /// </summary>
    public string S1_CHName { get; set; }

    /// <summary>
    /// 附卡姓名檢核結果
    /// </summary>
    public string S1_NameChecked { get; set; }

    /// <summary>
    /// 附卡是否重複進件
    /// </summary>
    public string S1_IsRepeatApply { get; set; }

    /// <summary>
    /// 來源
    /// </summary>
    public Source? Source { get; set; }

    /// <summary>
    /// 來源名稱
    /// </summary>
    public string SourceName => Source is not null ? Source.ToString() : string.Empty;

    /// <summary>
    /// 正附卡是否為原持卡人
    /// </summary>
    public string M_IsOriginalCardholder { get; set; }

    /// <summary>
    /// 附卡是否為原持卡人
    /// </summary>
    public string S1_IsOriginalCardholder { get; set; }

    /// <summary>
    /// 是否有命中姓名檢核Y
    /// </summary>
    public bool HasNameCheckedY => M_NameChecked == "Y" || S1_NameChecked == "Y";
}

/// <summary>
/// 申請卡別資訊
/// </summary>
public class ApplyCardInfoDto
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
    /// 正附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();

    /// <summary>
    /// 卡片階段名稱
    /// </summary>
    public string CardStepName => CardStep is not null ? CardStep.ToString() : string.Empty;

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
    /// 月收入檢核人員 ID
    /// </summary>
    public string MonthlyIncomeCheckUserId { get; set; }

    /// <summary>
    /// 月收入檢核人員姓名
    /// </summary>
    public string MonthlyIncomeCheckUserName { get; set; }

    /// <summary>
    /// 月收入檢核時間
    /// </summary>
    public DateTime? MonthlyIncomeTime { get; set; }

    /// <summary>
    /// 徵審人員 ID
    /// </summary>
    public string ReviewerUserId { get; set; }

    /// <summary>
    /// 徵審人員姓名
    /// </summary>
    public string ReviewerUserName { get; set; }

    /// <summary>
    /// 徵審時間
    /// </summary>
    public DateTime? ReviewerTime { get; set; }

    /// <summary>
    /// 核准人員 ID
    /// </summary>
    public string ApproveUserId { get; set; }

    /// <summary>
    /// 核准人員姓名
    /// </summary>
    public string ApproveUserName { get; set; }

    /// <summary>
    /// 核准時間
    /// </summary>
    public DateTime? ApproveTime { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }
}
