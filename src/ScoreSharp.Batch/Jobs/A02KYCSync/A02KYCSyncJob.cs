using Microsoft.Data.SqlClient;
using ScoreSharp.Batch.Jobs.A02KYCSync.Helpers;
using ScoreSharp.Batch.Jobs.A02KYCSync.Models;
using ScoreSharp.Common.Adapters.MW3.Models;

namespace ScoreSharp.Batch.Jobs.A02KYCSync;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("A02 KYC 同步排程")]
public class A02KYCSyncJob(ILogger<A02KYCSyncJob> logger, ScoreSharpContext dbContext, IMW3APAPIAdapter mw3APAPIAdapter)
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("A02 KYC 同步排程 - 執行人員：{0}")]
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
            if (systemBatchSet!.A02KYCSyncJob_IsEnabled == "N")
            {
                logger.LogInformation("系統參數設定不執行【A02 KYC 同步】排程，執行結束");
                return;
            }

            var param = await 取得排程參數();
            var isInScheduleTime = 檢查排程時間(now: DateTime.Now, kycFixStartTime: param.KYCFixStartTime, kycFixEndTime: param.KYCFixEndTime);

            if (isInScheduleTime)
            {
                logger.LogInformation("資訊部 KYC 系統維護中，結束排程");
                return;
            }

            var handlesGroupBy = await 取得需入檔案件(count: systemBatchSet.A02KYCSyncJob_BatchSize);

            if (handlesGroupBy.Count == 0)
            {
                logger.LogInformation("沒有需要 KYC 入檔的案件，結束排程");
                return;
            }

            foreach (var handleGroupBy in handlesGroupBy)
            {
                DateTime startTime = DateTime.Now;

                try
                {
                    var main = await 取得主卡人資料(handleGroupBy.ApplyNo);

                    SyncKycMW3Info syncKycMW3Info = MapHelper.Map原卡友SyncKycMW3Info(main);
                    var mw3Result = await 呼叫KYC入檔API(syncKycMW3Info);
                    var kycResult = mw3Result.Data.Info.Result.Data;
                    var kycProcess = new List<Reviewer_ApplyCreditCardInfoProcess>();

                    logger.LogInformation(
                        "A02 KYC 入檔作業結果，案件編號：{applyNo}，發查是否成功：{result}，KYC 代碼：{kycCode}，KYC Level：{kycRank}，失敗原因：{kycMsg}，例外錯誤：{error}",
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
                        apiName: "KYC00CREDIT"
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
                        type: "A02KYCSyncJob 檢核錯誤",
                        errorMessage: string.Empty,
                        errorDetail: ex.ToString(),
                        addTime: startTime
                    );

                    await dbContext.SaveChangesAsync();
                }
            }

            logger.LogInformation("A02 KYC 同步排程，執行完成");
        }
        catch (Exception ex)
        {
            logger.LogError("A02 KYC 同步排程，意外錯誤 {ex}", ex.ToString());

            dbContext.ChangeTracker.Clear();

            await 新增錯誤Log(
                applyNo: string.Empty,
                type: "A02KYCSyncJob 意外錯誤",
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
                SELECT Top({0}) A.[SeqNo]
                    ,A.[ApplyNo]
                    ,A.[ID]
                    ,A.[UserType]
                    ,A.[CardStatus]
                FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] A
                JOIN [ScoreSharp].[dbo].[Reviewer_FinanceCheckInfo] F ON  A.ApplyNo = F.ApplyNo AND A.ID = F.ID AND F.UserType = 1
                WHERE A.CardStatus = 30116
                AND NOT (F.KYC_RtnCode = 'KC001'
                AND F.KYC_QueryTime IS NOT NULL
                AND DATEDIFF(HOUR, F.KYC_QueryTime, GETDATE()) < {1})
            ";

        var handles = await dbContext.Database.SqlQueryRaw<GetSyncHandle>(sql, count, hour).ToListAsync();
        var groupByApplyNo = handles.GroupBy(x => x.ApplyNo).Select(x => new SyncHandleGroupBy { ApplyNo = x.Key, Handles = x.ToList() }).ToList();
        return groupByApplyNo;
    }

    private async Task<Reviewer_ApplyCreditCardInfoMain> 取得主卡人資料(string applyNo) =>
        await dbContext.Reviewer_ApplyCreditCardInfoMain.SingleAsync(x => x.ApplyNo == applyNo);

    private async Task<Reviewer_FinanceCheckInfo> 取得金融檢核(string applyNo, string id) =>
        await dbContext.Reviewer_FinanceCheckInfo.SingleAsync(x => x.ApplyNo == applyNo && x.ID == id);

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
        mainFinanceCheck.KYC_StrongReStatus = null;

        List<Reviewer_ApplyCreditCardInfoHandle> updateHandles = new();
        if (isMW3ApiSuccess && kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功)
        {
            main.CurrentHandleUserId = null;
            mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;
        }

        foreach (var handle in handles)
        {
            var updateHandle = new Reviewer_ApplyCreditCardInfoHandle { SeqNo = handle.SeqNo };
            if (kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功)
            {
                updateHandle.CardStatus = CardStatus.網路件_卡友_完成KYC入檔作業;
            }
            else if (kycResult.KycCode == MW3RtnCodeConst.入檔KYC_主機TimeOut)
            {
                updateHandle.CardStatus = handle.CardStatus;
            }
            else
            {
                updateHandle.CardStatus = CardStatus.網路件_待月收入預審;
            }
            updateHandles.Add(updateHandle);
        }

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

    private async Task 新增錯誤Log(string applyNo, string type, string errorMessage, string errorDetail, DateTime addTime)
    {
        var log = new System_ErrorLog()
        {
            ApplyNo = applyNo,
            Project = SystemErrorLogProjectConst.BATCH,
            Source = "A02KYCSyncJob",
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
}
