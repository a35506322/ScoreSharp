using ScoreSharp.API.Modules.Reviewer.ReviewManual.ReturnInManualReviewByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual
{
    public partial class ReviewManualController
    {
        ///<summary>
        /// 退回重審 By申請書編號 (退回給審查人員即可)
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewManual/ReturnInManualReviewByApplyNo/20240601A0001
        ///
        /// Notes:
        /// 1. 退回重審後，會將 main CurrentHandleUserId 設定為 PreviousHandleUserId(前手權限人員)
        ///
        /// </remarks>
        ///<returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(人工徵信_退回重審_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(人工徵信_退回重審_2000_ResEx),
            typeof(人工徵信_退回重審_卡片狀態不符合_4003_ResEx),
            typeof(人工徵信_退回重審_查無篩選最新指派卡片記錄_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ReturnInManualReviewByApplyNo")]
        [HttpPut("{applyNo}")]
        public async Task<IResult> ReturnInManualReviewByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] ReturnInManualReviewByApplyNoRequest request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request), cancellationToken);
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.ReturnInManualReviewByApplyNo
{
    public record Command(string applyNo, ReturnInManualReviewByApplyNoRequest request) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jWTProfilerHelper, ILogger<Handler> logger)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var updateRequest = request.request;
            var queryHandles = await context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo).ToListAsync();
            var main = await context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
            var currentUserId = jWTProfilerHelper.UserId;
            DateTime now = DateTime.Now;

            if (queryHandles.Count == 0 || main == null)
            {
                return ApiResponseHelper.NotFound<string>(null, "查無此申請書編號");
            }

            // 限定狀態才能將狀態改為退回重審　（申請核件中、申請退件中、申請補件中、申請撤件中）
            List<Reviewer_ApplyCreditCardInfoHandle> filterHandles = new();
            foreach (var handle in queryHandles)
            {
                if (
                    handle.CardStatus == CardStatus.申請核卡中
                    || handle.CardStatus == CardStatus.申請退件中
                    || handle.CardStatus == CardStatus.申請補件中
                    || handle.CardStatus == CardStatus.申請撤件中
                )
                {
                    filterHandles.Add(handle);
                }
                else
                {
                    logger.LogWarning("退回重審，申請書編號 {applyNo} 狀態 {cardStatus} 不符合，因此略過", applyNo, handle.CardStatus);
                }
            }

            if (filterHandles.Count == 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(
                    request.applyNo,
                    "執行退回重審，狀態只能為申請核卡中、申請退件中、申請補件中、申請撤件中"
                );
            }

            /* Tips:
                ## CurrentHandleUserId vs PreviousHandleUserId

                ### 規格

                | 欄位                 | 說明         |
                | -------------------- | ------------ |
                | CurrentHandleUserId  | 目前經辦人員 |
                | PreviousHandleUserId | 前手經辦人員 |

                ### 重點

                - 前手經辦人員於當下主管或系統進行派案時設定，用於退回重審時設定為該案件轉交人員
                - 當前經辦人員於當下主管或系統進行派案時設定，但他可能會被轉交，所以會一直更新
            */
            if (string.IsNullOrEmpty(main.PreviousHandleUserId))
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(request.applyNo, "執行退回重審，查無前手經辦人員");
            }

            main.CurrentHandleUserId = main.PreviousHandleUserId;
            main.LastUpdateUserId = currentUserId;
            main.LastUpdateTime = now;

            List<Reviewer_CardRecord> cardRecords = new();
            List<Reviewer_ApplyCreditCardInfoProcess> processRecords = new();

            foreach (var handle in filterHandles)
            {
                string userTypeName = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";
                string handleNote =
                    $"退回重審註記:{updateRequest.Note};轉交本案({main.PreviousHandleUserId});({userTypeName}_{handle.ApplyCardType})";

                handle.CardStatus = CardStatus.退回重審;
                handle.CaseChangeAction = null;
                handle.CreditCheckCode = null;
                handle.SupplementReasonCode = null;
                handle.OtherSupplementReason = null;
                handle.SupplementNote = null;
                handle.SupplementSendCardAddr = null;
                handle.WithdrawalNote = null;
                handle.RejectionReasonCode = null;
                handle.OtherRejectionReason = null;
                handle.RejectionNote = null;
                handle.RejectionSendCardAddr = null;
                handle.IsPrintSMSAndPaper = null;
                handle.IsOriginCardholderSameCardLimit = null;
                handle.CardLimit = null;
                handle.IsForceCard = null;
                handle.NuclearCardNote = null;
                handle.HandleNote = handleNote;
                handle.ApproveUserId = null;
                handle.ApproveTime = null;

                cardRecords.Add(
                    new Reviewer_CardRecord()
                    {
                        ApplyNo = handle.ApplyNo,
                        CardStatus = CardStatus.退回重審,
                        ApproveUserId = currentUserId,
                        AddTime = now,
                        ForwardedToUserId = main.PreviousHandleUserId,
                        HandleNote = handleNote,
                        HandleSeqNo = handle.SeqNo,
                        Action = ExecutionAction.退回重審,
                    }
                );

                processRecords.Add(
                    new Reviewer_ApplyCreditCardInfoProcess()
                    {
                        ApplyNo = handle.ApplyNo,
                        Process = CardStatus.退回重審.ToString(),
                        StartTime = now,
                        EndTime = now,
                        ProcessUserId = currentUserId,
                        Notes = handleNote,
                    }
                );
            }

            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processRecords);
            await context.Reviewer_CardRecord.AddRangeAsync(cardRecords);
            await context.SaveChangesAsync();

            return ApiResponseHelper.Success(data: request.applyNo, message: $"案件編號: {request.applyNo}，退回重審成功");
        }
    }
}
