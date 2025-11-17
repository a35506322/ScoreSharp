namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountryById;

public class GetEUSanctionCountryByIdResponse
{
    /// <summary>
    /// EU制裁國家代碼，範例 : TW
    /// </summary>
    public string EUSanctionCountryCode { get; set; }

    /// <summary>
    /// EU制裁國家名稱
    /// </summary>
    public string EUSanctionCountryName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
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
