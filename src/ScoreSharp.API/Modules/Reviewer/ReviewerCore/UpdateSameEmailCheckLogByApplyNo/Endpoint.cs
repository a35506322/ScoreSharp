using ScoreSharp.API.Infrastructures.JWTToken;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameEmailCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 回覆Email比對確認紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateSameEmailCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新相同Email比對紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新相同Email比對紀錄成功_2000_ResEx),
            typeof(更新相同Email比對紀錄查無此申請書編號_4001_ResEx),
            typeof(更新相同Email比對紀錄路由與Req比對錯誤_4003_ResEx),
            typeof(更新相同Email比對紀錄申請書編號與Req不一致_4004_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateSameEmailCheckLogByApplyNo")]
        public async Task<IResult> UpdateSameEmailCheckLogByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateSameEmailCheckLogByApplyNoRequest request
        ) => Results.Ok(await _mediator.Send(new Command(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameEmailCheckLogByApplyNo
{
    public record Command(string applyNo, UpdateSameEmailCheckLogByApplyNoRequest updateSameEmailCheckLogByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var _request = request.updateSameEmailCheckLogByApplyNoRequest;

            if (applyNo != _request.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(applyNo);

            var entity = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            // 只有當SameWebCaseEmailChecked = Y時才需要更新紀錄
            if (entity.SameEmail_Flag != "Y")
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號 {applyNo} 的Email比對紀錄為 N，無法更新");

            entity.SameEmail_CheckRecord = _request.CheckRecord;
            entity.SameEmail_IsError = _request.IsError;
            entity.SameEmail_UpdateTime = DateTime.Now;
            entity.SameEmail_UpdateUserId = jwtProfilerHelper.UserId;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
