using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalIPCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 回覆行內IP比對確認紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateInternalIPCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新行內IP比對紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新行內IP比對紀錄成功_2000_ResEx),
            typeof(更新行內IP比對紀錄查無此申請書編號_4001_ResEx),
            typeof(更新行內IP比對紀錄路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateInternalIPCheckLogByApplyNo")]
        public async Task<IResult> UpdateInternalIPCheckLogByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateInternalIPCheckLogByApplyNoRequest request
        ) => Results.Ok(await _mediator.Send(new Command(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateInternalIPCheckLogByApplyNo
{
    public record Command(string applyNo, UpdateInternalIPCheckLogByApplyNoRequest updateInternalIPCheckLogByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var updateInternalIPCheckLogByApplyNoRequest = request.updateInternalIPCheckLogByApplyNoRequest;

            if (applyNo != updateInternalIPCheckLogByApplyNoRequest.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(applyNo);

            var digistCheckInfo = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (digistCheckInfo is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            digistCheckInfo.EqualInternalIP_CheckRecord = updateInternalIPCheckLogByApplyNoRequest.CheckRecord;
            digistCheckInfo.EqualInternalIP_IsError = updateInternalIPCheckLogByApplyNoRequest.IsError;
            digistCheckInfo.EqualInternalIP_UpdateTime = DateTime.Now;
            digistCheckInfo.EqualInternalIP_UpdateUserId = jwtProfilerHelper.UserId;
            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
