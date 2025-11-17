namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitById;

public class GetPromotionUnitByIdResponse
{
    /// <summary>
    /// 推廣單位代碼，範例: 100，長度3碼
    /// </summary>
    public string PromotionUnitCode { get; set; } = null!;

    /// <summary>
    /// 推廣單位名稱
    /// </summary>
    public string PromotionUnitName { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    public string IsActive { get; set; } = null!;

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime UpdateTime { get; set; }
}
