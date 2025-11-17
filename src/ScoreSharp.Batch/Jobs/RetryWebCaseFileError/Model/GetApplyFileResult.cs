namespace ScoreSharp.Batch.Jobs.RetryWebCaseFileError.Model;

public class GetApplyFileResult
{
    public bool IsSuccess { get; set; } = true;
    public ApplyFile? ApplyFile { get; set; } = null;
    public string ApplyNo { get; set; } = null!;
    public string ErrorMessage { get; set; } = string.Empty;
}

public class ApplyFile
{
    public byte[] IdPic1 { get; set; } = null;
    public byte[] IdPic2 { get; set; } = null;
    public byte[] Upload1 { get; set; } = null;
    public byte[] Upload2 { get; set; } = null;
    public byte[] Upload3 { get; set; } = null;
    public byte[] Upload4 { get; set; } = null;
    public byte[] Upload5 { get; set; } = null;
    public byte[] Upload6 { get; set; } = null;
    public byte[] UploadPDF { get; set; } = null;
}
