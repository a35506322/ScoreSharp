using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementContactRecordsByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 修改補聯繫紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateSupplementContactRecordsByApplyNo/20250321G7943
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改補聯繫紀錄成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改補聯繫紀錄成功_2000_ResEx),
            typeof(修改補聯繫紀錄查無此申請書編號_4001_ResEx),
            typeof(修改補聯繫紀錄路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateSupplementContactRecordsByApplyNo")]
        public async Task<IResult> UpdateSupplementContactRecordsByApplyNo(
            [FromRoute] string applyNo,
            UpdateSupplementContactRecordsByApplyNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementContactRecordsByApplyNo
{
    public record Command(string applyNo, UpdateSupplementContactRecordsByApplyNoRequest updateSupplementContactRecordsByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            string applyNo = request.applyNo;
            var dto = request.updateSupplementContactRecordsByApplyNoRequest;

            if (applyNo != dto.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var single = await context.Reviewer_InternalCommunicate.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            single.SupplementContactRecords_Type = dto.SupplementContactRecords_Type;
            single.SupplementContactRecords_Result = dto.SupplementContactRecords_Result;
            single.SupplementContactRecords_Summary = dto.SupplementContactRecords_Summary;
            single.SupplementContactRecords_UserId = jwtHelper.UserId;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
