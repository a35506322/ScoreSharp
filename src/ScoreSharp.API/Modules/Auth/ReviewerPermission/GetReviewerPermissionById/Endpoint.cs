using ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionById;

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission
{
    public partial class ReviewerPermissionController
    {
        ///<summary>
        /// 查詢單筆徵審權限 By seqNo
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerPermission/GetReviewerPermissionById/4
        ///
        /// </remarks>
        /// <param name="seqNo">PK</param>
        /// <returns></returns>
        [HttpGet("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetReviewerPermissionByIdResponse>))]
        [EndpointSpecificExample(
            typeof(查詢單筆徵審權限_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(查詢單筆徵審權限查無此ID_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("GetReviewerPermissionById")]
        public async Task<IResult> GetReviewerPermissionById([FromRoute] int seqNo)
        {
            var result = await _mediator.Send(new Query(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionById
{
    public record Query(int seqNo) : IRequest<ResultResponse<GetReviewerPermissionByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<GetReviewerPermissionByIdResponse>>
    {
        public async Task<ResultResponse<GetReviewerPermissionByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var seqNo = request.seqNo;

            var entity = await context.Auth_ReviewerPermission.AsNoTracking().SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity == null)
                return ApiResponseHelper.NotFound<GetReviewerPermissionByIdResponse>(null, seqNo.ToString());

            var reponse = mapper.Map<GetReviewerPermissionByIdResponse>(entity);

            return ApiResponseHelper.Success(reponse);
        }
    }
}
