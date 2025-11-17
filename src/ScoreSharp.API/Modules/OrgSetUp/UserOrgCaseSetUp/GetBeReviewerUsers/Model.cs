namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetBeReviewerUsers;

public class GetBeReviewerUsersResponse
{
    /// <summary>
    /// 使用者帳號
    /// </summary>
    [Display(Name = "使用者帳號")]
    public string UserId { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    [Display(Name = "使用者姓名")]
    public string UserName { get; set; }
}

public class GetBeReviewerUsersRequest : IValidatableObject
{
    /// <summary>
    /// 查詢類型
    ///
    /// SELF: 自己
    /// ALL: 全部
    /// </summary>
    [Display(Name = "查詢類型")]
    [Required]
    public string QueryType { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (
            !QueryType.Equals(查詢類型.自己, StringComparison.OrdinalIgnoreCase)
            && !QueryType.Equals(查詢類型.全部, StringComparison.OrdinalIgnoreCase)
        )
        {
            yield return new ValidationResult("查詢類型只能是 SELF 或 ALL", new[] { nameof(QueryType) });
        }
    }
}

public static class 查詢類型
{
    public const string 自己 = "SELF";
    public const string 全部 = "ALL";
}
