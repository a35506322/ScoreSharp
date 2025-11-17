using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferInByApplyNo;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Helpers.KYC;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual
{
    public partial class ReviewManualController
    {
        ///<summary>
        /// 完成人工徵審案件異動_權限內 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewManual/CompleteManualReviewCaseChangeWithTransferInByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(typeof(完成人工徵信案件異動資料權限內_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(完成人工徵信案件異動資料權限內_2000_ResEx),
            typeof(完成權限內人工徵信案件異動_卡片狀態錯誤請先按儲存_4003_ResEx),
            typeof(完成人工徵信案件異動_資料檢核失敗_4006_ResEx),
            typeof(發查第三方API失敗_API意外錯誤_5002_ResEx),
            typeof(發查第三方API失敗_MW3回覆錯誤代碼_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(完成人工徵信案件異動_選取權限內有誤_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("CompleteManualReviewCaseChangeWithTransferInByApplyNo")]
        [HttpPut("{applyNo}")]
        public async Task<IResult> CompleteManualReviewCaseChangeWithTransferInByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] List<CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest> request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request), cancellationToken);
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferInByApplyNo
{
    public record Command(string applyNo, List<CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest> request)
        : IRequest<ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>>;

    public class Handler(
        ScoreSharpContext context,
        IJWTProfilerHelper jWTProfilerHelper,
        ILogger<Handler> logger,
        IMW3APAPIAdapter mw3Adapter,
        IJWTProfilerHelper _jwtHelper,
        IReviewerValidateHelper reviewerValidateHelper
    ) : IRequestHandler<Command, ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>>
    {
        public async Task<ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var updateCompleteHandleCases = request.request;
            var applyNo = request.applyNo;

            if (updateCompleteHandleCases.Any(x => x.ApplyNo != applyNo))
            {
                return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(
                    null,
                    "申請書編號與Req比對錯誤"
                );
            }

            CaseDataBundle bundle = await 取得案件相關資料(request.applyNo);

            if (bundle.Main is null || bundle.Handles.Count == 0)
            {
                return ApiResponseHelper.NotFound<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(null, applyNo);
            }

            if (updateCompleteHandleCases.Count != bundle.Handles.Count)
            {
                return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(
                    null,
                    "請求的案件數量與資料庫中的不符，請確認"
                );
            }

            var filterCompleteCaseEqualN = updateCompleteHandleCases.Where(x => x.IsCompleted == "N").ToList();

            foreach (var updateCase in filterCompleteCaseEqualN)
            {
                var handle = bundle.Handles.FirstOrDefault(x => x.SeqNo == updateCase.SeqNo);
                CardStatus[] cardStatuses =
                {
                    CardStatus.核卡_等待完成本案徵審,
                    CardStatus.退件_等待完成本案徵審,
                    CardStatus.補件_等待完成本案徵審,
                    CardStatus.撤件_等待完成本案徵審,
                };
                if (!cardStatuses.Contains(handle.CardStatus))
                {
                    return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(
                        null,
                        "卡片狀態錯誤，請先按儲存，再進行權限內本案徵審"
                    );
                }
            }

            var validationResult = 資料檢核(bundle, filterCompleteCaseEqualN);
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
                    return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(null, errorMessages);
                }

                // type=1 轉換為 RetryCheck 列表，使用 RtnCode 4006
                if (type1Errors.Any())
                {
                    var retryChecks = type1Errors.Select(x =>
                        ReviewerMapHelper.MapToRetryCheck(x, bundle.Main.CHName, bundle.Supplementary?.CHName ?? string.Empty)
                    );

                    return ApiResponseHelper.ReviewerBusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(
                        data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse() { RetryChecks = retryChecks.ToList() },
                        message: "請再次執行查詢檢核資料"
                    );
                }
            }

            List<Reviewer_ApplyCreditCardInfoProcess> processList = new();
            List<Reviewer_CardRecord> cardRecordList = new();
            List<Reviewer3rd_KYCQueryLog> kycQueryLogList = new();
            KYCContext kycContext = new();
            var now = DateTime.Now;

            // 20250819 add 更改建議核准KYC
            var seqNos = filterCompleteCaseEqualN.Select(x => x.SeqNo);
            var filter核退撤Handles = bundle
                .Handles.Where(x =>
                    seqNos.Contains(x.SeqNo)
                    && (
                        x.CardStatus == CardStatus.核卡_等待完成本案徵審
                        || x.CardStatus == CardStatus.退件_等待完成本案徵審
                        || x.CardStatus == CardStatus.撤件_等待完成本案徵審
                    )
                )
                .ToList();
            if (filter核退撤Handles.Count != 0)
            {
                var suggestCode = filter核退撤Handles.Any(x => x.CardStatus == CardStatus.核卡_等待完成本案徵審) ? "Y" : "N";
                SuggestKycMW3Info suggestKYCRequest = new() { ID = KYCHelper.轉換ID(bundle.Main.ID), UOpnion = suggestCode };

                var response = await mw3Adapter.SuggestKYC(suggestKYCRequest);
                var kycInfo = response.Data.Info;
                var kycResult = kycInfo.Result.Data;

                Reviewer_ApplyCreditCardInfoProcess process = MapHelper.MapKYCProcess(
                    applyNo: bundle.Main.ApplyNo,
                    process: ProcessConst.建議核准KYC,
                    startTime: now,
                    endTime: now,
                    isSuccess: kycInfo.Rc2 == "M000" && kycResult.KycCode == MW3RtnCodeConst.建議核准KYC_成功 ? "Y" : "N",
                    kycCode: kycResult.KycCode,
                    rc2: kycInfo.Rc2,
                    id: bundle.Main.ID,
                    userType: bundle.Main.UserType,
                    keyMsg: kycResult.ErrMsg,
                    userId: _jwtHelper.UserId,
                    suggestCode: suggestCode
                );
                processList.Add(process);

                Reviewer3rd_KYCQueryLog kycQueryLog = MapHelper.MapKYCQueryLog(
                    applyNo: applyNo,
                    cardStatus: string.Join("/", bundle.Handles.Select(x => x.CardStatus.ToName())),
                    id: bundle.Main.ID,
                    request: suggestKYCRequest,
                    response: response,
                    currentHandler: _jwtHelper.UserId
                );
                kycQueryLogList.Add(kycQueryLog);

                if (!response.IsSuccess)
                {
                    await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processList, cancellationToken);
                    await context.Reviewer3rd_KYCQueryLog.AddRangeAsync(kycQueryLogList, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(
                        data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse
                        {
                            ApplyNo = applyNo,
                            KYC_RtnCode = string.Empty,
                            KYC_Message = string.Empty,
                            KYC_Exception = response.ErrorMessage,
                            KYC_Rc2 = string.Empty,
                        },
                        checkThirdPartyApiName: "更改建議核准KYC"
                    );
                }
                else if (kycInfo.Rc2 != "M000" || kycResult.KycCode != MW3RtnCodeConst.建議核准KYC_成功)
                {
                    await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processList, cancellationToken);
                    await context.Reviewer3rd_KYCQueryLog.AddRangeAsync(kycQueryLogList, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(
                        data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse
                        {
                            ApplyNo = applyNo,
                            KYC_RtnCode = kycResult.KycCode,
                            KYC_Message = kycResult.ErrMsg,
                            KYC_Exception = string.Empty,
                            KYC_Rc2 = kycInfo.Rc2,
                        },
                        checkThirdPartyApiName: "更改建議核准KYC"
                    );
                }
                else
                {
                    kycContext.KYC_RtnCode = kycInfo.Rc2;
                    kycContext.KYC_Message = kycResult.ErrMsg;
                    kycContext.KYC_Rc2 = kycInfo.Rc2;
                    kycContext.KYC_Exception = response.ErrorMessage;
                }
            }

            // 如果有案件要執行補件作業中，會保留原來的 CurrentHandleUserId，之後狀態改為補回件，由原來的人接手
            string forwardedToUserId = string.Empty;
            if (!filterCompleteCaseEqualN.Any(x => x.CaseChangeAction == ManualReviewAction.補件作業))
            {
                forwardedToUserId = null;
            }
            else
            {
                forwardedToUserId = bundle.Main.CurrentHandleUserId;
            }

            foreach (var updateCase in filterCompleteCaseEqualN)
            {
                var handle = bundle.Handles.FirstOrDefault(x => x.SeqNo == updateCase.SeqNo);
                var status = ConvertCardStatus(updateCase.CaseChangeAction, handle.CardStatus);
                string userTypeName = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";
                string cardCode = handle.ApplyCardType;

                handle.CardStatus = status;
                handle.CaseChangeAction = null; // 20250714 完成本案後，CaseChangeAction 要清空

                if (status == CardStatus.退件作業中_終止狀態 && handle.IsPrintSMSAndPaper == "Y")
                {
                    handle.BatchRejectionStatus = "N";
                    handle.BatchRejectiontTime = null;
                }

                if (status == CardStatus.補件作業中 && handle.IsPrintSMSAndPaper == "Y")
                {
                    handle.BatchSupplementStatus = "N";
                    handle.BatchSupplementTime = null;
                }

                if (
                    status == CardStatus.核卡作業中
                    || status == CardStatus.退件作業中_終止狀態
                    || status == CardStatus.撤件作業中_終止狀態
                    || status == CardStatus.補件作業中
                )
                {
                    handle.ApproveUserId = jWTProfilerHelper.UserId;
                    handle.ApproveTime = now;
                }

                string notes = $"完成本案;({userTypeName}_{cardCode})";
                processList.Add(
                    new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = applyNo,
                        Process = handle.CardStatus.ToString(),
                        StartTime = now,
                        EndTime = now,
                        Notes = notes,
                        ProcessUserId = jWTProfilerHelper.UserId,
                    }
                );

                cardRecordList.Add(
                    new Reviewer_CardRecord
                    {
                        ApplyNo = applyNo,
                        CardStatus = handle.CardStatus,
                        CardLimit = handle.CardLimit ?? null,
                        ApproveUserId = jWTProfilerHelper.UserId,
                        AddTime = now,
                        HandleNote = notes,
                        ForwardedToUserId = string.Empty,
                        Action = ExecutionAction.權限內,
                        HandleSeqNo = handle.SeqNo,
                    }
                );
            }

            // TODO: 20250711 要確認是否當所有卡片為終止狀態(如核卡作業中)，更新為 null
            bundle.Main.CurrentHandleUserId = forwardedToUserId;
            bundle.Main.LastUpdateUserId = jWTProfilerHelper.UserId;
            bundle.Main.LastUpdateTime = now;

            // 此案件中所有卡片都是終止狀態才清空前手經辦
            if (
                // TODO: 20250821終止狀態應該不只這些
                bundle.Handles.All(x =>
                    x.CardStatus == CardStatus.退件作業中_終止狀態
                    || x.CardStatus == CardStatus.撤件作業中_終止狀態
                    || x.CardStatus == CardStatus.核卡作業中
                )
            )
            {
                bundle.Main.PreviousHandleUserId = null;
            }

            await context.Reviewer_CardRecord.AddRangeAsync(cardRecordList, cancellationToken);
            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processList, cancellationToken);
            if (kycQueryLogList.Count != 0)
            {
                await context.Reviewer3rd_KYCQueryLog.AddRangeAsync(kycQueryLogList, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.Success(
                data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse
                {
                    ApplyNo = applyNo,
                    KYC_RtnCode = kycContext.KYC_RtnCode,
                    KYC_Message = kycContext.KYC_Message,
                    KYC_Exception = kycContext.KYC_Exception,
                    KYC_Rc2 = kycContext.KYC_Rc2,
                },
                message: $"案件編號: {applyNo}，完成本案徵審成功({ExecutionAction.權限內})"
            );
        }

        private CardStatus ConvertCardStatus(ManualReviewAction action, CardStatus cardStatus) =>
            (action, cardStatus) switch
            {
                (ManualReviewAction.核卡作業, CardStatus.核卡_等待完成本案徵審) => CardStatus.核卡作業中,
                (ManualReviewAction.退件作業, CardStatus.退件_等待完成本案徵審) => CardStatus.退件作業中_終止狀態,
                (ManualReviewAction.補件作業, CardStatus.補件_等待完成本案徵審) => CardStatus.補件作業中,
                (ManualReviewAction.撤件作業, CardStatus.撤件_等待完成本案徵審) => CardStatus.撤件作業中_終止狀態,
                _ => throw new Exception($"Invalid card action: {action} and card status: {cardStatus}"),
            };

        private async Task<CaseDataBundle> 取得案件相關資料(string applyNo)
        {
            var handles = await context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo).ToListAsync();
            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);
            var financeChecks = await context.Reviewer_FinanceCheckInfo.AsNoTracking().Where(x => x.ApplyNo == applyNo).ToListAsync();
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

        private ReviewerValidationResult 資料檢核(
            CaseDataBundle bundle,
            List<CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest> filterCompleteCaseEqualN
        )
        {
            var main = bundle.Main;
            var supplementary = bundle.Supplementary;
            var handles = bundle.Handles;
            var bankTarce = bundle.BankTrace;
            var mainFinanceCheck = bundle.MainFinanceCheck;
            var supplementaryFinanceCheck = bundle.SupplementaryFinanceCheck;

            var caseInfo = ReviewerMapHelper.MapToCaseInfoContext(main);

            // 根據每個要完成的案件，建立對應的 HandleInfoContext
            var handleStatuses = filterCompleteCaseEqualN
                .Select(updateCase =>
                {
                    var handle = handles.FirstOrDefault(x => x.SeqNo == updateCase.SeqNo);
                    if (handle == null)
                        return null;

                    // 將 ManualReviewAction 轉換為 ReviewAction
                    ReviewAction reviewAction = updateCase.CaseChangeAction switch
                    {
                        ManualReviewAction.核卡作業 => ReviewAction.核卡作業,
                        ManualReviewAction.退件作業 => ReviewAction.退件作業,
                        ManualReviewAction.補件作業 => ReviewAction.補件作業,
                        ManualReviewAction.撤件作業 => ReviewAction.撤件作業,
                        _ => throw new ArgumentException($"Invalid ManualReviewAction: {updateCase.CaseChangeAction}"),
                    };

                    return ReviewerMapHelper.MapToHandleInfoContext(handle, reviewAction, bundle.CardDict);
                })
                .Where(x => x != null)
                .Cast<HandleInfoContext>()
                .ToList();

            var mainData = ReviewerMapHelper.MapToReviewerMainDataContext(main, handleStatuses);
            var mainAddressData = ReviewerMapHelper.MapToReviewerMainAddressContext(main);

            ReviewerMainBankTraceContext? bankTarceData = null;
            if (bankTarce != null)
            {
                bankTarceData = ReviewerMapHelper.MapToReviewerMainBankTraceContext(bankTarce, main.CHName);
            }
            ReviewerFinanceCheckMainContext? mainFinanceCheckDataContext = null;
            if (mainFinanceCheck != null)
            {
                mainFinanceCheckDataContext = ReviewerMapHelper.MapToReviewerFinanceCheckMainContext(mainFinanceCheck, main.NameChecked, main.CHName);
            }

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
    }
}
