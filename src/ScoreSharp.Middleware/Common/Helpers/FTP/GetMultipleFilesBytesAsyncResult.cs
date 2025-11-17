namespace ScoreSharp.Middleware.Common.Helpers.FTP;

public class GetMultipleFilesBytesAsyncResult
{
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    public string ErrorMessage { get; set; } = string.Empty;
    public List<GetMultipleFilesBytesAsyncItemResult> Results { get; set; } = new();
}

public class GetMultipleFilesBytesAsyncItemResult
{
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    public string ErrorMessage { get; set; } = string.Empty;
}
