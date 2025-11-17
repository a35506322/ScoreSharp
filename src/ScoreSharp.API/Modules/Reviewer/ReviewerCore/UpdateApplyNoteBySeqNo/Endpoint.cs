using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateApplyNoteBySeqNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 修改徵審行員備註資料 ByPK
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateInternalIPCheckLogByApplyNo/20241128M4480
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改徵審行員備註資料成功_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改徵審行員備註資料成功_2000_ResEx),
            typeof(修改徵審行員備註資料查無此申請書編號_4001_ResEx),
            typeof(修改徵審行員備註資料路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateApplyNoteBySeqNo")]
        public async Task<IResult> UpdateApplyNoteBySeqNo([FromRoute] int seqNo, UpdateApplyNoteBySeqNoRequest request)
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateApplyNoteBySeqNo
{
    public record Command(int seqNo, UpdateApplyNoteBySeqNoRequest updateApplyNoteBySeqNoRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            int seqNo = request.seqNo;
            var updateApplyNoteBySeqNoRequest = request.updateApplyNoteBySeqNoRequest;

            if (seqNo != updateApplyNoteBySeqNoRequest.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var single = await context.Reviewer_ApplyNote.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            single.Note = updateApplyNoteBySeqNoRequest.Note;
            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(seqNo.ToString(), seqNo.ToString());
        }
    }
}
