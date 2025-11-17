using ScoreSharp.API.Modules.SetUp.BillDay.GetBillDayByQueryString;

namespace ScoreSharp.API.Modules.SetUp.BillDay
{
    public partial class BillDayController
    {
        /// <summary>
        /// 查詢多筆帳單日 ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetBillDayByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得多筆帳單日_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetBillDayByQueryString")]
        public async Task<IResult> GetBillDayByQueryString([FromQuery] GetBillDayByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BillDay.GetBillDayByQueryString
{
    public record Query(GetBillDayByQueryStringRequest getBillDayByQueryStringRequest)
        : IRequest<ResultResponse<List<GetBillDayByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetBillDayByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetBillDayByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var getBillDayByQueryStringRequest = request.getBillDayByQueryStringRequest;

            var entity = await _context
                .SetUp_BillDay.AsNoTracking()
                .Where(x =>
                    String.IsNullOrWhiteSpace(getBillDayByQueryStringRequest.IsActive)
                    || x.IsActive == getBillDayByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetBillDayByQueryStringResponse>>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
