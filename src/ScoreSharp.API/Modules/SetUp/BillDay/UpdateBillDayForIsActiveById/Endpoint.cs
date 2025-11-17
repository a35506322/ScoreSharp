using System;
using ScoreSharp.API.Modules.SetUp.BillDay.UpdateBillDayForIsActiveById;

namespace ScoreSharp.API.Modules.SetUp.BillDay
{
    public partial class BillDayController
    {
        /// <summary>
        /// 單筆修改帳單日狀態
        /// </summary>
        /// <param name="billDay">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{billDay}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改帳單日狀態_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改帳單日狀態_2000_ResEx),
            typeof(修改帳單日狀態查無此資料_4001_ResEx),
            typeof(修改帳單日狀態路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateBillDayForIsActiveById")]
        public async Task<IResult> UpdateBillDayForIsActiveById(
            [FromRoute] string billDay,
            [FromBody] UpdateBillDayForIsActiveByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(billDay, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BillDay.UpdateBillDayForIsActiveById
{
    public record Command(string billDay, UpdateBillDayForIsActiveByIdRequest updateBillDayForIsActiveByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.billDay != request.updateBillDayForIsActiveByIdRequest.BillDay)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_BillDay.SingleOrDefaultAsync(x => x.BillDay == request.billDay);
            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.billDay);

            var dto = request.updateBillDayForIsActiveByIdRequest;
            var update = await _context.SetUp_BillDay.SingleOrDefaultAsync(x => x.BillDay == dto.BillDay);

            entity.IsActive = dto.IsActive;
            entity.UpdateUserId = _jwthelper.UserId;
            entity.UpdateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.billDay, request.billDay);
        }
    }
}
