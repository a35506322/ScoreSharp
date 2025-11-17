using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateCaseChangeByApplyNo;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome
{
    public partial class ReviewerMonthlyIncomeController
    {
        /// <summary>
        /// 更新月收入確認案件異動
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerMonthlyIncome/UpdateCaseChangeByApplyNo/1234567890
        ///
        /// Notes :
        ///     1. 補件原因代碼 以及 退件原因代碼 以 string陣列 傳入
        ///     2. CaseChangeAction 狀態變更需要一致，不能有兩張補件，兩張退件
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <response code="400">
        /// 檢查
        ///
        /// - 動作與對應欄位
        /// 　1. 撤件作業
        /// 　   - 撤件註記為必填。
        /// 　2. 退件作業
        /// 　   - 退件原因代碼為必填。
        /// 　   - 退件寄送地址為必填，且需為有效地址。
        /// 　   - 是否列印簡訊、紙本通知函為必填。
        /// 　3. 補件作業
        /// 　   - 補件原因代碼為必填。
        /// 　   - 補件寄送地址為必填，且需為有效地址。
        /// 　   - 是否列印簡訊、紙本通知函為必填。
        /// </response>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(補件作業_2000_ReqEx),
            typeof(退件作業_2000_ReqEx),
            typeof(撤件作業_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(更新月收入確認案件異動資料_2000_ResEx),
            typeof(更新月收入確認案件異動資料_4003_ResEx),
            typeof(更新月收入確認案件異動資料_4001_ResEx),
            typeof(更新月收入確認案件異動資料狀態變更需要一致_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(更新月收入確認案件異動資料撤件註記必填_4000_ResEx),
            typeof(更新月收入確認案件異動資料退件原因代碼必填_4000_ResEx),
            typeof(更新月收入確認案件異動資料退件寄送地址必填_4000_ResEx),
            typeof(更新月收入確認案件異動資料補件原因代碼必填_4000_ResEx),
            typeof(更新月收入確認案件異動資料補件寄送地址必填_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("UpdateCaseChangeByApplyNo")]
        public async Task<IResult> UpdateCaseChangeByApplyNo([FromRoute] string applyNo, [FromBody] List<UpdateCaseChangeByApplyNoRequest> request)
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateCaseChangeByApplyNo
{
    public record Command(string applyNo, List<UpdateCaseChangeByApplyNoRequest> updateCaseChangeByApplyNoRequest) : IRequest<ResultResponse<string>>;

    public class Handler(
        ScoreSharpContext context,
        IJWTProfilerHelper jwtHelper,
        IScoreSharpDapperContext dapperContext,
        IReviewerHelper reviewerHelper
    ) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dtos = request.updateCaseChangeByApplyNoRequest;
            var applyNo = request.applyNo;

            bool allApplyNosMatch = dtos.TrueForAll(x => x.ApplyNo == applyNo);
            if (!allApplyNosMatch)
            {
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);
            }

            /*
                🔔 20250709 狀態變更需要一致
                詢問過美娟，在月收入確認階段所有的狀態變更都需要一致
                如果有四張卡片，就要選擇一樣的狀態，不能有兩張補件，兩張退件
                如果真的有需要個別退件或撤件，目前會到人工在處理

                TODO: 20250709 討論完成月收入狀態是否要一致變更
            */
            var statusList = dtos.Select(x => x.CaseChangeAction).Distinct().ToList();
            if (statusList.Count > 1)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "狀態變更需要一致");
            }

            var (rejectionReasonList, supplementReasonList) = await QueryParms();
            var main = await context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == request.applyNo);

            if (main is null)
            {
                return ApiResponseHelper.NotFound<string>(null, request.applyNo);
            }

            // 驗證資料
            var supplementary = await context
                .Reviewer_ApplyCreditCardInfoSupplementary.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplyNo == request.applyNo);
            List<string> businessLogicFailedMessages = new List<string>();
            foreach (var dto in dtos)
            {
                if (dto.CaseChangeAction == IncomeConfirmationAction.退件作業)
                {
                    // 1. 驗證退件原因代碼
                    if (!(dto.RejectionReasonCode is null || dto.RejectionReasonCode?.Length == 0))
                    {
                        bool isAllValid = dto.RejectionReasonCode.All(item => rejectionReasonList.Contains(item));
                        if (!isAllValid)
                        {
                            businessLogicFailedMessages.Add($"含有無效退件原因代碼，請重新確認。({dto.SeqNo})");
                        }
                    }
                    // 2. 驗證退件寄送地址
                    if (dto.RejectionSendCardAddr != null)
                    {
                        // 3. 驗證地址資料完整性
                        var isCompleteAddress = reviewerHelper.檢查對應地址(main, dto.RejectionSendCardAddr.Value);

                        if (!isCompleteAddress)
                        {
                            businessLogicFailedMessages.Add($"月收入確認_退件寄送地址填寫不完整，請重新確認。({dto.SeqNo})");
                        }
                    }
                }
                else if (dto.CaseChangeAction == IncomeConfirmationAction.補件作業)
                {
                    if (!(dto.SupplementReasonCode is null || dto.SupplementReasonCode?.Length == 0))
                    {
                        bool isAllValid = dto.SupplementReasonCode.All(item => supplementReasonList.Contains(item));
                        if (!isAllValid)
                        {
                            businessLogicFailedMessages.Add($"含有無效補件原因代碼，請重新確認。({dto.SeqNo})");
                        }
                    }
                    if (dto.SupplementSendCardAddr != null)
                    {
                        // 3. 驗證地址資料完整性
                        var isCompleteAddress = reviewerHelper.檢查對應地址(main, dto.SupplementSendCardAddr.Value);

                        if (!isCompleteAddress)
                        {
                            businessLogicFailedMessages.Add($"月收入確認_補件寄送地址填寫不完整，請重新確認。({dto.SeqNo})");
                        }
                    }
                }
            }
            ValidateContext model = new ValidateContext
            {
                Email = main.EMail,
                M_BirthDay = main.BirthDay,
                M_IDIssueDate = main.IDIssueDate,
                M_ResidencePermitDeadline = main.ResidencePermitDeadline,
                M_ResidencePermitIssueDate = main.ResidencePermitIssueDate,
                M_PassportDate = main.PassportDate,
                M_ExpatValidityPeriod = main.ExpatValidityPeriod,
                IsCompleteMonthlyIncome = false,
            };

            if (supplementary is not null)
            {
                model.S1_BirthDay = supplementary.BirthDay;
                model.S1_ID = supplementary.ID;
                model.S1_IDIssueDate = supplementary.IDIssueDate;
                model.S1_ResidencePermitIssueDate = supplementary.ResidencePermitIssueDate;
                model.S1_ExpatValidityPeriod = supplementary.ExpatValidityPeriod;
                model.S1_PassportDate = supplementary.PassportDate;
                model.S1_ResidencePermitDeadline = supplementary.ResidencePermitDeadline;
            }
            var (isValid, validationErrors) = model.ValidateCompletely();
            if (!isValid)
            {
                businessLogicFailedMessages.AddRange(validationErrors.Select(x => x.ErrorMessage));
            }

            // 檢查正卡人地址
            var (isValidMainAddr, mainAddrErrors) = reviewerHelper.檢查正卡人必填地址(main);
            businessLogicFailedMessages.AddRange(mainAddrErrors);

            if (main.CardOwner != CardOwner.正卡)
            {
                var (isValidSuppAddr, suppAddrErrors) = reviewerHelper.檢查附卡人必填地址(supplementary);
                businessLogicFailedMessages.AddRange(suppAddrErrors);
            }

            if (businessLogicFailedMessages.Count > 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, string.Join(Environment.NewLine, businessLogicFailedMessages));
            }

            var handleList = await context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == request.applyNo).ToListAsync();

            if (handleList.Count == 0)
            {
                return ApiResponseHelper.NotFound<string>(null, request.applyNo);
            }

            foreach (var handle in handleList)
            {
                var dto = dtos.FirstOrDefault(x => x.SeqNo == handle.SeqNo);
                var supplementCount_MonthlyIncome = await context
                    .Reviewer_CardRecord.Where(x => x.HandleSeqNo == handle.SeqNo && x.CardStatus == CardStatus.補件作業中)
                    .CountAsync();

                if (dto.CaseChangeAction == IncomeConfirmationAction.補件作業 && supplementCount_MonthlyIncome == 2)
                {
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, $"卡片已達補件上限(2)，無法執行儲存補件作業。({handle.SeqNo})");
                }
            }

            var now = DateTime.Now;

            // update main
            main.LastUpdateUserId = jwtHelper.UserId;
            main.LastUpdateTime = now;

            // update handle
            var processList = new List<Reviewer_ApplyCreditCardInfoProcess>();
            var cardRecords = new List<Reviewer_CardRecord>();
            foreach (var dto in dtos)
            {
                var handle = handleList.FirstOrDefault(x => x.SeqNo == dto.SeqNo);
                (CardStatus status, string notes) = 取得狀態與歷程備註(dto.CaseChangeAction, dto);

                handle.CardStatus = status;
                handle.CaseChangeAction = 轉換成CaseChangeAction(dto.CaseChangeAction);
                handle.IsPrintSMSAndPaper = dto.IsPrintSMSAndPaper;

                switch (dto.CaseChangeAction)
                {
                    case IncomeConfirmationAction.撤件作業:
                        // 撤件作業時，清空與退件和補件相關的欄位
                        handle.SupplementReasonCode = null;
                        handle.OtherSupplementReason = null;
                        handle.SupplementNote = null;
                        handle.SupplementSendCardAddr = null;
                        handle.RejectionReasonCode = null;
                        handle.OtherRejectionReason = null;
                        handle.RejectionNote = null;
                        handle.RejectionSendCardAddr = null;
                        handle.WithdrawalNote = dto.WithdrawalNote;
                        handle.IsPrintSMSAndPaper = null;
                        break;

                    case IncomeConfirmationAction.退件作業:
                        // 退件作業時，清空與補件和撤件相關的欄位
                        handle.SupplementReasonCode = null;
                        handle.OtherSupplementReason = null;
                        handle.SupplementNote = null;
                        handle.SupplementSendCardAddr = null;
                        handle.WithdrawalNote = null;
                        handle.IsPrintSMSAndPaper = dto.IsPrintSMSAndPaper;
                        handle.RejectionReasonCode = 代碼陣列轉換成字串(dto.RejectionReasonCode);
                        handle.OtherRejectionReason = dto.OtherRejectionReason;
                        handle.RejectionNote = dto.RejectionNote;
                        handle.RejectionSendCardAddr = dto.RejectionSendCardAddr;
                        break;

                    case IncomeConfirmationAction.補件作業:
                        // 補件作業時，清空與退件和撤件相關的欄位
                        handle.RejectionReasonCode = null;
                        handle.OtherRejectionReason = null;
                        handle.RejectionNote = null;
                        handle.RejectionSendCardAddr = null;
                        handle.WithdrawalNote = null;
                        handle.IsPrintSMSAndPaper = dto.IsPrintSMSAndPaper;
                        handle.SupplementReasonCode = 代碼陣列轉換成字串(dto.SupplementReasonCode);
                        handle.OtherSupplementReason = dto.OtherSupplementReason;
                        handle.SupplementNote = dto.SupplementNote;
                        handle.SupplementSendCardAddr = dto.SupplementSendCardAddr;
                        break;
                }

                string userTypeName = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";

                var process = new Reviewer_ApplyCreditCardInfoProcess()
                {
                    ApplyNo = dto.ApplyNo,
                    Process = status.ToString(),
                    StartTime = now,
                    EndTime = now,
                    Notes = notes + $"({userTypeName}_{handle.ApplyCardType})",
                    ProcessUserId = jwtHelper.UserId,
                };
                processList.Add(process);

                var cardRecord = new Reviewer_CardRecord
                {
                    ApplyNo = request.applyNo,
                    CardStatus = status,
                    CardLimit = null,
                    HandleNote = notes + $"({userTypeName}_{handle.ApplyCardType})",
                    HandleSeqNo = handle.SeqNo,
                    ApproveUserId = jwtHelper.UserId,
                };

                cardRecords.Add(cardRecord);
            }

            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processList);
            await context.Reviewer_CardRecord.AddRangeAsync(cardRecords);
            await context.SaveChangesAsync();
            return ApiResponseHelper.UpdateByIdSuccess(request.applyNo, request.applyNo);
        }

        private (CardStatus Status, string Notes) 取得狀態與歷程備註(IncomeConfirmationAction action, UpdateCaseChangeByApplyNoRequest dto)
        {
            CardStatus status = action switch
            {
                IncomeConfirmationAction.補件作業 => CardStatus.補件_等待完成本案徵審,
                IncomeConfirmationAction.撤件作業 => CardStatus.撤件_等待完成本案徵審,
                IncomeConfirmationAction.退件作業 => CardStatus.退件_等待完成本案徵審,
            };

            string notes = action switch
            {
                IncomeConfirmationAction.補件作業 => $"補件原因：{代碼陣列轉換成字串(dto.SupplementReasonCode)}(TO:{dto.SupplementSendCardAddr})"
                    + 備註註記(dto.SupplementNote),
                IncomeConfirmationAction.撤件作業 => $"撤件註記：{dto.WithdrawalNote}",
                IncomeConfirmationAction.退件作業 => $"退件原因：{代碼陣列轉換成字串(dto.RejectionReasonCode)}(TO:{dto.RejectionSendCardAddr})"
                    + 備註註記(dto.RejectionNote),
            };

            return (status, notes);
        }

        private string? 代碼陣列轉換成字串(string[]? codeArray)
        {
            if (codeArray is null || codeArray.Length == 0)
                return null;
            return string.Join(",", codeArray);
        }

        private async Task<(List<string> rejectionReasonList, List<string> supplementReasonList)> QueryParms()
        {
            // 將資料庫名稱寫在settings中
            string sql =
                @"
                       SELECT RejectionReasonCode
                       FROM [dbo].[SetUp_RejectionReason]
                       WHERE IsActive = 'Y';

                       SELECT SupplementReasonCode
                       FROM [dbo].[SetUp_SupplementReason]
                       WHERE IsActive = 'Y';
               ";

            using var connection = dapperContext.CreateScoreSharpConnection();
            using var multiQuery = await connection.QueryMultipleAsync(sql);
            var rejectionReasonList = multiQuery.Read<string>().ToList();
            var supplementReasonList = multiQuery.Read<string>().ToList();

            return (rejectionReasonList, supplementReasonList);
        }

        private CaseChangeAction 轉換成CaseChangeAction(IncomeConfirmationAction action)
        {
            return action switch
            {
                IncomeConfirmationAction.補件作業 => CaseChangeAction.權限內_補件作業,
                IncomeConfirmationAction.撤件作業 => CaseChangeAction.權限內_撤件作業,
                IncomeConfirmationAction.退件作業 => CaseChangeAction.權限內_退件作業,
                _ => throw new ArgumentException($"Invalid action: {action}"),
            };
        }

        private string 備註註記(string note)
        {
            return (string.IsNullOrWhiteSpace(note) ? "" : $"註記：{note}");
        }
    }
}
