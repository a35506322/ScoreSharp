namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

public class GetApplyCreditCardBaseDataDto
{
    /// <summary>
    /// 身份證字號（選填）
    /// </summary>
    public string? ID { get; set; } = null;

    /// <summary>
    /// 中文姓名（選填）
    /// </summary>
    public string? CHName { get; set; } = null;

    /// <summary>
    /// 申請書編號（選填）
    /// </summary>
    public string? ApplyNo { get; set; }

    /// <summary>
    /// 卡片狀態（選填）- 會過濾「至少有一張卡片符合此狀態」的案件
    /// </summary>
    public List<CardStatus>? CardStatus { get; set; } = null;

    /// <summary>
    /// 申請卡別（選填）
    /// </summary>
    public string? ApplyCardType { get; set; } = null;

    /// <summary>
    /// 申請日期起（選填）
    /// </summary>
    public DateTime? ApplyDateStart { get; set; } = null;

    /// <summary>
    /// 申請日期迄（選填）
    /// </summary>
    public DateTime? ApplyDateEnd { get; set; } = null;

    /// <summary>
    /// 來源（選填）
    /// </summary>
    public List<Source>? Source { get; set; } = null;

    /// <summary>
    /// 當前經辦（選填）
    /// 如需查詢未指派案件，請填入特殊值 "NULL"（以尖括號包圍）
    /// </summary>
    public string? CurrentHandleUserId { get; set; } = null;

    /// <summary>
    /// 當前經辦列表（選填）
    /// 如需查詢多個經辦，請填入經辦ID，以逗號分隔
    /// </summary>
    public string? CurrentHandleUserIds { get; set; } = null;

    /// <summary>
    /// 取前幾筆（選填）
    /// NULL = 取全部，大於 0 = 取前 N 筆
    /// </summary>
    public int? Top { get; set; } = null;
}
