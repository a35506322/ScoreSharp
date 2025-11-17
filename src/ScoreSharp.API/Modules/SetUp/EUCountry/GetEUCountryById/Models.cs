namespace ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountryById;

public class GetEUCountryByIdResponse
{
    /// <summary>
    /// EU國家代碼，範例 : TW
    /// </summary>
    public string EUCountryCode { get; set; } = null!;

    /// <summary>
    /// EU國家名稱
    /// </summary>
    public string EUCountryName { get; set; } = null!;

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
