using ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForA02.GetWebApplyCardCheckJobForA02sByQueryString;

namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForA02
{
    public partial class WebApplyCardCheckJobForA02Controller
    {
        /// <summary>
        /// 查詢多筆網路件卡友案件檢核
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?startDate=2025/07/16;endDate=2025/07/17
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢多筆網路件卡友案件檢核_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetWebApplyCardCheckJobForA02sByQueryString")]
        public async Task<IResult> GetWebApplyCardCheckJobForA02sByQueryString([FromQuery] GetWebApplyCardCheckJobForA02sByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForA02.GetWebApplyCardCheckJobForA02sByQueryString
{
    public record Query(GetWebApplyCardCheckJobForA02sByQueryStringRequest getWebApplyCardCheckJobForA02sByQueryStringRequest)
        : IRequest<ResultResponse<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var req = request.getWebApplyCardCheckJobForA02sByQueryStringRequest;

            var entity = await context
                .ReviewerPedding_WebApplyCardCheckJobForA02.AsNoTracking()
                .Where(x => req.StartDate <= x.AddTime && x.AddTime <= req.EndDate)
                .ToListAsync(cancellationToken);

            var result = mapper.Map<List<GetWebApplyCardCheckJobForA02sByQueryStringResponse>>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
