namespace ScoreSharp.API.Modules.SetUp.BillDay.UpdateBillDayForIsActiveById;

public class UpdateBillDayForIsActiveByIdRequest
{
    /// <summary>
    /// 帳單日，範例：01、03
    /// 檢驗不可超過 31
    /// </summary>
    [Display(Name = "帳單日")]
    [Required]
    [RegularExpression("^(0[1-9]|1[0-9]|2[0-9]|3[01])$")]
    public string BillDay { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; } = null!;
}
