namespace ScoreSharp.Middleware.Common.Helpers.FTP;

public interface IFTPHelper
{
    Task<GetMultipleFilesBytesAsyncResult> GetMultipleFilesBytesAsync(
        string[] fileNames,
        string filePath,
        CancellationToken cancellationToken = default
    );
}
