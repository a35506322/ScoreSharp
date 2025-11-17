using Microsoft.Data.SqlClient;
using ScoreSharp.Batch.Infrastructures.FileData;
using ScoreSharp.Batch.Jobs.RetryWebCaseFileError.Model;
using ScoreSharp.Watermark;

namespace ScoreSharp.Batch.Jobs.RetryWebCaseFileError;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("網路件_申請書檔案異常重新抓取")]
public class RetryWebCaseFileErrorJob(
    ScoreSharpContext _scoreSharpContext,
    IScoreSharpDapperContext _dapperContext,
    ScoreSharpFileContext _scoreSharpFileContext,
    ILogger<RetryWebCaseFileErrorJob> _logger,
    IConfiguration _configuration,
    IWatermarkHelper _watermarkHelper
)
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("網路件_申請書檔案異常 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await _semaphore.WaitAsync(0))
        {
            _logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            var systemBatchSet = await _scoreSharpContext.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
            if (systemBatchSet!.RetryWebCaseFileErrorJob_IsEnabled == "N")
            {
                _logger.LogInformation("系統參數設定不執行【網路件_申請書檔案異常重新抓取】排程，執行結束");
                return;
            }

            var 執行案件數量 = systemBatchSet.RetryWebCaseFileErrorJob_BatchSize;
            var 異常案件清單 = await 取得申請書檔案異常案件(執行案件數量);

            if (異常案件清單.Count == 0)
            {
                _logger.LogInformation("無申請書檔案異常案件，執行結束");
                return;
            }
            else
            {
                _logger.LogInformation("取得申請書檔案異常案件數量：{Count}", 異常案件清單.Count);
            }

            foreach (var 案件內容 in 異常案件清單)
            {
                _logger.LogInformation("重新取得申請書異常案件，執行開始 申請書編號 {ApplyNo}", 案件內容.ApplyNo);

                _logger.LogInformation(
                    "申請書資訊－是否為國旅卡：{@IsCITSCard}，是否為原卡友：{@IsOriginalCardholder}",
                    案件內容.IsCITSCard,
                    案件內容.IDType is IDType.卡友
                );

                var (handle, main) = await 取得案件相關資料(案件內容.ApplyNo);

                var 檔案處理結果 = await 處理申請書檔案異常案件(案件內容);

                await 執行分散式交易(檔案處理結果, 案件內容, main, handle);

                _logger.LogInformation("執行結果：{@isSuccess}", 檔案處理結果.IsSuccess ? "成功" : "失敗");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "網路件_申請書檔案異常重新抓取排程執行發生例外");
            throw;
        }
        finally
        {
            _logger.LogInformation("執行結束");
            _semaphore.Release();
        }
    }

    private async Task<List<ApplyFileErrorContext>> 取得申請書檔案異常案件(int limit = 100)
    {
        string sql = """

            SELECT TOP (@Limit)
                M.ApplyNo,
                M.ApplyDate,
                H.CardStatus,
                M.CardAppId,
                M.IDType,
                C.IsCITSCard,
                M.MyDataCaseNo
            FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] M
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] H ON M.ApplyNo = H.ApplyNo AND M.ID = H.ID
                JOIN [ScoreSharp].[dbo].[SetUp_Card] C ON H.ApplyCardType = C.CardCode
            WHERE H.CardStatus IN @CardStatus

            """;

        using var connection = _dapperContext.CreateScoreSharpConnection();
        var result = await connection.QueryAsync<ApplyFileErrorContext>(
            sql,
            new
            {
                Limit = limit,
                CardStatus = new[]
                {
                    CardStatus.網路件_非卡友_申請書異常,
                    CardStatus.網路件_卡友_申請書異常,
                    CardStatus.網路件_卡友_檔案連線異常,
                    CardStatus.網路件_非卡友_檔案連線異常,
                    CardStatus.網路件_卡友_查無申請書_附件,
                    CardStatus.網路件_非卡友_查無申請書_附件,
                },
            }
        );

        return result.ToList();
    }

    private async Task<(Reviewer_ApplyCreditCardInfoHandle handle, Reviewer_ApplyCreditCardInfoMain main)> 取得案件相關資料(string applyNo)
    {
        var handle = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

        var main = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

        return (handle, main);
    }

    private async Task<HandleFileResult> 處理申請書檔案異常案件(ApplyFileErrorContext 案件內容)
    {
        var 檔案取得結果 = await 從EcardFileDB取得申請書檔案(案件內容);

        // 檢查檔案取得結果
        var 檔案驗證結果 = 驗證檔案取得結果(檔案取得結果, 案件內容);
        if (檔案驗證結果 != null)
        {
            _logger.LogError("處理申請書檔案異常案件發生錯誤－{@errorMessage}", 檔案驗證結果.ErrorType);
            return 檔案驗證結果;
        }

        // 處理申請書檔案
        var 申請書處理結果 = 處理申請書PDF檔案(檔案取得結果, 案件內容);
        if (申請書處理結果 != null)
        {
            _logger.LogError("處理申請書檔案異常案件發生錯誤－{@errorMessage}", 申請書處理結果.ErrorType);
            return 申請書處理結果;
        }

        // 處理附件檔案（非卡友才需要）
        var 檔案處理結果 = new ProcessApplyFileResult();
        檔案處理結果.ApplyFiles.Add("uploadPDF", 檔案取得結果.ApplyFile.UploadPDF);

        if (案件內容.IDType != IDType.卡友 || 案件內容.IsCITSCard == "Y")
        {
            var 附件處理結果 = 處理附件檔案並壓印浮水印(檔案處理結果, 檔案取得結果, 案件內容);
            if (附件處理結果 != null)
            {
                _logger.LogError("處理申請書檔案異常案件發生錯誤－{@errorMessage}", 申請書處理結果.ErrorType);
                return 附件處理結果;
            }
        }

        return 建立成功處理結果(檔案處理結果, 案件內容);
    }

    private async Task<GetApplyFileResult> 從EcardFileDB取得申請書檔案(ApplyFileErrorContext applicationError)
    {
        var applyFileResult = new GetApplyFileResult();
        applyFileResult.ApplyNo = applicationError.ApplyNo;

        try
        {
            var sql = """

                SELECT [idPic1],
                        [idPic2],
                        [upload1],
                        [upload2],
                        [upload3],
                        [upload4],
                        [upload5],
                        [upload6],
                        [uploadPDF]
                FROM [eCard_file].[dbo].[ApplyFile]
                WHERE [cCard_AppId] = @CardAppId

                """;

            using var connection = _dapperContext.CreateECardFileConnection();
            applyFileResult.ApplyFile = await connection.QueryFirstOrDefaultAsync<ApplyFile>(sql, new { applicationError.CardAppId });
        }
        catch (Exception ex)
        {
            applyFileResult.IsSuccess = false;
            applyFileResult.ErrorMessage = ex.ToString();
        }

        return applyFileResult;
    }

    private Reviewer_ApplyCreditCardInfoProcess 建立處理歷程記錄(string applyNo, DateTime dateTime, string note) =>
        new()
        {
            ApplyNo = applyNo,
            Process = ProcessConst.申請書檔案異常重新抓取,
            StartTime = dateTime,
            EndTime = dateTime,
            Notes = note,
            ProcessUserId = UserIdConst.SYSTEM,
        };

    private System_ErrorLog 建立錯誤紀錄(string applyNo, string errorType, DateTime dateTime, string errorMessage) =>
        new()
        {
            ApplyNo = applyNo,
            Project = SystemErrorLogProjectConst.BATCH,
            Source = "RetryWebCaseFileErrorJob",
            Type = errorType,
            ErrorMessage = errorMessage,
            AddTime = DateTime.Now,
            SendStatus = SendStatus.等待,
        };

    public void 壓印附件浮水印(ProcessApplyFileResult 處理結果, GetApplyFileResult 檔案結果)
    {
        string watermarkText = _configuration.GetValue<string>("WatermarkText") ?? "聯邦銀行股份授權";

        var 附件 = new Dictionary<string, byte[]>
        {
            { "idPic1", 檔案結果.ApplyFile.IdPic1 },
            { "idPic2", 檔案結果.ApplyFile.IdPic2 },
            { "upload1", 檔案結果.ApplyFile.Upload1 },
            { "upload2", 檔案結果.ApplyFile.Upload2 },
            { "upload3", 檔案結果.ApplyFile.Upload3 },
            { "upload4", 檔案結果.ApplyFile.Upload4 },
            { "upload5", 檔案結果.ApplyFile.Upload5 },
            { "upload6", 檔案結果.ApplyFile.Upload6 },
        };

        if (附件.Values.All(x => x == null))
        {
            處理結果.AppendixIsException = true;
            處理結果.AppendixErrorMessage = "所有附件皆無資料";
            return;
        }

        Dictionary<string, string> 錯誤檔案清單 = new();
        foreach (var image in 附件)
        {
            try
            {
                if (image.Value != null)
                {
                    var 浮水印處理後圖片 = _watermarkHelper.ImageWatermarkAndGetBytes(watermarkText, ".jpg", image.Value);
                    處理結果.ApplyFiles.Add(image.Key, 浮水印處理後圖片);
                }
            }
            catch (Exception ex)
            {
                錯誤檔案清單.Add(image.Key, ex.ToString());
            }
        }

        if (錯誤檔案清單.Any())
        {
            處理結果.AppendixIsException = true;
            處理結果.AppendixErrorMessage = string.Join("; ", 錯誤檔案清單.Select(x => $"{x.Key}: {x.Value}"));
        }
    }

    public (List<Reviewer_ApplyFile>, List<Reviewer_ApplyCreditCardInfoFile>) 轉換為申請檔案與信用卡資訊檔案(
        ProcessApplyFileResult processApplyFileResult,
        ApplyFileErrorContext fileErrorCaseContext
    )
    {
        var reviewerApplyFiles = new List<Reviewer_ApplyFile>();
        var reviewerApplyCreditCardInfoFiles = new List<Reviewer_ApplyCreditCardInfoFile>();
        var index = 2;
        foreach (var file in processApplyFileResult.ApplyFiles)
        {
            var fileId = Guid.NewGuid();
            string contentType = file.Key == "uploadPDF" ? "pdf" : "jpg";
            reviewerApplyFiles.Add(
                new Reviewer_ApplyFile
                {
                    ApplyNo = fileErrorCaseContext.ApplyNo,
                    FileId = fileId,
                    FileName = $"{fileErrorCaseContext.ApplyNo}_{file.Key}_{fileId}.{contentType}",
                    FileContent = file.Value,
                    FileType = FileType.申請書相關,
                }
            );

            reviewerApplyCreditCardInfoFiles.Add(
                new Reviewer_ApplyCreditCardInfoFile
                {
                    ApplyNo = fileErrorCaseContext.ApplyNo,
                    AddTime = fileErrorCaseContext.ApplyDate,
                    FileId = fileId,
                    Page = file.Key == "uploadPDF" ? 1 : index++,
                    Process = ProcessConst.申請書檔案異常重新抓取,
                    AddUserId = UserIdConst.SYSTEM,
                    IsHistory = "N",
                    DBName = "ScoreSharp_File",
                }
            );
        }
        return (reviewerApplyFiles, reviewerApplyCreditCardInfoFiles);
    }

    private async Task 執行分散式交易(
        HandleFileResult fileResult,
        ApplyFileErrorContext caseContext,
        Reviewer_ApplyCreditCardInfoMain main,
        Reviewer_ApplyCreditCardInfoHandle handle
    )
    {
        using var fileTransaction = await _scoreSharpFileContext.Database.BeginTransactionAsync();
        using var mainTransaction = await _scoreSharpContext.Database.BeginTransactionAsync();
        try
        {
            // Phase 1: 準備階段 - 檔案 Context
            await 執行檔案資料庫內容(fileResult.Files.reviewerApplyFiles, caseContext.ApplyNo);

            // Phase 1: 準備階段 - 主要 Context
            await 執行主要資料庫內容(fileResult.Files.applyCreditCardInfoFiles, fileResult.Process, fileResult.ErrorLog, caseContext.ApplyNo);

            if (fileResult.IsSuccess)
            {
                handle.CardStatus = 轉換卡片狀態(caseContext);
                _logger.LogInformation("狀態變更為：{@CardStatus}", handle.CardStatus.ToString());
            }
            else if (!fileResult.IsSuccess && fileResult.ErrorType == SystemErrorLogTypeConst.附件異常)
            {
                handle.CardStatus = 轉換卡片狀態(caseContext);
                main.ECard_AppendixIsException = "Y";
                _logger.LogInformation("狀態變更為：{@CardStatus}", handle.CardStatus.ToString());
            }

            // Phase 2: 提交階段
            await _scoreSharpFileContext.SaveChangesAsync();
            await _scoreSharpContext.SaveChangesAsync();
            await fileTransaction.CommitAsync();
            await mainTransaction.CommitAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2628)
        {
            try
            {
                _logger.LogError("申請書編號: {@ApplyNo}，資料異常資料長度過長，開始回滾", caseContext.ApplyNo);
                await mainTransaction.RollbackAsync();
                await fileTransaction.RollbackAsync();
                _logger.LogError("申請書編號: {@ApplyNo}，資料異常資料長度過長，回滾成功", caseContext.ApplyNo);
            }
            catch (Exception rollbackEx)
            {
                _logger.LogError("申請書編號: {@ApplyNo}，資料異常資料長度過長，回滾失敗，{@Error}", caseContext.ApplyNo, rollbackEx.ToString());
            }

            // SQL Server 錯誤代碼 2628 表示資料截斷錯誤
            _logger.LogError("申請書編號: {@ApplyNo} 資料異常資料長度過長: {@Error}", caseContext.ApplyNo, ex.ToString());

            await 發送錯誤通知_後續不在執行(建立錯誤紀錄(caseContext.ApplyNo, SystemErrorLogTypeConst.內部程式錯誤, DateTime.Now, ex.ToString()));
        }
        catch (Exception ex)
        {
            try
            {
                _logger.LogError("申請書編號: {@ApplyNo} 分散式交易失敗，開始回滾: {@Error}", caseContext.ApplyNo, ex.ToString());

                await mainTransaction.RollbackAsync();
                await fileTransaction.RollbackAsync();

                _logger.LogError("申請書編號: {@ApplyNo} 回滾成功", caseContext.ApplyNo);
            }
            catch (Exception rollbackEx)
            {
                _logger.LogError("申請書編號: {@ApplyNo} 回滾失敗，{@Error}", caseContext.ApplyNo, rollbackEx.ToString());
            }

            throw;
        }
        finally
        {
            await fileTransaction.DisposeAsync();
            await mainTransaction.DisposeAsync();
        }
    }

    private async Task 執行檔案資料庫內容(List<Reviewer_ApplyFile> reviewerApplyFiles, string applyNo)
    {
        var existingApplyFiles = await _scoreSharpFileContext
            .Reviewer_ApplyFile.Where(x => x.ApplyNo == applyNo && x.FileType == FileType.申請書相關)
            .ExecuteDeleteAsync();

        _logger.LogInformation("刪除申請書編號 {@ApplyNo} 的現有申請檔案數量：{Count}", applyNo, existingApplyFiles);

        await _scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(reviewerApplyFiles);
    }

    private async Task 執行主要資料庫內容(
        List<Reviewer_ApplyCreditCardInfoFile> reviewerApplyCreditCardInfoFiles,
        Reviewer_ApplyCreditCardInfoProcess process,
        System_ErrorLog systemLog,
        string applyNo
    )
    {
        var existingInfoFiles = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.Where(x => x.ApplyNo == applyNo).ExecuteDeleteAsync();
        _logger.LogInformation("刪除申請書編號 {@ApplyNo} 的現有信用卡資訊檔案數量：{Count}", applyNo, existingInfoFiles);

        await _scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(reviewerApplyCreditCardInfoFiles);
        await _scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process);
        if (systemLog is not null)
            await _scoreSharpContext.System_ErrorLog.AddAsync(systemLog);
    }

    private CardStatus 轉換卡片狀態(ApplyFileErrorContext caseContext)
    {
        var isCITSCard = caseContext.IsCITSCard == "Y";
        var isOriginalCardholder = caseContext.IDType == IDType.卡友;

        if (isCITSCard)
        {
            return CardStatus.國旅人事名冊確認;
        }

        if (isOriginalCardholder)
        {
            return CardStatus.網路件_卡友_待檢核;
        }
        else
        {
            if (caseContext.IDType == IDType.存戶 || caseContext.IDType == IDType.持他行卡 || caseContext.IDType == IDType.自然人憑證)
            {
                if (string.IsNullOrEmpty(caseContext.MyDataCaseNo))
                {
                    return CardStatus.網路件_非卡友_待檢核;
                }
                else
                {
                    return CardStatus.網路件_等待MyData附件;
                }
            }
            // ! 此為小白件，主因數位峰之前談的規格問題..
            else if (caseContext.IDType == null)
            {
                if (string.IsNullOrEmpty(caseContext.MyDataCaseNo))
                {
                    return CardStatus.網路件_書面申請等待列印申請書及回郵信封;
                }
                else
                {
                    return CardStatus.網路件_書面申請等待MyData;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(caseContext.MyDataCaseNo))
                {
                    return CardStatus.網路件_非卡友_待檢核;
                }
                else
                {
                    return CardStatus.網路件_等待MyData附件;
                }
            }
        }
    }

    private async Task 發送錯誤通知_後續不在執行(System_ErrorLog errorLog)
    {
        _scoreSharpFileContext.ChangeTracker.Clear();
        _scoreSharpContext.ChangeTracker.Clear();
        // 新增錯誤紀錄
        await _scoreSharpContext.System_ErrorLog.AddAsync(errorLog);
        // 儲存資料
        await _scoreSharpContext.SaveChangesAsync();
    }

    private HandleFileResult? 驗證檔案取得結果(GetApplyFileResult 檔案取得結果, ApplyFileErrorContext 案件內容)
    {
        if (!檔案取得結果.IsSuccess)
        {
            return 建立錯誤處理結果(案件內容, SystemErrorLogTypeConst.ECARD_FILE_DB_連線錯誤, 檔案取得結果.ErrorMessage);
        }

        if (檔案取得結果.ApplyFile is null)
        {
            return 建立錯誤處理結果(案件內容, SystemErrorLogTypeConst.ECARD_FILE_DB_查無申請書附件檔案, "查無申請書檔案");
        }

        return null;
    }

    private HandleFileResult? 處理申請書PDF檔案(GetApplyFileResult 檔案取得結果, ApplyFileErrorContext 案件內容)
    {
        if (檔案取得結果.ApplyFile.UploadPDF is null)
        {
            return 建立錯誤處理結果(案件內容, SystemErrorLogTypeConst.申請書異常, "申請書異常");
        }

        return null;
    }

    private HandleFileResult? 處理附件檔案並壓印浮水印(
        ProcessApplyFileResult 檔案處理結果,
        GetApplyFileResult 檔案取得結果,
        ApplyFileErrorContext 案件內容
    )
    {
        壓印附件浮水印(檔案處理結果, 檔案取得結果);

        if (檔案處理結果.AppendixIsException)
        {
            return 建立錯誤處理結果(案件內容, SystemErrorLogTypeConst.附件異常, 檔案處理結果.AppendixErrorMessage);
        }

        return null;
    }

    private HandleFileResult 建立成功處理結果(ProcessApplyFileResult 檔案處理結果, ApplyFileErrorContext 案件內容)
    {
        var 轉換後檔案 = 轉換為申請檔案與信用卡資訊檔案(檔案處理結果, 案件內容);
        return new HandleFileResult
        {
            Files = 轉換後檔案,
            Process = 建立處理歷程記錄(案件內容.ApplyNo, DateTime.Now, "成功"),
            ErrorLog = null,
        };
    }

    private HandleFileResult 建立錯誤處理結果(ApplyFileErrorContext 案件內容, string 錯誤類型, string 錯誤訊息)
    {
        return new HandleFileResult
        {
            Files = (new List<Reviewer_ApplyFile>(), new List<Reviewer_ApplyCreditCardInfoFile>()),
            Process = 建立處理歷程記錄(案件內容.ApplyNo, DateTime.Now, "失敗"),
            ErrorLog = 建立錯誤紀錄(案件內容.ApplyNo, 錯誤類型, DateTime.Now, 錯誤訊息),
            ErrorType = 錯誤類型,
        };
    }
}
