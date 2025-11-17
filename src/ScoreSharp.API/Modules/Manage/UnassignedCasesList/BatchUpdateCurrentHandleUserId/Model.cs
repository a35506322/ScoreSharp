namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchUpdateCurrentHandleUserId;

public class BatchUpdateCurrentHandleUserIdRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    public List<string> ApplyNo { get; set; }

    /// <summary>
    /// 當前狀態處理經辦
    /// </summary>
    [Required]
    [Display(Name = "當前狀態處理經辦")]
    public string CurrentHandleUserId { get; set; }

    /// <summary>
    /// 分案類型
    /// </summary>
    [Required]
    [Display(Name = "分案類型")]
    [ValidEnumValue]
    public CaseAssignmentType CaseAssignmentType { get; set; }
}
