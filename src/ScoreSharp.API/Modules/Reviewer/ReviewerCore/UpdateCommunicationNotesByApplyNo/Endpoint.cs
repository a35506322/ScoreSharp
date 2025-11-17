using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCommunicationNotesByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 修改溝通備註 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateCommunicationNotesByApplyNo/20250321G7943
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改溝通備註成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改溝通備註成功_2000_ResEx),
            typeof(修改溝通備註查無此申請書編號_4001_ResEx),
            typeof(修改溝通備註路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateCommunicationNotesByApplyNo")]
        public async Task<IResult> UpdateCommunicationNotesByApplyNo(
            [FromRoute] string applyNo,
            UpdateCommunicationNotesByApplyNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCommunicationNotesByApplyNo
{
    public record Command(string applyNo, UpdateCommunicationNotesByApplyNoRequest updateCommunicationNotesByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var dto = request.updateCommunicationNotesByApplyNoRequest;

            if (applyNo != dto.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var single = await context.Reviewer_InternalCommunicate.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            single.CommunicationNotes = dto.CommunicationNotes;
            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
