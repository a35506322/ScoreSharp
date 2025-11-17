using System.Diagnostics;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("網路進件_非卡友檢核新案件")]
public class EcardNotA02CheckNewCaseJob(
    ScoreSharpContext efContext,
    ILogger<EcardNotA02CheckNewCaseJob> logger,
    IEcardNotA02CheckNewCaseRepository repository,
    IEcardNotA02CheckNewCaseService service
)
{
    private List<string> errorCase = new();
    private CommonDBDataDto _commonDBDataDto = new();
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("網路進件_非卡友檢核新案件 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await _semaphore.WaitAsync(0))
        {
            logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            var systemBatchSet = await efContext.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
            if (systemBatchSet!.EcardNotA02CheckNewCase_IsEnabled == "N")
            {
                logger.LogInformation("系統參數設定不執行【網路進件_非卡友檢核新案件】排程，執行結束");
                return;
            }

            logger.LogInformation("查詢須檢核非卡友案件");
            var peddingChecks = await repository.查詢_須檢核非卡友案件(systemBatchSet.EcardNotA02CheckNewCase_BatchSize);

            if (!peddingChecks.Any())
            {
                logger.LogInformation("未有非卡友案件需要檢核");
                return;
            }

            logger.LogInformation("未檢核非卡友案件數量：{peddingCaseCount}", peddingChecks.Count());
            logger.LogInformation("案件編號：{applyNo}", String.Join("、", peddingChecks.Select(x => x.ApplyNo).ToArray()));

            try
            {
                logger.LogInformation("查詢通用資料");
                await 查詢通用資料();
            }
            catch (Exception ex)
            {
                logger.LogError("查詢通用資料發生錯誤不再執行此次檢核：{@ex}", ex.ToString());
                await service.通知_檢核異常信件(
                    applyNo: "",
                    type: SystemErrorLogTypeConst.檢核異常,
                    errorMessage: $"查詢通用資料發生錯誤不再執行此次檢核：{ex.Message}",
                    errorDetail: ex.ToString()
                );
                return;
            }

            foreach (var context in peddingChecks)
            {
                logger.LogInformation("案件編號：{applyNo} 開始檢核", context.ApplyNo);

                var verifyContext = MapHelper.MapToVerifyContext(context);

                LogVerifyContext(verifyContext);

                CreditCardValidateResult validateResult = new();

                try
                {
                    Dictionary<string, Task> taskDict = new();

                    if (verifyContext.是否檢核IP相同)
                    {
                        taskDict["CheckSameIP"] = service.檢核_相同IP比對(context, _commonDBDataDto); //TODO: 呼叫 SP
                    }

                    if (verifyContext.是否檢核行內IP)
                    {
                        taskDict["CheckEqualInternalIP"] = service.檢核_行內IP相同(context, _commonDBDataDto);
                    }

                    if (verifyContext.是否檢核網路電子郵件)
                    {
                        taskDict["CheckSameWebCaseEmail"] = service.檢核_相同電子郵件比對(context, _commonDBDataDto); //TODO: 呼叫 SP
                    }

                    if (verifyContext.是否檢核網路手機號碼)
                    {
                        taskDict["CheckSameWebCaseMobile"] = service.檢核_相同手機號碼比對(context, _commonDBDataDto); //TODO: 呼叫 SP
                    }

                    if (verifyContext.是否檢查短時間ID相同)
                    {
                        taskDict["CheckShortTimeID"] = service.檢核_短時間ID相同比對(context, _commonDBDataDto); //TODO: 呼叫 SP
                    }

                    if (verifyContext.是否檢核929)
                    {
                        taskDict["Check929"] = service.檢核_發查929(context);
                    }

                    if (verifyContext.是否檢核分行資訊)
                    {
                        taskDict["QueryBranchInfo"] = service.檢核_查詢分行資訊(context);
                    }

                    if (verifyContext.是否檢核關注名單)
                    {
                        taskDict["CheckFocus"] = service.檢核_查詢關注名單(context);
                    }

                    if (verifyContext.是否檢核姓名檢查)
                    {
                        taskDict["CheckName"] = service.檢核_查詢姓名檢核(context);
                    }

                    if (verifyContext.是否檢核行內Email)
                    {
                        taskDict["InternalEmailSame"] = service.檢核_行內Email資料(context);
                    }

                    if (verifyContext.是否檢核行內手機)
                    {
                        taskDict["InternalMobileSame"] = service.檢核_行內Mobile資料(context);
                    }

                    if (verifyContext.是否檢查重覆進件)
                    {
                        taskDict["CheckRepeatApply"] = service.檢查_是否為重覆進件(context);
                    }

                    await Task.WhenAll(taskDict.Values);

                    foreach (var task in taskDict)
                    {
                        if (task.Value.IsCompleted)
                        {
                            logger.LogInformation("任務：{taskName} 已完成", task.Key);
                        }

                        switch (task.Key)
                        {
                            case "CheckSameIP":
                                validateResult.SameIPCheckRes = (task.Value as Task<CheckCaseRes<CheckSameIP>>)?.Result;
                                logger.LogInformation(
                                    "相同IP執行是否成功:{isSameIPSuccess},相同IP結果：{isSameIPHit}",
                                    validateResult.SameIPCheckRes.IsSuccess,
                                    validateResult.SameIPCheckRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckEqualInternalIP":
                                validateResult.InternalIPCheckRes = (task.Value as Task<CheckCaseRes<bool>>)?.Result;
                                logger.LogInformation(
                                    "行內IP執行是否成功:{isInternalIPSuccess},行內IP結果：{isInternalIPHit}",
                                    validateResult.InternalIPCheckRes.IsSuccess,
                                    validateResult.InternalIPCheckRes?.SuccessData?.Data
                                );
                                break;
                            case "CheckSameWebCaseEmail":
                                validateResult.SameEmailCheckRes = (task.Value as Task<CheckCaseRes<CheckSameWebCaseEmail>>)?.Result;
                                logger.LogInformation(
                                    "相同電子郵件執行是否成功:{isSameEmailSuccess},相同電子郵件結果：{isSameEmailHit}",
                                    validateResult.SameEmailCheckRes.IsSuccess,
                                    validateResult.SameEmailCheckRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckSameWebCaseMobile":
                                validateResult.SameMobileCheckRes = (task.Value as Task<CheckCaseRes<CheckSameWebCaseMobile>>)?.Result;
                                logger.LogInformation(
                                    "相同手機號碼執行是否成功:{isSameMobileSuccess},相同手機號碼結果：{isSameMobileHit}",
                                    validateResult.SameMobileCheckRes.IsSuccess,
                                    validateResult.SameMobileCheckRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckShortTimeID":
                                validateResult.ShortTimeIDCheckRes = (task.Value as Task<CheckCaseRes<CheckShortTimeID>>)?.Result;
                                logger.LogInformation(
                                    "短時間ID執行是否成功:{isShortTimeIDSuccess},短時間ID結果：{isShortTimeIDHit}",
                                    validateResult.ShortTimeIDCheckRes.IsSuccess,
                                    validateResult.ShortTimeIDCheckRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "Check929":
                                validateResult.Check929Res = (task.Value as Task<CheckCaseRes<Query929Info>>)?.Result;
                                logger.LogInformation(
                                    "929執行是否成功:{is929Success},929結果：{is929Hit}",
                                    validateResult.Check929Res.IsSuccess,
                                    validateResult.Check929Res?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "QueryBranchInfo":
                                validateResult.QueryBranchInfoRes = (task.Value as Task<CheckCaseRes<QueryBranchInfo>>)?.Result;
                                logger.LogInformation(
                                    "分行資訊執行是否成功:{isBranchInfoSuccess},分行資訊結果：{isBranchInfoHit}",
                                    validateResult.QueryBranchInfoRes.IsSuccess,
                                    validateResult.QueryBranchInfoRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckFocus":
                                validateResult.CheckFocusRes = (task.Value as Task<CheckCaseRes<ConcernDetailInfo>>)?.Result;
                                logger.LogInformation(
                                    "關注名單1執行是否成功:{isFocus1Success},關注名單1結果：{isFocus1Hit}",
                                    validateResult.CheckFocusRes.IsSuccess,
                                    validateResult.CheckFocusRes?.SuccessData?.Data.Focus1Checked
                                );
                                logger.LogInformation(
                                    "關注名單2執行是否成功:{isFocus2Success},關注名單2結果：{isFocus2Hit}",
                                    validateResult.CheckFocusRes.IsSuccess,
                                    validateResult.CheckFocusRes?.SuccessData?.Data.Focus2Checked
                                );
                                break;
                            case "CheckName":
                                validateResult.CheckNameRes = (task.Value as Task<CheckCaseRes<QueryCheckName>>)?.Result;
                                logger.LogInformation(
                                    "姓名檢核執行是否成功:{isCheckNameSuccess},姓名檢核結果：{isCheckNameHit}",
                                    validateResult.CheckNameRes.IsSuccess,
                                    validateResult.CheckNameRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "InternalEmailSame":
                                validateResult.CheckInternalEmailSameRes = (task.Value as Task<CheckCaseRes<CheckInternalEmailSameResult>>)?.Result;
                                logger.LogInformation(
                                    "行內Email檢核執行是否成功:{isInternalEmailSuccess},行內Email檢核結果：{isInternalEmailHit}",
                                    validateResult.CheckInternalEmailSameRes.IsSuccess,
                                    validateResult.CheckInternalEmailSameRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "InternalMobileSame":
                                validateResult.CheckInternalMobileSameRes = (task.Value as Task<CheckCaseRes<CheckInternalMobileSameResult>>)?.Result;
                                logger.LogInformation(
                                    "行內Mobile檢核執行是否成功:{isInternalMobileSuccess},行內Mobile檢核結果：{isInternalMobileHit}",
                                    validateResult.CheckInternalMobileSameRes.IsSuccess,
                                    validateResult.CheckInternalMobileSameRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckRepeatApply":
                                validateResult.CheckRepeatApplyRes = (task.Value as Task<CheckCaseRes<bool>>)?.Result;
                                logger.LogInformation(
                                    "重覆進件執行是否成功:{isCheckRepeatApplySuccess},重覆進件資料結果：{isCheckRepeatApplyHit}",
                                    validateResult.CheckRepeatApplyRes.IsSuccess,
                                    validateResult.CheckRepeatApplyRes?.SuccessData?.Data
                                );
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("案件編號：{applyNo}，信用卡檢核有誤查看System_ErrorLog", context.ApplyNo);
                    var errorCount = await service.案件異常處理(context.ApplyNo, SystemErrorLogTypeConst.內部程式錯誤, "信用卡申請案件檢核有誤", ex);
                    if (errorCount == 2)
                    {
                        新增達錯誤2次案件需寄信(context.ApplyNo);
                    }
                    continue;
                }

                logger.LogInformation("案件編號：{applyNo} 開始儲存資料庫", context.ApplyNo);

                try
                {
                    // insert 錯誤紀錄
                    var errorLogs = new[]
                    {
                        validateResult.QueryBranchInfoRes.ErrorData,
                        validateResult.Check929Res.ErrorData,
                        validateResult.CheckFocusRes.ErrorData,
                        validateResult.SameIPCheckRes.ErrorData,
                        validateResult.InternalIPCheckRes.ErrorData,
                        validateResult.SameEmailCheckRes.ErrorData,
                        validateResult.SameMobileCheckRes.ErrorData,
                        validateResult.ShortTimeIDCheckRes.ErrorData,
                        validateResult.CheckNameRes.ErrorData,
                        validateResult.CheckInternalEmailSameRes.ErrorData,
                        validateResult.CheckInternalMobileSameRes.ErrorData,
                        validateResult.CheckRepeatApplyRes.ErrorData,
                    }
                        .Where(error => error != null)
                        .ToList();

                    if (errorLogs.Any())
                    {
                        logger.LogInformation("案件編號：{applyNo} 新增錯誤紀錄", context.ApplyNo);
                        await efContext.System_ErrorLog.AddRangeAsync(errorLogs);
                    }

                    bool jobIsSuccess = errorLogs.Any() ? false : true;

                    var verifyResultContext = MapHelper.MapToVerifyResultContext(verifyContext, validateResult);

                    // update handle
                    CardStatus cardStatus = ConvertHelper.ConvertCardStatus(context.CardStatus, jobIsSuccess);
                    UpdateHandledCase(context, cardStatus);

                    // update financeCheck
                    await UpdateFinanceCheck(context, verifyResultContext, validateResult);

                    // update 銀行追蹤
                    await UpdateBankTrace(context, verifyResultContext, validateResult);

                    // add NameCheckLog
                    await AddNameCheckLog(verifyResultContext, validateResult);

                    // insert into process
                    await InsertProcess(context, verifyResultContext, validateResult, cardStatus, verifyContext);

                    // update checklog
                    var errorCount = UpdateCheckLog(context, verifyResultContext, validateResult, verifyContext, jobIsSuccess);
                    if (errorCount == 2)
                    {
                        新增達錯誤2次案件需寄信(context.ApplyNo);
                    }

                    // update main
                    await UpdateMain(context, verifyResultContext, validateResult);

                    await efContext.SaveChangesAsync();
                    efContext.ChangeTracker.Clear();

                    if (jobIsSuccess)
                    {
                        logger.LogInformation("案件編號：{applyNo} 檢核案件成功", context.ApplyNo);
                    }
                    else
                    {
                        logger.LogInformation("案件編號：{applyNo} 檢核案件失敗，查看 System_ErrorLog", context.ApplyNo);
                    }

                    logger.LogInformation("");
                }
                catch (Exception ex)
                {
                    logger.LogError("案件編號：{applyNo} 儲存資料庫有誤查看 System_ErrorLog", context.ApplyNo);
                    var errorCount = await service.案件異常處理(context.ApplyNo, SystemErrorLogTypeConst.內部程式錯誤, "儲存資料庫有誤", ex);
                    if (errorCount == 2)
                    {
                        logger.LogInformation("案件編號：{applyNo} 達錯誤2次，需寄信", context.ApplyNo);
                        新增達錯誤2次案件需寄信(context.ApplyNo);
                    }

                    continue;
                }
            }

            if (errorCase.Count > 0)
            {
                logger.LogInformation("案件編號：{applyNo} 達錯誤2次，需寄信", String.Join(",", errorCase));
                await service.寄信給達錯誤2次案件(errorCase);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task 查詢通用資料()
    {
        _commonDBDataDto.InternalIPs = await repository.查詢_行內IP();
        _commonDBDataDto.SysParam = await repository.查詢_系統參數();
        _commonDBDataDto.AddressInfos = await repository.查詢_地址資訊();
        _commonDBDataDto.HistoryApplyCreditInfo = await repository.查詢_歷史申請資料(); // TODO 未來 ID EMAIL MOBILE 都讀 SP
    }

    private void 新增達錯誤2次案件需寄信(string applyNo)
    {
        ;
        errorCase.Add(applyNo);
    }

    private bool 是否計算郵遞區號(AddressContext address)
    {
        if (!string.IsNullOrEmpty(address.ZipCode))
        {
            return false;
        }

        if (
            string.IsNullOrEmpty(address.City)
            || string.IsNullOrEmpty(address.District)
            || string.IsNullOrEmpty(address.Road)
            || string.IsNullOrEmpty(address.Number)
        )
        {
            return false;
        }

        return true;
    }

    private void LogVerifyContext(VerifyContext verifyContext)
    {
        logger.LogInformation("是否執行檢核IP相同：{isCheckSameIP}", verifyContext.是否檢核IP相同);
        logger.LogInformation("是否執行檢核行內IP：{isCheckEqualInternalIP}", verifyContext.是否檢核行內IP);
        logger.LogInformation("是否執行檢核電子郵件：{isCheckSameWebCaseEmail}", verifyContext.是否檢核網路電子郵件);
        logger.LogInformation("是否執行檢核手機號碼：{isCheckSameWebCaseMobile}", verifyContext.是否檢核網路手機號碼);
        logger.LogInformation("是否執行檢核929：{isCheck929}", verifyContext.是否檢核929);
        logger.LogInformation("是否執行檢核分行資訊：{isQueryBranchInfo}", verifyContext.是否檢核分行資訊);
        logger.LogInformation("是否執行檢核關注名單：{isCheckFocus}", verifyContext.是否檢核關注名單);
        logger.LogInformation("是否執行檢核姓名：{isCheckName}", verifyContext.是否檢核姓名檢查);
        logger.LogInformation("是否檢查短時間ID相同：{IsCheckShortTimeID}", verifyContext.是否檢查短時間ID相同);
        logger.LogInformation("是否執行檢核行內Email：{isCheckInternalEmail}", verifyContext.是否檢核行內Email);
        logger.LogInformation("是否執行檢核行內手機：{isCheckInternalMobile}", verifyContext.是否檢核行內手機);
        logger.LogInformation("是否檢核重覆進件：{isCheckRepeatApply}", verifyContext.是否檢查重覆進件);
    }

    private void UpdateHandledCase(CheckJobContext context, CardStatus cardStatus)
    {
        var handleInfo = new Reviewer_ApplyCreditCardInfoHandle() { SeqNo = context.HandleSeqNo };
        efContext.Attach(handleInfo);
        efContext.Entry(handleInfo).Property(x => x.CardStatus).IsModified = true;
        handleInfo.CardStatus = cardStatus;
    }

    private async Task UpdateFinanceCheck(CheckJobContext context, VerifyResultContext verifyResultContext, CreditCardValidateResult validateResult)
    {
        var financeCheck = new Reviewer_FinanceCheckInfo()
        {
            ApplyNo = context.ApplyNo,
            ID = context.ID,
            UserType = context.UserType,
        };
        efContext.Attach(financeCheck);

        if (verifyResultContext.檢核929成功)
        {
            efContext.Entry(financeCheck).Property(x => x.Checked929).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Q929_RtnCode).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Q929_RtnMsg).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Q929_QueryTime).IsModified = true;

            var check929ResData = validateResult.Check929Res.SuccessData!.Data;
            financeCheck.Checked929 = check929ResData.是否命中;
            financeCheck.Q929_RtnCode = check929ResData.RtnCode;
            financeCheck.Q929_RtnMsg = check929ResData.RtnMsg;
            financeCheck.Q929_QueryTime = check929ResData.QueryTime;

            if (verifyResultContext.命中929)
            {
                await efContext.Reviewer3rd_929Log.AddRangeAsync(check929ResData.Reviewer3rd_929Logs);
            }
        }

        if (verifyResultContext.檢核關注名單成功)
        {
            efContext.Entry(financeCheck).Property(x => x.Focus1Check).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus1Hit).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus1_RtnCode).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus1_RtnMsg).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus1_QueryTime).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus1_TraceId).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus2Check).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus2Hit).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus2_RtnCode).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus2_RtnMsg).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus2_QueryTime).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.Focus2_TraceId).IsModified = true;

            var focusResData = validateResult.CheckFocusRes.SuccessData!.Data;
            financeCheck.Focus1Check = focusResData.Focus1Checked;
            financeCheck.Focus1_RtnCode = focusResData.RtnCode;
            financeCheck.Focus1_RtnMsg = focusResData.RtnMsg;
            financeCheck.Focus1_QueryTime = focusResData.QueryTime;
            financeCheck.Focus1_TraceId = focusResData.TraceId;
            financeCheck.Focus1Hit = String.Join("、", focusResData.Focus1HitList);

            financeCheck.Focus2Check = focusResData.Focus2Checked;
            financeCheck.Focus2_RtnCode = focusResData.RtnCode;
            financeCheck.Focus2_RtnMsg = focusResData.RtnMsg;
            financeCheck.Focus2_QueryTime = focusResData.QueryTime;
            financeCheck.Focus2_TraceId = focusResData.TraceId;
            financeCheck.Focus2Hit = String.Join("、", focusResData.Focus2HitList);

            // insert 關注名單
            if (verifyResultContext.命中關注名單1 || verifyResultContext.命中關注名單2)
            {
                if (focusResData.WarningCompanyLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_WarnCompLog.AddRangeAsync(focusResData.WarningCompanyLogs);
                }

                if (focusResData.RiskAccountLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_RiskAccountLog.AddRangeAsync(focusResData.RiskAccountLogs);
                }

                if (focusResData.WarnLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_WarnLog.AddRangeAsync(focusResData.WarnLogs);
                }

                if (focusResData.FledLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_FledLog.AddRangeAsync(focusResData.FledLogs);
                }

                if (focusResData.PunishLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_PunishLog.AddRangeAsync(focusResData.PunishLogs);
                }

                if (focusResData.ImmiLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_ImmiLog.AddRangeAsync(focusResData.ImmiLogs);
                }

                if (focusResData.FrdIdLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_FrdIdLog.AddRangeAsync(focusResData.FrdIdLogs);
                }

                if (focusResData.LayOffLogs.Count > 0)
                {
                    await efContext.Reviewer3rd_LayOffLog.AddRangeAsync(focusResData.LayOffLogs);
                }
            }

            // 失蹤人口 (G) 無論如何都會有資料
            if (focusResData.MissingPersonsLogs != null)
            {
                await efContext.Reviewer3rd_MissingPersonsLog.AddAsync(focusResData.MissingPersonsLogs);
            }
        }

        if (verifyResultContext.檢核分行資訊成功)
        {
            efContext.Entry(financeCheck).Property(x => x.IsBranchCustomer).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.BranchCus_RtnCode).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.BranchCus_RtnMsg).IsModified = true;
            efContext.Entry(financeCheck).Property(x => x.BranchCus_QueryTime).IsModified = true;

            var queryBranchInfoResData = validateResult.QueryBranchInfoRes.SuccessData!.Data;
            financeCheck.IsBranchCustomer = queryBranchInfoResData.是否命中;
            financeCheck.BranchCus_RtnCode = queryBranchInfoResData.RtnCode;
            financeCheck.BranchCus_RtnMsg = queryBranchInfoResData.RtnMsg;
            financeCheck.BranchCus_QueryTime = queryBranchInfoResData.QueryTime;

            if (verifyResultContext.命中分行資訊)
            {
                if (queryBranchInfoResData.BranchCusCusInfo.Count > 0)
                {
                    await efContext.Reviewer3rd_BranchCusCusInfo.AddRangeAsync(queryBranchInfoResData.BranchCusCusInfo);
                }

                if (queryBranchInfoResData.BranchCusWMCust.Count > 0)
                {
                    await efContext.Reviewer3rd_BranchCusWMCust.AddRangeAsync(queryBranchInfoResData.BranchCusWMCust);
                }

                if (queryBranchInfoResData.BranchCusCD.Count > 0)
                {
                    await efContext.Reviewer3rd_BranchCusCD.AddRangeAsync(queryBranchInfoResData.BranchCusCD);
                }

                if (queryBranchInfoResData.BranchCusDD.Count > 0)
                {
                    await efContext.Reviewer3rd_BranchCusDD.AddRangeAsync(queryBranchInfoResData.BranchCusDD);
                }

                if (queryBranchInfoResData.BranchCusCAD.Count > 0)
                {
                    await efContext.Reviewer3rd_BranchCusCAD.AddRangeAsync(queryBranchInfoResData.BranchCusCAD);
                }

                if (queryBranchInfoResData.BranchCusCreditOver.Count > 0)
                {
                    await efContext.Reviewer3rd_BranchCusCreditOver.AddRangeAsync(queryBranchInfoResData.BranchCusCreditOver);
                }
            }
        }
    }

    private async Task UpdateMain(CheckJobContext context, VerifyResultContext verifyResultContext, CreditCardValidateResult validateResult)
    {
        var main = await efContext.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == context.ApplyNo);
        main.IsOriginalCardholder = "N";
        main.LastUpdateUserId = UserIdConst.SYSTEM;
        main.LastUpdateTime = DateTime.Now;

        if (verifyResultContext.檢核分行資訊成功)
        {
            main.IsBranchCustomer = verifyResultContext.命中分行資訊 ? "Y" : "N";
        }

        if (verifyResultContext.檢核姓名檢核成功)
        {
            main.IsDunyangBlackList = verifyResultContext.命中姓名 ? "Y" : "N";
            main.NameChecked = verifyResultContext.命中姓名 ? "Y" : "N";
            var checkNameResData = validateResult.CheckNameRes.SuccessData!.Data;
            await efContext.Reviewer3rd_NameCheckLog.AddRangeAsync(checkNameResData.Reviewer3rd_NameCheckLog);
        }

        if (verifyResultContext.檢核重複進件成功)
        {
            main.IsRepeatApply = verifyResultContext.命中重複進件 ? "Y" : "N";
        }

        // update address 計算郵遞區號
        var regAddress = MapHelper.MapToRegAddress(main);
        if (是否計算郵遞區號(regAddress))
        {
            main.Reg_ZipCode = service.計算郵遞區號(regAddress, _commonDBDataDto.AddressInfos);
        }

        var liveAddress = MapHelper.MapToLiveAddress(main);
        if (是否計算郵遞區號(liveAddress))
        {
            main.Live_ZipCode = service.計算郵遞區號(liveAddress, _commonDBDataDto.AddressInfos);
        }

        var parentLiveAddress = MapHelper.MapToParentLiveAddress(main);
        if (是否計算郵遞區號(parentLiveAddress))
        {
            main.ParentLive_ZipCode = service.計算郵遞區號(parentLiveAddress, _commonDBDataDto.AddressInfos);
        }

        var compAddress = MapHelper.MapToCompAddress(main);
        if (是否計算郵遞區號(compAddress))
        {
            main.Comp_ZipCode = service.計算郵遞區號(compAddress, _commonDBDataDto.AddressInfos);
        }

        var billAddress = MapHelper.MapToBillAddress(main);
        if (是否計算郵遞區號(billAddress))
        {
            main.Bill_ZipCode = service.計算郵遞區號(billAddress, _commonDBDataDto.AddressInfos);
        }

        var sendCardAddress = MapHelper.MapToSendCardAddress(main);
        if (是否計算郵遞區號(sendCardAddress))
        {
            main.SendCard_ZipCode = service.計算郵遞區號(sendCardAddress, _commonDBDataDto.AddressInfos);
        }
    }

    private async Task UpdateBankTrace(CheckJobContext context, VerifyResultContext verifyResultContext, CreditCardValidateResult validateResult)
    {
        var bankTrace = new Reviewer_BankTrace()
        {
            ApplyNo = context.ApplyNo,
            ID = context.ID,
            UserType = context.UserType,
        };
        efContext.Attach(bankTrace);

        if (verifyResultContext.檢核行內IP成功)
        {
            efContext.Entry(bankTrace).Property(x => x.EqualInternalIP_Flag).IsModified = true;
            var internalIPCheck = validateResult.InternalIPCheckRes.SuccessData!.Data!;
            bankTrace.EqualInternalIP_Flag = internalIPCheck ? "Y" : "N";
        }

        if (verifyResultContext.檢核IP相同成功)
        {
            efContext.Entry(bankTrace).Property(x => x.SameIP_Flag).IsModified = true;
            var sameIPCheck = validateResult.SameIPCheckRes.SuccessData!.Data!;
            bankTrace.SameIP_Flag = sameIPCheck.是否命中;

            if (verifyResultContext.命中IP相同)
            {
                await efContext.Reviewer_CheckTrace.AddRangeAsync(sameIPCheck.Reviewer_CheckTraces);
            }
        }

        if (verifyResultContext.檢核電子郵件成功)
        {
            efContext.Entry(bankTrace).Property(x => x.SameEmail_Flag).IsModified = true;
            var sameEmailCheck = validateResult.SameEmailCheckRes.SuccessData!.Data!;
            bankTrace.SameEmail_Flag = sameEmailCheck.是否命中;

            if (verifyResultContext.命中電子郵件)
            {
                await efContext.Reviewer_CheckTrace.AddRangeAsync(sameEmailCheck.Reviewer_CheckTraces);
            }
        }

        if (verifyResultContext.檢核手機號碼成功)
        {
            efContext.Entry(bankTrace).Property(x => x.SameMobile_Flag).IsModified = true;
            var sameMobileCheck = validateResult.SameMobileCheckRes.SuccessData!.Data!;
            bankTrace.SameMobile_Flag = sameMobileCheck.是否命中;

            if (verifyResultContext.命中手機號碼)
            {
                await efContext.Reviewer_CheckTrace.AddRangeAsync(sameMobileCheck.Reviewer_CheckTraces);
            }
        }

        if (verifyResultContext.檢核短時間ID相同成功)
        {
            efContext.Entry(bankTrace).Property(x => x.ShortTimeID_Flag).IsModified = true;
            var sameShortTimeIDCheck = validateResult.ShortTimeIDCheckRes.SuccessData!.Data!;
            bankTrace.ShortTimeID_Flag = sameShortTimeIDCheck.是否命中;

            if (verifyResultContext.命中短時間ID相同)
            {
                await efContext.Reviewer_CheckTrace.AddRangeAsync(sameShortTimeIDCheck.Reviewer_CheckTraces);
            }
        }

        if (verifyResultContext.是否檢核行內Email成功)
        {
            efContext.Entry(bankTrace).Property(x => x.InternalEmailSame_Flag).IsModified = true;
            var sameInternalEmailCheck = validateResult.CheckInternalEmailSameRes.SuccessData!.Data!;
            bankTrace.InternalEmailSame_Flag = sameInternalEmailCheck.是否命中;

            if (verifyResultContext.命中行內Email)
            {
                await efContext.Reviewer_BankInternalSameLog.AddRangeAsync(sameInternalEmailCheck.BankInternalSameLogs);
            }
        }

        if (verifyResultContext.是否檢核行內手機成功)
        {
            efContext.Entry(bankTrace).Property(x => x.InternalMobileSame_Flag).IsModified = true;
            var sameInternalMobileCheck = validateResult.CheckInternalMobileSameRes.SuccessData!.Data!;
            bankTrace.InternalMobileSame_Flag = sameInternalMobileCheck.是否命中;

            if (verifyResultContext.命中行內手機)
            {
                await efContext.Reviewer_BankInternalSameLog.AddRangeAsync(sameInternalMobileCheck.BankInternalSameLogs);
            }
        }
    }

    private async Task InsertProcess(
        CheckJobContext context,
        VerifyResultContext verifyResultContext,
        CreditCardValidateResult validateResult,
        CardStatus cardStatus,
        VerifyContext verifyContext
    )
    {
        List<Reviewer_ApplyCreditCardInfoProcess> processes = new();
        string processNote = $"({context.UserType.ToString()}_{context.ID})";

        // insert into process
        if (verifyContext.是否檢核分行資訊)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成分行資訊查詢,
                validateResult.QueryBranchInfoRes.StartTime,
                validateResult.QueryBranchInfoRes.EndTime,
                verifyResultContext.檢核分行資訊成功 ? processNote : $"{processNote}({ProcessNoteConst.分行資訊查詢錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核行內IP)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成行內IP檢核,
                validateResult.InternalIPCheckRes.StartTime,
                validateResult.InternalIPCheckRes.EndTime,
                verifyResultContext.檢核行內IP成功 ? processNote : $"{processNote}({ProcessNoteConst.行內IP檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核IP相同)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成相同IP檢核,
                validateResult.SameIPCheckRes.StartTime,
                validateResult.SameIPCheckRes.EndTime,
                verifyResultContext.檢核IP相同成功 ? processNote : $"{processNote}({ProcessNoteConst.相同IP檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核929)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成929業務狀況查詢,
                validateResult.Check929Res.StartTime,
                validateResult.Check929Res.EndTime,
                verifyResultContext.檢核929成功 ? processNote : $"{processNote}({ProcessNoteConst.查詢929業務狀況錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核關注名單)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成關注名單1查詢,
                validateResult.CheckFocusRes.StartTime,
                validateResult.CheckFocusRes.EndTime,
                verifyResultContext.檢核關注名單成功 ? processNote : $"{processNote}({ProcessNoteConst.關注名單1查詢失敗})"
            );
            processes.Add(process);
            var process2 = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成關注名單2查詢,
                validateResult.CheckFocusRes.StartTime,
                validateResult.CheckFocusRes.EndTime,
                verifyResultContext.檢核關注名單成功 ? processNote : $"{processNote}({ProcessNoteConst.關注名單2查詢失敗})"
            );
            processes.Add(process2);
        }

        if (verifyContext.是否檢核網路電子郵件)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成網路相同電子郵件比對,
                validateResult.SameEmailCheckRes.StartTime,
                validateResult.SameEmailCheckRes.EndTime,
                verifyResultContext.檢核電子郵件成功 ? processNote : $"{processNote}({ProcessNoteConst.網路相同電子郵件比對錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核網路手機號碼)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成網路相同手機號碼比對,
                validateResult.SameMobileCheckRes.StartTime,
                validateResult.SameMobileCheckRes.EndTime,
                verifyResultContext.檢核手機號碼成功 ? processNote : $"{processNote}({ProcessNoteConst.網路相同手機號碼比對錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核姓名檢查)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成姓名檢核查詢,
                validateResult.CheckNameRes.StartTime,
                validateResult.CheckNameRes.EndTime,
                verifyResultContext.檢核姓名檢核成功 ? processNote : $"{processNote}({ProcessNoteConst.查詢姓名檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢查短時間ID相同)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成頻繁ID檢核,
                validateResult.ShortTimeIDCheckRes.StartTime,
                validateResult.ShortTimeIDCheckRes.EndTime,
                verifyResultContext.檢核短時間ID相同成功 ? processNote : $"{processNote}({ProcessNoteConst.頻繁ID檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核行內Email)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成行內Email檢核,
                validateResult.CheckInternalEmailSameRes.StartTime,
                validateResult.CheckInternalEmailSameRes.EndTime,
                verifyResultContext.是否檢核行內Email成功 ? processNote : $"{processNote}({ProcessNoteConst.行內Email檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核行內手機)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成行內手機檢核,
                validateResult.CheckInternalMobileSameRes.StartTime,
                validateResult.CheckInternalMobileSameRes.EndTime,
                verifyResultContext.是否檢核行內手機成功 ? processNote : $"{processNote}({ProcessNoteConst.行內手機檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢查重覆進件)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成重覆進件檢核,
                validateResult.CheckRepeatApplyRes.StartTime,
                validateResult.CheckRepeatApplyRes.EndTime,
                verifyResultContext.檢核重複進件成功 ? processNote : $"{processNote}({ProcessNoteConst.重覆進件檢核錯誤})"
            );
            processes.Add(process);
        }

        processes.Add(MapHelper.MapToProcess(context.ApplyNo, cardStatus.ToString(), DateTime.Now, DateTime.Now));
        await efContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
    }

    private int UpdateCheckLog(
        CheckJobContext context,
        VerifyResultContext verifyResultContext,
        CreditCardValidateResult validateResult,
        VerifyContext verifyContext,
        bool jobIsSuccess
    )
    {
        ReviewerPedding_WebApplyCardCheckJobForNotA02 checkLog = new() { ApplyNo = context.ApplyNo };
        efContext.Attach(checkLog);
        if (verifyContext.是否檢核929)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheck929).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.Check929LastTime).IsModified = true;
            checkLog.IsCheck929 = verifyResultContext.檢核929成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.Check929LastTime = validateResult.Check929Res.EndTime;
        }

        if (verifyContext.是否檢核關注名單)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckFocus).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckFocusLastTime).IsModified = true;
            checkLog.IsCheckFocus = verifyResultContext.檢核關注名單成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckFocusLastTime = validateResult.CheckFocusRes.EndTime;
        }

        if (verifyContext.是否檢核分行資訊)
        {
            efContext.Entry(checkLog).Property(x => x.IsQueryBranchInfo).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.QueryBranchInfoLastTime).IsModified = true;
            checkLog.IsQueryBranchInfo = verifyResultContext.檢核分行資訊成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.QueryBranchInfoLastTime = validateResult.QueryBranchInfoRes.EndTime;
        }

        if (verifyContext.是否檢核IP相同)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckSameIP).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckSameIPLastTime).IsModified = true;
            checkLog.IsCheckSameIP = verifyResultContext.檢核IP相同成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckSameIPLastTime = validateResult.SameIPCheckRes.EndTime;
        }

        if (verifyContext.是否檢核行內IP)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckEqualInternalIP).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckEqualInternalIPLastTime).IsModified = true;
            checkLog.IsCheckEqualInternalIP = verifyResultContext.檢核行內IP成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckEqualInternalIPLastTime = validateResult.InternalIPCheckRes.EndTime;
        }

        if (verifyContext.是否檢核網路電子郵件)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckSameWebCaseEmail).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckSameWebCaseEmailLastTime).IsModified = true;
            checkLog.IsCheckSameWebCaseEmail = verifyResultContext.檢核電子郵件成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckSameWebCaseEmailLastTime = validateResult.SameEmailCheckRes.EndTime;
        }

        if (verifyContext.是否檢核網路手機號碼)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckSameWebCaseMobile).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckSameWebCaseMobileLastTime).IsModified = true;
            checkLog.IsCheckSameWebCaseMobile = verifyResultContext.檢核手機號碼成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckSameWebCaseMobileLastTime = validateResult.SameMobileCheckRes.EndTime;
        }

        if (verifyContext.是否檢查短時間ID相同)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckShortTimeID).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckShortTimeIDLastTime).IsModified = true;
            checkLog.IsCheckShortTimeID = verifyResultContext.檢核短時間ID相同成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckShortTimeIDLastTime = validateResult.ShortTimeIDCheckRes.EndTime;
        }

        if (verifyContext.是否檢核姓名檢查)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckName).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckNameLastTime).IsModified = true;
            checkLog.IsCheckName = verifyResultContext.檢核姓名檢核成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckNameLastTime = validateResult.CheckNameRes.EndTime;
        }

        if (verifyContext.是否檢核行內Email)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckInternalEmail).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckInternalEmailLastTime).IsModified = true;
            checkLog.IsCheckInternalEmail = verifyResultContext.是否檢核行內Email成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckInternalEmailLastTime = validateResult.CheckInternalEmailSameRes.EndTime;
        }

        if (verifyContext.是否檢核行內手機)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckInternalMobile).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckInternalMobileLastTime).IsModified = true;
            checkLog.IsCheckInternalMobile = verifyResultContext.是否檢核行內手機成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckInternalMobileLastTime = validateResult.CheckInternalMobileSameRes.EndTime;
        }

        if (verifyContext.是否檢查重覆進件)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckRepeatApply).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckRepeatApplyLastTime).IsModified = true;
            checkLog.IsCheckRepeatApply = verifyResultContext.檢核重複進件成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckRepeatApplyLastTime = validateResult.CheckRepeatApplyRes.EndTime;
        }

        efContext.Entry(checkLog).Property(x => x.IsChecked).IsModified = true;
        efContext.Entry(checkLog).Property(x => x.ErrorCount).IsModified = true;
        checkLog.IsChecked = jobIsSuccess ? CaseCheckedStatus.完成 : CaseCheckedStatus.未完成;
        int errorCount = !jobIsSuccess ? context.ErrorCount + 1 : 0;
        checkLog.ErrorCount = errorCount;

        return errorCount;
    }

    private async Task AddNameCheckLog(VerifyResultContext verifyResultContext, CreditCardValidateResult validateResult)
    {
        if (verifyResultContext.檢核姓名檢核成功)
        {
            var nameCheckLog = validateResult.CheckNameRes.SuccessData!.Data!.Reviewer3rd_NameCheckLog;
            await efContext.Reviewer3rd_NameCheckLog.AddAsync(nameCheckLog);
        }
    }
}
