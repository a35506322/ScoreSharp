using ScoreSharp.API.Modules.SetUp.WorkDay.DeleteWorkDayById;

namespace ScoreSharp.API.Modules.SetUp.WorkDay
{
    public partial class WorkDayController
    {
        /// <summary>
        /// 刪除單筆工作日
        /// </summary>
        /// <param name="date">欲刪除之日期，格式 yyyyMMdd</param>
        /// <returns></returns>
        /// <response code="200">刪除成功返回日期</response>
        [HttpDelete("{date}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除工作日_2000_ResEx),
            typeof(刪除工作日查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteWorkDayById")]
        public async Task<IResult> DeleteWorkDayById([FromRoute] string date)
        {
            var result = await _mediator.Send(new Command(date));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.WorkDay.DeleteWorkDayById
{
    public record Command(string Date) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingEntity = await context.SetUp_WorkDay.SingleOrDefaultAsync(x => x.Date == request.Date, cancellationToken);

            if (existingEntity is null)
                return ApiResponseHelper.NotFound<string>(null, request.Date);

            context.SetUp_WorkDay.Remove(existingEntity);
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.DeleteByIdSuccess<string>(request.Date);
        }
    }
}
