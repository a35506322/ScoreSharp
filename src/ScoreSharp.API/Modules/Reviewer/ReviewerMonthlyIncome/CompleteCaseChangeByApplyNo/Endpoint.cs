using ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.CompleteCaseChangeByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome
{
    public partial class ReviewerMonthlyIncomeController
    {
        /// <summary>
        /// 完成月收入確認案件異動
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerMonthlyIncome/CompleteCaseChangeByApplyNo/1234567890
        ///
        /// 此API僅將卡片狀態更新為對應的作業狀態
        /// 1. 退件_等待完成本案徵審 => 退件作業中_終止狀態
        /// 2. 撤件_等待完成本案徵審 => 撤件作業中_終止狀態
        /// 3. 補件_等待完成本案徵審 => 補件作業中
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(完成月收入確認案件異動_2000_ResEx),
            typeof(完成月收入確認案件異動_卡片狀態錯誤請先按儲存_4003_ResEx),
            typeof(完成月收入確認案件異動_所有狀態需相同請先按儲存_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("CompleteCaseChangeByApplyNo")]
        public async Task<IResult> CompleteCaseChangeByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Command(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.CompleteCaseChangeByApplyNo
{
    public record Command(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper _jwtHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == request.applyNo);
            var dbHandles = await context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == request.applyNo).ToListAsync();

            var filiterStatusHandles = dbHandles
                .Where(x =>
                    x.CardStatus == CardStatus.補件_等待完成本案徵審
                    || x.CardStatus == CardStatus.退件_等待完成本案徵審
                    || x.CardStatus == CardStatus.撤件_等待完成本案徵審
                )
                .ToList();

            if (main is null)
            {
                return ApiResponseHelper.NotFound<string>(null, request.applyNo);
            }

            if (filiterStatusHandles.Count == 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "卡片狀態錯誤，請先按儲存，再進行權限內本案徵審");
            }
            else if (filiterStatusHandles.DistinctBy(x => x.CardStatus).Count() > 1)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "卡片狀態錯誤，所有狀態需相同，請先按儲存，再進行完成月收入確認");
            }

            List<Reviewer_ApplyCreditCardInfoProcess> processList = new();
            List<Reviewer_CardRecord> cardRecords = new();
            DateTime now = DateTime.Now;

            foreach (var handle in filiterStatusHandles)
            {
                handle.CardStatus = 轉換下一步狀態(handle.CardStatus);

                if (handle.CardStatus == CardStatus.補件作業中 && handle.IsPrintSMSAndPaper == "Y")
                {
                    handle.BatchSupplementStatus = "N";
                    handle.BatchSupplementTime = null;
                }

                if (handle.CardStatus == CardStatus.退件作業中_終止狀態 && handle.IsPrintSMSAndPaper == "Y")
                {
                    handle.BatchRejectionStatus = "N";
                    handle.BatchRejectiontTime = null;
                }
                if (
                    handle.CardStatus == CardStatus.撤件作業中_終止狀態
                    || handle.CardStatus == CardStatus.退件作業中_終止狀態
                    || handle.CardStatus == CardStatus.補件作業中
                )
                {
                    handle.ApproveUserId = _jwtHelper.UserId;
                    handle.ApproveTime = now;
                }

                // insert log
                string userType = handle.UserType == UserType.正卡人 ? "正卡" : "附卡";
                var process = new Reviewer_ApplyCreditCardInfoProcess
                {
                    ApplyNo = request.applyNo,
                    Process = handle.CardStatus.ToString(),
                    StartTime = now,
                    EndTime = now,
                    Notes = $"完成本案({userType}_{handle.ApplyCardType})",
                    ProcessUserId = _jwtHelper.UserId,
                };
                processList.Add(process);

                var cardRecord = new Reviewer_CardRecord
                {
                    ApplyNo = request.applyNo,
                    CardStatus = handle.CardStatus,
                    CardLimit = null,
                    HandleNote = $"完成本案({userType}_{handle.ApplyCardType})",
                    HandleSeqNo = handle.SeqNo,
                    ApproveUserId = _jwtHelper.UserId,
                };

                cardRecords.Add(cardRecord);
            }

            /*
                 如果退件或撤件，因為沒有人可以再碰此專案，所以main.CurrentHandleUserId = null;
                 如果補件，狀態不同
                 月收入確認階段，會採取重新派案，所以main.CurrentHandleUserId = null;
                 人工徵新階段，會為同一人所以main.CurrentHandleUserId = _jwtHelper.UserId;
            */
            main.CurrentHandleUserId = null;
            main.LastUpdateUserId = _jwtHelper.UserId;
            main.LastUpdateTime = now;

            // 此案件中所有卡片都是終止狀態才清空前手經辦
            if (dbHandles.All(x => x.CardStatus == CardStatus.退件作業中_終止狀態 || x.CardStatus == CardStatus.撤件作業中_終止狀態))
            {
                main.PreviousHandleUserId = null;
            }

            await context.Reviewer_CardRecord.AddRangeAsync(cardRecords);
            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processList);
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.Success(request.applyNo, $"完成月收入確認案件異動，申請書編號：{request.applyNo}");
        }

        private CardStatus 轉換下一步狀態(CardStatus status)
        {
            return status switch
            {
                CardStatus.退件_等待完成本案徵審 => CardStatus.退件作業中_終止狀態,
                CardStatus.撤件_等待完成本案徵審 => CardStatus.撤件作業中_終止狀態,
                CardStatus.補件_等待完成本案徵審 => CardStatus.補件作業中,
            };
        }
    }
}
