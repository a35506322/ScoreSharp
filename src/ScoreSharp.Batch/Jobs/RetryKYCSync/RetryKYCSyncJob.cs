using ScoreSharp.Batch.Jobs.RetryKYCSync.Helpers;
using ScoreSharp.Batch.Jobs.RetryKYCSync.Models;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Helpers.KYC;

namespace ScoreSharp.Batch.Jobs.RetryKYCSync;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("重試 KYC 入檔作業排程")]
public class RetryKYCSyncJob(ILogger<RetryKYCSyncJob> logger, ScoreSharpContext dbContext, IMW3APAPIAdapter mw3APAPIAdapter)
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("重試 KYC 入檔作業排程 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await _semaphore.WaitAsync(0))
        {
            logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            var systemBatchSet = await dbContext.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
            if (systemBatchSet!.RetryKYCSync_IsEnabled == "N")
            {
                logger.LogInformation("系統參數設定不執行【重試 KYC 入檔作業】排程，執行結束");
                return;
            }

            var param = await 取得排程參數();
            var isInScheduleTime = 檢查排程時間(now: DateTime.Now, kycFixStartTime: param.KYCFixStartTime, kycFixEndTime: param.KYCFixEndTime);

            if (isInScheduleTime)
            {
                logger.LogInformation("資訊部 KYC 系統維護中，結束排程");
                return;
            }

            var handlesGroupBy = await 取得需入檔案件(systemBatchSet.RetryKYCSync_BatchSize);

            if (handlesGroupBy.Count == 0)
            {
                logger.LogInformation("沒有需要重試 KYC 入檔的案件，結束排程");
                return;
            }

            foreach (var handleGroupBy in handlesGroupBy)
            {
                DateTime startTime = DateTime.Now;

                try
                {
                    var main = await 取得主卡人資料(handleGroupBy.ApplyNo);

                    foreach (var handle in handleGroupBy.Handles)
                    {
                        logger.LogInformation(
                            "開始 KYC 入檔作業，序號：{seqNo}，案件編號：{applyNo}，案件狀態：{cardStatus}",
                            handle.SeqNo,
                            handleGroupBy.ApplyNo,
                            handle.CardStatus
                        );
                    }

                    SyncKycMW3Info syncKycMW3Info;
                    string apiName = string.Empty;
                    if (main.Source != Source.紙本 && main.IsOriginalCardholder == "Y")
                    {
                        syncKycMW3Info = MapHelper.Map原卡友SyncKycMW3Info(main);
                        apiName = "KYC00CREDIT";
                    }
                    else if (main.IsOriginalCardholder == "N")
                    {
                        var nameCheckLog = await 取得姓名檢核Log(handleGroupBy.ApplyNo, main.ID);

                        if (nameCheckLog is null)
                        {
                            logger.LogInformation("案件編號：{applyNo}，未做姓名檢核，將狀態更為待月收入確認", handleGroupBy.ApplyNo);

                            var updateCardStatus = 更新案件狀態＿待月收入確認(handleGroupBy.Handles);
                            var nameCheckErrorProcess = MapHelper.MapNameCheckErrorProcess(
                                applyNo: handleGroupBy.ApplyNo,
                                process: updateCardStatus.ToString(),
                                startTime: startTime,
                                endTime: DateTime.Now
                            );
                            await 新增KYC歷程(new List<Reviewer_ApplyCreditCardInfoProcess> { nameCheckErrorProcess });

                            await dbContext.SaveChangesAsync();
                            dbContext.ChangeTracker.Clear();
                            continue;
                        }

                        syncKycMW3Info = MapHelper.Map非卡友SyncKycMW3Info(main, nameCheckLog);
                        apiName = "KYC00CREDIT";
                    }
                    else
                    {
                        throw new Exception(
                            $"案件編號：{handleGroupBy.ApplyNo}，未知的案件來源：{main.Source}，是否為原卡友：{main.IsOriginalCardholder}"
                        );
                    }

                    var mw3Result = await 呼叫KYC入檔API(syncKycMW3Info);
                    var kycResult = mw3Result.Data.Info.Result.Data;
                    var kycProcess = new List<Reviewer_ApplyCreditCardInfoProcess>();

                    logger.LogInformation(
                        "KYC 入檔作業結果，案件編號：{applyNo}，發查是否成功：{result}，KYC 代碼：{kycCode}，KYC Level：{kycRank}，失敗原因：{kycMsg}，例外錯誤：{error}",
                        handleGroupBy.ApplyNo,
                        mw3Result.IsSuccess ? "成功" : "失敗",
                        kycResult.KycCode,
                        kycResult.RaRank,
                        kycResult.ErrMsg,
                        mw3Result.IsSuccess ? string.Empty : mw3Result.ErrorMessage
                    );

                    // Tips: 如果有錯誤，會寄送此錯誤訊息 Log
                    var kycQueryLog = MapHelper.MapKYCQueryLog(
                        applyNo: handleGroupBy.ApplyNo,
                        cardStatus: string.Join("/", handleGroupBy.Handles.Select(x => x.CardStatus)),
                        id: main.ID,
                        request: syncKycMW3Info,
                        response: mw3Result,
                        kycCode: kycResult.KycCode,
                        kycRank: kycResult.RaRank,
                        kycMsg: kycResult.ErrMsg,
                        queryTime: startTime,
                        querySuccess: mw3Result.IsSuccess,
                        apiName: apiName
                    );

                    kycProcess.Add(
                        MapHelper.MapKYCProcess(
                            applyNo: handleGroupBy.ApplyNo,
                            process: ProcessConst.完成KYC入檔,
                            startTime: startTime,
                            endTime: DateTime.Now,
                            isSuccess: mw3Result.IsSuccess,
                            kycCode: kycResult.KycCode,
                            kycRank: kycResult.RaRank,
                            id: main.ID,
                            userType: main.UserType,
                            keyMsg: kycResult.ErrMsg
                        )
                    );

                    logger.LogInformation("資料庫開始寫入，案件編號：{applyNo}", handleGroupBy.ApplyNo);

                    if (mw3Result.IsSuccess)
                    {
                        var cardStatus = 更新風險等級及加強審核資訊及狀態(mw3Result.IsSuccess, main, kycResult, startTime, handleGroupBy.Handles);
                        kycProcess.Add(
                            MapHelper.MapKYCProcess(
                                applyNo: handleGroupBy.ApplyNo,
                                process: cardStatus.ToString(),
                                startTime: startTime,
                                endTime: DateTime.Now,
                                isSuccess: mw3Result.IsSuccess,
                                kycCode: kycResult.KycCode,
                                kycRank: kycResult.RaRank,
                                id: main.ID,
                                userType: main.UserType,
                                keyMsg: kycResult.ErrMsg
                            )
                        );
                    }

                    await 新增KYC歷程(kycProcess);
                    await 新增KYC查詢Log(kycQueryLog);

                    await dbContext.SaveChangesAsync();
                    dbContext.ChangeTracker.Clear();

                    logger.LogInformation("資料庫寫入完成，案件編號：{applyNo}", handleGroupBy.ApplyNo);
                }
                catch (Exception ex)
                {
                    logger.LogError("KYC 同步排程，檢核發生錯誤 {ex}", ex.ToString());

                    dbContext.ChangeTracker.Clear();

                    await 新增錯誤Log(
                        applyNo: handleGroupBy.ApplyNo,
                        type: "KYCSyncJob 檢核錯誤",
                        errorMessage: string.Empty,
                        errorDetail: ex.ToString(),
                        addTime: startTime
                    );

                    await dbContext.SaveChangesAsync();
                }
            }

            logger.LogInformation("KYC 同步排程，執行完成");
        }
        catch (Exception ex)
        {
            logger.LogError("KYC 同步排程，意外錯誤 {ex}", ex.ToString());

            dbContext.ChangeTracker.Clear();

            await 新增錯誤Log(
                applyNo: string.Empty,
                type: "KYCSyncJob 意外錯誤",
                errorMessage: string.Empty,
                errorDetail: ex.ToString(),
                addTime: DateTime.Now
            );

            await dbContext.SaveChangesAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<List<SyncHandleGroupBy>> 取得需入檔案件(int count = 100, int hour = 1)
    {
        string sql =
            @"
                SELECT  A.[SeqNo]
                    ,A.[ApplyNo]
                    ,A.[ID]
                    ,A.[UserType]
                    ,A.[CardStatus]
                FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] A
                JOIN [ScoreSharp].[dbo].[Reviewer_FinanceCheckInfo] F ON  A.ApplyNo = F.ApplyNo AND F.UserType = 1
                WHERE A.CardStatus in (30117,30118)
                AND NOT (F.KYC_RtnCode = 'KC001'
                AND F.KYC_QueryTime IS NOT NULL
                AND DATEDIFF(HOUR, F.KYC_QueryTime, GETDATE()) < 1)
            ";
        var handles = await dbContext.Database.SqlQueryRaw<GetSyncHandle>(sql, count).ToListAsync();
        var groupByApplyNo = handles.GroupBy(x => x.ApplyNo).Select(x => new SyncHandleGroupBy { ApplyNo = x.Key, Handles = x.ToList() }).ToList();
        return groupByApplyNo;
    }

    private async Task<Reviewer_ApplyCreditCardInfoMain> 取得主卡人資料(string applyNo) =>
        await dbContext.Reviewer_ApplyCreditCardInfoMain.SingleAsync(x => x.ApplyNo == applyNo);

    private async Task<Reviewer3rd_NameCheckLog> 取得姓名檢核Log(string applyNo, string id) =>
        await dbContext.Reviewer3rd_NameCheckLog.OrderByDescending(x => x.StartTime).FirstOrDefaultAsync(x => x.ApplyNo == applyNo && x.ID == id);

    private async Task<BaseMW3Response<SyncKycResponse>> 呼叫KYC入檔API(SyncKycMW3Info syncKycMW3Info) =>
        await mw3APAPIAdapter.SyncKYC(syncKycMW3Info);

    private CardStatus 更新風險等級及加強審核資訊及狀態(
        bool isMW3ApiSuccess,
        Reviewer_ApplyCreditCardInfoMain main,
        SyncKycMW3Data kycResult,
        DateTime queryTime,
        List<GetSyncHandle> handles
    )
    {
        main.AMLRiskLevel = kycResult.RaRank;
        main.LastUpdateUserId = UserIdConst.SYSTEM;
        main.LastUpdateTime = queryTime;

        Reviewer_FinanceCheckInfo mainFinanceCheck = new()
        {
            ApplyNo = main.ApplyNo,
            ID = main.ID,
            UserType = main.UserType,
        };

        dbContext.Attach(mainFinanceCheck);
        dbContext.Entry(mainFinanceCheck).Property(x => x.AMLRiskLevel).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_Message).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_RtnCode).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_QueryTime).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_Handler).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_Handler_SignTime).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_Reviewer).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_Reviewer_SignTime).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_KYCManager).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_KYCManager_SignTime).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_StrongReDetailJson).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_Suggestion).IsModified = true;
        dbContext.Entry(mainFinanceCheck).Property(x => x.KYC_StrongReStatus).IsModified = true;

        mainFinanceCheck.AMLRiskLevel = kycResult.RaRank;
        mainFinanceCheck.KYC_Message = kycResult.ErrMsg;
        mainFinanceCheck.KYC_RtnCode = kycResult.KycCode;
        mainFinanceCheck.KYC_QueryTime = queryTime;
        mainFinanceCheck.KYC_Handler = null;
        mainFinanceCheck.KYC_Handler_SignTime = null;
        mainFinanceCheck.KYC_Reviewer = null;
        mainFinanceCheck.KYC_Reviewer_SignTime = null;
        mainFinanceCheck.KYC_KYCManager = null;
        mainFinanceCheck.KYC_KYCManager_SignTime = null;
        mainFinanceCheck.KYC_StrongReDetailJson = null;
        mainFinanceCheck.KYC_Suggestion = null;

        if (isMW3ApiSuccess && kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功)
        {
            main.CurrentHandleUserId = null;

            if (main.IsOriginalCardholder == "Y")
            {
                // Tips: 如果為原卡友，則不需加強審核
                mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;
            }
            else
            {
                // Tips: KYC 風險等級為 H 或 M 時，需要加強審核執行
                if (kycResult.RaRank == KYCRiskLevelConst.高風險 || kycResult.RaRank == KYCRiskLevelConst.中風險)
                {
                    mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.未送審;
                    mainFinanceCheck.KYC_StrongReDetailJson = JsonHelper.序列化物件(
                        KYCHelper.產生KYC加強審核執行表(
                            version: mainFinanceCheck.KYC_StrongReVersion,
                            id: main.ID,
                            name: main.CHName,
                            riskLevel: kycResult.RaRank
                        )
                    );
                }
                else
                {
                    mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;
                }
            }
        }

        var updateHandles = new List<Reviewer_ApplyCreditCardInfoHandle>();
        foreach (var handle in handles)
        {
            var updateHandle = new Reviewer_ApplyCreditCardInfoHandle { SeqNo = handle.SeqNo };
            updateHandle.CardStatus = 轉換案件狀態(handle.CardStatus, kycResult);
            updateHandles.Add(updateHandle);
        }

        // 批次更新
        foreach (var handle in updateHandles)
        {
            dbContext.Attach(handle);
            dbContext.Entry(handle).Property(x => x.CardStatus).IsModified = true;
        }

        return updateHandles.First().CardStatus;
    }

    private async Task 新增KYC歷程(List<Reviewer_ApplyCreditCardInfoProcess> processes) =>
        await dbContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);

    private async Task 新增KYC查詢Log(Reviewer3rd_KYCQueryLog log) => await dbContext.Reviewer3rd_KYCQueryLog.AddAsync(log);

    private CardStatus 轉換案件狀態(CardStatus oldCardStatus, SyncKycMW3Data kycResult)
    {
        if (oldCardStatus == CardStatus.KYC入檔作業_紙本件待月收入預審)
        {
            if (kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功)
            {
                return CardStatus.完成月收入確認;
            }
            else if (kycResult.KycCode == MW3RtnCodeConst.入檔KYC_主機TimeOut)
            {
                return oldCardStatus;
            }
            else
            {
                return CardStatus.紙本件_待月收入預審;
            }
        }
        else if (oldCardStatus == CardStatus.KYC入檔作業_網路件待月收入預審)
        {
            if (kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功)
            {
                return CardStatus.完成月收入確認;
            }
            else if (kycResult.KycCode == MW3RtnCodeConst.入檔KYC_主機TimeOut)
            {
                return oldCardStatus;
            }
            else
            {
                return CardStatus.網路件_待月收入預審;
            }
        }
        else
        {
            throw new Exception($"未知的案件狀態：{oldCardStatus}");
        }
    }

    private async Task 新增錯誤Log(string applyNo, string type, string errorMessage, string errorDetail, DateTime addTime)
    {
        var log = new System_ErrorLog()
        {
            ApplyNo = applyNo,
            Project = SystemErrorLogProjectConst.BATCH,
            Source = "KYCSyncJob",
            Type = type,
            ErrorMessage = errorMessage,
            ErrorDetail = errorDetail,
            AddTime = addTime,
            SendStatus = SendStatus.等待,
            SendEmailTime = null,
        };

        await dbContext.System_ErrorLog.AddAsync(log);
    }

    private async Task<SysParamManage_SysParam> 取得排程參數() => await dbContext.SysParamManage_SysParam.FirstAsync();

    private bool 檢查排程時間(DateTime now, DateTime? kycFixStartTime, DateTime? kycFixEndTime) =>
        kycFixStartTime is not null && kycFixEndTime is not null && now > kycFixStartTime.Value && now < kycFixEndTime.Value;

    private CardStatus? 更新案件狀態＿待月收入確認(List<GetSyncHandle> handles)
    {
        if (!handles.Any())
            return null;

        var updateHandles = new List<Reviewer_ApplyCreditCardInfoHandle>();
        foreach (var handle in handles)
        {
            var updateHandle = new Reviewer_ApplyCreditCardInfoHandle { SeqNo = handle.SeqNo };

            if (handle.CardStatus == CardStatus.KYC入檔作業_紙本件待月收入預審)
            {
                updateHandle.CardStatus = CardStatus.紙本件_待月收入預審;
            }
            else if (handle.CardStatus == CardStatus.KYC入檔作業_網路件待月收入預審)
            {
                updateHandle.CardStatus = CardStatus.網路件_待月收入預審;
            }
            else
            {
                throw new Exception($"未知的案件狀態：{handle.CardStatus}");
            }

            updateHandles.Add(updateHandle);
        }

        // 批次更新
        foreach (var handle in updateHandles)
        {
            dbContext.Attach(handle);
            dbContext.Entry(handle).Property(x => x.CardStatus).IsModified = true;
        }
        return updateHandles.First().CardStatus;
    }
}
