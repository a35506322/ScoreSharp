namespace ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizesByQueryString;

public class GetOrganizeByQueryStringRequest { }

public class GetOrganizesByQueryStringResponse
{
    /// <summary>
    /// 部門代碼
    /// </summary>
    public string OrganizeCode { get; set; } = null!;

    /// <summary>
    /// 部門名稱
    /// </summary>
    public string OrganizeName { get; set; } = null!;

    /// <summary>
    /// 法定代理人
    /// </summary>
    public string LegalRepresentative { get; set; } = null!;

    /// <summary>
    /// 郵遞區號
    /// </summary>
    public string ZIPCode { get; set; } = null!;

    /// <summary>
    /// 住址
    /// </summary>
    public string FullAddress { get; set; } = null!;

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
