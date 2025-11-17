namespace ScoreSharp.Batch.Jobs.RetryWebCaseFileError.Model;

public class HandleFileResult
{
    public (List<Reviewer_ApplyFile> reviewerApplyFiles, List<Reviewer_ApplyCreditCardInfoFile> applyCreditCardInfoFiles) Files { get; set; }
    public Reviewer_ApplyCreditCardInfoProcess Process { get; set; }
    public System_ErrorLog? ErrorLog { get; set; }
    public string? ErrorType { get; set; } = null;
    public bool IsSuccess => ErrorLog == null;
}
