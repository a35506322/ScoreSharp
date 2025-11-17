using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignment;

public class UpdateObligedAssignmentRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    [Required]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 指派徵信案件列表
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<AssignCaseDto> AssignCaseList { get; set; } = new();

    /// <summary>
    /// 指派徵信人員
    /// </summary>
    [Display(Name = "指派徵信人員")]
    public string? AssignToUserId { get; set; }
}

public class AssignCaseDto
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "PK")]
    [Required]
    public string SeqNo { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    [Required]
    [Display(Name = "卡片狀態")]
    [ValidEnumValue]
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 人員指派變更狀態
    /// </summary>
    [Required]
    [Display(Name = "人員指派變更狀態")]
    [ValidEnumValue]
    public AssignmentChangeStatus AssignmentChangeStatus { get; set; }
}

public class CheckCaseCanAssignDto
{
    public string AssignedUserId { get; set; }
    public string? AssignedEmployeeNo { get; set; }
    public string? PromotionUserId { get; set; }
    public HashSet<string>? MonthlyIncomeCheckUserId { get; set; }
    public ILookup<string, Reviewer_Stakeholder> StakeholderLookup { get; set; }
    public HashSet<string> IdSet { get; set; }
}

public class BaseDataGroup
{
    public string ApplyNo { get; set; }
    public List<GetApplyCreditCardBaseDataResult> Cases { get; set; } = new();
}
