using ScoreSharp.API.Modules.SetUp.WorkDay.InsertWorkDay;

namespace ScoreSharp.API.Modules.SetUp.WorkDay
{
    public partial class WorkDayController
    {
        /// <summary>
        /// 新增單筆工作日
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     {
        ///       "date": "20250101",
        ///       "year": "2025",
        ///       "name": "中華民國開國紀念日",
        ///       "isHoliday": "Y",
        ///       "holidayCategory": "放假之紀念日及節日",
        ///       "description": "全國各機關學校放假一日。"
        ///     }
        ///
        /// 驗證規則：
        /// <ul>
        ///     <li>日期格式不正確</li>
        /// </ul>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回日期</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增工作日_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增工作日_2000_ResEx),
            typeof(新增工作日資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertWorkDay")]
        public async Task<IResult> InsertWorkDay([FromBody] InsertWorkDayRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.WorkDay.InsertWorkDay
{
    public record Command(InsertWorkDayRequest insertWorkDayRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertWorkDayRequest;

            // 檢查是否已存在相同的日期
            var existingEntity = await context.SetUp_WorkDay.SingleOrDefaultAsync(x => x.Date == dto.Date, cancellationToken);

            if (existingEntity is not null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.Date);

            var newEntity = new SetUp_WorkDay
            {
                Date = dto.Date,
                Year = dto.Year,
                Name = dto.Name,
                IsHoliday = dto.IsHoliday,
                HolidayCategory = dto.HolidayCategory,
                Description = dto.Description,
                AddUserId = jwtProfilerHelper.UserId,
                AddTime = DateTime.Now,
            };

            await context.SetUp_WorkDay.AddAsync(newEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.InsertSuccess(dto.Date, dto.Date);
        }
    }
}
