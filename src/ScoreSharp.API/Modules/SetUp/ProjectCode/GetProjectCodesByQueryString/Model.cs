namespace ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodesByQueryString;

public class GetProjectCodesByQueryStringRequest
{
    /// <summary>
    /// 活動名稱
    /// </summary>
    [Display(Name = "活動名稱")]
    public string? ProjectName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetProjectCodesByQueryStringResponse
{
    /// <summary>
    /// 專案代碼，範例: 100，長度3碼
    /// </summary>
    public string ProjectCode { get; set; } = null!;

    /// <summary>
    /// 活動名稱
    /// </summary>
    public string ProjectName { get; set; } = null!;

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
