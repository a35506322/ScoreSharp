using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Helpers;

public static class ValidateDataFormatBase
{
    public static void AddRequired(List<ValidationResult> results, string? value, string message, string memberName)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        results.Add(CreateResult(message, memberName));
    }

    public static ValidationResult CreateResult(string message, string memberName)
    {
        return new ValidationResult(message, new[] { memberName });
    }

    /// <summary>
    /// 是否需要填寫外籍文件。
    /// </summary>
    public static bool ShouldRequireForeignDocumentation(string? id, bool requiresAction)
    {
        if (!IsForeignId(id))
        {
            return false;
        }

        return requiresAction;
    }

    public static bool RequiresActionFor(UserType userType, IEnumerable<HandleInfoContext> handles)
    {
        return handles.Any(handle => RequiredActions.Contains(handle.RecentAction) && handle.UserType == userType);
    }

    public static bool RequiresMonthlyIncome(IEnumerable<HandleInfoContext> handles)
    {
        return handles.Any(handle => RequiredActions.Contains(handle.RecentAction));
    }

    /// <summary>
    /// 是否為外籍身分證字號。
    /// </summary>
    public static bool IsForeignId(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var normalized = id.Trim().ToUpperInvariant();
        return normalized.Length >= 2 && normalized[1] is not ('1' or '2');
    }

    public static readonly HashSet<ReviewAction> RequiredActions = new()
    {
        ReviewAction.完成月收入確認,
        ReviewAction.排入核卡,
        ReviewAction.核卡作業,
    };

    private const string PermanentResidenceValue = "99991231";
    private const string PermanentResidenceValue2 = "999912";
    private const string YesValue = "Y";
    private const string NoValue = "N";
    private const int ValidityPrefixLength = 6;

    public static void ValidateResidencePermitValues(
        List<ValidationResult> results,
        string? isForeverResidencePermit,
        string? residencePermitDeadline,
        string? expatValidityPeriod,
        string applicantLabel,
        string residencePermitMember,
        string expatValidityMember
    )
    {
        if (string.Equals(isForeverResidencePermit, YesValue, StringComparison.OrdinalIgnoreCase))
        {
            EnsureEquals(
                results,
                residencePermitDeadline,
                PermanentResidenceValue,
                $"{applicantLabel}_居留證期限需為{PermanentResidenceValue}",
                residencePermitMember
            );
            EnsureEquals(
                results,
                expatValidityPeriod,
                PermanentResidenceValue2,
                $"{applicantLabel}_外籍人士指定效期需為{PermanentResidenceValue2}",
                expatValidityMember
            );
            return;
        }

        if (!string.Equals(isForeverResidencePermit, NoValue, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(residencePermitDeadline) || string.IsNullOrWhiteSpace(expatValidityPeriod))
        {
            return;
        }

        var deadlinePrefix =
            residencePermitDeadline.Length >= ValidityPrefixLength ? residencePermitDeadline[..ValidityPrefixLength] : residencePermitDeadline;

        if (string.Equals(deadlinePrefix, expatValidityPeriod, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        results.Add(CreateResult($"{applicantLabel}_外籍人士指定效期需與居留證期限前六碼一致", expatValidityMember));
    }

    public static void EnsureEquals(List<ValidationResult> results, string? value, string targetValue, string message, string memberName)
    {
        if (string.Equals(value, targetValue, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        results.Add(CreateResult(message, memberName));
    }
}
