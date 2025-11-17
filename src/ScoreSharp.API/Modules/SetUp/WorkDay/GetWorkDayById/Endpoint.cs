using ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDayById;

namespace ScoreSharp.API.Modules.SetUp.WorkDay
{
    public partial class WorkDayController
    {
        /// <summary>
        /// 取得單筆工作日資料
        /// </summary>
        /// <param name="date">工作日日期，格式 yyyyMMdd</param>
        /// <returns></returns>
        [HttpGet("{date}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetWorkDayByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得單筆工作日資料_2000_ResEx),
            typeof(查無工作日資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetWorkDayById")]
        public async Task<IResult> GetWorkDayById([FromRoute] string date)
        {
            var result = await _mediator.Send(new Query(date));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDayById
{
    public record Query(string Date) : IRequest<ResultResponse<GetWorkDayByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetWorkDayByIdResponse>>
    {
        public async Task<ResultResponse<GetWorkDayByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await context.SetUp_WorkDay.AsNoTracking().SingleOrDefaultAsync(x => x.Date == request.Date, cancellationToken);

            if (entity is null)
                return ApiResponseHelper.NotFound<GetWorkDayByIdResponse>(null, request.Date);

            var result = mapper.Map<GetWorkDayByIdResponse>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
