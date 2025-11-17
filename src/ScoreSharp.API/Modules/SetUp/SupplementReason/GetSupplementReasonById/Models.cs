namespace ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonById;

public class GetSupplementReasonByIdResponse
{
    /// <summary>
    /// 補件代碼，範例: 01
    /// </summary>
    public string SupplementReasonCode { get; set; }

    /// <summary>
    /// 補件名稱
    /// </summary>
    public string SupplementReasonName { get; set; }

    /// <summary>
    /// 是否啟用，範例: Y | N
    /// </summary>
    public string IsActive { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; }

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
    public DateTime? UpdateTime { get; set; }
}
