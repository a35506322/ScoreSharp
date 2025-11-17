using ScoreSharp.API.Modules.SetUp.CardPromotion.InsertCardPromotion;

namespace ScoreSharp.API.Modules.SetUp.CardPromotion
{
    public partial class CardPromotionController
    {
        /// <summary>
        /// 新增單筆信用卡優惠辦法
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增信用卡優惠辦法_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增信用卡優惠辦法_2000_ResEx),
            typeof(新增信用卡優惠辦法資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertCardPromotion")]
        public async Task<IResult> InsertCardPromotion([FromBody] InsertCardPromotionRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CardPromotion.InsertCardPromotion
{
    public record Command(InsertCardPromotionRequest insertCardPromotionRequest) : IRequest<ResultResponse<string>>;

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
            var insertCardPromotionRequest = request.insertCardPromotionRequest;

            var single = await _context
                .SetUp_CardPromotion.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CardPromotionCode == insertCardPromotionRequest.CardPromotionCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertCardPromotionRequest.CardPromotionCode);

            var entity = _mapper.Map<SetUp_CardPromotion>(insertCardPromotionRequest);

            await _context.SetUp_CardPromotion.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(
                insertCardPromotionRequest.CardPromotionCode,
                insertCardPromotionRequest.CardPromotionCode
            );
        }
    }
}
