namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 正卡人資料格式驗證所需欄位集合。
/// </summary>
public sealed class ReviewerMainDataContext : IValidatableObject
{
    /// <summary>
    /// 正卡人身分證字號。
    /// </summary>
    [Display(Name = "正卡人_身分證字號")]
    [TWID]
    public string ID { get; init; }

    /// <summary>
    /// 正卡人中文姓名。
    /// </summary>
    [Display(Name = "正卡人_中文姓名")]
    public string? CHName { get; init; }

    /// <summary>
    /// 使用者類型。
    /// </summary>
    public UserType UserType { get; init; } = UserType.正卡人;

    /// <summary>
    /// 正卡人電子郵件地址。
    /// </summary>
    [Display(Name = "正卡人_E-MAIL")]
    [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "電子郵件格式錯誤")]
    public string? Email { get; init; }

    /// <summary>
    /// 正卡人行動電話。
    /// </summary>
    [Display(Name = "正卡人_行動電話")]
    public string? Mobile { get; init; }

    /// <summary>
    /// 正卡人出生年月日（民國年格式）。
    /// </summary>
    [Display(Name = "正卡人_出生年月日")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string BirthDay { get; init; }

    /// <summary>
    /// 正卡人身分證發證日期。
    /// </summary>
    [Display(Name = "正卡人_身分證發證日期")]
    [ValidDate(format: "yyyMMdd", isROC: true)]
    public string? IDIssueDate { get; init; }

    /// <summary>
    /// 正卡人居留證發證日期。
    /// </summary>
    [Display(Name = "正卡人_居留證發證日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? ResidencePermitIssueDate { get; init; }

    /// <summary>
    /// 正卡人居留證期限。
    /// </summary>
    [Display(Name = "正卡人_居留證期限")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? ResidencePermitDeadline { get; init; }

    /// <summary>
    /// 正卡人護照日期。
    /// </summary>
    [Display(Name = "正卡人_護照日期")]
    [ValidDate(format: "yyyyMMdd", isROC: false)]
    public string? PassportDate { get; init; }

    /// <summary>
    /// 正卡人外籍人士指定效期。
    /// </summary>
    [Display(Name = "正卡人_外籍人士指定效期")]
    [RegularExpression(@"^$|^[0-9]{6}([0-9]{2})?$", ErrorMessage = "附卡人_外籍人士指定效期格式錯誤")]
    public string? ExpatValidityPeriod { get; init; }

    /// <summary>
    /// 正卡人是否永久居留證。
    /// </summary>
    [Display(Name = "正卡人_是否永久居留")]
    [RegularExpression(@"^$|^[YN]$", ErrorMessage = "正卡人_是否永久居留需填寫Y或N")]
    public string? IsForeverResidencePermit { get; init; }

    /// <summary>
    /// 正卡人_居留證背面號碼
    /// </summary>
    [Display(Name = "正卡人_居留證背面號碼")]
    [RegularExpression(@"^$|^[A-Z]{2}\d{8}$", ErrorMessage = "正卡人_居留證背面號碼格式錯誤")]
    public string? ResidencePermitBackendNum { get; init; }

    /// <summary>
    /// 正卡人舊照查驗結果。
    /// </summary>
    [Display(Name = "正卡人_舊照查驗")]
    [RegularExpression(@"^$|^[YN]$", ErrorMessage = "正卡人_舊照查驗需填寫Y或N")]
    public string? OldCertificateVerified { get; init; }

    /// <summary>
    /// 正卡人現職月收入金額。
    /// </summary>
    [Display(Name = "現職月收入(元)")]
    public int? CurrentMonthIncome { get; init; }

    /// <summary>
    /// 申請卡片狀態清單。
    /// </summary>
    public IReadOnlyCollection<HandleInfoContext> Handles { get; set; } = Array.Empty<HandleInfoContext>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        var statuses = Handles ?? Array.Empty<HandleInfoContext>();
        bool requiresMainStatuses = ValidateDataFormatBase.RequiresActionFor(UserType.正卡人, statuses);
        bool requiresMainMonthlyIncome = ValidateDataFormatBase.RequiresMonthlyIncome(statuses);

        if (requiresMainMonthlyIncome && (!CurrentMonthIncome.HasValue || CurrentMonthIncome.Value <= 0))
        {
            results.Add(ValidateDataFormatBase.CreateResult("現職月收入需必填", nameof(CurrentMonthIncome)));
        }

        ValidateDataFormatBase.AddRequired(results, ID, "正卡人_身分證字號為必填", nameof(ID));
        ValidateDataFormatBase.AddRequired(results, BirthDay, "正卡人_出生年月日為必填", nameof(BirthDay));

        if (ValidateDataFormatBase.ShouldRequireForeignDocumentation(ID, requiresMainStatuses))
        {
            ValidateDataFormatBase.AddRequired(results, IsForeverResidencePermit, "正卡人_是否永久居留需填寫", nameof(IsForeverResidencePermit));
            ValidateDataFormatBase.AddRequired(results, ResidencePermitIssueDate, "正卡人_居留證發證日期為必填", nameof(ResidencePermitIssueDate));
            ValidateDataFormatBase.AddRequired(results, ResidencePermitDeadline, "正卡人_居留證期限為必填", nameof(ResidencePermitDeadline));
            ValidateDataFormatBase.AddRequired(results, ResidencePermitBackendNum, "正卡人_居留證背面號碼為必填", nameof(ResidencePermitBackendNum));
            ValidateDataFormatBase.AddRequired(results, ExpatValidityPeriod, "正卡人_外籍人士指定效期為必填", nameof(ExpatValidityPeriod));
            ValidateDataFormatBase.AddRequired(results, OldCertificateVerified, "正卡人_舊照查驗需填寫", nameof(OldCertificateVerified));

            ValidateDataFormatBase.ValidateResidencePermitValues(
                results,
                IsForeverResidencePermit,
                ResidencePermitDeadline,
                ExpatValidityPeriod,
                "正卡人",
                nameof(ResidencePermitDeadline),
                nameof(ExpatValidityPeriod)
            );
        }

        return results;
    }
}
