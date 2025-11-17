using ScoreSharp.API.Modules.SetUp.BillDay.InsertIBillDay;

namespace ScoreSharp.API.Modules.SetUp.BillDay
{
    public partial class BillDayController
    {
        /// <summary>
        /// 新增單筆帳單日
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(新增帳單日_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增帳單日_2000_ResEx),
            typeof(新增帳單日資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertIBillDay")]
        public async Task<IResult> InsertBillDay([FromBody] InsertBillDayRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BillDay.InsertIBillDay
{
    public record Command(InsertBillDayRequest insertIBillDayRequest) : IRequest<ResultResponse<string>>;

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
            var dto = request.insertIBillDayRequest;

            var single = await _context.SetUp_BillDay.AsNoTracking().SingleOrDefaultAsync(x => x.BillDay == dto.BillDay);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.BillDay);

            SetUp_BillDay setUp_BillDay = new SetUp_BillDay()
            {
                BillDay = dto.BillDay,
                IsActive = dto.IsActive,
                AddUserId = _jwthelper.UserId,
                AddTime = DateTime.Now,
            };

            await _context.AddAsync(setUp_BillDay);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(setUp_BillDay.BillDay, setUp_BillDay.BillDay);
        }
    }
}
