namespace ScoreSharp.API.Modules.SetUp.FixTime.UpdateFixTime;

using System.ComponentModel.DataAnnotations;

public class UpdateFixTimeRequest : IValidatableObject
{
    /// <summary>
    /// KYC是否維護 (Y/N)
    /// </summary>
    [Display(Name = "KYC是否維護")]
    [Required(ErrorMessage = "KYC是否維護為必填")]
    [RegularExpression("[YN]", ErrorMessage = "KYC是否維護只能是 Y 或 N")]
    public string KYC_IsFix { get; set; } = null!;

    /// <summary>
    /// KYC維護開始時間
    /// </summary>
    [Display(Name = "KYC維護開始時間")]
    public DateTime? KYC_StartTime { get; set; }

    /// <summary>
    /// KYC維護結束時間
    /// </summary>
    [Display(Name = "KYC維護結束時間")]
    public DateTime? KYC_EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (KYC_IsFix == "Y")
        {
            if (!KYC_StartTime.HasValue || !KYC_EndTime.HasValue)
            {
                yield return new ValidationResult("當KYC維護設為Y時，開始時間與結束時間為必填", new[] { nameof(KYC_StartTime) });

                if (KYC_StartTime > KYC_EndTime)
                {
                    yield return new ValidationResult("開始時間不能大於結束時間", new[] { nameof(KYC_StartTime) });
                }
            }
        }
    }
}
