using FluentFTP;
using FluentFTP.Logging;
using ScoreSharp.Common.Adapters.EcardMiddleware;
using ScoreSharp.Common.Adapters.EcardMiddleware.Model;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Helpers.MW3Security;

namespace ScoreSharp.Batch.Jobs.CompareMissingCases;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("每2小時比對漏進案件批次")]
public class CompareMissingCasesJob(
    ScoreSharpContext _context,
    IScoreSharpDapperContext _dapperContext,
    IOptions<EcardFtpOption> _ftpOption,
    IOptions<TemplateReportOption> _templateReport,
    ILogger<CompareMissingCasesJob> _logger,
    IMW3APAPIAdapter _mw3APAPIAdapter,
    [FromKeyedServices("APAPI")] IMW3SecurityHelper _mw3SecurityHelper,
    IMiddlewareAdapter middlewareAdapter
)
{
    [DisplayName("每2小時比對漏進案件批次 - 執行人員：{0}")]
    public async Task Execute(string createBy, string processDate)
    {
        var systemBatchSet = await _context.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
        if (systemBatchSet.CompareMissingCases_IsEnabled == "N")
        {
            _logger.LogInformation("系統參數設定不執行【每2小時比對漏進案件批次】排程，執行結束");
            return;
        }

        if (string.IsNullOrEmpty(processDate))
            processDate = DateTime.Now.ToString("yyyyMMdd");

        _logger.LogInformation("執行日期：{ProcessDate}", processDate);

        var localFolderPath = Path.Combine(_ftpOption.Value.CompareMissingCasesLocalFolderPath, processDate);

        int fileCount = 取得本地資料夾檔案數量(localFolderPath);

        var localFilePath = Path.Combine(localFolderPath, $"APPLYCard_Total_{processDate}_{fileCount + 1}.txt");
        var remoteFilePath = Path.Combine(_ftpOption.Value.CompareMissingCasesRemoteFolderPath, $"APPLYCard_Total_{processDate}.txt");

        if (!await Ftp取得進件比對檔(localFilePath, remoteFilePath))
        {
            _logger.LogInformation("Ftp取得進件比對檔失敗，執行結束");
            return;
        }
        else
        {
            _logger.LogInformation("執行結束");
        }

        HashSet<string> applyNosFromLocal = await 取得本地比對檔申請書編號(localFolderPath);

        HashSet<string> applyNosFromSystem = await 查詢系統申請書編號(processDate);

        var diffApplyNos = applyNosFromLocal.Except(applyNosFromSystem).ToList();

        if (diffApplyNos.Count == 0)
        {
            _logger.LogInformation("無差異資料，執行結束");
            return;
        }

        _logger.LogInformation("未進件案件，共 {Count} 筆", diffApplyNos.Count);

        List<EcardNewCaseRequest> ecardNewCaseRequests = await 查詢MW3_eCardService取得申請資料(diffApplyNos);

        if (ecardNewCaseRequests.Count == 0)
        {
            _logger.LogInformation("eCardService 查無申請資料，執行結束");
            return;
        }

        _logger.LogInformation("eCardService 取得申請資料，共 {Count} 筆", ecardNewCaseRequests.Count);

        await 發送Ecard新件API(ecardNewCaseRequests);
    }

    private async Task 發送Ecard新件API(List<EcardNewCaseRequest> requests)
    {
        foreach (var request in requests)
        {
            _logger.LogInformation("執行發送 Ecard 新件 API，申請書編號：{ApplyNo}", request.ApplyNo);

            var result = await middlewareAdapter.PostEcardNewCaseAsync(request);

            if (result.IsSuccess)
            {
                PostEcardNewCaseResult postEcardNewCaseResult = result.Result;

                if (postEcardNewCaseResult.ID != "0000")
                {
                    _logger.LogError(
                        "Ecard 新件 API 發送內容有誤，申請書編號：{ApplyNo}，回覆代碼：{ID}，回覆結果：{RESULT}",
                        request.ApplyNo,
                        postEcardNewCaseResult.ID,
                        postEcardNewCaseResult.Result
                    );
                }
                else
                {
                    _logger.LogInformation(
                        "Ecard 新件 API 發送成功，回覆代碼：{ID}，回覆結果：{RESULT}",
                        postEcardNewCaseResult.ID,
                        postEcardNewCaseResult.Result
                    );
                }
            }
            else
            {
                _logger.LogError("Ecard 新件 API 發送失敗，申請書編號：{ApplyNo}，錯誤訊息：{ErrorMessage}", request.ApplyNo, result.ErrorMessage);
            }
        }
    }

    private async Task<List<EcardNewCaseRequest>> 查詢MW3_eCardService取得申請資料(List<string> diffApplyNos)
    {
        List<EcardNewCaseRequest> eCardNewCaseRequests = [];

        _logger.LogInformation("查詢 MW3 eCardService 取得申請資料，執行開始");

        foreach (var applyNo in diffApplyNos)
        {
            var result = await _mw3APAPIAdapter.QueryEcardNewCase(applyNo);

            if (result.IsSuccess)
            {
                var ecardNewCaseData = result.Data;

                if (ecardNewCaseData.RtnCode == MW3RtnCodeConst.查詢eCardService_作業成功)
                {
                    _logger.LogInformation(
                        "eCardService 取得申請資料成功，申請書編號：{ApplyNo}，回傳代碼：{RtnCode}，訊息：{Msg}",
                        applyNo,
                        ecardNewCaseData.RtnCode,
                        ecardNewCaseData.Msg
                    );

                    eCardNewCaseRequests.Add(ecardNewCaseData.Info);
                }
                else
                {
                    _logger.LogError(
                        "eCardService 取得申請資料失敗，申請書編號：{ApplyNo}，回傳代碼：{RtnCode}，訊息：{Msg}，TraceID：{TraceID}",
                        applyNo,
                        ecardNewCaseData.RtnCode,
                        ecardNewCaseData.Msg,
                        ecardNewCaseData.TransID
                    );
                }
            }
            else
            {
                _logger.LogError("MW3 取得申請資料失敗，申請書編號：{ApplyNo}，錯誤訊息：{ErrorMessage}", applyNo, result.ErrorMessage);
            }
        }

        _logger.LogInformation("執行結束");

        return eCardNewCaseRequests;
    }

    private async Task<HashSet<string>> 取得本地比對檔申請書編號(string localFolderPath)
    {
        var files = Directory
            .GetFiles(localFolderPath)
            .Select(path => new { Path = path, FileName = Path.GetFileName(path) })
            .Where(x => x.FileName.StartsWith("APPLYCard_Total_"))
            .OrderByDescending(x => x.FileName)
            .Take(2)
            .ToList();

        if (files.Count < 2)
        {
            // 第一次執行，直接回傳所有資料
            var datas = new HashSet<string>(await File.ReadAllLinesAsync(files[0].Path));
            var applyNos = datas.Select(x => x.Split(',')[1]).ToHashSet();

            _logger.LogInformation("首次執行，Ecard 共進件 {Count} 筆", applyNos.Count);

            return applyNos;
        }
        else
        {
            // 「ID,申請書編號」作為一個字串來比對
            var newDatas = new HashSet<string>(await File.ReadAllLinesAsync(files[0].Path));
            var previousDatas = new HashSet<string>(await File.ReadAllLinesAsync(files[1].Path));

            var diff = newDatas.Except(previousDatas).ToList();

            // 取得差異的申請書編號
            var applyNos = diff.Select(x => x.Split(',')[1]).ToHashSet();

            _logger.LogInformation("Ecard 新申請案件，共 {Count} 筆", diff.Count);

            return applyNos;
        }
    }

    private int 取得本地資料夾檔案數量(string localFolderPath)
    {
        if (!Directory.Exists(localFolderPath))
            Directory.CreateDirectory(localFolderPath);

        var files = Directory.GetFiles(localFolderPath);

        return files.Length;
    }

    private async Task<HashSet<string>> 查詢系統申請書編號(string date)
    {
        var dateTime = DateTime.ParseExact(date, "yyyyMMdd", null);
        var nextDateTime = dateTime.AddDays(1);

        var mainCaseApplyNos = await _context
            .Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
            .Where(x => dateTime <= x.ApplyDate && x.ApplyDate < nextDateTime)
            .Select(x => x.ApplyNo)
            .ToListAsync();

        _logger.LogInformation("徵審系統已進件案件，共 {Count} 筆", mainCaseApplyNos.Count);

        return mainCaseApplyNos.ToHashSet();
    }

    private async Task<bool> Ftp取得進件比對檔(string localFilePath, string remoteFilePath, CancellationToken cancellationToken = default)
    {
        bool result = false;

        FtpConfig config = new()
        {
            EncryptionMode = _ftpOption.Value.UseSsl ? FtpEncryptionMode.Explicit : FtpEncryptionMode.None,
            ValidateAnyCertificate = false,
            RetryAttempts = 3,
            ConnectTimeout = _ftpOption.Value.ConnectTimeout,
            DataConnectionConnectTimeout = _ftpOption.Value.DataConnectionTimeout,
        };

        using var client = new AsyncFtpClient(
            _ftpOption.Value.Host,
            _ftpOption.Value.Username,
            _ftpOption.Value.Mima,
            _ftpOption.Value.Port,
            config,
            new FtpLogAdapter(_logger)
        );

        _logger.LogInformation("FTP 取得進件比對檔，執行開始");

        try
        {
            await client.Connect();
            _logger.LogInformation("已連接到 FTP 伺服器 {Host}:{Port}", _ftpOption.Value.Host, _ftpOption.Value.Port);

            _logger.LogInformation("開始下載檔案：{RemoteFilePath}", remoteFilePath);

            if (!await client.FileExists(remoteFilePath))
            {
                _logger.LogError("下載失敗，檔案不存在：{RemoteFilePath}", remoteFilePath);
            }
            else
            {
                await client.DownloadFile(localFilePath, remoteFilePath, token: cancellationToken);

                _logger.LogInformation("下載完成，儲存於：{LocalFilePath}", localFilePath);

                result = true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FTP 操作過程中發生錯誤");
            throw;
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
        return result;
    }
}
