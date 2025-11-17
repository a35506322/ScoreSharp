using ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.GetUserTakeVacationByQueryString;

namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation
{
    public partial class UserTakeVacationController
    {
        ///<summary>
        /// 查詢多筆員工休假 By QuertString
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /UserTakeVacation/GetUserTakeVacationByQueryString?Year=2024&amp;Month=12
        ///
        /// </remarks>
        /// <param name="seqNo">PK</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetUserTakeVacationByQueryStringResponse>))]
        [EndpointSpecificExample(
            typeof(查詢多筆員工休假_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetUserTakeVacationByQueryString")]
        public async Task<IResult> GetUserTakeVacationByQueryString([FromQuery] GetUserTakeVacationByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.GetUserTakeVacationByQueryString
{
    public record Query(GetUserTakeVacationByQueryStringRequest getUserTakeVacationByQueryStringRequest)
        : IRequest<ResultResponse<List<GetUserTakeVacationByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetUserTakeVacationByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetUserTakeVacationByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var userDic = await context.OrgSetUp_User.ToDictionaryAsync(x => x.UserId, x => x.UserName);

            var getUserTakeVacationByQueryStringRequest = request.getUserTakeVacationByQueryStringRequest;

            var entities = await context
                .OrgSetUp_UserTakeVacation.AsNoTracking()
                .Where(x =>
                    x.StartTime.Year == getUserTakeVacationByQueryStringRequest.Year
                    && x.StartTime.Month == getUserTakeVacationByQueryStringRequest.Month
                )
                .OrderBy(x => x.StartTime)
                .ToListAsync();

            var response = mapper.Map<List<GetUserTakeVacationByQueryStringResponse>>(entities);
            response.ForEach(x => x.UserName = userDic[x.UserId]);

            return ApiResponseHelper.Success(response);
        }
    }
}
