using ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderByQueryString;

namespace ScoreSharp.API.Modules.Manage.Stakeholder
{
    public partial class StakeholderController
    {
        /// <summary>
        /// 查詢多筆利害關係人
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?userId=user123&amp;isActive=Y
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetStakeholderByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢多筆利害關係人_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetStakeholderByQueryString")]
        public async Task<IResult> GetStakeholderByQueryString([FromQuery] GetStakeholderByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderByQueryString
{
    public record Query(GetStakeholderByQueryStringRequest getStakeholderByQueryStringRequest)
        : IRequest<ResultResponse<List<GetStakeholderByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetStakeholderByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetStakeholderByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dto = request.getStakeholderByQueryStringRequest;

            var entities = await context
                .Reviewer_Stakeholder.AsNoTracking()
                .Where(x => (string.IsNullOrEmpty(dto.UserId) || x.UserId == dto.UserId))
                .Where(x => (string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive))
                .ToListAsync();

            var response = mapper.Map<List<GetStakeholderByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(response.ToList());
        }
    }
}
