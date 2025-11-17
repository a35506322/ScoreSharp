using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.SetUp.AMLProfession.UpdateAMLProfessionById;

public class UpdateAMLProfessionByIdRequest : IValidatableObject
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "PK")]
    [Required]
    public string SeqNo { get; set; }

    /// <summary>
    /// AML職業別代碼
    /// </summary>
    [Display(Name = "AML職業別代碼")]
    [Required]
    [RegularExpression("^[0-9]+$")]
    public string AMLProfessionCode { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [Display(Name = "版本")]
    [Required]
    public string Version { get; set; }

    /// <summary>
    /// AML職業別名稱
    /// </summary>
    [Display(Name = "AML職業別名稱")]
    [Required]
    public string AMLProfessionName { get; set; }

    /// <summary>
    /// AML職業別比對結果，Y | N
    /// </summary>
    [Display(Name = "AML職業別比對結果")]
    [RegularExpression("[YN]")]
    [Required]
    public string AMLProfessionCompareResult { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        DateTime result;
        string dateFormat = "yyyyMMdd";

        if (!DateTime.TryParseExact(Version, dateFormat, null, DateTimeStyles.None, out result))
        {
            yield return new ValidationResult("不符合格式", new[] { this.GetDisplayName(nameof(Version)) });
        }
    }
}
