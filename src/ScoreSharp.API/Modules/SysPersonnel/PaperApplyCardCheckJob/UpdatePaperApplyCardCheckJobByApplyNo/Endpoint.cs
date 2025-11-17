using ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.UpdatePaperApplyCardCheckJobByApplyNo;

namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob
{
    public partial class PaperApplyCardCheckJobController
    {
        /// <summary>
        /// 更新單筆紙本件案件檢核 By ApplyNo
        /// </summary>
        /// <remarks>
        ///
        /// Sample Route:
        ///
        ///     /PaperApplyCardCheckJob/UpdatePaperApplyCardCheckJobByApplyNo/20250709770001
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新單筆紙本件案件檢核_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新單筆紙本件案件檢核_2000_ResEx),
            typeof(更新單筆紙本件案件檢核查無資料_4001_ResEx),
            typeof(更新單筆紙本件案件檢核呼叫有誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdatePaperApplyCardCheckJobByApplyNo")]
        public async Task<IResult> UpdatePaperApplyCardCheckJobByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdatePaperApplyCardCheckJobByApplyNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.UpdatePaperApplyCardCheckJobByApplyNo
{
    public record Command(string applyNo, UpdatePaperApplyCardCheckJobByApplyNoRequest updatePaperApplyCardCheckJobByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            string applyNo = request.applyNo;
            var req = request.updatePaperApplyCardCheckJobByApplyNoRequest;

            if (applyNo != req.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = context.ReviewerPedding_PaperApplyCardCheckJob.SingleOrDefault(x => x.ApplyNo == applyNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            entity.IsQueryOriginalCardholderData = req.IsQueryOriginalCardholderData;
            entity.QueryOriginalCardholderDataLastTime = req.QueryOriginalCardholderDataLastTime;
            entity.IsCheckName = req.IsCheckName;
            entity.CheckNameLastTime = req.CheckNameLastTime;
            entity.IsCheck929 = req.IsCheck929;
            entity.Check929LastTime = req.Check929LastTime;
            entity.IsQueryBranchInfo = req.IsQueryBranchInfo;
            entity.QueryBranchInfoLastTime = req.QueryBranchInfoLastTime;
            entity.IsCheckFocus = req.IsCheckFocus;
            entity.CheckFocusLastTime = req.CheckFocusLastTime;
            entity.IsCheckShortTimeID = req.IsCheckShortTimeID;
            entity.CheckShortTimeIDLastTime = req.CheckShortTimeIDLastTime;
            entity.IsCheckInternalEmail = req.IsCheckInternalEmail;
            entity.CheckInternalEmailLastTime = req.CheckInternalEmailLastTime;
            entity.IsCheckInternalMobile = req.IsCheckInternalMobile;
            entity.CheckInternalMobileLastTime = req.CheckInternalMobileLastTime;
            entity.IsChecked = req.IsChecked;
            entity.ErrorCount = req.ErrorCount;
            entity.IsCheckRepeatApply = req.IsCheckRepeatApply;
            entity.CheckRepeatApplyLastTime = req.CheckRepeatApplyLastTime;

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.UpdateByIdSuccess<string>(applyNo, applyNo);
        }
    }
}
