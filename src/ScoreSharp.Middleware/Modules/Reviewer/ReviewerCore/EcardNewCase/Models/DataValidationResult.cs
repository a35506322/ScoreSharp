namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

public class DataValidationResult
{
    public bool IsValid { get; set; } = true;
    public string ErrorMessage { get; set; } = string.Empty;
}
