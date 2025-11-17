namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.InsertUserTakeVacation;

public class InsertUserTakeVacationRequest
{
    /// <summary>
    /// 帳號
    /// </summary>
    [Display(Name = "帳號")]
    [MaxLength(30)]
    [Required]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 開始時間
    /// </summary>
    [Display(Name = "開始時間")]
    [Required]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    [Display(Name = "結束時間")]
    [Required]
    public DateTime EndTime { get; set; }
}
