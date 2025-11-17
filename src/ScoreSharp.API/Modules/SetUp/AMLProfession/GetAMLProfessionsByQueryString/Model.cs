using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionsByQueryString;

public class GetAMLProfessionsByQueryStringRequest
{
    /// <summary>
    /// 版本
    /// </summary>
    [Display(Name = "版本")]
    public string? Version { get; set; }

    /// <summary>
    /// AML職業別名稱
    /// </summary>
    [Display(Name = "AML職業別名稱")]
    public string? AMLProfessionName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    public string? IsActive { get; set; }
}

public class GetAMLProfessionsByQueryStringResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public string SeqNo { get; set; } = null!;

    /// <summary>
    /// AML職業別代碼
    /// </summary>
    public string AMLProfessionCode { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = null!;

    /// <summary>
    /// AML職業別名稱
    /// </summary>
    public string AMLProfessionName { get; set; } = null!;

    /// <summary>
    /// AML職業別比對結果，Y | N
    /// </summary>
    public string AMLProfessionCompareResult { get; set; } = null!;

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
