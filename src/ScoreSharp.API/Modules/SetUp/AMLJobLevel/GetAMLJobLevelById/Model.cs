namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelById;

public class GetAMLJobLevelByIdResponse
{
    /// <summary>
    /// AML職級別代碼，範例: 1
    /// </summary>
    public string AMLJobLevelCode { get; set; }

    /// <summary>
    /// AML職級別名稱
    /// </summary>
    public string AMLJobLevelName { get; set; } = null!;

    /// <summary>
    /// 是否為高階管理人，Y | N
    /// </summary>
    public string IsSeniorManagers { get; set; } = null!;

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
