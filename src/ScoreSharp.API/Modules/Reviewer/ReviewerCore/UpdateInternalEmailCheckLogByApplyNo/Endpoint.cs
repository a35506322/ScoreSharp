using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalEmailCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 修改_回覆行內EMail重號確認紀錄
        /// </summary>
        /// <param name="applyNo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [OpenApiOperation("UpdateInternalEmailCheckLogByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新相同行內Email比對紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新相同行內Email比對紀錄成功_2000_ResEx),
            typeof(更新相同行內Email比對紀錄查無此申請書編號_4001_ResEx),
            typeof(更新相同行內Email比對紀錄路由與Req比對錯誤_4003_ResEx),
            typeof(更新相同行內Email比對紀錄_比對紀錄為N_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> UpdateInternalEmailCheckLogByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateInternalEmailCheckLogByApplyNoRequest request
        ) => Results.Ok(await _mediator.Send(new Command(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalEmailCheckLogByApplyNo
{
    public record Command(string applyNo, UpdateInternalEmailCheckLogByApplyNoRequest updateInternalEmailCheckLogByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            /*
             * 路由與Req比對
             * BankTrace查詢
             * 更新BankTrace資料InternalEmailSame_CheckRecord、InternalEmailSame_IsError、InternalEmailSame_UpdateTime(now)、InternalEmailSame_UpdateUserId(currentUser)
             */

            var req = request.updateInternalEmailCheckLogByApplyNoRequest;

            if (request.applyNo != req.ApplyNo)
            {
                return ApiResponseHelper.路由與Req比對錯誤<string>(req.ApplyNo);
            }
            var entity = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == req.ApplyNo && x.UserType == UserType.正卡人);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, req.ApplyNo);

            // 只有當 InternalEmailSame_Flag = Y時才需要更新紀錄
            if (entity.InternalEmailSame_Flag != "Y")
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"申請書編號 {req.ApplyNo} 的行內Email比對紀錄為 N，無法更新");

            entity.InternalEmailSame_CheckRecord = req.CheckRecord;
            entity.InternalEmailSame_IsError = req.IsError;
            entity.InternalEmailSame_UpdateTime = DateTime.Now;
            entity.InternalEmailSame_UpdateUserId = jwtProfilerHelper.UserId;
            if (req.IsError == "N")
            {
                entity.InternalEmailSame_Relation = req.Relation;
            }

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(req.ApplyNo, req.ApplyNo);
        }
    }
}
