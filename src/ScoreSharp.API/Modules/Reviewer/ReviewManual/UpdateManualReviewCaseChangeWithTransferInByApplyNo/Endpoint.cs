using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.API.Modules.Reviewer.ReviewManual.UpdateManualReviewCaseChangeWithTransferInByApplyNo;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual
{
    public partial class ReviewManualController
    {
        ///<summary>
        /// 更新人工徵審案件異動_權限內 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewManual/UpdateManualReviewCaseChangeWithTransferInByApplyNo/20240601A0001
        ///
        /// 備註
        ///
        ///     1. 前端帶入所有卡片，使用 GetManualReviewCaseChangeByApplyNo 的 IsCompleted ，放在 UpdateHandleCases 裡面
        ///
        /// </remarks>
        ///<returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(核卡作業_2000_ReqEx),
            typeof(退件作業_2000_ReqEx),
            typeof(補件作業_2000_ReqEx),
            typeof(撤件作業_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(更新人工徵信案件異動資料權限內_2000_ResEx),
            typeof(更新人工徵信案件異動資料權限內_核卡額度不足_4003_ResEx),
            typeof(更新人工徵信案件異動資料權限內_補退件地址不完整_4003_ResEx),
            typeof(更新人工徵信案件異動_卡片狀態不能更新_4003_ResEx),
            typeof(更新人工徵信案件異動_權限內使用者核卡額度為0_4003_ResEx),
            typeof(更新人工徵信案件異動_核卡案件的徵信代碼皆需要一致_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(更新人工徵信案件異動_選取權限內有誤_4000_ResEx),
            typeof(更新人工徵信案件異動_核卡作業必填資料_4000_ResEx),
            typeof(更新人工徵信案件異動_退件作業必填資料_4000_ResEx),
            typeof(更新人工徵信案件異動_補件作業必填資料_4000_ResEx),
            typeof(更新人工徵信案件異動_撤件作業必填資料_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("UpdateManualReviewCaseChangeWithTransferInByApplyNo")]
        [HttpPut("{applyNo}")]
        public async Task<IResult> UpdateManualReviewCaseChangeWithTransferInByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest> request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.UpdateManualReviewCaseChangeWithTransferInByApplyNo
{
    public record Command(
        string applyNo,
        List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest> updateManualReviewCaseChangeByApplyNoRequest
    ) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jWTProfilerHelper, IReviewerHelper reviewerHelper)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateCases = request.updateManualReviewCaseChangeByApplyNoRequest;
            var applyNo = request.applyNo;

            if (updateCases.Any(x => x.ApplyNo != applyNo))
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號與Req比對錯誤");
            }

            var handleList = await context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo).ToListAsync();

            if (handleList.Count == 0)
            {
                return ApiResponseHelper.NotFound<string>(null, applyNo);
            }

            if (updateCases.Count != handleList.Count)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "更新的卡片數量與資料庫不符，請重新確認。");
            }

            var user = await context.OrgSetUp_UserOrgCaseSetUp.FirstOrDefaultAsync(x => x.UserId == jWTProfilerHelper.UserId);
            var main = await context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
            var supplementary = await context
                .Reviewer_ApplyCreditCardInfoSupplementary.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            // 20250717 新增使用者核卡額度檢查
            if (user.CardLimit <= 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(applyNo, "使用者核卡額度為0，請聯絡管理員設定核卡額度。");
            }

            var filterCompleteEqualN = updateCases.Where(x => x.IsCompleted == "N").ToList();

            List<string> errorMsgs = new();

            foreach (var dto in filterCompleteEqualN)
            {
                var handle = handleList.FirstOrDefault(x => x.SeqNo == dto.SeqNo);

                if (user.CardLimit < dto.CardLimit && dto.CaseChangeAction == ManualReviewAction.核卡作業)
                {
                    var userType = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";
                    var cardCode = handle.ApplyCardType;
                    errorMsgs.Add($"卡別:{cardCode}({userType})，建議額度為{dto.CardLimit}，超過使用者核卡額度{user.CardLimit}。");

                    continue;
                }

                if ((dto.CaseChangeAction == ManualReviewAction.補件作業) && dto.IsPrintSMSAndPaper == "Y" && dto.SupplementSendCardAddr.HasValue)
                {
                    if (!reviewerHelper.檢查對應地址(main, dto.SupplementSendCardAddr.Value))
                    {
                        errorMsgs.Add($"選取補件寄送地址填寫不完整，請重新確認。({dto.SeqNo})");
                    }
                    continue;
                }

                if ((dto.CaseChangeAction == ManualReviewAction.退件作業) && dto.IsPrintSMSAndPaper == "Y" && dto.RejectionSendCardAddr.HasValue)
                {
                    if (!reviewerHelper.檢查對應地址(main, dto.RejectionSendCardAddr.Value))
                    {
                        errorMsgs.Add($"選取退件寄送地址填寫不完整，請重新確認。({dto.SeqNo})");
                    }
                }
            }

            // 核卡案件的徵信代碼皆需要一致
            if (
                filterCompleteEqualN
                    .Where(x => !string.IsNullOrWhiteSpace(x.CreditCheckCode))
                    .Where(x => x.CaseChangeAction == ManualReviewAction.核卡作業)
                    .Select(x => x.CreditCheckCode)
                    .Distinct()
                    .Count() > 1
            )
            {
                errorMsgs.Add("核卡案件的徵信代碼皆需要一致。");
            }

            if (errorMsgs.Count > 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(applyNo, string.Join(Environment.NewLine, errorMsgs.Distinct()));
            }

            var now = DateTime.Now;

            main.LastUpdateUserId = jWTProfilerHelper.UserId;
            main.LastUpdateTime = now;

            List<Reviewer_ApplyCreditCardInfoProcess> processList = new();
            List<Reviewer_CardRecord> cardRecordList = new();

            foreach (var item in filterCompleteEqualN)
            {
                var handle = handleList.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                handle.CardStatus = ActionToCardStatus(item.CaseChangeAction);
                string handleNote = string.Empty;

                var userTypeName = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";
                var cardCode = handle.ApplyCardType;

                InitHandleData(handle);

                if (item.CaseChangeAction == ManualReviewAction.核卡作業)
                {
                    handle.CaseChangeAction = MapToCaseChangeAction(item.CaseChangeAction);
                    handle.CardLimit = item.CardLimit;
                    handle.IsForceCard = item.IsForceCard;
                    handle.NuclearCardNote = item.NuclearCardNote;
                    handleNote =
                        $"建議額度：{item.CardLimit};" + (string.IsNullOrWhiteSpace(item.NuclearCardNote) ? "" : $"註記：{item.NuclearCardNote};");
                    handle.CreditCheckCode = item.CreditCheckCode;

                    // TODO: 20250710 核卡排程應該要設定，但先跳過這個報表之後補上
                }
                else if (item.CaseChangeAction == ManualReviewAction.退件作業)
                {
                    handle.CaseChangeAction = MapToCaseChangeAction(item.CaseChangeAction);
                    handle.RejectionReasonCode = string.Join(",", item.RejectionReasonCode);
                    handle.OtherRejectionReason = item.OtherRejectionReason;
                    handle.RejectionNote = item.RejectionNote;
                    handle.RejectionSendCardAddr = item.RejectionSendCardAddr;
                    handle.IsPrintSMSAndPaper = item.IsPrintSMSAndPaper;
                    handleNote =
                        $"退件代碼：{string.Join(",", item.RejectionReasonCode)};(TO {item.RejectionSendCardAddr})"
                        + (string.IsNullOrWhiteSpace(item.RejectionNote) ? "" : $"註記：{item.RejectionNote};");
                    handle.CreditCheckCode = null;
                }
                else if (item.CaseChangeAction == ManualReviewAction.補件作業)
                {
                    handle.CaseChangeAction = MapToCaseChangeAction(item.CaseChangeAction);
                    handle.SupplementReasonCode = string.Join(",", item.SupplementReasonCode);
                    handle.OtherSupplementReason = item.OtherSupplementReason;
                    handle.SupplementNote = item.SupplementNote;
                    handle.SupplementSendCardAddr = item.SupplementSendCardAddr;
                    handle.IsPrintSMSAndPaper = item.IsPrintSMSAndPaper;
                    handleNote =
                        $"補件代碼：{string.Join(",", item.SupplementReasonCode)};(TO {item.SupplementSendCardAddr})"
                        + (string.IsNullOrWhiteSpace(item.SupplementNote) ? "" : $"註記：{item.SupplementNote};");
                    handle.CreditCheckCode = null;
                }
                else if (item.CaseChangeAction == ManualReviewAction.撤件作業)
                {
                    handle.WithdrawalNote = item.WithdrawalNote;
                    handle.CaseChangeAction = MapToCaseChangeAction(item.CaseChangeAction);
                    handleNote = $"撤件註記：{item.WithdrawalNote};";
                    handle.CreditCheckCode = null;
                }
                handle.HandleNote = handleNote;

                processList.Add(
                    new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = applyNo,
                        Process = handle.CardStatus.ToString(),
                        StartTime = now,
                        EndTime = now,
                        Notes = handle.HandleNote + $"({userTypeName}_{cardCode})",
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
                        HandleNote = handle.HandleNote + $"({userTypeName}_{cardCode})",
                        ForwardedToUserId = string.Empty,
                        Action = ExecutionAction.權限內,
                        HandleSeqNo = handle.SeqNo,
                    }
                );
            }

            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processList, cancellationToken);
            await context.Reviewer_CardRecord.AddRangeAsync(cardRecordList, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.Success(data: applyNo, message: $"案件編號: {applyNo}，更新成功({ExecutionAction.權限內})");
        }

        private CardStatus ActionToCardStatus(ManualReviewAction action) =>
            action switch
            {
                ManualReviewAction.核卡作業 => CardStatus.核卡_等待完成本案徵審,
                ManualReviewAction.退件作業 => CardStatus.退件_等待完成本案徵審,
                ManualReviewAction.補件作業 => CardStatus.補件_等待完成本案徵審,
                ManualReviewAction.撤件作業 => CardStatus.撤件_等待完成本案徵審,
                _ => throw new ArgumentException($"Invalid action: {action}"),
            };

        private void InitHandleData(Reviewer_ApplyCreditCardInfoHandle handle)
        {
            handle.RejectionReasonCode = null;
            handle.OtherRejectionReason = null;
            handle.RejectionNote = null;
            handle.RejectionSendCardAddr = null;
            handle.IsPrintSMSAndPaper = null;
            handle.SupplementReasonCode = null;
            handle.OtherSupplementReason = null;
            handle.SupplementNote = null;
            handle.SupplementSendCardAddr = null;
            handle.WithdrawalNote = null;
            handle.NuclearCardNote = null;
            handle.CardLimit = null;
            handle.IsForceCard = null;
            handle.HandleNote = null;
        }

        /* Tips: ManualReviewAction 是原本舊系統UI的動作，但因為太難懂了，
                所以我自己轉換成 CaseChangeAction 是新系統的動作，比較好懂
        */
        private CaseChangeAction MapToCaseChangeAction(ManualReviewAction action) =>
            action switch
            {
                ManualReviewAction.核卡作業 => CaseChangeAction.權限內_核卡作業,
                ManualReviewAction.退件作業 => CaseChangeAction.權限內_退件作業,
                ManualReviewAction.補件作業 => CaseChangeAction.權限內_補件作業,
                ManualReviewAction.撤件作業 => CaseChangeAction.權限內_撤件作業,
                _ => throw new ArgumentException($"Invalid action: {action}"),
            };

        // （移除 GetBankTraceNuclearCardErrors：改採就地檢核以提升可讀性）
    }
}
