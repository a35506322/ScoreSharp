namespace ScoreSharp.API.Modules.SetUp.BlackListReason.UpdateBlackListReasonById;

public class UpdateBlackListReasonByIdRequest
{
    /// <summary>
    /// 黑名單理由代碼，範例: 01、AZ
    /// </summary>
    [Display(Name = "黑名單理由代碼")]
    [MaxLength(2)]
    [Required]
    public string BlackListReasonCode { get; set; }

    /// <summary>
    /// 黑名單理由名稱
    /// </summary>
    [Display(Name = "黑名單理由名稱")]
    [MaxLength(30)]
    [Required]
    public string BlackListReasonName { get; set; }

    /// <summary>
    /// 理由強度，範圍 1-  99，用來判斷黑名單理由是否顯示，目前系統只會顯示最強那筆
    /// </summary>
    [Display(Name = "理由強度")]
    [Range(1, 99)]
    [Required]
    public int ReasonStrength { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
