using ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02.GetWebApplyCardCheckJobForNotA02sByQueryString;

namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02
{
    public partial class WebApplyCardCheckJobForNotA02Controller
    {
        /// <summary>
        /// 查詢多筆網路件非卡友案件檢核
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?startDate=2025/07/09;endDate=2025/07/10
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetWebApplyCardCheckJobForNotA02sByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢多筆網路件非卡友案件檢核_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetWebApplyCardCheckJobForNotA02sByQueryString")]
        public async Task<IResult> GetWebApplyCardCheckJobForNotA02sByQueryString(
            [FromQuery] GetWebApplyCardCheckJobForNotA02sByQueryStringRequest request
        )
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02.GetWebApplyCardCheckJobForNotA02sByQueryString
{
    public record Query(GetWebApplyCardCheckJobForNotA02sByQueryStringRequest getWebApplyCardCheckJobForNotA02sByQueryStringRequest)
        : IRequest<ResultResponse<List<GetWebApplyCardCheckJobForNotA02sByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetWebApplyCardCheckJobForNotA02sByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetWebApplyCardCheckJobForNotA02sByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var req = request.getWebApplyCardCheckJobForNotA02sByQueryStringRequest;

            var entity = await context
                .ReviewerPedding_WebApplyCardCheckJobForNotA02.AsNoTracking()
                .Where(x => req.StartDate <= x.AddTime && x.AddTime <= req.EndDate)
                .ToListAsync(cancellationToken);

            var result = mapper.Map<List<GetWebApplyCardCheckJobForNotA02sByQueryStringResponse>>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
