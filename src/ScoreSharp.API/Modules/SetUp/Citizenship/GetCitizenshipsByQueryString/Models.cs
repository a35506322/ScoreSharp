namespace ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipsByQueryString;

public class GetCitizenshipsByQueryStringRequest
{
    /// <summary>
    /// 國籍代碼，範例 : TW
    /// </summary>
    [Display(Name = "國籍代碼")]
    public string? CitizenshipCode { get; set; }

    /// <summary>
    /// 國籍名稱
    /// </summary>
    [Display(Name = "國籍名稱")]
    public string? CitizenshipName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetCitizenshipsByQueryStringResponse
{
    /// <summary>
    /// 國籍代碼，範例 : TW
    /// </summary>
    public string CitizenshipCode { get; set; } = null!;

    /// <summary>
    /// 國籍名稱
    /// </summary>
    public string CitizenshipName { get; set; } = null!;

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
