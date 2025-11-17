namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 申請信用卡Base資料查詢結果
/// </summary>
public class QueryUspGetApplyCreditCardBaseDataByConditionsResult
{
    /// <summary>
    /// 申請編號
    /// </summary>
    public string ApplyNo { get; set; } = string.Empty;

    /// <summary>
    /// 主卡_身分證字號
    /// </summary>
    public string M_ID { get; set; } = string.Empty;

    /// <summary>
    /// 主卡_中文姓名
    /// </summary>
    public string M_CHName { get; set; } = string.Empty;

    /// <summary>
    /// 主卡_姓名檢核結果
    /// </summary>
    public string M_NameChecked { get; set; }

    /// <summary>
    /// 申請日期
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 案件類型
    /// </summary>
    public CaseType? CaseType { get; set; }

    /// <summary>
    /// 主卡_是否重複進件
    /// </summary>
    public string M_IsRepeatApply { get; set; }

    /// <summary>
    /// 原持卡人
    /// </summary>
    public CardOwner CardOwner { get; set; }

    /// <summary>
    /// 推廣單位
    /// </summary>
    public string PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員
    /// </summary>
    public string PromotionUser { get; set; }

    /// <summary>
    /// 目前處理人員 ID
    /// </summary>
    public string CurrentHandleUserId { get; set; }

    /// <summary>
    /// 最後更新人員 ID
    /// </summary>
    public string LastUpdateUserId { get; set; }

    /// <summary>
    /// 最後更新時間
    /// </summary>
    public DateTime? LastUpdateTime { get; set; }

    // H 表 - 處理歷程資料
    /// <summary>
    /// 處理序號
    /// </summary>
    public string HandleSeqNo { get; set; }

    /// <summary>
    /// 卡片階段
    /// </summary>
    public CardStep? CardStep { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 申請卡別
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 月收入檢核人員 ID
    /// </summary>
    public string MonthlyIncomeCheckUserId { get; set; }

    /// <summary>
    /// 月收入檢核時間
    /// </summary>
    public DateTime? MonthlyIncomeTime { get; set; }

    /// <summary>
    /// 徵審人員 ID
    /// </summary>
    public string ReviewerUserId { get; set; }

    /// <summary>
    /// 徵審時間
    /// </summary>
    public DateTime? ReviewerTime { get; set; }

    /// <summary>
    /// 核准人員 ID
    /// </summary>
    public string ApproveUserId { get; set; }

    /// <summary>
    /// 核准時間
    /// </summary>
    public DateTime? ApproveTime { get; set; }

    // S 表 - 附卡人資料
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
    /// 正附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 來源
    /// </summary>
    public Source? Source { get; set; }

    /// <summary>
    /// 正附卡是否為原持卡人
    /// </summary>
    public string M_IsOriginalCardholder { get; set; }

    /// <summary>
    /// 附卡是否為原持卡人
    /// </summary>
    public string S1_IsOriginalCardholder { get; set; }
}
