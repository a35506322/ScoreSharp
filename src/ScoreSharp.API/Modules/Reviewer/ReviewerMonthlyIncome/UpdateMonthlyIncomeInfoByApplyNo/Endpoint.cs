using ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateMonthlyIncomeInfoByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome
{
    public partial class ReviewerMonthlyIncomeController
    {
        /// <summary>
        /// 更新月收入簽核資料
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerMonthlyIncome/UpdateMonthlyIncomeInfoByApplyNo/1234567890
        ///
        /// 檢查與資料庫定義值相同
        ///
        ///     CreditCheckCode (徵信代碼) => 關聯 SetUp_CreditCheckCode
        ///
        /// Notes :
        ///     1. 所有卡片的徵信代碼皆會一致更新同個值
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新月收入簽核資料_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新月收入簽核資料_2000_ResEx),
            typeof(更新月收入簽核資料_4001_ResEx),
            typeof(更新月收入簽核資料_4003_ResEx),
            typeof(更新月收入簽核資料查無定義值_4003_ResEx),
            typeof(更新月收入簽核資料卡片狀態錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateMonthlyIncomeInfoByApplyNo")]
        public async Task<IResult> UpdateMonthlyIncomeInfoByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateMonthlyIncomeInfoByApplyNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateMonthlyIncomeInfoByApplyNo
{
    public record Command(string applyNo, UpdateMonthlyIncomeInfoByApplyNoRequest updateRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.updateRequest.ApplyNo != request.applyNo)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號與Req比對錯誤");
            }

            if (!string.IsNullOrEmpty(request.updateRequest.CreditCheckCode))
            {
                var isExist = await context
                    .SetUp_CreditCheckCode.AsNoTracking()
                    .AnyAsync(x => x.CreditCheckCode == request.updateRequest.CreditCheckCode);
                if (!isExist)
                {
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, "徵信代碼不存在");
                }
            }

            var handles = await context
                .Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == request.applyNo)
                .Where(x => x.CardStatus == CardStatus.網路件_待月收入預審 || x.CardStatus == CardStatus.紙本件_待月收入預審)
                .ToListAsync();

            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == request.applyNo);

            if (main is null)
                return ApiResponseHelper.NotFound<string>(null, request.applyNo);

            if (handles.Count == 0)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "卡片狀態錯誤，無待月收入預審狀態，無法更新月收入簽核資料");

            // Tips 所有卡片的徵信代碼皆會一致
            var creditCheckCode = request.updateRequest.CreditCheckCode;
            foreach (var handle in handles)
            {
                handle.CreditCheckCode = request.updateRequest.CreditCheckCode;
            }
            main.CurrentMonthIncome = request.updateRequest.CurrentMonthIncome;
            main.LastUpdateUserId = jwtHelper.UserId;
            main.LastUpdateTime = DateTime.Now;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.applyNo, request.applyNo);
        }
    }
}
