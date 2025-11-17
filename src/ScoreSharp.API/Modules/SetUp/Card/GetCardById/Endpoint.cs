using ScoreSharp.API.Modules.SetUp.Card.GetCardById;

namespace ScoreSharp.API.Modules.SetUp.Card
{
    public partial class CardController
    {
        /// <summary>
        /// 查詢單筆信用卡卡片種類
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /Card/GetCardById/33413256
        ///
        /// </remarks>
        /// <param name="code">BIN</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetCardByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得信用卡卡片種類_2000_ResEx),
            typeof(取得信用卡卡片種類查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCardById")]
        public async Task<IResult> GetCardById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Card.GetCardById
{
    public record Query(string code) : IRequest<ResultResponse<GetCardByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetCardByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetCardByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_Card.AsNoTracking().SingleOrDefaultAsync(x => x.BINCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetCardByIdResponse>(null, request.code);

            var entity = _context.View_CardJoinCardPromotion.Where(x => x.BINCode == request.code).ToList();

            var result = entity
                .GroupBy(x => new
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
                .Select(x => new GetCardByIdResponse
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
                .Single();

            return ApiResponseHelper.Success(result);
        }
    }
}
