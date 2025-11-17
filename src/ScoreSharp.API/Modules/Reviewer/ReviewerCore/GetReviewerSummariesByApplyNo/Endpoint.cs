using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetReviewerSummariesByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得徵審照會摘要 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetReviewerSummariesByApplyNo/20241126F5500
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetReviewerSummariesByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得徵審照會摘要_2000_ResEx),
            //typeof(取得徵審照會摘要查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetReviewerSummariesByApplyNo")]
        public async Task<IResult> GetReviewerSummariesByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetReviewerSummariesByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetReviewerSummariesByApplyNoResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetReviewerSummariesByApplyNoResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetReviewerSummariesByApplyNoResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var applyNo = request.applyNo;

            var userDic = await _context.OrgSetUp_User.ToDictionaryAsync(x => x.UserId, x => x.UserName);

            var entities = await _context.Reviewer_ReviewerSummary.Where(x => x.ApplyNo == applyNo).AsNoTracking().ToListAsync();

            var result = _mapper.Map<List<GetReviewerSummariesByApplyNoResponse>>(entities);
            result = result
                .Select(x =>
                {
                    x.AddUserName = x.AddUserId == "SYSTEM" ? "SYSTEM" : userDic.GetValueOrDefault(x.AddUserId);
                    x.UpdateUserName = x.UpdateUserId is null ? null : userDic.GetValueOrDefault(x.UpdateUserId);

                    return x;
                })
                .ToList();

            return ApiResponseHelper.Success(result);
        }
    }
}
