namespace ScoreSharp.API.Modules.SetUp.LongTermReason.UpdateLongTermReasonById;

public class UpdateLongTermReasonByIdRequest
{
    /// <summary>
    /// 長循分期戶理由碼代碼，範例: 01、AZ
    /// </summary>
    [Display(Name = "長循分期戶理由碼代碼")]
    [MaxLength(2)]
    [Required]
    public string LongTermReasonCode { get; set; }

    /// <summary>
    /// 長循分期戶理由碼名稱
    /// </summary>
    [Display(Name = "長循分期戶理由碼名稱")]
    [MaxLength(30)]
    [Required]
    public string LongTermReasonName { get; set; }

    /// <summary>
    /// 理由強度，範圍 1-  99
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
