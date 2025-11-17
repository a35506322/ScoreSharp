using ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitById;

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit
{
    public partial class PromotionUnitController
    {
        /// <summary>
        /// 查詢單筆推廣單位
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /PromotionUnit/GetPromotionUnitById/200
        ///
        /// </remarks>
        /// <param name="code">推廣單位代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetPromotionUnitByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得推廣單位_2000_ResEx),
            typeof(取得推廣單位查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetPromotionUnitById")]
        public async Task<IResult> GetPromotionUnitById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitById
{
    public record Query(string code) : IRequest<ResultResponse<GetPromotionUnitByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetPromotionUnitByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetPromotionUnitByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_PromotionUnit.AsNoTracking().SingleOrDefaultAsync(x => x.PromotionUnitCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetPromotionUnitByIdResponse>(null, request.code.ToString());

            var result = _mapper.Map<GetPromotionUnitByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
