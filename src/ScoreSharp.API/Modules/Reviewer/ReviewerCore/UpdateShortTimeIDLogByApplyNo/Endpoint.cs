using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateShortTimeIDLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 回覆頻繁申請ID確認紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateShortTimeIDLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(回覆頻繁申請ID確認紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(回覆頻繁申請ID確認紀錄成功_2000_ResEx),
            typeof(回覆頻繁申請ID確認紀錄查無此申請書編號_4001_ResEx),
            typeof(回覆頻繁申請ID確認紀錄路由與Req比對錯誤_4003_ResEx),
            typeof(回覆頻繁申請ID確認紀錄商業邏輯驗證失敗_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateShortTimeIDLogByApplyNo")]
        public async Task<IResult> UpdateShortTimeIDLogByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateShortTimeIDLogByApplyNoRequest request
        ) => Results.Ok(await _mediator.Send(new Command(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateShortTimeIDLogByApplyNo
{
    public record Command(string applyNo, UpdateShortTimeIDLogByApplyNoRequest updateShortTimeIDLogByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var updateShortTimeIDLogByApplyNoRequest = request.updateShortTimeIDLogByApplyNoRequest;

            if (applyNo != updateShortTimeIDLogByApplyNoRequest.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(applyNo);

            var bankTrace = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (bankTrace is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            if (bankTrace.ShortTimeID_Flag != "Y")
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"申請書編號 {applyNo} 的短時間頻繁申請紀錄為 N，無法更新");

            bankTrace.ShortTimeID_CheckRecord = updateShortTimeIDLogByApplyNoRequest.CheckRecord;
            bankTrace.ShortTimeID_IsError = updateShortTimeIDLogByApplyNoRequest.IsError;
            bankTrace.ShortTimeID_UpdateUserId = jwtProfilerHelper.UserId;
            bankTrace.ShortTimeID_UpdateTime = DateTime.Now;
            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
