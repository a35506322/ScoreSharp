using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.CompleteMonthlyIncomeByApplyNo;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Extenstions;
using ScoreSharp.Common.Helpers.KYC;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome
{
    public partial class ReviewerMonthlyIncomeController
    {
        /// <summary>
        /// 完成月收入簽核資料
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerMonthlyIncome/CompleteMonthlyIncomeByApplyNo/1234567890
        ///
        /// 此API僅將卡片狀態更新為完成月收入確認
        /// 需要檢核資料欄位是否必填
        /// 1. 現職月收入需必填
        ///
        /// 以及卡片狀態是否正確
        /// 1. 卡片狀態需為紙本件月收入確認或待月收入預審
        ///
        /// 徵信代碼不為必填，系統審查當發現不為退件時，自然踢去人工徵審
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(完成月收入確認_2000_ResEx),
            typeof(完成月收入確認_卡片狀態錯誤_4003_ResEx),
            typeof(完成月收入確認欄位必填_4003_ResEx),
            typeof(非卡友案件需有姓名檢核紀錄_4003_ResEx),
            typeof(完成月收入確認_KYC入檔發查失敗_5002_ResEx),
            typeof(完成月收入確認_資料檢核失敗_4006_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("CompleteMonthlyIncomeByApplyNo")]
        public async Task<IResult> CompleteMonthlyIncomeByApplyNo([FromRoute] string applyNo) =>
            Results.Ok(await _mediator.Send(new Command(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.CompleteMonthlyIncomeByApplyNo
{
    public record Command(string applyNo) : IRequest<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>;

    public class Handler(
        ScoreSharpContext context,
        IJWTProfilerHelper _jwtHelper,
        IMW3APAPIAdapter _mw3Adapter,
        IConfiguration _configuration,
        IReviewerValidateHelper reviewerValidateHelper
    ) : IRequestHandler<Command, ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>
    {
        public async Task<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            CaseDataBundle bundle = await 取得案件相關資料(request.applyNo);

            if (bundle.Main is null)
                return ApiResponseHelper.NotFound<CompleteMonthlyIncomeByApplyNoResponse>(null, request.applyNo);

            if (bundle.Handles.Any(x => !(x.CardStatus == CardStatus.紙本件_待月收入預審 || x.CardStatus == CardStatus.網路件_待月收入預審)))
                return ApiResponseHelper.BusinessLogicFailed<CompleteMonthlyIncomeByApplyNoResponse>(
                    null,
                    "卡片狀態錯誤，只能進行紙本件月收入確認或待月收入預審"
                );

            var validationResult = 資料檢核(bundle);
            if (!validationResult.IsValid)
            {
                // 區分 type=0（驗證失敗）和 type=1（檢核尚未執行）的錯誤
                var type0Errors = validationResult
                    .Errors.Where(x => x.Type == ReviewerValidationErrorType.ValidationFailure)
                    .OrderBy(x => x.Type)
                    .ToList();
                var type1Errors = validationResult
                    .Errors.Where(x => x.Type == ReviewerValidationErrorType.MissingCheck)
                    .OrderBy(x => x.Type)
                    .ToList();

                // type=0 優先回傳，使用 RtnCode 4000
                if (type0Errors.Any())
                {
                    var errorMessages = string.Join(Environment.NewLine, type0Errors.Select(x => x.Message));
                    return ApiResponseHelper.BusinessLogicFailed<CompleteMonthlyIncomeByApplyNoResponse>(null, errorMessages);
                }

                // type=1 轉換為 RetryCheck 列表，使用 RtnCode 4006
                if (type1Errors.Any())
                {
                    var retryChecks = type1Errors.Select(x =>
                        ReviewerMapHelper.MapToRetryCheck(x, bundle.Main.CHName, bundle.Supplementary?.CHName ?? string.Empty)
                    );

                    return ApiResponseHelper.ReviewerBusinessLogicFailed<CompleteMonthlyIncomeByApplyNoResponse>(
                        data: new CompleteMonthlyIncomeByApplyNoResponse() { RetryChecks = retryChecks.ToList() },
                        message: "請再次執行查詢檢核資料"
                    );
                }
            }

            DateTime now = DateTime.Now;
            CaseContext caseContext = await CaseContext(now, bundle);
            KYCProcessResult kycProcessResult = await 執行KYC流程(caseContext: caseContext, caseDataBundle: bundle, processTime: now);

            // Tips: KYC 流程失敗且需要回傳錯誤訊息
            if (!kycProcessResult.IsSuccess && !kycProcessResult.RequiresRetry)
            {
                // Tips: KYC 流程失敗，但有寫入KYC查詢Log，所以需要寫入資料庫
                await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(kycProcessResult.Processes);
                await context.Reviewer3rd_KYCQueryLog.AddRangeAsync(kycProcessResult.KYCQueryLogs);
                await context.SaveChangesAsync(cancellationToken);

                return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(
                    data: new CompleteMonthlyIncomeByApplyNoResponse()
                    {
                        ApplyNo = caseContext.申請書編號,
                        KYC_RtnCode = kycProcessResult.RtnCode,
                        KYC_Message = kycProcessResult.Message,
                        KYC_RiskLevel = kycProcessResult.RiskLevel,
                        KYC_QueryTime = kycProcessResult.QueryTime,
                        KYC_Exception = kycProcessResult.Exception,
                    },
                    checkThirdPartyApiName: caseContext.案件種類 == 案件種類.紙本件_原卡友 ? "簡易查詢風險等級" : "KYC入檔"
                );
            }

            var (processes, cardRecords) = UpdateHandlesProcess(caseContext, bundle, now);
            UpdateApplyCreditCardInfoMainDatas(bundle, now);

            // 儲存所有變更
            await SaveAllChangeDatasAsync(kycProcessResult, processes, cardRecords, cancellationToken);

            return ApiResponseHelper.Success(
                data: new CompleteMonthlyIncomeByApplyNoResponse()
                {
                    ApplyNo = bundle.Main.ApplyNo,
                    KYC_RtnCode = caseContext.KYCAPIResult.KYC_RtnCode,
                    KYC_Message = caseContext.KYCAPIResult.KYC_Message,
                    KYC_RiskLevel = caseContext.KYCAPIResult.KYC_RiskLevel,
                    KYC_QueryTime = now,
                    KYC_Exception = caseContext.KYCAPIResult.ExceptionMessage,
                },
                message: 取得回傳訊息(caseContext)
            );
        }

        private async Task SaveAllChangeDatasAsync(
            KYCProcessResult kycProcessResult,
            List<Reviewer_ApplyCreditCardInfoProcess> processes,
            List<Reviewer_CardRecord> cardRecords,
            CancellationToken cancellationToken
        )
        {
            if (kycProcessResult.Processes.Any())
                await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(kycProcessResult.Processes, cancellationToken);

            if (kycProcessResult.KYCQueryLogs.Any())
                await context.Reviewer3rd_KYCQueryLog.AddRangeAsync(kycProcessResult.KYCQueryLogs, cancellationToken);

            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes, cancellationToken);
            await context.Reviewer_CardRecord.AddRangeAsync(cardRecords, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        private void UpdateApplyCreditCardInfoMainDatas(CaseDataBundle bundle, DateTime updateTime)
        {
            /*
            Tips:
            如果所有卡片狀態都是完成月收入確認，則清除派案指定人員，
            如果不是代表還有事情未做完保留當前經辦，
            例如 KYC 可能因主機Timeout 需要重新入檔，
            待排程跑完，會將狀態改為待月收入預審
            此時經辦就看的到
            */
            if (bundle.Handles.All(x => x.CardStatus == CardStatus.完成月收入確認 || x.CardStatus == CardStatus.網路件_卡友_完成KYC入檔作業))
            {
                bundle.Main.CurrentHandleUserId = null;
                bundle.Main.PreviousHandleUserId = null;
            }

            // 需求單1140627 : 檢核「是否同意提供資料於第三人行銷」欄位是否「空白」，如為「空白」，補上「否」。
            if (string.IsNullOrEmpty(bundle.Main.IsAgreeMarketing))
            {
                bundle.Main.IsAgreeMarketing = "N";
            }

            bundle.Main.LastUpdateUserId = _jwtHelper.UserId;
            bundle.Main.LastUpdateTime = updateTime;
        }

        private (List<Reviewer_ApplyCreditCardInfoProcess> processes, List<Reviewer_CardRecord> cardRecords) UpdateHandlesProcess(
            CaseContext caseContext,
            CaseDataBundle caseDataBundle,
            DateTime updateTime
        )
        {
            List<Reviewer_ApplyCreditCardInfoProcess> processes = [];
            List<Reviewer_CardRecord> cardRecords = [];

            foreach (var handle in caseDataBundle.Handles)
            {
                handle.CardStatus = ConvertToCardStatus(caseContext);
                handle.CaseChangeAction = null;
                handle.SupplementReasonCode = null;
                handle.OtherSupplementReason = null;
                handle.SupplementNote = null;
                handle.SupplementSendCardAddr = null;
                handle.RejectionReasonCode = null;
                handle.OtherRejectionReason = null;
                handle.RejectionNote = null;
                handle.RejectionSendCardAddr = null;
                handle.WithdrawalNote = null;
                handle.MonthlyIncomeCheckUserId = _jwtHelper.UserId;
                handle.MonthlyIncomeTime = updateTime;

                string userType = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";
                string notes = handle.CardStatus is CardStatus.KYC入檔作業_網路件待月收入預審 or CardStatus.KYC入檔作業_紙本件待月收入預審
                    ? $"排入KYC Retry 排程;({userType}_{handle.ApplyCardType})"
                    : $"({userType}_{handle.ApplyCardType})";

                var process = new Reviewer_ApplyCreditCardInfoProcess
                {
                    ApplyNo = caseContext.申請書編號,
                    Process = handle.CardStatus.ToString(),
                    StartTime = updateTime,
                    EndTime = updateTime,
                    ProcessUserId = _jwtHelper.UserId,
                    Notes = notes,
                };
                processes.Add(process);

                var cardRecord = new Reviewer_CardRecord
                {
                    ApplyNo = caseContext.申請書編號,
                    CardStatus = handle.CardStatus,
                    CardLimit = null,
                    HandleSeqNo = handle.SeqNo,
                    ApproveUserId = _jwtHelper.UserId,
                };
                cardRecords.Add(cardRecord);
            }

            return (processes, cardRecords);
        }

        private async Task<KYCProcessResult> 執行KYC流程(CaseContext caseContext, CaseDataBundle caseDataBundle, DateTime processTime)
        {
            if (!caseContext.是否有徵信代碼)
                return KYCProcessResult.無需執行KYC();

            return caseContext.案件種類 switch
            {
                案件種類.紙本件_原卡友 => await 執行查詢KYC風險等級流程(caseContext, caseDataBundle, processTime),
                案件種類.網路件_原卡友 => await 執行網路件原卡友KYC入檔流程(caseContext, caseDataBundle, processTime),
                案件種類.紙本件_非卡友 or 案件種類.網路件_非卡友 => await 執行非卡友KYC入檔流程(caseContext, caseDataBundle, processTime),
                _ => throw new ArgumentException("invalid 案件種類"),
            };
        }

        private async Task<KYCProcessResult> 執行非卡友KYC入檔流程(CaseContext caseContext, CaseDataBundle caseDataBundle, DateTime processTime)
        {
            if (caseContext.是否系統維護)
                return KYCProcessResult.系統維護中();

            var nameCheck = await context
                .Reviewer3rd_NameCheckLog.AsNoTracking()
                .Where(x => x.ApplyNo == caseContext.申請書編號 && x.ID == caseDataBundle.Main.ID)
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefaultAsync();

            if (nameCheck is null)
                return new KYCProcessResult { IsSuccess = false, Message = "非卡友案件需有姓名檢核紀錄，請重新執行" };

            var kycRequest = MapTo非卡友SyncKycRequest(caseDataBundle.Main, nameCheck, caseDataBundle.MainFinanceCheck);
            var kycResponse = await 呼叫KYC入檔API(kycRequest);

            KYCAPIResult kYCAPIResult = new()
            {
                呼叫是否成功 = kycResponse.IsSuccess,
                KYC_RtnCode = kycResponse.Data.Info.Result.Data.KycCode,
                KYC_Rc = kycResponse.Data.Info.Rc,
                KYC_Rc2 = kycResponse.Data.Info.Rc2,
                KYC_Message = kycResponse.Data.Info.Result.Data.ErrMsg,
                KYC_RiskLevel = kycResponse.Data.Info.Result.Data.RaRank,
                ExceptionMessage = kycResponse.ErrorMessage,
            };
            caseContext.KYCAPIResult = kYCAPIResult;

            // Process
            Reviewer_ApplyCreditCardInfoProcess syncKycProcess = MapToProcess(
                caseContext: caseContext,
                caseDataBundle: caseDataBundle,
                process: ProcessConst.完成KYC入檔,
                startTime: processTime,
                userId: _jwtHelper.UserId
            );

            // KycQueryLog
            Reviewer3rd_KYCQueryLog syncKycQueryLog = MapToKycQueryLog(
                caseContext: caseContext,
                caseDataBundle: caseDataBundle,
                queryTime: processTime,
                userId: _jwtHelper.UserId,
                request: kycRequest,
                response: kycResponse
            );

            // 更新 Main 與 FinanceCheckInfo 資料
            caseDataBundle.Main.AMLRiskLevel = kYCAPIResult.KYC_RiskLevel;
            UpdateFinanceCheckInfo(caseDataBundle.MainFinanceCheck, kYCAPIResult, processTime);

            bool 是否需要重新執行 = kYCAPIResult.呼叫是否成功 && kYCAPIResult.KYC_RtnCode == MW3RtnCodeConst.入檔KYC_主機TimeOut;

            // 若沒有錯誤 => 變更加強審核狀態
            if (!kYCAPIResult.KYC入檔作業_是否回傳錯誤訊息() && !是否需要重新執行)
                處理加強審核(caseDataBundle.MainFinanceCheck, kYCAPIResult, caseDataBundle.Main);

            return new KYCProcessResult
            {
                IsSuccess = !kYCAPIResult.KYC入檔作業_是否回傳錯誤訊息(),
                RtnCode = kYCAPIResult.KYC_RtnCode,
                Message = kYCAPIResult.KYC_Message,
                RiskLevel = kYCAPIResult.KYC_RiskLevel,
                QueryTime = processTime,
                Exception = kYCAPIResult.ExceptionMessage,
                RequiresRetry = 是否需要重新執行,
                Processes = [syncKycProcess],
                KYCQueryLogs = [syncKycQueryLog],
            };
        }

        private async Task<KYCProcessResult> 執行網路件原卡友KYC入檔流程(CaseContext caseContext, CaseDataBundle caseDataBundle, DateTime processTime)
        {
            if (caseContext.是否系統維護)
                return KYCProcessResult.系統維護中();

            var kycRequest = MapTo原卡友SyncKycRequest(caseDataBundle.Main);
            var kycResponse = await 呼叫KYC入檔API(kycRequest);

            KYCAPIResult kYCAPIResult = new()
            {
                呼叫是否成功 = kycResponse.IsSuccess,
                KYC_RtnCode = kycResponse.Data.Info.Result.Data.KycCode,
                KYC_Rc = kycResponse.Data.Info.Rc,
                KYC_Rc2 = kycResponse.Data.Info.Rc2,
                KYC_Message = kycResponse.Data.Info.Result.Data.ErrMsg,
                KYC_RiskLevel = kycResponse.Data.Info.Result.Data.RaRank,
                ExceptionMessage = kycResponse.ErrorMessage,
            };
            caseContext.KYCAPIResult = kYCAPIResult;

            // Process
            Reviewer_ApplyCreditCardInfoProcess syncKycProcess = MapToProcess(
                caseContext: caseContext,
                caseDataBundle: caseDataBundle,
                process: ProcessConst.完成KYC入檔,
                startTime: processTime,
                userId: _jwtHelper.UserId
            );

            // KycQueryLog
            Reviewer3rd_KYCQueryLog syncKycQueryLog = MapToKycQueryLog(
                caseContext: caseContext,
                caseDataBundle: caseDataBundle,
                queryTime: processTime,
                userId: _jwtHelper.UserId,
                request: kycRequest,
                response: kycResponse
            );

            // 更新 Main 與 FinanceCheckInfo 資料
            caseDataBundle.Main.AMLRiskLevel = kYCAPIResult.KYC_RiskLevel;
            UpdateFinanceCheckInfo(caseDataBundle.MainFinanceCheck, kYCAPIResult, processTime);

            bool 是否需要重新執行 = kYCAPIResult.呼叫是否成功 && kYCAPIResult.KYC_RtnCode == MW3RtnCodeConst.入檔KYC_主機TimeOut;

            // 若沒有錯誤 => 變更加強審核狀態為不需檢核
            if (!kYCAPIResult.KYC入檔作業_是否回傳錯誤訊息() && !是否需要重新執行)
                caseDataBundle.MainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;

            return new KYCProcessResult
            {
                IsSuccess = !kYCAPIResult.KYC入檔作業_是否回傳錯誤訊息(),
                RtnCode = kYCAPIResult.KYC_RtnCode,
                Message = kYCAPIResult.KYC_Message,
                RiskLevel = kYCAPIResult.KYC_RiskLevel,
                QueryTime = processTime,
                Exception = kYCAPIResult.ExceptionMessage,
                RequiresRetry = 是否需要重新執行,
                Processes = [syncKycProcess],
                KYCQueryLogs = [syncKycQueryLog],
            };
        }

        private async Task<KYCProcessResult> 執行查詢KYC風險等級流程(CaseContext caseContext, CaseDataBundle caseDataBundle, DateTime processTime)
        {
            var kycRequest = MapTo紙本件原卡友Request(caseDataBundle.Main.ID);
            var kycResponse = await 呼叫查詢KYC風險等級(kycRequest);

            KYCAPIResult kYCAPIResult = new()
            {
                呼叫是否成功 = kycResponse.IsSuccess,
                KYC_RtnCode = kycResponse.Data.Info.Result.Data.KYC,
                KYC_Rc = kycResponse.Data.Info.Rc,
                KYC_Rc2 = kycResponse.Data.Info.Rc2,
                KYC_Message = kycResponse.Data.Info.Result.Data.KycMsg,
                KYC_RiskLevel = kycResponse.Data.Info.Result.Data.Data.RARank,
                ExceptionMessage = kycResponse.ErrorMessage,
            };
            caseContext.KYCAPIResult = kYCAPIResult;

            // Process
            Reviewer_ApplyCreditCardInfoProcess syncKycProcess = MapToProcess(
                caseContext: caseContext,
                caseDataBundle: caseDataBundle,
                process: ProcessConst.完成KYC入檔,
                startTime: processTime,
                userId: _jwtHelper.UserId
            );

            // KycQueryLog
            Reviewer3rd_KYCQueryLog syncKycQueryLog = MapToKycQueryLog(
                caseContext: caseContext,
                caseDataBundle: caseDataBundle,
                queryTime: processTime,
                userId: _jwtHelper.UserId,
                request: kycRequest,
                response: kycResponse
            );

            // 更新 Main 與 FinanceCheckInfo 資料
            caseDataBundle.Main.AMLRiskLevel = kYCAPIResult.KYC_RiskLevel;
            UpdateFinanceCheckInfo(caseDataBundle.MainFinanceCheck, kYCAPIResult, processTime);

            return new KYCProcessResult
            {
                IsSuccess = !kYCAPIResult.簡單查詢風險等級_是否回傳錯誤訊息(),
                RtnCode = kYCAPIResult.KYC_RtnCode,
                Message = kYCAPIResult.KYC_Message,
                RiskLevel = kYCAPIResult.KYC_RiskLevel,
                QueryTime = processTime,
                Exception = kYCAPIResult.ExceptionMessage,
                RequiresRetry = false,
                Processes = [syncKycProcess],
                KYCQueryLogs = [syncKycQueryLog],
            };
        }

        private void 處理加強審核(Reviewer_FinanceCheckInfo financeCheck, KYCAPIResult kYCAPIResult, Reviewer_ApplyCreditCardInfoMain main)
        {
            HashSet<string> highKYCRiskLevel = [KYCRiskLevelConst.高風險, KYCRiskLevelConst.中風險];

            if (highKYCRiskLevel.Contains(kYCAPIResult.KYC_RiskLevel))
            {
                financeCheck.KYC_StrongReStatus = KYCStrongReStatus.未送審;
                financeCheck.KYC_StrongReDetailJson = JsonHelper.序列化物件(
                    KYCHelper.產生KYC加強審核執行表(
                        version: financeCheck.KYC_StrongReVersion,
                        id: main.ID,
                        name: main.CHName,
                        riskLevel: kYCAPIResult.KYC_RiskLevel
                    )
                );
            }
            else
            {
                financeCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;
            }
        }

        private void UpdateFinanceCheckInfo(Reviewer_FinanceCheckInfo mainFinanceCheck, KYCAPIResult kYCAPIResult, DateTime processTime)
        {
            mainFinanceCheck.AMLRiskLevel = kYCAPIResult.KYC_RiskLevel;
            mainFinanceCheck.KYC_RtnCode = kYCAPIResult.KYC_RtnCode;
            mainFinanceCheck.KYC_QueryTime = processTime;
            mainFinanceCheck.KYC_Message = kYCAPIResult.KYC_Message;
            mainFinanceCheck.KYC_Handler = null;
            mainFinanceCheck.KYC_Handler_SignTime = null;
            mainFinanceCheck.KYC_Reviewer = null;
            mainFinanceCheck.KYC_Reviewer_SignTime = null;
            mainFinanceCheck.KYC_KYCManager = null;
            mainFinanceCheck.KYC_KYCManager_SignTime = null;
            mainFinanceCheck.KYC_StrongReDetailJson = null;
            mainFinanceCheck.KYC_Suggestion = null;
            mainFinanceCheck.KYC_StrongReStatus = null;
        }

        private ReviewerValidationResult 資料檢核(CaseDataBundle bundle)
        {
            var main = bundle.Main;
            var supplementary = bundle.Supplementary;
            var handles = bundle.Handles;
            var bankTarce = bundle.BankTrace;
            var mainFinanceCheck = bundle.MainFinanceCheck;
            var supplementaryFinanceCheck = bundle.SupplementaryFinanceCheck;

            var caseInfo = ReviewerMapHelper.MapToCaseInfoContext(main);
            var handleStatuses = bundle
                .Handles.Select(x => ReviewerMapHelper.MapToHandleInfoContext(x, ReviewAction.完成月收入確認, bundle.CardDict))
                .ToList();
            var mainData = ReviewerMapHelper.MapToReviewerMainDataContext(main, handleStatuses);
            var mainAddressData = ReviewerMapHelper.MapToReviewerMainAddressContext(main);

            var bankTarceData = ReviewerMapHelper.MapToReviewerMainBankTraceContext(bankTarce, main.CHName);
            var mainFinanceCheckDataContext = ReviewerMapHelper.MapToReviewerFinanceCheckMainContext(mainFinanceCheck, main.NameChecked, main.CHName);

            var validationContext = new ReviewerValidationContext
            {
                CaseInfo = caseInfo,
                MainData = mainData,
                MainBankTrace = bankTarceData,
                MainFinanceCheck = mainFinanceCheckDataContext,
                HandleStatuses = handleStatuses,
                MainAddress = mainAddressData,
            };

            if (supplementary != null)
            {
                var supplementaryData = ReviewerMapHelper.MapToReviewerSupplementaryDataContext(supplementary, handleStatuses);
                var supplementaryFinanceCheckDataContext = ReviewerMapHelper.MapToReviewerFinanceCheckSupplementaryContext(
                    supplementaryFinanceCheck,
                    supplementary.NameChecked,
                    supplementary.CHName
                );

                validationContext.SupplementaryData = supplementaryData;
                validationContext.SupplementaryFinanceCheck = supplementaryFinanceCheckDataContext;
                validationContext.SupplementaryAddress = ReviewerMapHelper.MapToReviewerSupplementaryAddressContext(supplementary);
            }

            var result = reviewerValidateHelper.Validate(
                context: validationContext,
                rules: ReviewerValidationRuleSet.DataFormat
                    | ReviewerValidationRuleSet.BankTrace
                    | ReviewerValidationRuleSet.FinanceCheck
                    | ReviewerValidationRuleSet.Addresses
            );

            return result;
        }

        private async Task<CaseDataBundle> 取得案件相關資料(string applyNo)
        {
            var handles = await context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo).ToListAsync();
            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);
            var financeChecks = await context.Reviewer_FinanceCheckInfo.Where(x => x.ApplyNo == applyNo).ToListAsync();
            var supplementary = await context
                .Reviewer_ApplyCreditCardInfoSupplementary.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplyNo == applyNo);
            var bankTarce = await context
                .Reviewer_BankTrace.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);
            var cardDict = await context.SetUp_Card.AsNoTracking().ToDictionaryAsync(x => x.CardCode, x => x.IsCITSCard);

            var mainFinanceCheck = financeChecks.FirstOrDefault(x => x.UserType == UserType.正卡人);
            var supplementaryFinanceCheck = financeChecks.FirstOrDefault(x => x.UserType == UserType.附卡人);

            return new CaseDataBundle
            {
                Main = main,
                MainFinanceCheck = mainFinanceCheck,
                SupplementaryFinanceCheck = supplementaryFinanceCheck,
                Handles = handles,
                Supplementary = supplementary,
                BankTrace = bankTarce,
                CardDict = cardDict,
            };
        }

        private Reviewer3rd_KYCQueryLog MapToKycQueryLog(
            CaseContext caseContext,
            CaseDataBundle caseDataBundle,
            DateTime queryTime,
            string userId,
            object request,
            object response
        )
        {
            var kycResult = caseContext.KYCAPIResult;
            var apiName = caseContext.案件種類 == 案件種類.紙本件_原卡友 ? "KYC00QNBDRA" : "KYC00CREDIT";

            List<string> successKycCode = [MW3RtnCodeConst.入檔KYC_成功, MW3RtnCodeConst.簡單查詢風險等級_成功];
            var kycLastSendStatus =
                kycResult.呼叫是否成功 && successKycCode.Contains(kycResult.KYC_RtnCode) ? KYCLastSendStatus.不需發送 : KYCLastSendStatus.等待;

            return new()
            {
                ApplyNo = caseContext.申請書編號,
                CardStatus = string.Join("/", caseDataBundle.Handles.Select(x => x.CardStatus.ToName())),
                ID = caseDataBundle.Main.ID,
                Request = JsonHelper.序列化物件(request),
                Response = JsonHelper.序列化物件(response),
                KYCCode = kycResult.KYC_RtnCode,
                KYCRank = kycResult.KYC_RiskLevel,
                KYCMsg = kycResult.KYC_Message,
                AddTime = queryTime,
                KYCLastSendStatus = kycLastSendStatus,
                KYCLastSendTime = null,
                CurrentHandler = userId,
                APIName = apiName,
                Source = "CompleteMonthlyIncomeByApplyNo",
            };
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(
            CaseContext caseContext,
            CaseDataBundle caseDataBundle,
            string process,
            DateTime startTime,
            string userId
        )
        {
            var kycReuslt = caseContext.KYCAPIResult;
            var isSuccessStr = kycReuslt.呼叫是否成功 ? "成功" : "失敗";
            var userTypeStr = caseDataBundle.Main.UserType == UserType.正卡人 ? "正卡人" : "附卡人";
            var apiCHName = caseContext.案件種類 == 案件種類.紙本件_原卡友 ? "查詢簡易風險等級" : "KYC入檔";

            return new()
            {
                ApplyNo = caseContext.申請書編號,
                Process = process,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes =
                    $"KYC串接狀態: {isSuccessStr}; KYC Api: {apiCHName}; KYC 代碼: {kycReuslt.KYC_RtnCode}; KYC Level: {kycReuslt.KYC_RiskLevel}; KYC Message: {kycReuslt.KYC_Message}; ({userTypeStr}_{caseDataBundle.Main.ID})",
                ProcessUserId = userId,
            };
        }

        private async Task<CaseContext> CaseContext(DateTime now, CaseDataBundle bundle)
        {
            CaseContext syncKYCContext = new();

            syncKYCContext.案件種類 = MapTo案件種類(bundle.Main);

            // 檢查系統維護
            var sysParam = await context.SysParamManage_SysParam.FirstAsync();
            if (!sysParam.KYCFixStartTime.HasValue || !sysParam.KYCFixEndTime.HasValue)
                syncKYCContext.是否系統維護 = false;
            else if (sysParam.KYCFixStartTime <= now && now <= sysParam.KYCFixEndTime)
                syncKYCContext.是否系統維護 = true;
            else
                syncKYCContext.是否系統維護 = false;

            // 檢查是否有徵信代碼
            if (!string.IsNullOrEmpty(bundle.Handles.First().CreditCheckCode))
                syncKYCContext.是否有徵信代碼 = true;
            else
                syncKYCContext.是否有徵信代碼 = false;

            syncKYCContext.申請書編號 = bundle.Main.ApplyNo;

            return syncKYCContext;
        }

        private 案件種類 MapTo案件種類(Reviewer_ApplyCreditCardInfoMain main) =>
            (main.Source == Source.紙本, main.IsOriginalCardholder == "Y") switch
            {
                (true, true) => 案件種類.紙本件_原卡友,
                (true, false) => 案件種類.紙本件_非卡友,
                (false, true) => 案件種類.網路件_原卡友,
                (false, false) => 案件種類.網路件_非卡友,
            };

        private SyncKycMW3Info MapTo非卡友SyncKycRequest(
            Reviewer_ApplyCreditCardInfoMain main,
            Reviewer3rd_NameCheckLog? nameCheck,
            Reviewer_FinanceCheckInfo? financeCheck
        )
        {
            bool 國籍是台灣 = main.CitizenshipCode == "TW";
            bool 出生地是台灣 = main.BirthCitizenshipCode == BirthCitizenshipCode.中華民國;
            bool AML風險等級是高風險 = main.AMLRiskLevel == "H";

            var AML職業類別其他代碼 = _configuration.GetSection($"ValidationSetting:AMLProfessionOther_{main.AMLProfessionCode_Version}").Value;
            bool AML職業別是其他 = main.AMLProfessionCode == AML職業類別其他代碼;

            var 主要所得及資金來源其他代碼 = _configuration.GetSection($"ValidationSetting:MainIncomeAndFundOther").Value;
            bool 所得與資金來源是其他 = main.MainIncomeAndFundCodes == 主要所得及資金來源其他代碼;

            var syncKycMW3Info = new SyncKycMW3Info
            {
                BRRFlag = KYCRequestConst.信用卡業務風險因子_透過線上方式申請信用卡業務, // TODO: 20250730 再確認BRRFlag計算
                ID = KYCHelper.轉換ID(main.ID),
                CHName = main.CHName.Trim().ToFullWidth(),
                ENName = main.ENName.Trim().ToFullWidth(),
                BirthDay = main.BirthDay.ToWesternDate(outputFormat: "yyyy-MM-dd"),
                NameCheckedReasonCodes = string.IsNullOrWhiteSpace(main.NameCheckedReasonCodes)
                    ? KYCRequestConst.姓名檢核定義值_無
                    : main.NameCheckedReasonCodes,
                ISRCAForCurrentPEP = main.ISRCAForCurrentPEP ?? string.Empty,
                ResignPEPKind = main.ResignPEPKind.HasValue ? ((int)main.ResignPEPKind.Value).ToString() : string.Empty,
                PEPRange = main.PEPRange.HasValue ? ((int)main.PEPRange).ToString() : string.Empty,
                IsCurrentPositionRelatedPEPPosition = main.IsCurrentPositionRelatedPEPPosition ?? string.Empty,
                NameCheckStartTime = nameCheck.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                NameCheckResponseResult = nameCheck.ResponseResult == "Y" ? KYCRequestConst.姓名檢核命中_是 : KYCRequestConst.姓名檢核命中_否,
                NameCheckRcPoint = nameCheck.RcPoint.ToString(),
                NameCheckAMLId = nameCheck.AMLId,
                Nation = 國籍是台灣 ? KYCRequestConst.國籍_中華民國 : KYCRequestConst.國籍_其他,
                NationO = 國籍是台灣 ? string.Empty : main.CitizenshipCode.ToHalfWidth(),
                BirthCitizenshipCode = main.BirthCitizenshipCode.HasValue ? ((int)main.BirthCitizenshipCode).ToString() : string.Empty,
                BirthCitizenshipCodeOther = 出生地是台灣 ? string.Empty : main.BirthCitizenshipCodeOther.ToHalfWidth(),
                AMLProfessionCode = string.IsNullOrWhiteSpace(main.AMLProfessionCode) ? string.Empty : main.AMLProfessionCode.PadLeft(2, '0'),
                AMLProfessionOther = AML職業別是其他 ? AML職業類別其他代碼 : string.Empty,
                AMLJobLevelCode = main.AMLJobLevelCode ?? string.Empty,
                PosO = string.Empty,
                HomeAddFlag = KYCRequestConst.戶籍地址_中華民國, // TODO: 20250730 再確認
                Reg_ZipCode = main.Reg_ZipCode.ToHalfWidth() ?? string.Empty,
                Reg_City = main.Reg_City ?? string.Empty,
                Reg_District = main.Reg_District ?? string.Empty,
                Reg_Road = main.Reg_Road ?? string.Empty,
                Reg_Lane = main.Reg_Lane.ToFullWidth() ?? string.Empty,
                Reg_Alley = main.Reg_Alley.ToFullWidth() ?? string.Empty,
                Reg_Number = main.Reg_Number.ToFullWidth() ?? string.Empty,
                Reg_SubNumber = main.Reg_SubNumber.ToFullWidth() ?? string.Empty,
                Reg_Floor = main.Reg_Floor.ToFullWidth() ?? string.Empty,
                HomeO = main.CitizenshipCode.ToHalfWidth() ?? string.Empty,
                HomeAddZip = string.Empty,
                HomeAdd = string.Empty,
                MainIncomeAndFundCodes = string.Join(",", main.MainIncomeAndFundCodes.Split(',').Select(x => x.PadLeft(2, '0'))) ?? string.Empty,
                MainIncomeAndFundOther = 所得與資金來源是其他 ? 主要所得及資金來源其他代碼 : string.Empty,
                RAreaFlag = KYCRequestConst.客戶風險評估_非卡友固定值,
                UOpnion = KYCRequestConst.經辦審查意見_建議核准,
                EditTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                MId = AML風險等級是高風險 ? "卡處主管" : string.Empty,
                Mobile = main.Mobile ?? string.Empty,
                Source = KYCRequestConst.資料來源_新卡友,
            };

            /*
                Tips: 當發生兩筆非卡友同時審核時

                申請書編號: 2025081400001 ID:F123456789 非卡友
                申請書編號: 2025081400002 ID:F123456789 非卡友

                徵審人員先審核 2025081400001 成功,打新增非卡友規格至 KYC 系統
                此時徵審人員再審核 2025081400001 如果沒改資料不會怎麼樣
                但改了資料，KYC 系統會因為資料驗證失敗而跳出錯誤訊息
                此時請徵審人員再按一次押上強押記號，即可解決
            */
            if (financeCheck.Focus1_RtnCode == MW3RtnCodeConst.入檔KYC_資料驗證失敗)
            {
                syncKycMW3Info.CHName = KYCHelper.標註強押記號(syncKycMW3Info.CHName);
                syncKycMW3Info.Nation = KYCHelper.標註強押記號(syncKycMW3Info.Nation);
                syncKycMW3Info.BirthCitizenshipCode = KYCHelper.標註強押記號(syncKycMW3Info.BirthCitizenshipCode);
                syncKycMW3Info.AMLProfessionCode = KYCHelper.標註強押記號(syncKycMW3Info.AMLProfessionCode);
                syncKycMW3Info.AMLJobLevelCode = KYCHelper.標註強押記號(syncKycMW3Info.AMLJobLevelCode);
                syncKycMW3Info.HomeAddFlag = KYCHelper.標註強押記號(syncKycMW3Info.HomeAddFlag);
                syncKycMW3Info.MainIncomeAndFundCodes = KYCHelper.標註強押記號(syncKycMW3Info.MainIncomeAndFundCodes);
            }

            return syncKycMW3Info;
        }

        private SyncKycMW3Info MapTo原卡友SyncKycRequest(Reviewer_ApplyCreditCardInfoMain main)
        {
            return new SyncKycMW3Info()
            {
                BRRFlag = string.Empty,
                ID = KYCHelper.轉換ID(main.ID),
                CHName = main.CHName.ToFullWidth(),
                ENName = main.ENName.ToFullWidth(),
                BirthDay = main.BirthDay.ToWesternDate(outputFormat: "yyyy-MM-dd"),
                NameCheckedReasonCodes = string.Empty,
                ISRCAForCurrentPEP = string.Empty,
                ResignPEPKind = string.Empty,
                PEPRange = string.Empty,
                IsCurrentPositionRelatedPEPPosition = string.Empty,
                NameCheckStartTime = string.Empty,
                NameCheckResponseResult = string.Empty,
                NameCheckRcPoint = string.Empty,
                NameCheckAMLId = string.Empty,
                Nation = main.CitizenshipCode == "TW" ? KYCRequestConst.國籍_中華民國 : KYCRequestConst.國籍_其他,
                NationO = main.CitizenshipCode != "TW" ? main.CitizenshipCode : string.Empty,
                BirthCitizenshipCode =
                    main.BirthCitizenshipCode == BirthCitizenshipCode.中華民國 ? KYCRequestConst.出生地_中華民國 : KYCRequestConst.出生地_其他,
                BirthCitizenshipCodeOther = main.BirthCitizenshipCode == BirthCitizenshipCode.其他 ? main.BirthCitizenshipCodeOther : string.Empty,
                AMLProfessionCode = string.IsNullOrWhiteSpace(main.AMLProfessionCode) ? string.Empty : main.AMLProfessionCode.PadLeft(2, '0'),
                AMLProfessionOther = main.AMLProfessionOther,
                AMLJobLevelCode = main.AMLJobLevelCode,
                PosO = string.Empty, // TODO:20250729 確認新系統是否開啟此欄位
                HomeAddFlag = KYCRequestConst.戶籍地址_其他,
                Reg_ZipCode = string.Empty,
                Reg_City = string.Empty,
                Reg_District = string.Empty,
                Reg_Road = string.Empty,
                Reg_Lane = string.Empty,
                Reg_Alley = string.Empty,
                Reg_Number = string.Empty,
                Reg_SubNumber = string.Empty,
                Reg_Floor = string.Empty,
                Reg_Other = string.Empty,
                HomeO = main.CitizenshipCode,
                HomeAddZip = string.Empty,
                HomeAdd = main.Reg_FullAddr,
                MainIncomeAndFundCodes = string.Join(",", main.MainIncomeAndFundCodes.Split(',').Select(x => x.PadLeft(2, '0'))),
                MainIncomeAndFundOther = main.MainIncomeAndFundOther,
                RAreaFlag = KYCRequestConst.客戶風險評估_卡友固定值,
                UOpnion = KYCRequestConst.經辦審查意見_建議核准,
                EditTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Mobile = main.Mobile,
                Source = KYCRequestConst.資料來源_原卡友加辦卡,
            };
        }

        private async Task<BaseMW3Response<SyncKycResponse>> 呼叫KYC入檔API(SyncKycMW3Info request) => await _mw3Adapter.SyncKYC(request);

        private QueryCustKYCRiskLevelRequestInfo MapTo紙本件原卡友Request(string iD) => new QueryCustKYCRiskLevelRequestInfo { ID = iD };

        private async Task<BaseMW3Response<QueryCustKYCRiskLevelResponse>> 呼叫查詢KYC風險等級(QueryCustKYCRiskLevelRequestInfo request) =>
            await _mw3Adapter.QueryCustKYCRiskLevel(request);

        private CardStatus ConvertToCardStatus(CaseContext syncKYCContext)
        {
            // Tips: 切記所有狀態皆需要一致
            if (syncKYCContext.是否有徵信代碼)
            {
                if (
                    (syncKYCContext.KYCAPIResult.呼叫是否成功 && syncKYCContext.KYCAPIResult.KYC_RtnCode == MW3RtnCodeConst.入檔KYC_主機TimeOut)
                    || syncKYCContext.是否系統維護
                )
                {
                    // Tips: 會排入KYC入檔排程
                    var updateCardStatus = syncKYCContext.案件種類 switch
                    {
                        案件種類.網路件_原卡友 => CardStatus.KYC入檔作業_完成卡友檢核, // Job: A02KYCSync
                        案件種類.網路件_非卡友 => CardStatus.KYC入檔作業_網路件待月收入預審, // Job: RetryKYCSync
                        案件種類.紙本件_非卡友 => CardStatus.KYC入檔作業_紙本件待月收入預審, // Job: RetryKYCSync
                        _ => throw new ArgumentException($"未知的案件種類: {syncKYCContext.案件種類}"),
                    };

                    return updateCardStatus;
                }

                if (syncKYCContext.案件種類 == 案件種類.網路件_原卡友)
                {
                    return CardStatus.網路件_卡友_完成KYC入檔作業;
                }

                return CardStatus.完成月收入確認;
            }
            else
            {
                return CardStatus.完成月收入確認;
            }
        }

        private string 取得回傳訊息(CaseContext caseContext)
        {
            if (!caseContext.是否有徵信代碼)
            {
                return $"完成月收入確認，申請書編號：{caseContext.申請書編號}";
            }

            if (caseContext.是否系統維護)
            {
                return $"完成月收入確認，因 KYC 入檔主機維護中，已排入 RetryKYCSync 排程";
            }

            List<string> timeoutReturnCodes = [MW3RtnCodeConst.入檔KYC_主機TimeOut];
            if (caseContext.KYCAPIResult.呼叫是否成功 && timeoutReturnCodes.Contains(caseContext.KYCAPIResult.KYC_RtnCode))
            {
                return $"入檔KYC失敗，錯誤代碼: {caseContext.KYCAPIResult.KYC_RtnCode}，錯誤訊息: {caseContext.KYCAPIResult.KYC_Message}，已排入KYC入檔排程，申請書編號：{caseContext.申請書編號}";
            }

            return $"完成月收入確認，申請書編號：{caseContext.申請書編號}";
        }
    }
}
