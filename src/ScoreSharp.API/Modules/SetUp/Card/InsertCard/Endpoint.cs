using ScoreSharp.API.Modules.SetUp.Card.InsertCard;

namespace ScoreSharp.API.Modules.SetUp.Card
{
    public partial class CardController
    {
        /// <summary>
        /// 新增單筆信用卡卡片種類
        /// </summary>
        /// <param name="request"></param>
        /// <response code="400">
        /// 檢查選優惠辦法
        /// 1. 可選優惠辦法至少為一個
        /// 2. 不在可選優惠辦法名單中
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增信用卡卡片種類_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增信用卡卡片種類_2000_ResEx),
            typeof(新增信用卡卡片種類資料已存在_4002_ResEx),
            typeof(新增信用卡卡片種類查無帳單日資料_4003_ResEx),
            typeof(新增信用卡卡片種類查無優惠辦法資料_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(新增信用卡卡片種類預設優惠不在可選名單中_4000_ResEx),
            typeof(新增信用卡卡片種類可選優惠至少一個_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("InsertCard")]
        public async Task<IResult> InsertCard([FromBody] InsertCardRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Card.InsertCard
{
    public record Command(InsertCardRequest insertCardRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertCardRequest = request.insertCardRequest;

            var single = await _context
                .SetUp_Card.AsNoTracking()
                .SingleOrDefaultAsync(x => x.BINCode == insertCardRequest.BINCode || x.CardCode == insertCardRequest.CardCode);
            if (single != null)
            {
                if (single.BINCode == insertCardRequest.BINCode)
                    return ApiResponseHelper.DataAlreadyExists<string>(null, insertCardRequest.BINCode);
                else
                    return ApiResponseHelper.DataAlreadyExists<string>(null, insertCardRequest.CardCode);
            }

            var validBillDays = _context.SetUp_BillDay.Where(x => x.IsActive == "Y").Select(x => x.BillDay).ToList();
            if (!validBillDays.Contains(insertCardRequest.DefaultBillDay))
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無帳單日資料，請檢查");

            var validCardPromotions = _context.SetUp_CardPromotion.Where(x => x.IsActive == "Y").Select(x => x.CardPromotionCode).ToList();
            if (!insertCardRequest.OptionalCardPromotions.All(optionalCardPromotion => validCardPromotions.Contains(optionalCardPromotion)))
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無優惠辦法資料，請檢查");
            }
            var entities = insertCardRequest
                .OptionalCardPromotions.Select(cardPromotion => new SetUp_Card_CardPromotion
                {
                    BINCode = insertCardRequest.BINCode,
                    CardPromotionCode = cardPromotion,
                })
                .ToList();

            var entity = _mapper.Map<SetUp_Card>(insertCardRequest);

            await _context.SetUp_Card.AddAsync(entity);
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(insertCardRequest.BINCode, insertCardRequest.BINCode);
        }
    }
}
