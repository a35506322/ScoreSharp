namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

public class ErrorNotice
{
    public string ApplyNo { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ErrorTitle { get; set; } = string.Empty;
    public EcardNewCaseRequest Request { get; set; } = new();
    public string ErrorDetail { get; set; } = string.Empty;
}
