using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameIPCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 回傳相同IP檢核結果 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateSameIPCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新相同IP比對紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新相同IP比對紀錄成功_2000_ResEx),
            typeof(更新相同IP比對紀錄查無此申請書編號_4001_ResEx),
            typeof(更新相同IP比對紀錄路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateSameIPCheckLogByApplyNo")]
        public async Task<IResult> UpdateSameIPCheckLogByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateSameIPCheckLogByApplyNoRequest request
        ) => Results.Ok(await _mediator.Send(new Command(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameIPCheckLogByApplyNo
{
    public record Command(string applyNo, UpdateSameIPCheckLogByApplyNoRequest updateSameIPCheckLogByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var updateSameIPCheckLogByApplyNoRequest = request.updateSameIPCheckLogByApplyNoRequest;

            if (applyNo != updateSameIPCheckLogByApplyNoRequest.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(applyNo);

            var entity = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            // 只有當SameWebCaseEmailChecked = Y時才需要更新紀錄
            if (entity.SameIP_Flag != "Y")
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號 {applyNo} 的Email比對紀錄為 N，無法更新");

            entity.SameIP_CheckRecord = updateSameIPCheckLogByApplyNoRequest.CheckRecord;
            entity.SameIP_IsError = updateSameIPCheckLogByApplyNoRequest.IsError;
            entity.SameIP_UpdateTime = DateTime.Now;
            entity.SameIP_UpdateUserId = jwtProfilerHelper.UserId;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
