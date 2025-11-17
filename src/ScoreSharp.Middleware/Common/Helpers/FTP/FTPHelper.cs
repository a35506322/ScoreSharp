using FluentFTP.Logging;

namespace ScoreSharp.Middleware.Common.Helpers.FTP;

public class FTPHelper : IFTPHelper
{
    private readonly FTPOption _ftpOption;
    private readonly ILogger<FTPHelper> _logger;

    public FTPHelper(IOptions<FTPOption> ftpOption, ILogger<FTPHelper> logger)
    {
        _ftpOption = ftpOption.Value;
        _logger = logger;
    }

    private AsyncFtpClient CreateFtpClient()
    {
        // 建立 FTP 連線
        FtpConfig config = new FtpConfig()
        {
            EncryptionMode = _ftpOption.UseSsl ? FtpEncryptionMode.Explicit : FtpEncryptionMode.None,
            ValidateAnyCertificate = false,
            RetryAttempts = 3,
            ConnectTimeout = _ftpOption.ConnectTimeout,
            DataConnectionConnectTimeout = _ftpOption.DataConnectionTimeout,
            // LogToConsole = true,
        };

        return new AsyncFtpClient(
            _ftpOption.Host,
            _ftpOption.Username,
            _ftpOption.Mima,
            _ftpOption.Port,
            config,
            new FtpLogAdapter(_logger)
        );
    }

    public async Task<GetMultipleFilesBytesAsyncResult> GetMultipleFilesBytesAsync(
        string[] fileNames,
        string filePath,
        CancellationToken cancellationToken = default
    )
    {
        using var activity = Log.Logger.StartActivity("FTPHelper_GetMultipleFilesBytesAsync");
        if (fileNames == null || fileNames.Length == 0)
        {
            throw new ArgumentException("檔案數量不能為零");
        }

        var results = new GetMultipleFilesBytesAsyncResult();

        using var client = CreateFtpClient();
        try
        {
            await client.Connect();
            _logger.LogInformation("已連接到 FTP 伺服器 {Host}:{Port}", _ftpOption.Host, _ftpOption.Port);

            foreach (var fileName in fileNames)
            {
                var remoteFilePath = Path.Combine(filePath, fileName);
                try
                {
                    _logger.LogInformation("開始下載檔案: {RemoteFilePath}", remoteFilePath);

                    var fileBytes = await client.DownloadBytes(remoteFilePath, token: cancellationToken);

                    results.Results.Add(
                        new GetMultipleFilesBytesAsyncItemResult()
                        {
                            FilePath = remoteFilePath,
                            FileBytes = fileBytes,
                            FileName = fileName,
                        }
                    );

                    _logger.LogInformation("下載完成: {RemoteFilePath}", remoteFilePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "FTP 操作過程中發生錯誤");
                    results.Results.Add(
                        new GetMultipleFilesBytesAsyncItemResult()
                        {
                            FilePath = remoteFilePath,
                            ErrorMessage = ex.ToString(),
                            FileName = fileName,
                        }
                    );
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FTP 操作過程中發生錯誤");
            results.ErrorMessage = ex.ToString();
        }
        finally
        {
            // 確保斷開連接
            if (client.IsConnected)
            {
                await client.Disconnect();
                _logger.LogInformation("已斷開 FTP 連接");
            }
        }
        return results;
    }
}
