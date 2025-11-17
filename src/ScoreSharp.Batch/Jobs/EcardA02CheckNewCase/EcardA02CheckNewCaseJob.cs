using DocumentFormat.OpenXml.Drawing;
using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("網路進件_卡友檢核新案件")]
public class EcardA02CheckNewCaseJob(
    ScoreSharpContext efContext,
    ILogger<EcardA02CheckNewCaseJob> logger,
    IEcardA02CheckNewCaseRepository repository,
    IEcardA02CheckNewCaseService service
)
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private CommonDBDataDto _commonDBDataDto = new();
    private List<string> errorCase = new();

    [DisplayName("網路進件_卡友檢核新案件 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        logger.LogInformation("網路進件_卡友檢核新案件 - 執行人員：{createBy} 開始執行", createBy);
        logger.LogInformation("Job Context HashCode: {hashCode}", efContext.GetHashCode());

        if (!await _semaphore.WaitAsync(0))
        {
            logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            var systemBatchSet = await efContext.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
            if (systemBatchSet!.EcardA02CheckNewCase_IsEnabled == "N")
            {
                logger.LogInformation("系統參數設定不執行【網路進件_卡友檢核新案件】排程，執行結束");
                return;
            }

            logger.LogInformation("查詢須檢核卡友案件");
            var peddingChecks = await repository.查詢_須檢核卡友案件(systemBatchSet.EcardNotA02CheckNewCase_BatchSize);

            if (!peddingChecks.Any())
            {
                logger.LogInformation("未有卡友案件需要檢核");
                return;
            }

            logger.LogInformation("未檢核卡友案件數量：{peddingCaseCount}", peddingChecks.Count());
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
                    var originalCardholderData = await service.檢核_原持卡人資料(context);
                    validateResult.QueryOriginalCardholderDataRes = originalCardholderData;
                    logger.LogInformation(
                        "原持卡人資料是否成功: {isQueryOriginalCardholderDataSuccess}, 原持卡人資料結果：{isQueryOriginalCardholderDataHit}",
                        validateResult.QueryOriginalCardholderDataRes.IsSuccess,
                        validateResult.QueryOriginalCardholderDataRes.IsSuccess
                    );

                    if (!validateResult.QueryOriginalCardholderDataRes.IsSuccess)
                    {
                        throw new Exception($"原持卡人資料查詢失敗，後續操作停止，案件編號：{context.ApplyNo}，ID：{context.ID}");
                    }

                    Dictionary<string, Task> taskDict = new();

                    if (verifyContext.是否檢核行內Email)
                    {
                        taskDict["InternalEmailSame"] = service.檢核_行內Email資料(context);
                    }

                    if (verifyContext.是否檢核行內手機)
                    {
                        taskDict["InternalMobileSame"] = service.檢核_行內Mobile資料(context);
                    }

                    if (verifyContext.是否檢核929)
                    {
                        taskDict["Query929"] = service.檢核_發查929(context);
                    }

                    if (verifyContext.是否檢核行內IP)
                    {
                        taskDict["CheckInternalIP"] = service.檢核_行內IP相同(context, _commonDBDataDto);
                    }

                    if (verifyContext.是否檢核IP相同)
                    {
                        taskDict["CheckSameIP"] = service.檢核_IP比對相同(context);
                    }

                    if (verifyContext.是否檢核網路電子郵件)
                    {
                        taskDict["CheckSameWebCaseEmail"] = service.檢核_網路電子郵件相同(context);
                    }

                    if (verifyContext.是否檢核網路手機)
                    {
                        taskDict["CheckSameMobile"] = service.檢核_網路手機相同(context);
                    }

                    if (verifyContext.是否檢查短時間ID相同)
                    {
                        taskDict["CheckShortTimeID"] = service.檢核_頻繁ID(context);
                    }

                    if (verifyContext.是否檢核關注名單)
                    {
                        taskDict["CheckFocus"] = service.檢核_查詢關注名單(context);
                    }

                    if (verifyContext.是否檢查重覆進件)
                    {
                        taskDict["CheckRepeatApply"] = service.檢查_是否為重覆進件(context);
                    }

                    // TODO: 檢核黑名單

                    await Task.WhenAll(taskDict.Values);

                    foreach (var task in taskDict)
                    {
                        if (task.Value.IsCompleted)
                        {
                            logger.LogInformation("任務：{taskName} 已完成", task.Key);
                        }

                        switch (task.Key)
                        {
                            case "InternalEmailSame":
                                validateResult.CheckInternalEmailSameRes = (task.Value as Task<CheckCaseRes<CheckInternalEmailSameResult>>)?.Result;
                                logger.LogInformation(
                                    "行內Email執行是否成功:{isInternalEmailSameSuccess},行內Email資料結果：{isInternalEmailSameHit}",
                                    validateResult.CheckInternalEmailSameRes.IsSuccess,
                                    validateResult.CheckInternalEmailSameRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "InternalMobileSame":
                                validateResult.CheckInternalMobileSameRes = (task.Value as Task<CheckCaseRes<CheckInternalMobileSameResult>>)?.Result;
                                logger.LogInformation(
                                    "行內手機執行是否成功:{isInternalMobileSameSuccess},行內手機資料結果：{isInternalMobileSameHit}",
                                    validateResult.CheckInternalMobileSameRes.IsSuccess,
                                    validateResult.CheckInternalMobileSameRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "Query929":
                                validateResult.Check929Res = (task.Value as Task<CheckCaseRes<Check929Info>>)?.Result;
                                logger.LogInformation(
                                    "929執行是否成功:{isQuery929Success},929資料結果：{isQuery929Hit}",
                                    validateResult.Check929Res.IsSuccess,
                                    validateResult.Check929Res?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckInternalIP":
                                validateResult.CheckInternalIPRes = (task.Value as Task<CheckCaseRes<bool>>)?.Result;
                                logger.LogInformation(
                                    "行內IP執行是否成功:{isCheckInternalIPSuccess},行內IP資料結果：{isCheckInternalIPHit}",
                                    validateResult.CheckInternalIPRes.IsSuccess,
                                    validateResult.CheckInternalIPRes?.SuccessData?.Data
                                );
                                break;
                            case "CheckSameIP":
                                validateResult.CheckSameIPRes = (task.Value as Task<CheckCaseRes<CheckSameIP>>)?.Result;
                                logger.LogInformation(
                                    "IP比對執行是否成功:{isCheckSameIPSuccess},IP比對資料結果：{isCheckSameIPHit}",
                                    validateResult.CheckSameIPRes.IsSuccess,
                                    validateResult.CheckSameIPRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckSameWebCaseEmail":
                                validateResult.CheckSameWebCaseEmailRes = (task.Value as Task<CheckCaseRes<CheckSameWebCaseEmail>>)?.Result;
                                logger.LogInformation(
                                    "網路電子郵件執行是否成功:{isCheckSameWebCaseEmailSuccess},網路電子郵件資料結果：{isCheckSameWebCaseEmailHit}",
                                    validateResult.CheckSameWebCaseEmailRes.IsSuccess,
                                    validateResult.CheckSameWebCaseEmailRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckSameMobile":
                                validateResult.CheckSameWebMobileRes = (task.Value as Task<CheckCaseRes<CheckSameWebMobile>>)?.Result;
                                logger.LogInformation(
                                    "網路手機執行是否成功:{isCheckSameMobileSuccess},網路手機資料結果：{isCheckSameMobileHit}",
                                    validateResult.CheckSameWebMobileRes.IsSuccess,
                                    validateResult.CheckSameWebMobileRes?.SuccessData?.Data.是否命中
                                );
                                break;
                            case "CheckShortTimeID":
                                validateResult.CheckShortTimeIDRes = (task.Value as Task<CheckCaseRes<CheckShortTimeID>>)?.Result;
                                logger.LogInformation(
                                    "短時間ID執行是否成功:{isCheckShortTimeIDSuccess},短時間ID資料結果：{isCheckShortTimeIDHit}",
                                    validateResult.CheckShortTimeIDRes.IsSuccess,
                                    validateResult.CheckShortTimeIDRes?.SuccessData?.Data.是否命中
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
                            case "CheckRepeatApply":
                                validateResult.CheckRepeatApplyRes = (task.Value as Task<CheckCaseRes<bool>>)?.Result;
                                logger.LogInformation(
                                    "重覆進件執行是否成功:{isCheckRepeatApplySuccess},重覆進件資料結果：{isCheckRepeatApplyHit}",
                                    validateResult.CheckRepeatApplyRes.IsSuccess,
                                    validateResult.CheckRepeatApplyRes?.SuccessData?.Data
                                );
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("案件編號：{applyNo}，信用卡檢核有誤，請查看System_ErrorLog", context.ApplyNo);
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
                    var verifyResultContext = MapHelper.MapToVerifyResultContext(verifyContext, validateResult);

                    // insert 錯誤紀錄
                    var taskErrorLogs = new[]
                    {
                        validateResult.QueryOriginalCardholderDataRes.ErrorData,
                        validateResult.CheckInternalEmailSameRes.ErrorData,
                        validateResult.CheckInternalMobileSameRes.ErrorData,
                        validateResult.Check929Res.ErrorData,
                        validateResult.CheckInternalIPRes.ErrorData,
                        validateResult.CheckSameIPRes.ErrorData,
                        validateResult.CheckSameWebCaseEmailRes.ErrorData,
                        validateResult.CheckSameWebMobileRes.ErrorData,
                        validateResult.CheckShortTimeIDRes.ErrorData,
                        validateResult.CheckFocusRes.ErrorData,
                        validateResult.CheckRepeatApplyRes.ErrorData,
                    }
                        .Where(error => error != null)
                        .ToList();

                    if (taskErrorLogs.Any())
                    {
                        logger.LogInformation("案件編號：{applyNo} 新增錯誤紀錄", context.ApplyNo);
                        await efContext.System_ErrorLog.AddRangeAsync(taskErrorLogs);
                    }

                    // update main
                    var main = await efContext.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == context.ApplyNo);
                    main.LastUpdateUserId = UserIdConst.SYSTEM;
                    main.LastUpdateTime = DateTime.Now;

                    if (verifyResultContext.是否檢查黑名單成功)
                    {
                        main.BlackListNote = ""; // TODO: 20250812 黑名單
                    }

                    if (verifyResultContext.是否檢查重覆進件成功)
                    {
                        main.IsRepeatApply = verifyResultContext.命中重覆進件 ? "Y" : "N";
                    }

                    // 與MW3原持卡人資料比對當客人為填寫，使用原持卡人資料
                    if (verifyResultContext.是否查詢原持卡人成功)
                    {
                        與原持卡人資料比對當未填寫時補齊資料(main, validateResult.QueryOriginalCardholderDataRes.SuccessData!.Data);
                        var zipCodeErrorLogs = 計算郵遞區號(main);
                        if (zipCodeErrorLogs.Any())
                        {
                            await efContext.System_ErrorLog.AddRangeAsync(zipCodeErrorLogs);
                            verifyResultContext.郵遞區號計算成功 = false;
                        }
                        main.IsOriginalCardholder = "Y";
                    }

                    bool jobIsSuccess = taskErrorLogs.Any() ? false : true;

                    // update handle
                    CardStatus cardStatus = ConvertHelper.ConvertCardStatus(context.CardStatus, jobIsSuccess, verifyResultContext);
                    UpdateHandledCase(context, cardStatus, validateResult);

                    // update financeCheck
                    await UpdateFinanceCheck(context, verifyResultContext, validateResult);

                    // update 銀行追蹤
                    await UpdateBankTrace(context, verifyResultContext, validateResult);

                    // insert into process
                    await InsertProcess(context, verifyResultContext, validateResult, cardStatus, verifyContext);

                    // update checklog
                    var errorCount = UpdateCheckLog(context, verifyResultContext, validateResult, verifyContext, jobIsSuccess);
                    if (errorCount == 2)
                    {
                        新增達錯誤2次案件需寄信(context.ApplyNo);
                    }

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
                    logger.LogError("案件編號：{applyNo}，信用卡儲存資料庫有誤，請查看System_ErrorLog", context.ApplyNo);

                    var errorCount = await service.案件異常處理(
                        context.ApplyNo,
                        SystemErrorLogTypeConst.儲存資料庫有誤,
                        "信用卡申請案件儲存資料庫有誤",
                        ex
                    );

                    if (errorCount == 2)
                    {
                        新增達錯誤2次案件需寄信(context.ApplyNo);
                    }
                    continue;
                }
            }

            if (errorCase.Count > 0)
            {
                logger.LogInformation("達錯誤2次案件需寄信：{errorCase}", errorCase);
                await service.寄信給達錯誤2次案件(errorCase);
            }
        }
        finally
        {
            _semaphore.Release();
        }

        logger.LogInformation("網路進件_卡友檢核新案件 - 執行人員：{createBy} 執行完成", createBy);
    }

    private async Task 查詢通用資料()
    {
        _commonDBDataDto.AddressInfos = await repository.查詢_地址資訊();
        _commonDBDataDto.InternalIPs = await repository.查詢_行內IP();
        _commonDBDataDto.SysParam = await repository.查詢_系統參數();
    }

    private void LogVerifyContext(VerifyContext verifyContext)
    {
        logger.LogInformation("是否執行檢核原持卡人：{isQueryOriginalCardholderData}", verifyContext.是否查詢原持卡人);
        logger.LogInformation("是否執行檢核929：{isCheck929}", verifyContext.是否檢核929);
        logger.LogInformation("是否執行檢核行內Email：{isCheckInternalEmail}", verifyContext.是否檢核行內Email);
        logger.LogInformation("是否執行檢核行內手機：{isCheckInternalMobile}", verifyContext.是否檢核行內手機);
        logger.LogInformation("是否執行檢核IP相同：{isCheckSameIP}", verifyContext.是否檢核IP相同);
        logger.LogInformation("是否執行檢核行內IP：{isCheckEqualInternalIP}", verifyContext.是否檢核行內IP);
        logger.LogInformation("是否執行檢核網路電子郵件：{isCheckSameWebCaseEmail}", verifyContext.是否檢核網路電子郵件);
        logger.LogInformation("是否執行檢核網路手機：{isCheckSameWebCaseMobile}", verifyContext.是否檢核網路手機);
        logger.LogInformation("是否執行檢核關注名單：{isCheckFocus}", verifyContext.是否檢核關注名單);
        logger.LogInformation("是否檢查短時間ID相同：{IsCheckShortTimeID}", verifyContext.是否檢查短時間ID相同);
        logger.LogInformation("是否檢核黑名單：{isBlackList}", verifyContext.是否檢查黑名單);
        logger.LogInformation("是否檢核重覆進件：{isCheckRepeatApply}", verifyContext.是否檢查重覆進件);
    }

    private void 與原持卡人資料比對當未填寫時補齊資料(Reviewer_ApplyCreditCardInfoMain main, QueryOriginalCardholderData origin)
    {
        // logger.LogInformation("與原持卡人資料比對當未填寫時補齊資料");
        // logger.LogInformation("原持卡人資料：{origin}", JsonHelper.序列化物件(MapHelper.MapToCompareMain(origin)));
        // logger.LogInformation("原主檔資料：{main}", JsonHelper.序列化物件(MapHelper.MapToCompareMain(main)));

        main.ENName = string.IsNullOrWhiteSpace(main.ENName) ? origin.EnglishName : main.ENName;
        main.CHName = string.IsNullOrWhiteSpace(main.CHName) ? origin.ChineseName : main.CHName;
        main.BirthDay = string.IsNullOrWhiteSpace(main.BirthDay) ? origin.BirthDate : main.BirthDay;
        main.Bill_ZipCode = string.IsNullOrWhiteSpace(main.Bill_ZipCode) ? origin.BillZip : main.Bill_ZipCode;
        main.LivePhone = string.IsNullOrWhiteSpace(main.LivePhone) ? origin.HomeTel : main.LivePhone;
        main.CompPhone = string.IsNullOrWhiteSpace(main.CompPhone) ? origin.CompanyTel : main.CompPhone;
        main.Mobile = string.IsNullOrWhiteSpace(main.Mobile) ? origin.CellTel : main.Mobile;
        main.CompID = string.IsNullOrWhiteSpace(main.CompID) ? origin.UniformNumber : main.CompID;
        main.EMail = string.IsNullOrWhiteSpace(main.EMail) ? origin.Email : main.EMail;
        main.Sex = main.Sex ??= origin.Sex;

        // Tips: 原卡友資料不會有切割地址，不管從 Ecard 或 MW3 取得
        if (string.IsNullOrWhiteSpace(main.Bill_FullAddr))
        {
            var (postalCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.BillAddr);
            main.Bill_FullAddr = address;
            main.Bill_ZipCode = postalCode;
        }
        if (string.IsNullOrWhiteSpace(main.Reg_FullAddr))
        {
            var (postalCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.HomeAddr);
            main.Reg_FullAddr = address;
            main.Reg_ZipCode = postalCode;
        }

        if (string.IsNullOrWhiteSpace(main.Comp_FullAddr))
        {
            var (postalCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.CompanyAddr);
            main.Comp_FullAddr = address;
            main.Comp_ZipCode = postalCode;
        }

        if (string.IsNullOrWhiteSpace(main.SendCard_FullAddr))
        {
            var (postalCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.SendAddr);
            main.SendCard_FullAddr = address;
            main.SendCard_ZipCode = postalCode;
        }

        main.CompName = string.IsNullOrWhiteSpace(main.CompName) ? origin.CompanyName : main.CompName;
        main.CompJobTitle = string.IsNullOrWhiteSpace(main.CompJobTitle) ? origin.CompanyTitle : main.CompJobTitle;
        main.Education ??= origin.EducateCode;
        main.MarriageState ??= origin.MarriageCode;
        main.CompSeniority ??= origin.ProfessionPeriod;
        main.CurrentMonthIncome ??= origin.MonthlySalary;
        main.CitizenshipCode = string.IsNullOrWhiteSpace(main.CitizenshipCode) ? origin.National : main.CitizenshipCode;
        main.PassportNo = string.IsNullOrWhiteSpace(main.PassportNo) ? origin.Passport : main.PassportNo;
        main.PassportDate = string.IsNullOrWhiteSpace(main.PassportDate) ? origin.PassportDate : main.PassportDate;
        main.ResidencePermitIssueDate = string.IsNullOrWhiteSpace(main.ResidencePermitIssueDate)
            ? origin.ForeignerIssueDate
            : main.ResidencePermitIssueDate;
        main.GraduatedElementarySchool = string.IsNullOrWhiteSpace(main.GraduatedElementarySchool)
            ? origin.SchoolName
            : main.GraduatedElementarySchool;

        if (string.IsNullOrWhiteSpace(main.Live_FullAddr))
        {
            var (postalCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.ContactAddr);
            main.Live_FullAddr = address;
            main.Live_ZipCode = postalCode;
        }
        main.LiveYear ??= origin.ResideNBR;
        main.CompTrade ??= origin.CompTrade;
        main.CompJobLevel ??= origin.CompJobLevel;

        // logger.LogInformation("比對後主檔資料：{main}", JsonHelper.序列化物件(MapHelper.MapToCompareMain(main)));
    }

    private void UpdateHandledCase(CheckA02JobContext context, CardStatus cardStatus, CreditCardValidateResult validateResult)
    {
        int cardLimit = 0;
        if (validateResult.QueryOriginalCardholderDataRes.IsSuccess)
        {
            var crlimit = validateResult.QueryOriginalCardholderDataRes.SuccessData!.Data.CRCrlimit;
            logger.LogInformation("原持卡人資料信用額度：{crlimit}", crlimit);
            if (crlimit > 0)
            {
                cardLimit = (int)Math.Truncate(crlimit);
            }
        }
        var handleInfo = new Reviewer_ApplyCreditCardInfoHandle() { SeqNo = context.HandleSeqNo };
        efContext.Attach(handleInfo);
        efContext.Entry(handleInfo).Property(x => x.CardStatus).IsModified = true;
        efContext.Entry(handleInfo).Property(x => x.CardLimit).IsModified = true;
        handleInfo.CardStatus = cardStatus;
        handleInfo.CardLimit = cardLimit;
    }

    private async Task UpdateBankTrace(CheckA02JobContext context, VerifyResultContext verifyResultContext, CreditCardValidateResult validateResult)
    {
        var bankTrace = new Reviewer_BankTrace()
        {
            ApplyNo = context.ApplyNo,
            ID = context.ID,
            UserType = context.UserType,
        };
        efContext.Attach(bankTrace);

        if (verifyResultContext.是否檢核行內IP成功)
        {
            efContext.Entry(bankTrace).Property(x => x.EqualInternalIP_Flag).IsModified = true;
            var internalIPCheck = validateResult.CheckInternalIPRes.SuccessData!.Data!;
            bankTrace.EqualInternalIP_Flag = internalIPCheck ? "Y" : "N";
        }

        if (verifyResultContext.是否檢核IP相同成功)
        {
            efContext.Entry(bankTrace).Property(x => x.SameIP_Flag).IsModified = true;
            var sameIPCheck = validateResult.CheckSameIPRes.SuccessData!.Data!;
            bankTrace.SameIP_Flag = sameIPCheck.是否命中;

            if (verifyResultContext.命中IP相同)
            {
                await efContext.Reviewer_CheckTrace.AddRangeAsync(sameIPCheck.Reviewer_CheckTraces);
            }
        }

        if (verifyResultContext.是否檢核網路電子郵件成功)
        {
            efContext.Entry(bankTrace).Property(x => x.SameEmail_Flag).IsModified = true;
            var sameEmailCheck = validateResult.CheckSameWebCaseEmailRes.SuccessData!.Data!;
            bankTrace.SameEmail_Flag = sameEmailCheck.是否命中;

            if (verifyResultContext.命中網路電子郵件相同)
            {
                await efContext.Reviewer_CheckTrace.AddRangeAsync(sameEmailCheck.Reviewer_CheckTraces);
            }
        }

        if (verifyResultContext.是否檢核網路手機成功)
        {
            efContext.Entry(bankTrace).Property(x => x.SameMobile_Flag).IsModified = true;
            var sameMobileCheck = validateResult.CheckSameWebMobileRes.SuccessData!.Data!;
            bankTrace.SameMobile_Flag = sameMobileCheck.是否命中;

            if (verifyResultContext.命中網路手機相同)
            {
                await efContext.Reviewer_CheckTrace.AddRangeAsync(sameMobileCheck.Reviewer_CheckTraces);
            }
        }

        if (verifyResultContext.是否檢查短時間ID成功)
        {
            efContext.Entry(bankTrace).Property(x => x.ShortTimeID_Flag).IsModified = true;
            var sameShortTimeIDCheck = validateResult.CheckShortTimeIDRes.SuccessData!.Data!;
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

    private async Task UpdateFinanceCheck(
        CheckA02JobContext context,
        VerifyResultContext verifyResultContext,
        CreditCardValidateResult validateResult
    )
    {
        var financeCheck = new Reviewer_FinanceCheckInfo()
        {
            ApplyNo = context.ApplyNo,
            ID = context.ID,
            UserType = context.UserType,
        };
        efContext.Attach(financeCheck);

        if (verifyResultContext.是否檢核929成功)
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

        if (verifyResultContext.是否檢核關注名單成功)
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
    }

    private void 新增達錯誤2次案件需寄信(string applyNo)
    {
        logger.LogInformation("案件編號：{applyNo}，達錯誤2次案件需寄信", applyNo);
        errorCase.Add(applyNo);
    }

    private async Task InsertProcess(
        CheckA02JobContext context,
        VerifyResultContext verifyResultContext,
        CreditCardValidateResult validateResult,
        CardStatus cardStatus,
        VerifyContext verifyContext
    )
    {
        List<Reviewer_ApplyCreditCardInfoProcess> processes = new();
        string processNote = $"({context.UserType.ToString()}_{context.ID})";

        if (verifyContext.是否查詢原持卡人)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成原持卡人資料查詢,
                validateResult.QueryOriginalCardholderDataRes.StartTime,
                validateResult.QueryOriginalCardholderDataRes.EndTime,
                verifyResultContext.是否查詢原持卡人成功 ? processNote : $"{processNote}({ProcessNoteConst.查詢原持卡人資料錯誤})"
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

        if (verifyContext.是否檢核929)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成929業務狀況查詢,
                validateResult.Check929Res.StartTime,
                validateResult.Check929Res.EndTime,
                verifyResultContext.是否檢核929成功 ? processNote : $"{processNote}({ProcessNoteConst.查詢929業務狀況錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核行內IP)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成行內IP檢核,
                validateResult.CheckInternalIPRes.StartTime,
                validateResult.CheckInternalIPRes.EndTime,
                verifyResultContext.是否檢核行內IP成功 ? processNote : $"{processNote}({ProcessNoteConst.行內IP檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核IP相同)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成相同IP檢核,
                validateResult.CheckSameIPRes.StartTime,
                validateResult.CheckSameIPRes.EndTime,
                verifyResultContext.是否檢核IP相同成功 ? processNote : $"{processNote}({ProcessNoteConst.相同IP檢核錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核網路電子郵件)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成網路相同電子郵件比對,
                validateResult.CheckSameWebCaseEmailRes.StartTime,
                validateResult.CheckSameWebCaseEmailRes.EndTime,
                verifyResultContext.是否檢核網路電子郵件成功 ? processNote : $"{processNote}({ProcessNoteConst.網路相同電子郵件比對錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢核網路手機)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成網路相同手機號碼比對,
                validateResult.CheckSameWebMobileRes.StartTime,
                validateResult.CheckSameWebMobileRes.EndTime,
                verifyResultContext.是否檢核網路手機成功 ? processNote : $"{processNote}({ProcessNoteConst.網路相同手機號碼比對錯誤})"
            );
            processes.Add(process);
        }

        if (verifyContext.是否檢查短時間ID相同)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成頻繁ID檢核,
                validateResult.CheckShortTimeIDRes.StartTime,
                validateResult.CheckShortTimeIDRes.EndTime,
                verifyResultContext.是否檢查短時間ID成功 ? processNote : $"{processNote}({ProcessNoteConst.頻繁ID檢核錯誤})"
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
                verifyResultContext.是否檢核關注名單成功 ? processNote : $"{processNote}({ProcessNoteConst.關注名單1查詢失敗})"
            );
            processes.Add(process);
            var process2 = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成關注名單2查詢,
                validateResult.CheckFocusRes.StartTime,
                validateResult.CheckFocusRes.EndTime,
                verifyResultContext.是否檢核關注名單成功 ? processNote : $"{processNote}({ProcessNoteConst.關注名單2查詢失敗})"
            );
            processes.Add(process2);
        }

        // TODO: 檢查黑名單
        if (verifyContext.是否檢查黑名單)
        {
            var process = MapHelper.MapToProcess(
                context.ApplyNo,
                ProcessConst.完成黑名單查詢,
                DateTime.Now,
                DateTime.Now,
                verifyResultContext.是否檢查黑名單成功 ? $"測試未完成_{processNote}" : $"{processNote}({ProcessNoteConst.黑名單查詢錯誤})"
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
                verifyResultContext.是否檢查重覆進件成功 ? processNote : $"{processNote}({ProcessNoteConst.重覆進件檢核錯誤})"
            );
            processes.Add(process);
        }

        processes.Add(
            MapHelper.MapToProcess(
                context.ApplyNo,
                action: cardStatus.ToString(),
                startTime: DateTime.Now,
                endTime: DateTime.Now,
                notes: verifyResultContext.郵遞區號計算成功 ? string.Empty : "郵遞區號錯誤，請檢查申請書資料"
            )
        );
        await efContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
    }

    private int UpdateCheckLog(
        CheckA02JobContext context,
        VerifyResultContext verifyResultContext,
        CreditCardValidateResult validateResult,
        VerifyContext verifyContext,
        bool jobIsSuccess
    )
    {
        ReviewerPedding_WebApplyCardCheckJobForA02 checkLog = new() { ApplyNo = context.ApplyNo };
        efContext.Attach(checkLog);

        if (verifyContext.是否查詢原持卡人)
        {
            efContext.Entry(checkLog).Property(x => x.IsQueryOriginalCardholderData).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.QueryOriginalCardholderDataLastTime).IsModified = true;
            checkLog.IsQueryOriginalCardholderData = verifyResultContext.是否查詢原持卡人成功
                ? CaseCheckStatus.需檢核_成功
                : CaseCheckStatus.需檢核_失敗;
            checkLog.QueryOriginalCardholderDataLastTime = validateResult.QueryOriginalCardholderDataRes.EndTime;
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

        if (verifyContext.是否檢核929)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheck929).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.Check929LastTime).IsModified = true;
            checkLog.IsCheck929 = verifyResultContext.是否檢核929成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.Check929LastTime = validateResult.Check929Res.EndTime;
        }

        if (verifyContext.是否檢核行內IP)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckEqualInternalIP).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckEqualInternalIPLastTime).IsModified = true;
            checkLog.IsCheckEqualInternalIP = verifyResultContext.是否檢核行內IP成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckEqualInternalIPLastTime = validateResult.CheckInternalIPRes.EndTime;
        }

        if (verifyContext.是否檢核IP相同)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckSameIP).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckSameIPLastTime).IsModified = true;
            checkLog.IsCheckSameIP = verifyResultContext.是否檢核IP相同成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckSameIPLastTime = validateResult.CheckSameIPRes.EndTime;
        }

        if (verifyContext.是否檢核網路電子郵件)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckSameWebCaseEmail).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckSameWebCaseEmailLastTime).IsModified = true;
            checkLog.IsCheckSameWebCaseEmail = verifyResultContext.是否檢核網路電子郵件成功
                ? CaseCheckStatus.需檢核_成功
                : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckSameWebCaseEmailLastTime = validateResult.CheckSameWebCaseEmailRes.EndTime;
        }

        if (verifyContext.是否檢核網路手機)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckSameWebCaseMobile).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckSameWebCaseMobileLastTime).IsModified = true;
            checkLog.IsCheckSameWebCaseMobile = verifyResultContext.是否檢核網路手機成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckSameWebCaseMobileLastTime = validateResult.CheckSameWebMobileRes.EndTime;
        }

        if (verifyContext.是否檢查短時間ID相同)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckShortTimeID).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckShortTimeIDLastTime).IsModified = true;
            checkLog.IsCheckShortTimeID = verifyResultContext.是否檢查短時間ID成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckShortTimeIDLastTime = validateResult.CheckShortTimeIDRes.EndTime;
        }

        if (verifyContext.是否檢核關注名單)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckFocus).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckFocusLastTime).IsModified = true;
            checkLog.IsCheckFocus = verifyResultContext.是否檢核關注名單成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckFocusLastTime = validateResult.CheckFocusRes.EndTime;
        }

        if (verifyContext.是否檢查黑名單)
        {
            efContext.Entry(checkLog).Property(x => x.IsBlackList).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.BlackListLastTime).IsModified = true;
            checkLog.IsBlackList = verifyResultContext.是否檢查黑名單成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.BlackListLastTime = DateTime.Now;
        }

        if (verifyContext.是否檢查重覆進件)
        {
            efContext.Entry(checkLog).Property(x => x.IsCheckRepeatApply).IsModified = true;
            efContext.Entry(checkLog).Property(x => x.CheckRepeatApplyLastTime).IsModified = true;
            checkLog.IsCheckRepeatApply = verifyResultContext.是否檢查重覆進件成功 ? CaseCheckStatus.需檢核_成功 : CaseCheckStatus.需檢核_失敗;
            checkLog.CheckRepeatApplyLastTime = validateResult.CheckRepeatApplyRes.EndTime;
        }

        efContext.Entry(checkLog).Property(x => x.IsChecked).IsModified = true;
        efContext.Entry(checkLog).Property(x => x.ErrorCount).IsModified = true;
        checkLog.IsChecked = jobIsSuccess ? CaseCheckedStatus.完成 : CaseCheckedStatus.未完成;
        int errorCount = !jobIsSuccess ? context.ErrorCount + 1 : 0;
        checkLog.ErrorCount = errorCount;
        return errorCount;
    }

    private List<System_ErrorLog> 計算郵遞區號(Reviewer_ApplyCreditCardInfoMain main)
    {
        List<System_ErrorLog> errorLogs = new();
        if (string.IsNullOrWhiteSpace(main.Reg_ZipCode) && !string.IsNullOrWhiteSpace(main.Reg_FullAddr))
        {
            var result = service.計算_郵遞區號(main.Reg_FullAddr, _commonDBDataDto, main.ApplyNo);
            if (result.IsSuccess)
            {
                main.Reg_ZipCode = result.SuccessData.Data;
            }
            else
            {
                errorLogs.Add(result.ErrorData);
            }
        }

        if (string.IsNullOrWhiteSpace(main.SendCard_ZipCode) && !string.IsNullOrWhiteSpace(main.SendCard_FullAddr))
        {
            var result = service.計算_郵遞區號(main.SendCard_FullAddr, _commonDBDataDto, main.ApplyNo);
            if (result.IsSuccess)
            {
                main.SendCard_ZipCode = result.SuccessData.Data;
            }
            else
            {
                errorLogs.Add(result.ErrorData);
            }
        }

        if (string.IsNullOrWhiteSpace(main.Bill_ZipCode) && !string.IsNullOrWhiteSpace(main.Bill_FullAddr))
        {
            var result = service.計算_郵遞區號(main.Bill_FullAddr, _commonDBDataDto, main.ApplyNo);
            if (result.IsSuccess)
            {
                main.Bill_ZipCode = result.SuccessData.Data;
            }
            else
            {
                errorLogs.Add(result.ErrorData);
            }
        }

        // Tips: 居住地址郵遞區號不必填寫
        if (string.IsNullOrWhiteSpace(main.Live_ZipCode) && !string.IsNullOrWhiteSpace(main.Live_FullAddr))
        {
            var result = service.計算_郵遞區號(main.Live_FullAddr, _commonDBDataDto, main.ApplyNo);
            if (result.IsSuccess)
            {
                main.Live_ZipCode = result.SuccessData.Data;
            }
        }

        // Tips: 公司地址郵遞區號不必填寫
        if (string.IsNullOrWhiteSpace(main.Comp_ZipCode) && !string.IsNullOrWhiteSpace(main.Comp_FullAddr))
        {
            var result = service.計算_郵遞區號(main.Comp_FullAddr, _commonDBDataDto, main.ApplyNo);
            if (result.IsSuccess)
            {
                main.Comp_ZipCode = result.SuccessData.Data;
            }
        }

        return errorLogs;
    }
}
