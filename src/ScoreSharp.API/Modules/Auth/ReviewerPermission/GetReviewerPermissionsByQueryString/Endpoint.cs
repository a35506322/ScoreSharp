using ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionsByQueryString;

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission
{
    public partial class ReviewerPermissionController
    {
        ///<summary>
        /// 查詢多筆徵審權限 By QueryString
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerPermission/GetReviewerPermissionsByQueryString
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetReviewerPermissionsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢多筆申請書權限_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetReviewerPermissionsByQueryString")]
        public async Task<IResult> GetReviewerPermissionsByQueryString([FromQuery] GetReviewerPermissionsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionsByQueryString
{
    public record Query(GetReviewerPermissionsByQueryStringRequest getReviewerPermissionsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetReviewerPermissionsByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetReviewerPermissionsByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetReviewerPermissionsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var entities = await context.Auth_ReviewerPermission.AsNoTracking().ToListAsync();

            var response = mapper.Map<List<GetReviewerPermissionsByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(response);
        }
    }
}
