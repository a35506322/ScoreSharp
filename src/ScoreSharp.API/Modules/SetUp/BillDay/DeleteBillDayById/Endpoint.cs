using ScoreSharp.API.Modules.SetUp.BillDay.DeleteBillDayById;

namespace ScoreSharp.API.Modules.SetUp.BillDay
{
    public partial class BillDayController
    {
        /// <summary>
        /// 刪除單筆帳單日
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /BillDay/DeleteBillDayById/01
        ///
        /// </remarks>
        /// <param name="billDay">PK</param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpDelete("{billDay}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(
            typeof(刪除帳單日_2000_ResEx),
            typeof(刪除帳單日查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteBillDayById")]
        public async Task<IResult> DeleteBillDayById([FromRoute] string billDay)
        {
            var result = await _mediator.Send(new Command(billDay));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BillDay.DeleteBillDayById
{
    public record Command(string billDay) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_BillDay.SingleOrDefaultAsync(x => x.BillDay == request.billDay);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.billDay);

            _context.SetUp_BillDay.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.billDay);
        }
    }
}
