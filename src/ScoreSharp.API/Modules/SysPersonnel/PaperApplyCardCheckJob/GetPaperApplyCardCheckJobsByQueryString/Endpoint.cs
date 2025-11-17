using ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.GetPaperApplyCardCheckJobsByQueryString;

namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob
{
    public partial class PaperApplyCardCheckJobController
    {
        /// <summary>
        /// 查詢多筆紙本件案件檢核
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetPaperApplyCardCheckJobsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢多筆紙本件案件檢核_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetPaperApplyCardCheckJobsByQueryString")]
        public async Task<IResult> GetPaperApplyCardCheckJobsByQueryString([FromQuery] GetPaperApplyCardCheckJobsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.GetPaperApplyCardCheckJobsByQueryString
{
    public record Query(GetPaperApplyCardCheckJobsByQueryStringRequest getPaperApplyCardCheckJobsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetPaperApplyCardCheckJobsByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetPaperApplyCardCheckJobsByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetPaperApplyCardCheckJobsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var req = request.getPaperApplyCardCheckJobsByQueryStringRequest;

            var entities = await context
                .ReviewerPedding_PaperApplyCardCheckJob.AsNoTracking()
                .Where(x => req.StartDate <= x.AddTime && x.AddTime <= req.EndDate)
                .ToListAsync(cancellationToken);

            var result = mapper.Map<List<GetPaperApplyCardCheckJobsByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
