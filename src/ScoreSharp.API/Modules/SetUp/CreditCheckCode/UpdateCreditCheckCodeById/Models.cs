namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.UpdateCreditCheckCodeById;

public class UpdateCreditCheckCodeByIdRequest
{
    /// <summary>
    /// 徵信代碼代碼，範例: A01
    /// </summary>
    [Display(Name = "徵信代碼代碼")]
    [MaxLength(3)]
    [Required]
    public string CreditCheckCode { get; set; }

    /// <summary>
    /// 徵信代碼名稱
    /// </summary>
    [Display(Name = "徵信代碼名稱")]
    [MaxLength(30)]
    [Required]
    public string CreditCheckCodeName { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }
}
