namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

public class GetApplyFileResult
{
    public bool IsException { get; set; } = false;
    public ApplyFile? ApplyFile { get; set; } = null;
    public string ErrorMessage { get; set; } = string.Empty;
    public string ApplyNo { get; set; }
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
