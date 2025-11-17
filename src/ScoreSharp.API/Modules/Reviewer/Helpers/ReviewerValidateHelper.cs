using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Reviewer.Helpers;

public class ReviewerValidateHelper : IReviewerValidateHelper
{
    public ReviewerValidationResult Validate(ReviewerValidationContext context, ReviewerValidationRuleSet rules)
    {
        var errors = new List<ReviewerValidationError>();
        var caseInfo = context.CaseInfo;

        if (context.HandleStatuses.Count == 0)
        {
            throw new ArgumentException("HandleStatuses 不能為空");
        }

        if (
            caseInfo.CardOwner == CardOwner.正卡_附卡
            && !context.HandleStatuses.Any(s => s.UserType == UserType.正卡人)
            && !context.HandleStatuses.Any(s => s.UserType == UserType.附卡人)
        )
        {
            throw new ArgumentException("正卡_附卡 時，HandleStatuses 中必須包含正卡人及附卡人，請檢查");
        }

        if (caseInfo.CardOwner == CardOwner.正卡 && !context.HandleStatuses.Any(s => s.UserType == UserType.正卡人))
        {
            throw new ArgumentException("正卡 時，HandleStatuses 中必須包含正卡人，請檢查");
        }

        if (caseInfo.CardOwner == CardOwner.附卡 && !context.HandleStatuses.Any(s => s.UserType == UserType.附卡人))
        {
            throw new ArgumentException("附卡 時，HandleStatuses 中必須包含附卡人，請檢查");
        }

        if (rules.HasFlag(ReviewerValidationRuleSet.DataFormat))
        {
            if (caseInfo.CardOwner == CardOwner.正卡_附卡 && (context.MainData is null || context.SupplementaryData is null))
            {
                throw new ArgumentException("正卡_附卡 時，MainData 和 SupplementaryData 不能為空");
            }
            else if (caseInfo.CardOwner == CardOwner.正卡 && context.MainData is null)
            {
                throw new ArgumentException("正卡 時，MainData 不能為空");
            }
            else if (caseInfo.CardOwner == CardOwner.附卡 && (context.SupplementaryData is null || context.MainData is null))
            {
                throw new ArgumentException("附卡 時，MainData 和 SupplementaryData 不能為空");
            }

            var result = ValidateDataFormat(
                caseInfo: caseInfo,
                main: context.MainData,
                supplementary: context.SupplementaryData,
                handleStatuses: context.HandleStatuses
            );
            if (!result.IsValid)
            {
                errors.AddRange(result.Errors);
            }
        }

        if (rules.HasFlag(ReviewerValidationRuleSet.Addresses))
        {
            if (caseInfo.CardOwner == CardOwner.正卡_附卡 && (context.MainAddress is null || context.SupplementaryAddress is null))
            {
                throw new ArgumentException("正卡_附卡 時，MainAddress 和 SupplementaryAddress 不能為空");
            }
            else if (caseInfo.CardOwner == CardOwner.正卡 && context.MainAddress is null)
            {
                throw new ArgumentException("正卡 時，MainAddress 不能為空");
            }
            else if (caseInfo.CardOwner == CardOwner.附卡 && (context.SupplementaryAddress is null || context.MainAddress is not null))
            {
                throw new ArgumentException("附卡 時，MainAddress 和 SupplementaryAddress 不能為空");
            }

            var result = ValidateAddresses(caseInfo: caseInfo, main: context.MainAddress, supplementary: context.SupplementaryAddress);
            if (!result.IsValid)
            {
                errors.AddRange(result.Errors);
            }
        }

        if (rules.HasFlag(ReviewerValidationRuleSet.BankTrace))
        {
            if (context.MainBankTrace is null)
            {
                throw new ArgumentException("MainBankTrace 不能為空");
            }

            var result = ValidateBankTrace(
                caseInfo: caseInfo,
                main: context.MainBankTrace,
                mainData: context.MainData,
                handleStatuses: context.HandleStatuses
            );
            if (!result.IsValid)
            {
                errors.AddRange(result.Errors);
            }
        }

        if (rules.HasFlag(ReviewerValidationRuleSet.FinanceCheck))
        {
            if (caseInfo.CardOwner == CardOwner.正卡_附卡 && (context.MainFinanceCheck is null || context.SupplementaryFinanceCheck is null))
            {
                throw new ArgumentException("正卡_附卡 時，MainFinanceCheck 和 SupplementaryFinanceCheck 不能為空");
            }
            else if (caseInfo.CardOwner == CardOwner.正卡 && context.MainFinanceCheck is null)
            {
                throw new ArgumentException("正卡 時，MainFinanceCheck 不能為空");
            }
            else if (caseInfo.CardOwner == CardOwner.附卡 && (context.SupplementaryFinanceCheck is null || context.MainFinanceCheck is null))
            {
                throw new ArgumentException("附卡 時，MainFinanceCheck 和 SupplementaryFinanceCheck 不能為空");
            }

            var result = ValidateFinanceChecks(
                caseInfo: caseInfo,
                main: context.MainFinanceCheck,
                supplementary: context.SupplementaryFinanceCheck,
                handleStatuses: context.HandleStatuses
            );
            if (!result.IsValid)
            {
                errors.AddRange(result.Errors);
            }
        }

        return errors.Count == 0 ? ReviewerValidationResult.Success() : ReviewerValidationResult.Fail(errors);
    }

    public ReviewerValidationResult ValidateDataFormat(
        CaseInfoContext caseInfo,
        ReviewerMainDataContext? main,
        ReviewerSupplementaryDataContext? supplementary,
        IReadOnlyCollection<HandleInfoContext> handleStatuses
    )
    {
        var errors = new List<ReviewerValidationError>();
        main.Handles = handleStatuses.Where(s => s.UserType == UserType.正卡人).ToList();
        var (isValidMain, validationErrorsMain) = main.ValidateCompletely();

        if (!isValidMain)
        {
            errors.AddRange(
                validationErrorsMain
                    ?.Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                    .Select(e =>
                    {
                        var memberName = e.MemberNames?.FirstOrDefault();
                        return CreateValidationError(
                            type: ReviewerValidationErrorType.ValidationFailure,
                            source: ValidateSourceType.正卡人,
                            message: e.ErrorMessage!,
                            field: memberName,
                            rule: ReviewerValidationRule.DataFormat,
                            id: main.ID,
                            chName: main.CHName
                        );
                    })
                    .ToList() ?? new List<ReviewerValidationError>()
            );
        }

        if (caseInfo.CardOwner == CardOwner.正卡_附卡 || caseInfo.CardOwner == CardOwner.附卡)
        {
            supplementary.Handles = handleStatuses.Where(s => s.UserType == UserType.附卡人).ToList();
            var (isValidSupplementary, validationErrorsSupplementary) = supplementary.ValidateCompletely();

            if (!isValidSupplementary)
            {
                errors.AddRange(
                    validationErrorsSupplementary
                        ?.Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                        .Select(e =>
                        {
                            var memberName = e.MemberNames?.FirstOrDefault();
                            return CreateValidationError(
                                type: ReviewerValidationErrorType.ValidationFailure,
                                source: ValidateSourceType.附卡人,
                                message: e.ErrorMessage!,
                                field: memberName,
                                rule: ReviewerValidationRule.DataFormat,
                                id: supplementary.ID,
                                chName: supplementary.CHName
                            );
                        })
                        .ToList() ?? new List<ReviewerValidationError>()
                );
            }
        }
        return errors.Count == 0 ? ReviewerValidationResult.Success() : ReviewerValidationResult.Fail(errors);
    }

    public ReviewerValidationResult ValidateAddresses(
        CaseInfoContext caseInfo,
        ReviewerMainAddressContext? main,
        ReviewerSupplementaryAddressContext? supplementary
    )
    {
        var result = new List<ReviewerValidationError>();

        var prefixes = new List<string> { "Reg_", "Bill_", "SendCard_" };
        if (main?.IsStudent == "Y")
            prefixes.Add("ParentLive_");

        var (isErrorMain, errorsMain) = EvaluateRequiredAddress(
            UserType.正卡人,
            main,
            prefixes,
            main?.IsOriginalCardholder == "Y",
            (userType, prefix) => GetChineseAddressPrefix(userType, prefix)
        );

        if (isErrorMain)
        {
            result.AddRange(
                errorsMain.Select(error =>
                    CreateValidationError(
                        ReviewerValidationErrorType.ValidationFailure,
                        ValidateSourceType.正卡人,
                        error.Message,
                        error.Field,
                        rule: ReviewerValidationRule.Addresses,
                        id: main?.ID,
                        chName: main?.CHName
                    )
                )
            );
        }

        if (caseInfo.CardOwner == CardOwner.正卡_附卡 || caseInfo.CardOwner == CardOwner.附卡)
        {
            var (isErrorSupplementary, errorsSupplementary) = EvaluateRequiredAddress(
                UserType.附卡人,
                supplementary,
                prefixes,
                supplementary?.IsOriginalCardholder == "Y",
                (userType, prefix) => GetChineseAddressPrefix(userType, prefix)
            );

            if (isErrorSupplementary)
            {
                result.AddRange(
                    errorsSupplementary.Select(error =>
                        CreateValidationError(
                            ReviewerValidationErrorType.ValidationFailure,
                            ValidateSourceType.附卡人,
                            error.Message,
                            error.Field,
                            rule: ReviewerValidationRule.Addresses,
                            id: supplementary?.ID,
                            chName: supplementary?.CHName
                        )
                    )
                );
            }
        }

        return result.Count == 0 ? ReviewerValidationResult.Success() : ReviewerValidationResult.Fail(result);
    }

    public ReviewerValidationResult ValidateBankTrace(
        CaseInfoContext caseInfo,
        ReviewerMainBankTraceContext? main,
        ReviewerMainDataContext? mainData,
        IReadOnlyCollection<HandleInfoContext> handleStatuses
    )
    {
        var result = new List<ReviewerValidationError>();

        var (isErrorMain, errorsMain) = EvaluateBankTraceRequiredFields(caseInfo: caseInfo, main: main, mainData: mainData);
        if (isErrorMain)
        {
            result.AddRange(
                errorsMain.Select(error =>
                    CreateValidationError(
                        ReviewerValidationErrorType.MissingCheck,
                        ValidateSourceType.正卡人,
                        error.Message,
                        error.Field,
                        rule: ReviewerValidationRule.BankTraceRequired,
                        id: main?.ID,
                        chName: main?.CHName
                    )
                )
            );
        }

        var (isErrorReply, errorsReply) = EvaluateBankTraceReply(caseInfo: caseInfo, main: main, mainData: mainData);
        if (isErrorReply)
        {
            result.AddRange(
                errorsReply.Select(error =>
                    CreateValidationError(
                        ReviewerValidationErrorType.ValidationFailure,
                        ValidateSourceType.正卡人,
                        error.Message,
                        error.Field,
                        rule: ReviewerValidationRule.BankTraceReply,
                        id: main?.ID,
                        chName: main?.CHName
                    )
                )
            );
        }

        var blockingErrors = EvaluateBankTraceActionBlocking(main: main, handleStatuses: handleStatuses, chName: main?.CHName);
        if (blockingErrors.Count != 0)
        {
            result.AddRange(blockingErrors);
        }

        return result.Count == 0 ? ReviewerValidationResult.Success() : ReviewerValidationResult.Fail(result);
    }

    private static List<ReviewerValidationError> EvaluateBankTraceActionBlocking(
        ReviewerMainBankTraceContext main,
        IReadOnlyCollection<HandleInfoContext> handleStatuses,
        string? chName
    )
    {
        if (!handleStatuses.Any(status => BankTraceBlockingActions.Contains(status.RecentAction)))
        {
            return new List<ReviewerValidationError>();
        }

        var errors = new List<ReviewerValidationError>();

        if (main.InternalEmailSame_Flag == "Y" && main.InternalEmailSame_IsError == "Y")
        {
            errors.Add(
                CreateValidationError(
                    ReviewerValidationErrorType.ValidationFailure,
                    ValidateSourceType.正卡人,
                    "行內Email重複確認-異常，不能核卡",
                    "InternalEmailSame_IsError",
                    ReviewerValidationRule.InternalEmailSame,
                    main.ID,
                    chName
                )
            );
        }

        if (main.InternalMobileSame_Flag == "Y" && main.InternalMobileSame_IsError == "Y")
        {
            errors.Add(
                CreateValidationError(
                    ReviewerValidationErrorType.ValidationFailure,
                    ValidateSourceType.正卡人,
                    "行內手機重號確認-異常，不能核卡",
                    "InternalMobileSame_IsError",
                    ReviewerValidationRule.InternalMobileSame,
                    main.ID,
                    chName
                )
            );
        }

        return errors;
    }

    public ReviewerValidationResult ValidateFinanceChecks(
        CaseInfoContext caseInfo,
        ReviewerFinanceCheckMainContext? main,
        ReviewerFinanceCheckSupplementaryContext? supplementary,
        IReadOnlyCollection<HandleInfoContext> handleStatuses
    )
    {
        var errors = new List<ReviewerValidationError>();

        var mainStatuses = handleStatuses.Where(s => s.UserType == UserType.正卡人).ToList();
        if (main is not null)
        {
            errors.AddRange(EvaluateFinanceCheckMain(main: main, handleStatuses: mainStatuses, chName: main.CHName));
        }

        if (caseInfo.CardOwner == CardOwner.正卡_附卡 || caseInfo.CardOwner == CardOwner.附卡)
        {
            var supplementaryStatuses = handleStatuses.Where(s => s.UserType == UserType.附卡人).ToList();
            if (supplementary is not null)
            {
                errors.AddRange(
                    EvaluateFinanceCheckSupplementary(
                        supplementary: supplementary,
                        handleStatuses: supplementaryStatuses,
                        chName: supplementary.CHName
                    )
                );
            }
        }

        return errors.Count == 0 ? ReviewerValidationResult.Success() : ReviewerValidationResult.Fail(errors);
    }

    /// <summary>
    /// 檢查正卡人金融檢核資料。
    /// </summary>
    /// <param name="main">正卡人金融檢核資料。</param>
    /// <param name="handleStatuses">處理狀態清單。</param>
    /// <param name="chName">中文姓名。</param>
    /// <returns>錯誤訊息清單。</returns>
    private static IEnumerable<ReviewerValidationError> EvaluateFinanceCheckMain(
        ReviewerFinanceCheckMainContext main,
        IReadOnlyCollection<HandleInfoContext> handleStatuses,
        string? chName
    )
    {
        var errors = new List<ReviewerValidationError>();

        bool mainRequiresSameDay = RequiresSameDayCheck(handleStatuses);
        bool mainProhibitsHit = RequiresProhibitHit(handleStatuses);

        errors.AddRange(
            Evaluate929(
                checkedFlag: main.Checked929,
                queryTime: main.Q929_QueryTime,
                requiresSameDay: mainRequiresSameDay,
                prohibitsHit: mainProhibitsHit,
                source: ValidateSourceType.正卡人,
                id: main.ID,
                chName: chName
            )
        );
        errors.AddRange(EvaluateNameCheck(flag: main.NameChecked, source: ValidateSourceType.正卡人, id: main.ID, chName: chName));
        errors.AddRange(EvaluateBranchCustomer(flag: main.IsBranchCustomer, source: ValidateSourceType.正卡人, id: main.ID, chName: chName));
        errors.AddRange(
            EvaluateFocusList(
                flag: main.Focus1Check,
                queryTime: main.Focus1_QueryTime,
                requiresSameDay: mainRequiresSameDay,
                source: ValidateSourceType.正卡人,
                listName: ReviewerValidationRule.Focus1,
                prohibitsHit: mainProhibitsHit,
                treatHitAsBlocking: false,
                handleStatuses: handleStatuses,
                id: main.ID,
                chName: chName
            )
        );
        errors.AddRange(
            EvaluateFocusList(
                flag: main.Focus2Check,
                queryTime: main.Focus2_QueryTime,
                requiresSameDay: mainRequiresSameDay,
                source: ValidateSourceType.正卡人,
                listName: ReviewerValidationRule.Focus2,
                prohibitsHit: mainProhibitsHit,
                treatHitAsBlocking: true,
                handleStatuses: handleStatuses,
                id: main.ID,
                chName: chName
            )
        );

        errors.AddRange(EvaluateAMLRiskLevel(context: main, prohibitsHit: mainProhibitsHit, id: main.ID, chName: chName));

        return errors;
    }

    private static IEnumerable<ReviewerValidationError> EvaluateAMLRiskLevel(
        ReviewerFinanceCheckMainContext context,
        bool prohibitsHit,
        string id,
        string? chName
    )
    {
        var errors = new List<ReviewerValidationError>();

        if (string.IsNullOrWhiteSpace(context.AMLRiskLevel) && prohibitsHit)
        {
            errors.Add(
                CreateValidationError(
                    type: ReviewerValidationErrorType.MissingCheck,
                    source: ValidateSourceType.正卡人,
                    message: "請先執行洗防風險等級檢核",
                    field: nameof(ReviewerFinanceCheckMainContext.AMLRiskLevel),
                    rule: ReviewerValidationRule.KYC,
                    id: id,
                    chName: chName
                )
            );

            return errors;
        }

        if ((context.AMLRiskLevel == "M" || context.AMLRiskLevel == "H") && context.KYC_StrongReStatus != KYCStrongReStatus.核准 & prohibitsHit)
        {
            errors.Add(
                CreateValidationError(
                    type: ReviewerValidationErrorType.ValidationFailure,
                    source: ValidateSourceType.正卡人,
                    message: "正卡人 KYC 加強審核尚未核准",
                    field: nameof(ReviewerFinanceCheckMainContext.KYC_StrongReStatus),
                    rule: ReviewerValidationRule.KYC,
                    id: id,
                    chName: chName
                )
            );
        }

        return errors;
    }

    private static IEnumerable<ReviewerValidationError> EvaluateFinanceCheckSupplementary(
        ReviewerFinanceCheckSupplementaryContext supplementary,
        IReadOnlyCollection<HandleInfoContext> handleStatuses,
        string? chName
    )
    {
        var errors = new List<ReviewerValidationError>();

        bool requiresSameDay = RequiresSameDayCheck(handleStatuses);
        bool prohibitsHit = RequiresProhibitHit(handleStatuses);

        errors.AddRange(
            Evaluate929(
                checkedFlag: supplementary.Checked929,
                queryTime: supplementary.Q929_QueryTime,
                requiresSameDay: requiresSameDay,
                prohibitsHit: prohibitsHit,
                source: ValidateSourceType.附卡人,
                id: supplementary.ID,
                chName: chName
            )
        );
        errors.AddRange(EvaluateNameCheck(flag: supplementary.NameChecked, source: ValidateSourceType.附卡人, id: supplementary.ID, chName: chName));
        errors.AddRange(
            EvaluateFocusList(
                flag: supplementary.Focus1Check,
                queryTime: supplementary.Focus1_QueryTime,
                requiresSameDay: requiresSameDay,
                source: ValidateSourceType.附卡人,
                listName: ReviewerValidationRule.Focus1,
                handleStatuses: handleStatuses,
                id: supplementary.ID,
                chName: chName
            )
        );
        errors.AddRange(
            EvaluateFocusList(
                flag: supplementary.Focus2Check,
                queryTime: supplementary.Focus2_QueryTime,
                requiresSameDay: requiresSameDay,
                source: ValidateSourceType.附卡人,
                listName: ReviewerValidationRule.Focus2,
                handleStatuses: handleStatuses,
                prohibitsHit: prohibitsHit,
                treatHitAsBlocking: true,
                id: supplementary.ID,
                chName: chName
            )
        );

        return errors;
    }

    private static IEnumerable<ReviewerValidationError> Evaluate929(
        string? checkedFlag,
        DateTime? queryTime,
        bool requiresSameDay,
        bool prohibitsHit,
        ValidateSourceType source,
        string id,
        string? chName
    )
    {
        var errors = new List<ReviewerValidationError>();

        if (string.IsNullOrWhiteSpace(checkedFlag))
        {
            errors.Add(
                CreateValidationError(
                    type: ReviewerValidationErrorType.MissingCheck,
                    source: source,
                    message: $"{source} 929 檢核尚未完成，請先執行檢核",
                    field: "Checked929",
                    rule: ReviewerValidationRule._929,
                    id: id,
                    chName: chName
                )
            );
            return errors;
        }

        if (requiresSameDay)
        {
            if (queryTime is not null && queryTime.Value.Date != DateTime.Today)
            {
                errors.Add(
                    CreateValidationError(
                        type: ReviewerValidationErrorType.MissingCheck,
                        source: source,
                        message: $"{source} 929 檢核日期需為當日",
                        field: "Q929_QueryTime",
                        rule: ReviewerValidationRule._929,
                        id: id,
                        chName: chName
                    )
                );
            }
        }

        if (prohibitsHit && string.Equals(checkedFlag, "Y", StringComparison.OrdinalIgnoreCase))
        {
            errors.Add(
                CreateValidationError(
                    type: ReviewerValidationErrorType.ValidationFailure,
                    source: source,
                    message: $"{source} 929 檢核命中，不得核卡",
                    field: "Checked929",
                    rule: ReviewerValidationRule._929,
                    id: id,
                    chName: chName
                )
            );
        }

        return errors;
    }

    private static IEnumerable<ReviewerValidationError> EvaluateNameCheck(string? flag, ValidateSourceType source, string id, string? chName)
    {
        if (string.IsNullOrWhiteSpace(flag))
        {
            return new[]
            {
                CreateValidationError(
                    ReviewerValidationErrorType.MissingCheck,
                    source,
                    $"{source} 姓名檢核尚未完成，請先執行檢核",
                    "NameChecked",
                    ReviewerValidationRule.NameCheck,
                    id,
                    chName
                ),
            };
        }

        return Array.Empty<ReviewerValidationError>();
    }

    private static IEnumerable<ReviewerValidationError> EvaluateBranchCustomer(string? flag, ValidateSourceType source, string id, string? chName)
    {
        if (string.IsNullOrWhiteSpace(flag))
        {
            return new[]
            {
                CreateValidationError(
                    ReviewerValidationErrorType.MissingCheck,
                    source,
                    $"{source} 分行客戶檢核尚未完成，請先執行檢核",
                    "IsBranchCustomer",
                    ReviewerValidationRule.BranchCustomer,
                    id,
                    chName
                ),
            };
        }

        return Array.Empty<ReviewerValidationError>();
    }

    private static IEnumerable<ReviewerValidationError> EvaluateFocusList(
        string? flag,
        DateTime? queryTime,
        bool requiresSameDay,
        ValidateSourceType source,
        string listName,
        IReadOnlyCollection<HandleInfoContext> handleStatuses,
        bool prohibitsHit = false,
        bool treatHitAsBlocking = false,
        string? id = null,
        string? chName = null
    )
    {
        var errors = new List<ReviewerValidationError>();

        var field = listName switch
        {
            "關注名單1" => "Focus1Check",
            "關注名單2" => "Focus2Check",
            _ => "FocusCheck",
        };

        var dateField = listName switch
        {
            "關注名單1" => "Focus1_QueryTime",
            "關注名單2" => "Focus2_QueryTime",
            _ => "Focus_QueryTime",
        };

        if (string.IsNullOrWhiteSpace(flag))
        {
            errors.Add(
                CreateValidationError(
                    type: ReviewerValidationErrorType.MissingCheck,
                    source: source,
                    message: $"{source} {listName} 尚未完成檢核，請先執行檢核",
                    field: field,
                    rule: listName,
                    id: id,
                    chName: chName
                )
            );
            return errors;
        }

        if (requiresSameDay)
        {
            if (queryTime is not null && queryTime.Value.Date != DateTime.Today)
            {
                errors.Add(
                    CreateValidationError(
                        type: ReviewerValidationErrorType.MissingCheck,
                        source: source,
                        message: $"{source} {listName} 檢核日期需為當日",
                        field: dateField,
                        rule: listName,
                        id: id,
                        chName: chName
                    )
                );
            }
        }

        if (treatHitAsBlocking && prohibitsHit && string.Equals(flag, "Y", StringComparison.OrdinalIgnoreCase))
        {
            foreach (var handleStatus in handleStatuses)
            {
                if (handleStatus.IsCITSCard != "Y")
                {
                    errors.Add(
                        CreateValidationError(
                            type: ReviewerValidationErrorType.ValidationFailure,
                            source: source,
                            message: $"{source} {listName} 檢核命中，僅限國旅卡可核卡，請確認卡別 ({handleStatus.ApplyCardType})",
                            field: field,
                            rule: listName,
                            id: id,
                            chName: chName
                        )
                    );
                }
            }
        }

        return errors;
    }

    /// <summary>
    /// 需要同日檢核的動作
    /// </summary>
    /// <param name="statuses">動作狀態</param>
    /// <returns>是否需要同日檢核</returns>
    private static bool RequiresSameDayCheck(IEnumerable<HandleInfoContext> statuses) => statuses.Any(s => SameDayActions.Contains(s.RecentAction));

    /// <summary>
    /// 禁止命中後可核卡的動作
    /// </summary>
    /// <param name="statuses">動作狀態</param>
    /// <returns>是否禁止命中後可核卡</returns>
    private static bool RequiresProhibitHit(IEnumerable<HandleInfoContext> statuses) =>
        statuses.Any(s => ProhibitHitActions.Contains(s.RecentAction));

    /// <summary>
    /// 建立驗證錯誤
    /// </summary>
    /// <param name="type">錯誤類型</param>
    /// <param name="source">來源</param>
    /// <param name="message">錯誤訊息</param>
    /// <param name="field">欄位名稱</param>
    /// <param name="rule">規則名稱</param>
    /// <returns>驗證錯誤</returns>
    private static ReviewerValidationError CreateValidationError(
        ReviewerValidationErrorType type,
        ValidateSourceType source,
        string message,
        string? field = null,
        string? rule = null,
        string? id = null,
        string? chName = null
    )
    {
        return new ReviewerValidationError(type, source, message, field, rule, id, chName);
    }

    /// <summary>
    /// 需要同日檢核的動作
    /// </summary>
    private static readonly HashSet<ReviewAction> SameDayActions = new()
    {
        ReviewAction.完成月收入確認,
        ReviewAction.排入核卡,
        ReviewAction.核卡作業,
    };

    /// <summary>
    /// 核卡必填的動作
    /// </summary>
    private static readonly HashSet<ReviewAction> ProhibitHitActions = new() { ReviewAction.排入核卡, ReviewAction.核卡作業 };

    private static readonly HashSet<ReviewAction> BankTraceBlockingActions = new()
    {
        ReviewAction.核卡作業,
        ReviewAction.排入核卡,
        ReviewAction.完成月收入確認,
    };

    /// <summary>
    /// 檢核地址欄位是否必填
    /// </summary>
    /// <param name="data">要檢查的 DTO</param>
    /// <param name="addressPrefixes">地址欄位前綴，例如 ["Reg_", "Bill_"]</param>
    /// <param name="isOriginalCardholder">是否為原卡友</param>
    /// <param name="prefixNameResolver">將欄位前綴轉成中文名稱的方法</param>
    /// <returns>(是否通過驗證, 錯誤訊息清單)</returns>
    private (bool isError, List<ValidateResult> errors) EvaluateRequiredAddress(
        UserType userType,
        object data,
        IEnumerable<string> addressPrefixes,
        bool isOriginalCardholder,
        Func<UserType, string, string> prefixNameResolver
    )
    {
        // 判斷檢查的欄位清單
        var validAddressFields = isOriginalCardholder ? new[] { "ZipCode", "FullAddr" } : new[] { "ZipCode", "City", "District", "Road", "Number" };

        bool IsRequiredAddressProp(string name) => addressPrefixes.Any(prefix => validAddressFields.Any(suffix => name.Equals($"{prefix}{suffix}")));

        // 找出符合條件的屬性
        var addressProperties = data.GetType().GetProperties().Where(p => IsRequiredAddressProp(p.Name));

        var errors = new List<ValidateResult>();

        foreach (var prefix in addressPrefixes.Select(p => p.TrimEnd('_')))
        {
            var props = addressProperties.Where(p => p.Name.StartsWith(prefix + "_"));
            foreach (var prop in props)
            {
                if (!string.IsNullOrWhiteSpace(prop.GetValue(data) as string))
                {
                    continue;
                }

                if (isOriginalCardholder)
                {
                    errors.Add(
                        new ValidateResult { Field = prop.Name, Message = $"{prefixNameResolver(userType, prefix)}之郵遞區號、完整地址為必填" }
                    );
                }
                else
                {
                    errors.Add(
                        new ValidateResult
                        {
                            Field = prop.Name,
                            Message = $"{prefixNameResolver(userType, prefix)}之郵遞區號、縣市、鄉鎮市區、路、號為必填",
                        }
                    );
                }
            }
        }

        return (errors.Count != 0, errors);
    }

    /// <summary>
    /// 驗證銀行追蹤回覆是否有經過檢核
    /// </summary>
    /// <param name="caseInfo">案件資訊。</param>
    /// <param name="main">銀行追蹤資訊。</param>
    /// <param name="mainData">正卡人資料。</param>
    /// <returns>(是否通過驗證, 錯誤訊息清單)</returns>
    /// <remarks>
    /// 檢查欄位:
    /// 1. 與行內IP相同 (EqualInternalIP) => ECARD/APP
    /// 2. 網路件手機號碼相同 (SameMobile) => ECARD/APP (需檢查 Mobile 是否填寫，或帳單類型為簡訊帳單)
    /// 3. 網路件E-Mail相同 (SameEmail) => ECARD/APP (需檢查 Email 是否填寫)
    /// 4. IP相同 (SameIP) => ECARD/APP/紙本
    /// 5. 行內E-Mail相同 (InternalEmailSame) => ECARD/APP/紙本 (需檢查 Email 是否填寫，或帳單類型為電子帳單)
    /// 6. 行內手機號碼相同 (InternalMobileSame) => ECARD/APP/紙本 (需檢查 Mobile 是否填寫，或帳單類型為簡訊帳單)
    /// 7. 頻繁申請ID (ShortTimeID) => ECARD/APP
    /// </remarks>
    /// <returns>(是否通過驗證, 錯誤訊息清單)</returns>
    private (bool isError, List<ValidateResult> errors) EvaluateBankTraceRequiredFields(
        CaseInfoContext caseInfo,
        ReviewerMainBankTraceContext main,
        ReviewerMainDataContext? mainData
    )
    {
        var type = main.GetType();
        var result = new List<ValidateResult>();
        var flagTypes = type.GetProperties().Where(p => p.Name.EndsWith("_Flag")).ToList();
        var webCheckFields = new List<string>
        {
            "EqualInternalIP",
            "SameMobile",
            "SameEmail",
            "SameIP",
            "InternalEmailSame",
            "InternalMobileSame",
            "ShortTimeID",
        };
        var paperCheckFields = new List<string> { "InternalEmailSame", "InternalMobileSame", "ShortTimeID" };

        foreach (var flagProp in flagTypes)
        {
            var flagName = flagProp.Name.Replace("_Flag", "");

            if (caseInfo.Source == Source.ECARD || caseInfo.Source == Source.APP)
            {
                if (!webCheckFields.Contains(flagName))
                {
                    continue;
                }
            }
            else if (caseInfo.Source == Source.紙本)
            {
                if (!paperCheckFields.Contains(flagName))
                {
                    continue;
                }
            }

            // 特殊規則: SameEmail (網路件 E-Mail 相同)
            if (flagName == "SameEmail")
            {
                // 如果 Email 未填寫且帳單類型不是電子帳單，不需要檢核
                // 如果帳單類型為電子帳單，Email 為必填，因此必須檢核
                if (string.IsNullOrWhiteSpace(mainData?.Email) && caseInfo.M_BillType != BillType.電子帳單)
                {
                    continue;
                }
            }

            // 特殊規則: SameMobile (網路件手機號碼相同)
            if (flagName == "SameMobile")
            {
                // 如果 Mobile 未填寫且帳單類型不是簡訊帳單，不需要檢核
                if (string.IsNullOrWhiteSpace(mainData?.Mobile) && caseInfo.M_BillType != BillType.簡訊帳單)
                {
                    continue;
                }
            }

            // 特殊規則: InternalEmailSame (行內 E-Mail 相同)
            if (flagName == "InternalEmailSame")
            {
                // 如果 Email 未填寫且帳單類型不是電子帳單，不需要檢核
                if (string.IsNullOrWhiteSpace(mainData?.Email) && caseInfo.M_BillType != BillType.電子帳單)
                {
                    continue;
                }

                if (caseInfo.M_BillType != BillType.電子帳單)
                {
                    continue;
                }
            }

            // 特殊規則: InternalMobileSame (行內手機號碼相同)
            if (flagName == "InternalMobileSame")
            {
                // 如果 Mobile 未填寫且帳單類型不是簡訊帳單，不需要檢核
                if (string.IsNullOrWhiteSpace(mainData?.Mobile) && caseInfo.M_BillType != BillType.簡訊帳單)
                {
                    continue;
                }

                if (caseInfo.M_BillType != BillType.簡訊帳單)
                {
                    continue;
                }
            }

            // 檢查 Flag 欄位
            var isErrorProp = type.GetProperty(flagName + "_Flag");
            var isErrorValue = isErrorProp?.GetValue(main)?.ToString();
            if (string.IsNullOrEmpty(isErrorValue))
            {
                result.Add(
                    new ValidateResult { Field = flagName + "_Flag", Message = $"【{GetHitBankTraceChineseName(flagName)}】，未檢核，請案重新查詢。" }
                );
            }
        }

        return (result.Count != 0, result);
    }

    /// <summary>
    /// 檢查命中銀行追蹤項目需填寫確認紀錄和異常判斷
    /// </summary>
    /// <param name="caseInfo">案件資訊。</param>
    /// <param name="main">銀行追蹤資訊。</param>
    /// <param name="mainData">正卡人資料。</param>
    /// <returns>(是否通過驗證, 錯誤訊息清單)</returns>
    /// <remarks>
    /// 檢查規則:
    /// 1. 命中銀行追蹤項目 = Y 時，需檢查填寫:
    ///    - 是否異常 (IsError)
    ///    - 確認紀錄 (CheckRecord)
    /// 2. 重要規則：只要檢核命中（flag = "Y"），就必須檢查回覆欄位，不管原本是否需要檢核。
    ///    即使 Email/Mobile 未填寫且帳單類型不符合，只要 flag 為 "Y"，就必須確認是否異常及撰寫紀錄。
    ///
    /// 檢查欄位:
    /// 1. 與行內IP相同 (EqualInternalIP)
    /// 2. 網路件手機號碼相同 (SameMobile)
    /// 3. 網路件E-Mail相同 (SameEmail)
    /// 4. IP相同 (SameIP)
    /// 5. 行內E-Mail相同 (InternalEmailSame)
    /// 6. 行內手機號碼相同 (InternalMobileSame)
    /// 7. 頻繁申請ID (ShortTimeID)
    /// </remarks>
    /// <returns>(是否通過驗證, 錯誤訊息清單)</returns>
    private (bool isError, List<ValidateResult> errors) EvaluateBankTraceReply(
        CaseInfoContext caseInfo,
        ReviewerMainBankTraceContext main,
        ReviewerMainDataContext? mainData
    )
    {
        var type = main.GetType();
        var result = new List<ValidateResult>();
        var flagTypes = type.GetProperties().Where(p => p.Name.EndsWith("_Flag")).ToList();

        // 取得所有命中的Flag欄位 (Flag = "Y") 並驗證是否回復
        var hitBankTraceList = flagTypes.Where(p => p.GetValue(main)?.ToString() == "Y").ToList();

        foreach (var flagProp in hitBankTraceList)
        {
            var flagName = flagProp.Name.Replace("_Flag", "");

            // 注意：只要檢核命中（flag = "Y"），就必須檢查回覆欄位
            // 不管原本是否需要檢核（即使 Email/Mobile 未填寫且帳單類型不符合）

            // 檢查 IsError 欄位
            var isErrorProp = type.GetProperty(flagName + "_IsError");
            var isErrorValue = isErrorProp?.GetValue(main)?.ToString();
            if (string.IsNullOrEmpty(isErrorValue))
            {
                result.Add(
                    new ValidateResult
                    {
                        Field = flagName + "_IsError",
                        Message = $"命中【{GetHitBankTraceChineseName(flagName)}】，請填寫是否異常。",
                    }
                );
            }

            // 檢查 CheckRecord 欄位
            var checkRecordProp = type.GetProperty(flagName + "_CheckRecord");
            var checkRecordValue = checkRecordProp?.GetValue(main)?.ToString();
            if (string.IsNullOrEmpty(checkRecordValue))
            {
                result.Add(
                    new ValidateResult
                    {
                        Field = flagName + "_CheckRecord",
                        Message = $"命中【{GetHitBankTraceChineseName(flagName)}】，請填寫確認紀錄。",
                    }
                );
            }
        }

        return (result.Count != 0, result);
    }

    private string GetHitBankTraceChineseName(string name) =>
        name switch
        {
            "EqualInternalIP" => "與行內 IP 相同",
            "SameIP" => "IP 相同",
            "SameEmail" => "網路件 E-Mail 相同",
            "SameMobile" => "網路件手機號碼相同",
            "ShortTimeID" => "短期間頻繁申請 ID",
            "InternalEmailSame" => "行內 E-Mail 相同",
            "InternalMobileSame" => "行內手機號碼相同",
            _ => name, // 沒對應到時直接回傳原字串
        };

    private string GetChineseAddressPrefix(UserType userType, string prefix) =>
        (userType, prefix) switch
        {
            (UserType.正卡人, "Reg") => "正卡人戶籍地址",
            (UserType.正卡人, "Bill") => "正卡人帳單地址",
            (UserType.正卡人, "Comp") => "正卡人公司地址",
            (UserType.正卡人, "SendCard") => "正卡人寄卡地址",
            (UserType.正卡人, "ParentLive") => "正卡人家長居住地址",
            (UserType.附卡人, "SendCard") => "附卡人寄卡地址",
            _ => throw new ArgumentException($"Invalid user type: {userType} or prefix: {prefix}"),
        };
}
