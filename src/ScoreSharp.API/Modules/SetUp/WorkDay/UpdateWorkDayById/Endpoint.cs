using ScoreSharp.API.Modules.SetUp.WorkDay.UpdateWorkDayById;

namespace ScoreSharp.API.Modules.SetUp.WorkDay
{
    public partial class WorkDayController
    {
        /// <summary>
        /// 更新單筆工作日
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     {
        ///       "date": "20250101",
        ///       "year": "2025",
        ///       "name": "中華民國開國紀念日",
        ///       "isHoliday": "N",
        ///       "holidayCategory": "調整放假",
        ///       "description": "因政策調整取消放假。"
        ///     }
        ///
        /// </remarks>
        /// <param name="date">欲更新之日期，格式 yyyyMMdd</param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">更新成功返回日期</response>
        [HttpPut("{date}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新工作日_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新工作日_2000_ResEx),
            typeof(更新工作日查無資料_4001_ResEx),
            typeof(更新工作日路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateWorkDayById")]
        public async Task<IResult> UpdateWorkDayById([FromRoute] string date, [FromBody] UpdateWorkDayByIdRequest request)
        {
            var result = await _mediator.Send(new Command(date, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.WorkDay.UpdateWorkDayById
{
    public record Command(string Date, UpdateWorkDayByIdRequest updateWorkDayByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.updateWorkDayByIdRequest;

            if (dto.Date != request.Date)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var existingEntity = await context.SetUp_WorkDay.SingleOrDefaultAsync(x => x.Date == request.Date, cancellationToken);

            if (existingEntity is null)
                return ApiResponseHelper.NotFound<string>(null, request.Date);

            // 更新屬性
            existingEntity.Year = dto.Year;
            existingEntity.Name = dto.Name;
            existingEntity.IsHoliday = dto.IsHoliday;
            existingEntity.HolidayCategory = dto.HolidayCategory;
            existingEntity.Description = dto.Description;
            existingEntity.UpdateUserId = jwtProfilerHelper.UserId;
            existingEntity.UpdateTime = DateTime.Now;

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.Date, request.Date);
        }
    }
}
