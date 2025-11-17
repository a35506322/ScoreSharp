using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyNoteByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得徵審行員備註資料 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetApplyNoteByApplyNo/20241128M4480
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetApplyNoteByApplyNoResponse>>))]
        [EndpointSpecificExample(
            typeof(取得徵審行員備註資料_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyNoteByApplyNo")]
        public async Task<IResult> GetApplyNoteByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyNoteByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetApplyNoteByApplyNoResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<List<GetApplyNoteByApplyNoResponse>>>
    {
        public async Task<ResultResponse<List<GetApplyNoteByApplyNoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            var userDic = await context.OrgSetUp_User.AsNoTracking().ToDictionaryAsync(x => x.UserId, x => x.UserName);

            var notes = await context.Reviewer_ApplyNote.Where(x => x.ApplyNo == applyNo).ToListAsync();

            if (notes.Count == 0)
            {
                return ApiResponseHelper.NotFound<List<GetApplyNoteByApplyNoResponse>>(null, applyNo);
            }

            var response = mapper.Map<List<GetApplyNoteByApplyNoResponse>>(notes);
            response = response
                .Select(x =>
                {
                    x.UpdateUserName = !string.IsNullOrEmpty(x.UpdateUserId)
                        ? (
                            x.UpdateUserId == "SYSTEM" ? "SYSTEM"
                            : userDic.TryGetValue(x.UpdateUserId, out var userName) ? userName
                            : null
                        )
                        : null;
                    return x;
                })
                .ToList();

            return ApiResponseHelper.Success(response);
        }
    }
}
