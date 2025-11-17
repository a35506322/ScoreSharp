using ScoreSharp.API.Modules.SetUp.Card.GetCardsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.Card
{
    public partial class CardController
    {
        /// <summary>
        /// 查詢多筆信用卡卡片種類
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;CardName=&amp;OrderBy=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetCardsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得信用卡卡片種類_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCardsByQueryString")]
        public async Task<IResult> GetCardsByQueryString([FromQuery] GetCardsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Card.GetCardsByQueryString
{
    public record Query(GetCardsByQueryStringRequest getCardsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetCardsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetCardsByQueryStringResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ScoreSharpContext _context;

        public Handler(IMapper mapper, ScoreSharpContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ResultResponse<List<GetCardsByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var getCardsByQueryStringRequest = request.getCardsByQueryStringRequest;

            List<string> sortBy = new List<string> { "J", "M", "V" };
            bool 是否排序卡片 =
                !String.IsNullOrEmpty(getCardsByQueryStringRequest.OrderBy) && getCardsByQueryStringRequest.OrderBy == "CardCode";

            var dto = _context
                .View_CardJoinCardPromotion.Where(x =>
                    String.IsNullOrEmpty(getCardsByQueryStringRequest.IsActive) || x.IsActive == getCardsByQueryStringRequest.IsActive
                )
                .Where(x =>
                    String.IsNullOrEmpty(getCardsByQueryStringRequest.CardName)
                    || x.CardName.Contains(getCardsByQueryStringRequest.CardName)
                )
                .Where(x =>
                    String.IsNullOrEmpty(getCardsByQueryStringRequest.CardCode)
                    || x.CardCode.Contains(getCardsByQueryStringRequest.CardCode)
                )
                .AsNoTracking()
                .ToList();

            var response = dto.GroupBy(x => new
                {
                    x.BINCode,
                    x.CardCode,
                    x.CardName,
                    x.CardCategory,
                    x.SampleRejectionLetter,
                    x.DefaultBillDay,
                    x.SaleLoanCategory,
                    x.DefaultDiscount,
                    x.IsActive,
                    x.PrimaryCardQuotaUpperlimit,
                    x.PrimaryCardQuotaLowerlimit,
                    x.PrimaryCardYearUpperlimit,
                    x.PrimaryCardYearLowerlimit,
                    x.SupplementaryCardQuotaUpperlimit,
                    x.SupplementaryCardQuotaLowerlimit,
                    x.SupplementaryCardYearUpperlimit,
                    x.SupplementaryCardYearLowerlimit,
                    x.IsCARDPAUnderLimit,
                    x.CARDPACQuotaLimit,
                    x.IsApplyAdditionalCard,
                    x.IsIndependentCard,
                    x.IsIVRvCTIQuery,
                    x.IsCITSCard,
                    x.IsQuickCardIssuance,
                    x.IsTicket,
                    x.IsJointGroup,
                    x.AddUserId,
                    x.AddTime,
                    x.UpdateUserId,
                    x.UpdateTime,
                })
                .Select(x => new GetCardsByQueryStringResponse
                {
                    BINCode = x.Key.BINCode,
                    CardCode = x.Key.CardCode,
                    CardName = x.Key.CardName,
                    CardCategory = x.Key.CardCategory,
                    SampleRejectionLetter = x.Key.SampleRejectionLetter,
                    DefaultBillDay = x.Key.DefaultBillDay,
                    SaleLoanCategory = x.Key.SaleLoanCategory,
                    DefaultDiscount = new DefaultCardPromotionDto()
                    {
                        CardPromotionCode = x.Key.DefaultDiscount,
                        CardPromotionName = x.Single(y => y.CardPromotionCode == x.Key.DefaultDiscount).CardPromotionName,
                    },
                    IsActive = x.Key.IsActive,
                    PrimaryCardQuotaUpperlimit = x.Key.PrimaryCardQuotaUpperlimit,
                    PrimaryCardQuotaLowerlimit = x.Key.PrimaryCardQuotaLowerlimit,
                    PrimaryCardYearUpperlimit = x.Key.PrimaryCardYearUpperlimit,
                    PrimaryCardYearLowerlimit = x.Key.PrimaryCardYearLowerlimit,
                    SupplementaryCardQuotaUpperlimit = x.Key.SupplementaryCardQuotaUpperlimit,
                    SupplementaryCardQuotaLowerlimit = x.Key.SupplementaryCardQuotaLowerlimit,
                    SupplementaryCardYearUpperlimit = x.Key.SupplementaryCardYearUpperlimit,
                    SupplementaryCardYearLowerlimit = x.Key.SupplementaryCardYearLowerlimit,
                    IsCARDPAUnderLimit = x.Key.IsCARDPAUnderLimit,
                    CARDPACQuotaLimit = x.Key.CARDPACQuotaLimit,
                    IsApplyAdditionalCard = x.Key.IsApplyAdditionalCard,
                    IsIndependentCard = x.Key.IsIndependentCard,
                    IsIVRvCTIQuery = x.Key.IsIVRvCTIQuery,
                    IsCITSCard = x.Key.IsCITSCard,
                    IsQuickCardIssuance = x.Key.IsQuickCardIssuance,
                    IsTicket = x.Key.IsTicket,
                    IsJointGroup = x.Key.IsJointGroup,
                    AddUserId = x.Key.AddUserId,
                    AddTime = x.Key.AddTime,
                    UpdateUserId = x.Key.UpdateUserId,
                    UpdateTime = x.Key.UpdateTime,
                    CardCategoryName = x.Key.CardCategory.ToString(),
                    SaleLoanCategoryName = x.Key.SaleLoanCategory.ToString(),
                    SampleRejectionLetterName = x.Key.SampleRejectionLetter.ToString(),
                    OptionalCardPromotions = x.Select(y => new OptionalCardPromotionsDto
                        {
                            CardPromotionCode = y.CardPromotionCode,
                            CardPromotionName = y.CardPromotionName,
                        })
                        .ToList(),
                })
                .ToList();

            if (是否排序卡片)
            {
                response = response
                    .OrderBy(sorted => sortBy.Contains(sorted.CardCode.Substring(0, 1)) ? 0 : 1)
                    .ThenBy(sorted => sortBy.IndexOf(sorted.CardCode.Substring(0, 1)))
                    .ToList();
            }

            return ApiResponseHelper.Success(response);
        }
    }
}
