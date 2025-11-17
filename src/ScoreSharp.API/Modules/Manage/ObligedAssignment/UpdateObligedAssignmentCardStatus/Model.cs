namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignmentCardStatus;

public class UpdateObligedAssignmentCardStatusRequest
{
    [Required]
    [Display(Name = "卡片狀態")]
    [ValidEnumValue]
    public CardStatus CardStatus { get; set; }

    [Required]
    [Display(Name = "角色ID列表")]
    [MinLength(1)]
    public List<string> RoleIds { get; set; } = new();
}
