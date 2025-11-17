using ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionById;

namespace ScoreSharp.API.Modules.SetUp.CardPromotion
{
    public partial class CardPromotionController
    {
        /// <summary>
        /// 查詢單筆信用卡優惠辦法
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///       /CardPromotion/GetCardPromotionById/0001
        ///
        /// </remarks>
        /// <param name="code">信用卡優惠辦法代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetCardPromotionByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得信用卡優惠辦法_2000_ResEx),
            typeof(取得信用卡優惠辦法查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCardPromotionById")]
        public async Task<IResult> GetCardPromotionById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionById
{
    public record Query(string code) : IRequest<ResultResponse<GetCardPromotionByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetCardPromotionByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetCardPromotionByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_CardPromotion.AsNoTracking().SingleOrDefaultAsync(x => x.CardPromotionCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetCardPromotionByIdResponse>(null, request.code);

            var result = _mapper.Map<GetCardPromotionByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
