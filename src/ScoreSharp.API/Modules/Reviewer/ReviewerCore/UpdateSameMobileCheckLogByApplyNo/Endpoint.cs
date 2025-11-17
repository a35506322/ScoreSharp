using ScoreSharp.API.Infrastructures.JWTToken;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameMobileCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 回覆手機號碼比對確認紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateSameMobileCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新相同手機號碼比對紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新相同手機號碼比對紀錄成功_2000_ResEx),
            typeof(更新相同手機號碼比對紀錄查無此申請書編號_4001_ResEx),
            typeof(更新相同手機號碼比對紀錄路由與Req比對錯誤_4003_ResEx),
            typeof(更新相同手機號碼比對紀錄申請書編號與Req不一致_4004_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateSameMobileCheckLogByApplyNo")]
        public async Task<IResult> UpdateSameMobileCheckLogByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateSameMobileCheckLogByApplyNoRequest request
        ) => Results.Ok(await _mediator.Send(new Command(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSameMobileCheckLogByApplyNo
{
    public record Command(string applyNo, UpdateSameMobileCheckLogByApplyNoRequest updateSameMobileCheckLogByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var _request = request.updateSameMobileCheckLogByApplyNoRequest;

            if (applyNo != _request.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(applyNo);

            var entity = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            // 只有當SameWebCaseMobileChecked = Y時才需要更新紀錄
            if (entity.SameMobile_Flag != "Y")
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號 {applyNo} 的手機號碼比對紀錄為 N，無法更新");

            entity.SameMobile_CheckRecord = _request.CheckRecord;
            entity.SameMobile_IsError = _request.IsError;
            entity.SameMobile_UpdateTime = DateTime.Now;
            entity.SameMobile_UpdateUserId = jwtProfilerHelper.UserId;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
