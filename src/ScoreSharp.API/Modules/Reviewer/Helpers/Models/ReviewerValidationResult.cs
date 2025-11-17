namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

/// <summary>
/// 驗證結果資訊。
/// </summary>
public sealed class ReviewerValidationResult
{
    public ReviewerValidationResult() { }

    public ReviewerValidationResult(bool isValid, IReadOnlyCollection<ReviewerValidationError> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    /// <summary>
    /// 是否通過所有驗證。
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// 驗證錯誤清單。
    /// </summary>
    public IReadOnlyCollection<ReviewerValidationError> Errors { get; }

    public static ReviewerValidationResult Success() => new(true, Array.Empty<ReviewerValidationError>());

    public static ReviewerValidationResult Fail(IEnumerable<ReviewerValidationError> errors)
    {
        var materialized = errors?.ToArray() ?? Array.Empty<ReviewerValidationError>();
        return new ReviewerValidationResult(materialized.Length == 0, materialized);
    }
}

/// <summary>
/// 驗證錯誤詳細資訊。
/// </summary>
public sealed class ReviewerValidationError
{
    public ReviewerValidationError(
        ReviewerValidationErrorType type,
        ValidateSourceType source,
        string message,
        string? field = null,
        string? rule = null,
        string? id = null,
        string? chName = null
    )
    {
        Type = type;
        Source = source;
        Message = message;
        Field = field;
        Rule = rule;
        Id = id;
        CHName = chName;
    }

    /// <summary>
    /// 錯誤類型。
    /// </summary>
    public ReviewerValidationErrorType Type { get; }

    /// <summary>
    /// 錯誤類型名稱。
    /// </summary>
    public string TypeName => Type.ToDescription();

    /// <summary>
    /// 錯誤來源（正卡人／附卡人等）。
    /// </summary>
    public ValidateSourceType Source { get; }

    public string SourceName => Source.ToString();

    /// <summary>
    /// 錯誤訊息。
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// 對應欄位名稱。
    /// </summary>
    public string? Field { get; }

    /// <summary>
    /// 所屬驗證規則名稱。
    /// </summary>
    public string? Rule { get; }

    /// <summary>
    /// 所屬身分證字號。
    /// </summary>
    public string? Id { get; }

    /// <summary>
    /// 中文姓名。
    /// </summary>
    public string? CHName { get; }
}

/// <summary>
/// 驗證錯誤類型。
/// </summary>
public enum ReviewerValidationErrorType
{
    /// <summary>
    /// 驗證失敗（資料不符合規則）。
    /// </summary>
    [Description("驗證失敗_資料不符合規則")]
    ValidationFailure = 0,

    /// <summary>
    /// 檢核尚未執行或資料缺失。
    /// </summary>
    [Description("檢核尚未執行或資料缺失")]
    MissingCheck = 1,
}
