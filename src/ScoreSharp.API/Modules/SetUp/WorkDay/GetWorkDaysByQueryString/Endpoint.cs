using ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDaysByQueryString;

namespace ScoreSharp.API.Modules.SetUp.WorkDay
{
    public partial class WorkDayController
    {
        /// <summary>
        /// 查詢多筆工作日資料 ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     ?Year=2025&amp;IsHoliday=Y
        ///     ?Date=20250101
        ///     ?StartDate=20250101&amp;EndDate=20250131
        ///
        /// 驗證規則：
        /// <ul>
        ///     <li>所有查詢參數皆為非必填</li>
        ///     <li>可使用 Date 參數查詢單一日期</li>
        ///     <li>可使用 StartDate + EndDate 查詢日期區間</li>
        ///     <li>當起始日期有值時，結束日期也必須有值</li>
        ///     <li>當結束日期有值時，起始日期也必須有值</li>
        ///     <li>起始日期不能大於結束日期</li>
        ///     <li>日期格式必須為 yyyyMMdd</li>
        /// </ul>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetWorkDaysByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得多筆工作日資料_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetWorkDaysByQueryString")]
        public async Task<IResult> GetWorkDaysByQueryString([FromQuery] GetWorkDaysByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDaysByQueryString
{
    public record Query(GetWorkDaysByQueryStringRequest getWorkDaysByQueryStringRequest)
        : IRequest<ResultResponse<List<GetWorkDaysByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<List<GetWorkDaysByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetWorkDaysByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryRequest = request.getWorkDaysByQueryStringRequest;

            var entity = await context
                .SetUp_WorkDay.AsNoTracking()
                .Where(x =>
                    (String.IsNullOrWhiteSpace(queryRequest.Date) || x.Date == queryRequest.Date)
                    && (
                        (String.IsNullOrWhiteSpace(queryRequest.StartDate) && String.IsNullOrWhiteSpace(queryRequest.EndDate))
                        || (String.Compare(x.Date, queryRequest.StartDate) >= 0 && String.Compare(x.Date, queryRequest.EndDate) <= 0)
                    )
                    && (String.IsNullOrWhiteSpace(queryRequest.Year) || x.Year == queryRequest.Year)
                    && (String.IsNullOrWhiteSpace(queryRequest.IsHoliday) || x.IsHoliday == queryRequest.IsHoliday)
                )
                .OrderBy(x => x.Date)
                .ToListAsync(cancellationToken);

            var result = mapper.Map<List<GetWorkDaysByQueryStringResponse>>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
