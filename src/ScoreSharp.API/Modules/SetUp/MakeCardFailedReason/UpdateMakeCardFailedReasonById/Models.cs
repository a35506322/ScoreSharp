namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.UpdateMakeCardFailedReasonById;

public class UpdateMakeCardFailedReasonByIdRequest
{
    /// <summary>
    /// 製卡失敗原因代碼，範例: 01、AZ
    /// </summary>
    [Display(Name = "製卡失敗原因代碼")]
    [MaxLength(2)]
    [Required]
    public string MakeCardFailedReasonCode { get; set; }

    /// <summary>
    /// 製卡失敗原因名稱
    /// </summary>
    [Display(Name = "製卡失敗原因名稱")]
    [MaxLength(30)]
    [Required]
    public string MakeCardFailedReasonName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
