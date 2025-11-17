using ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.CardPromotion
{
    public partial class CardPromotionController
    {
        /// <summary>
        /// 查詢多筆信用卡優惠辦法
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;CardPromotionName=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetCardPromotionsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得信用卡優惠辦法_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCardPromotionsByQueryString")]
        public async Task<IResult> GetCardPromotionsByQueryString([FromQuery] GetCardPromotionsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionsByQueryString
{
    public record Query(GetCardPromotionsByQueryStringRequest getCardPromotionsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetCardPromotionsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetCardPromotionsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetCardPromotionsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getCardPromotionsByQueryStringRequest = request.getCardPromotionsByQueryStringRequest;

            var entites = await _context
                .SetUp_CardPromotion.Where(x =>
                    string.IsNullOrEmpty(getCardPromotionsByQueryStringRequest.CardPromotionName)
                    || x.CardPromotionName.Contains(getCardPromotionsByQueryStringRequest.CardPromotionName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getCardPromotionsByQueryStringRequest.IsActive)
                    || x.IsActive == getCardPromotionsByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetCardPromotionsByQueryStringResponse>>(entites);

            return ApiResponseHelper.Success(result);
        }
    }
}
