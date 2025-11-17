namespace ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonById;

public class GetLongTermReasonByIdResponse
{
    /// <summary>
    /// 長循分期戶理由碼代碼，範例: 01、AZ
    /// </summary>
    public string LongTermReasonCode { get; set; } = null!;

    /// <summary>
    /// 長循分期戶理由碼名稱
    /// </summary>
    public string LongTermReasonName { get; set; } = null!;

    /// <summary>
    /// 理由強度，範圍 1-  99
    /// </summary>
    public int ReasonStrength { get; set; }

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
    public DateTime? UpdateTime { get; set; }
}
