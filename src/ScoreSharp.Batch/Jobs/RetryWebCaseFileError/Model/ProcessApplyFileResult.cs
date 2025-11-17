namespace ScoreSharp.Batch.Jobs.RetryWebCaseFileError.Model;

public class ProcessApplyFileResult
{
    public bool ApplicationIsException { get; set; } = false;
    public string ApplicationErrorMessage { get; set; } = string.Empty;
    public bool AppendixIsException { get; set; } = false;
    public string AppendixErrorMessage { get; set; } = string.Empty;
    public Dictionary<string, byte[]> ApplyFiles { get; set; } = new();
    public string ApplyNo { get; set; }
}
