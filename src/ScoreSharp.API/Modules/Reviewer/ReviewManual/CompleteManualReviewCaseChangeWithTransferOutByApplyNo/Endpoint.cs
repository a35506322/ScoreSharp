using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferOutByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual
{
    public partial class ReviewManualController
    {
        ///<summary>
        /// 完成人工徵審案件異動_權限外 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewManual/CompleteManualReviewCaseChangeWithTransferOutByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(typeof(完成人工徵信案件異動資料權限外_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(完成人工徵信案件異動資料權限外_2000_ResEx),
            typeof(完成人工徵信案件異動_無指定主管資訊_4003_ResEx),
            typeof(完成權限外人工徵信案件異動_卡片狀態錯誤請先按儲存_4003_ResEx),
            typeof(完成權限外人工徵信案件異動_資料檢核失敗_4006_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(完成人工徵信案件異動_選取權限外有誤_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("CompleteManualReviewCaseChangeWithTransferOutByApplyNo")]
        [HttpPut("{applyNo}")]
        public async Task<IResult> CompleteManualReviewCaseChangeWithTransferOutByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] List<CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest> request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request), cancellationToken);
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferOutByApplyNo
{
    public record Command(string applyNo, List<CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest> request)
        : IRequest<ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>>;

    public class Handler(
        ScoreSharpContext context,
        IJWTProfilerHelper jWTProfilerHelper,
        ILogger<Handler> logger,
        IReviewerValidateHelper reviewerValidateHelper
    ) : IRequestHandler<Command, ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>>
    {
        public async Task<ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var updateCompleteHandleCases = request.request;
            var applyNo = request.applyNo;

            if (updateCompleteHandleCases.Any(x => x.ApplyNo != applyNo))
            {
                return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                    new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                    "申請書編號與Req比對錯誤"
                );
            }

            CaseDataBundle bundle = await 取得案件相關資料(request.applyNo);

            if (bundle.Main is null || bundle.Handles.Count == 0)
            {
                return ApiResponseHelper.NotFound<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                    new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                    applyNo
                );
            }

            var dbHandles = bundle.Handles;
            var dbMain = bundle.Main;

            if (updateCompleteHandleCases.Count != dbHandles.Count)
            {
                return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                    new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                    "請求的案件數量與資料庫中的不符，請確認"
                );
            }

            var filterCompleteCaseEqualN = updateCompleteHandleCases.Where(x => x.IsCompleted == "N").ToList();

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
                    return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                        new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                        errorMessages
                    );
                }

                // type=1 轉換為 RetryCheck 列表，使用 RtnCode 4006
                if (type1Errors.Any())
                {
                    var retryChecks = type1Errors
                        .Select(x => ReviewerMapHelper.MapToRetryCheck(x, bundle.Main.CHName, bundle.Supplementary?.CHName ?? string.Empty))
                        .ToList();

                    return ApiResponseHelper.ReviewerBusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                        data: new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo, RetryChecks = retryChecks },
                        message: "請再次執行查詢檢核資料"
                    );
                }
            }

            foreach (var updateCase in filterCompleteCaseEqualN)
            {
                var handle = dbHandles.FirstOrDefault(x => x.SeqNo == updateCase.SeqNo);
                CardStatus[] cardStatuses =
                {
                    CardStatus.申請核卡_等待完成本案徵審,
                    CardStatus.申請退件_等待完成本案徵審,
                    CardStatus.申請補件_等待完成本案徵審,
                    CardStatus.申請撤件_等待完成本案徵審,
                };

                if (!cardStatuses.Contains(handle.CardStatus))
                {
                    return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                        new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                        "卡片狀態錯誤，請先按儲存，再進行權限外本案徵審"
                    );
                }
            }

            // 判斷 CurrentHandleUserId
            string? forwardedToUserId = null;
            /*
                權限外　CurrentHandleUserId 處理

                申請核卡_等待完成本案徵審  =>
                抓取 ReviewerUserId 於 人員組織分案群組設定抓指定主管1和指定主管2 判斷，
                例如建議額度 100,000 指定主管1 200,000 指定主管2 300,000 ，則遞給指定主管1
                例如建議額度 400,000 指定主管1 200,000 指定主管2 300,000 ，則遞給指定主管2

                申請退件_等待完成本案徵審  => 指定主管1
                申請補件_等待完成本案徵審  => 指定主管1
                申請撤件_等待完成本案徵審  => 指定主管1
            */
            var approveUser = await context.OrgSetUp_UserOrgCaseSetUp.FirstOrDefaultAsync(x => x.UserId == jWTProfilerHelper.UserId);
            if (string.IsNullOrEmpty(approveUser.DesignatedSupervisor1) && string.IsNullOrEmpty(approveUser.DesignatedSupervisor2))
            {
                return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                    new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                    $"{jWTProfilerHelper.UserId} 無指定主管資訊，請先設定指定主管"
                );
            }

            var 是否有申請核卡中 = filterCompleteCaseEqualN.Any(x => x.CaseChangeAction == ManualReviewAction.排入核卡);
            if (是否有申請核卡中)
            {
                var 需要核卡最大額度 = dbHandles
                    .Where(x => filterCompleteCaseEqualN.Any(y => y.SeqNo == x.SeqNo))
                    .Where(x => x.CardStatus == CardStatus.申請核卡_等待完成本案徵審)
                    .Max(x => x.CardLimit);

                // Tips: 月收入確認人員都會一致
                var monthlyIncomeCheckUserId = dbHandles.Select(x => x.MonthlyIncomeCheckUserId).FirstOrDefault();

                var designatedSupervisors = await context
                    .OrgSetUp_UserOrgCaseSetUp.Where(x =>
                        x.UserId == approveUser.DesignatedSupervisor1 || x.UserId == approveUser.DesignatedSupervisor2
                    )
                    .AsNoTracking()
                    .OrderBy(x => x.CardLimit)
                    .ToListAsync();

                logger.LogInformation(
                    "指定主管1：{designatedSupervisor1} 指定主管2：{designatedSupervisor2} 月收入確認人員：{monthlyIncomeUserId}",
                    approveUser.DesignatedSupervisor1,
                    approveUser.DesignatedSupervisor2,
                    monthlyIncomeCheckUserId
                );

                // Tips: 月收入確認人員不能與審查人員相同
                var fileterDesignatedSupervisors = designatedSupervisors.Where(x => x.UserId != monthlyIncomeCheckUserId).ToList();

                var 指定主管1 = fileterDesignatedSupervisors.FirstOrDefault();
                var 指定主管2 = fileterDesignatedSupervisors.LastOrDefault();

                if (fileterDesignatedSupervisors.Count == 1)
                {
                    forwardedToUserId = 指定主管1 is null ? 指定主管2.UserId : 指定主管1.UserId;
                }
                else if (fileterDesignatedSupervisors.Count == 2)
                {
                    if (需要核卡最大額度 <= 指定主管1.CardLimit)
                    {
                        forwardedToUserId = 指定主管1.UserId;
                    }
                    else
                    {
                        forwardedToUserId = 指定主管2.UserId;
                    }
                }
                else
                {
                    return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
                        new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                        $"{jWTProfilerHelper.UserId} 經篩選無指定主管資訊，請先設定指定主管"
                    );
                }
            }
            else
            {
                forwardedToUserId = approveUser.DesignatedSupervisor1;
            }

            List<Reviewer_ApplyCreditCardInfoProcess> processList = new();
            List<Reviewer_CardRecord> cardRecordList = new();
            var now = DateTime.Now;
            foreach (var updateCase in filterCompleteCaseEqualN)
            {
                var handle = dbHandles.FirstOrDefault(x => x.SeqNo == updateCase.SeqNo);
                var status = ConvertCardStatus(updateCase.CaseChangeAction, handle.CardStatus);
                string userTypeName = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";
                string cardCode = handle.ApplyCardType;

                handle.CardStatus = status;

                var forwardUser = await context.OrgSetUp_User.FirstOrDefaultAsync(x => x.UserId == forwardedToUserId);
                string notes = $"轉交本案;轉交({forwardUser.UserName});({userTypeName}_{cardCode})";

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
                        ForwardedToUserId = forwardedToUserId,
                        Action = ExecutionAction.權限外_轉交,
                        HandleSeqNo = handle.SeqNo,
                    }
                );
            }

            // TODO: 20250711 要確認是否當所有卡片為終止狀態(如核卡作業中)，更新為 null
            dbMain.CurrentHandleUserId = forwardedToUserId;
            dbMain.LastUpdateUserId = jWTProfilerHelper.UserId;
            dbMain.LastUpdateTime = now;

            await context.Reviewer_CardRecord.AddRangeAsync(cardRecordList, cancellationToken);
            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processList, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.Success(
                data: new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = applyNo },
                message: $"案件編號: {applyNo}，完成本案徵審成功({ExecutionAction.權限外_轉交})"
            );
        }

        private ReviewerValidationResult 資料檢核(
            CaseDataBundle bundle,
            List<CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest> filterCompleteCaseEqualN
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
                        ManualReviewAction.排入核卡 => ReviewAction.排入核卡,
                        ManualReviewAction.排入退件 => ReviewAction.排入退件,
                        ManualReviewAction.排入補件 => ReviewAction.排入補件,
                        ManualReviewAction.排入撤件 => ReviewAction.排入撤件,
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

        private CardStatus ConvertCardStatus(ManualReviewAction action, CardStatus cardStatus) =>
            (action, cardStatus) switch
            {
                (ManualReviewAction.排入核卡, CardStatus.申請核卡_等待完成本案徵審) => CardStatus.申請核卡中,
                (ManualReviewAction.排入退件, CardStatus.申請退件_等待完成本案徵審) => CardStatus.申請退件中,
                (ManualReviewAction.排入補件, CardStatus.申請補件_等待完成本案徵審) => CardStatus.申請補件中,
                (ManualReviewAction.排入撤件, CardStatus.申請撤件_等待完成本案徵審) => CardStatus.申請撤件中,
                _ => throw new Exception($"Invalid card action: {action} and card status: {cardStatus}"),
            };
    }
}
