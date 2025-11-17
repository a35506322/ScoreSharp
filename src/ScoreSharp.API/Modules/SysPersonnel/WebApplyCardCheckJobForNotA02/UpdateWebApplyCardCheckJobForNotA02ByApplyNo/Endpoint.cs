using ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02.UpdateWebApplyCardCheckJobForNotA02ByApplyNo;

namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02
{
    public partial class WebApplyCardCheckJobForNotA02Controller
    {
        /// <summary>
        /// 更新單筆網路件非卡友案件檢核 By ApplyNo
        /// </summary>
        /// <remarks>
        ///
        /// Sample Route:
        ///
        ///     /WebApplyCardCheckJobForNotA02/UpdateWebApplyCardCheckJobForNotA02ByApplyNo/20250710B1124
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新單筆網路件非卡友案件檢核_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新單筆網路件非卡友案件檢核_2000_ResEx),
            typeof(更新單筆網路件非卡友案件檢核查無資料_4001_ResEx),
            typeof(更新單筆網路件非卡友案件檢核呼叫有誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateWebApplyCardCheckJobForNotA02ByApplyNo")]
        public async Task<IResult> UpdateWebApplyCardCheckJobForNotA02ByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateWebApplyCardCheckJobForNotA02ByApplyNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02.UpdateWebApplyCardCheckJobForNotA02ByApplyNo
{
    public record Command(string applyNo, UpdateWebApplyCardCheckJobForNotA02ByApplyNoRequest updateWebApplyCardCheckJobForNotA02ByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            string applyNo = request.applyNo;
            var req = request.updateWebApplyCardCheckJobForNotA02ByApplyNoRequest;

            if (applyNo != req.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.ReviewerPedding_WebApplyCardCheckJobForNotA02.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            entity.IsCheckName = req.IsCheckName;
            entity.CheckNameLastTime = req.CheckNameLastTime;
            entity.IsQueryBranchInfo = req.IsQueryBranchInfo;
            entity.QueryBranchInfoLastTime = req.QueryBranchInfoLastTime;
            entity.IsCheck929 = req.IsCheck929;
            entity.Check929LastTime = req.Check929LastTime;
            entity.IsCheckSameIP = req.IsCheckSameIP;
            entity.CheckSameIPLastTime = req.CheckSameIPLastTime;
            entity.IsCheckEqualInternalIP = req.IsCheckEqualInternalIP;
            entity.CheckEqualInternalIPLastTime = req.CheckEqualInternalIPLastTime;
            entity.IsChecked = req.IsChecked;
            entity.ErrorCount = req.ErrorCount;
            entity.IsCheckSameWebCaseEmail = req.IsCheckSameWebCaseEmail;
            entity.CheckSameWebCaseEmailLastTime = req.CheckSameWebCaseEmailLastTime;
            entity.IsCheckSameWebCaseMobile = req.IsCheckSameWebCaseMobile;
            entity.CheckSameWebCaseMobileLastTime = req.CheckSameWebCaseMobileLastTime;
            entity.IsCheckFocus = req.IsCheckFocus;
            entity.CheckFocusLastTime = req.CheckFocusLastTime;
            entity.IsCheckShortTimeID = req.IsCheckShortTimeID;
            entity.CheckShortTimeIDLastTime = req.CheckShortTimeIDLastTime;
            entity.IsCheckInternalEmail = req.IsCheckInternalEmail;
            entity.CheckInternalEmailLastTime = req.CheckInternalEmailLastTime;
            entity.IsCheckInternalMobile = req.IsCheckInternalMobile;
            entity.CheckInternalMobileLastTime = req.CheckInternalMobileLastTime;
            entity.IsCheckRepeatApply = req.IsCheckRepeatApply;
            entity.CheckRepeatApplyLastTime = req.CheckRepeatApplyLastTime;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(applyNo, applyNo);
        }
    }
}
