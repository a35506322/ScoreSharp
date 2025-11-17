using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalMobileCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 修改_回覆行內手機重號確認紀錄
        /// </summary>
        /// <param name="applyNo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [OpenApiOperation("UpdateInternalMobileCheckLogByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新相同行內手機比對紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新相同行內手機比對紀錄成功_2000_ResEx),
            typeof(更新相同行內手機比對紀錄查無此申請書編號_4001_ResEx),
            typeof(更新相同行內手機比對紀錄路由與Req比對錯誤_4003_ResEx),
            typeof(更新相同行內手機比對紀錄_比對紀錄為N_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> UpdateInternalMobileCheckLogByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateInternalMobileCheckLogByApplyNoRequest request
        ) => Results.Ok(await _mediator.Send(new Command(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalMobileCheckLogByApplyNo
{
    public record Command(string applyNo, UpdateInternalMobileCheckLogByApplyNoRequest updateInternalMobileCheckLogByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            /*
             * 路由與Req比對
             * BankTrace查詢
             * 更新BankTrace資料InternalMobileSame_CheckRecord、InternalMobileSame_IsError、InternalMobileSame_UpdateTime(Now)、InternalMobileSame_UpdateUserId(currentUser)
             */

            var applyNo = request.applyNo;
            var req = request.updateInternalMobileCheckLogByApplyNoRequest;

            if (applyNo != req.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var bankTrace = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (bankTrace is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            // 只有當 InternalMobileSame_Flag = Y時才需要更新紀錄
            if (bankTrace.InternalMobileSame_Flag != "Y")
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"申請書編號 {applyNo} 的行內手機比對紀錄為 N，無法更新");

            bankTrace.InternalMobileSame_CheckRecord = req.CheckRecord;
            bankTrace.InternalMobileSame_IsError = req.IsError;
            bankTrace.InternalMobileSame_UpdateTime = DateTime.Now;
            bankTrace.InternalMobileSame_UpdateUserId = jwtProfilerHelper.UserId;
            if (req.IsError == "N")
            {
                bankTrace.InternalMobileSame_Relation = req.Relation;
            }

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.applyNo, request.applyNo);
        }
    }
}
