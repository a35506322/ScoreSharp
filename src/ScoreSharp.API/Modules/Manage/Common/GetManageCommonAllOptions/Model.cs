namespace ScoreSharp.API.Modules.Manage.Common.GetManageCommonAllOptions;

public class GetManageCommonAllOptionsResponse
{
    /// <summary>
    /// 分案類型
    /// </summary>
    public List<OptionsDtoTypeInt> CaseAssignmentType { get; set; }

    /// <summary>
    /// 人員指派變更狀態
    /// </summary>
    public List<OptionsDtoTypeInt> AssignmentChangeStatus { get; set; }

    /// <summary>
    /// 調撥案件類型
    /// </summary>
    public List<OptionsDtoTypeInt> TransferCaseType { get; set; }

    /// <summary>
    /// 派案類型
    /// </summary>
    public List<OptionsDtoTypeInt> CaseStatisticType { get; set; }
}
