using ScoreSharp.API.Modules.SetUp.CardPromotion.UpdateCardPromotionById;

namespace ScoreSharp.API.Modules.SetUp.CardPromotion
{
    public partial class CardPromotionController
    {
        /// <summary>
        /// 修改單筆信用卡優惠辦法
        /// </summary>
        /// <param name="code">信用卡優惠辦法代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改信用卡優惠辦法_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改信用卡優惠辦法_2000_ResEx),
            typeof(修改信用卡優惠辦法查無資料_4001_ResEx),
            typeof(修改信用卡優惠辦法路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateCardPromotionById")]
        public async Task<IResult> UpdateCardPromotionById([FromRoute] string code, [FromBody] UpdateCardPromotionByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CardPromotion.UpdateCardPromotionById
{
    public record Command(string code, UpdateCardPromotionByIdRequest updateCardPromotionByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateCardPromotionByIdRequest = request.updateCardPromotionByIdRequest;

            if (request.code != updateCardPromotionByIdRequest.CardPromotionCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_CardPromotion.SingleOrDefaultAsync(x => x.CardPromotionCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            entity.CardPromotionName = updateCardPromotionByIdRequest.CardPromotionName;
            entity.PrimaryCardReservedPOT = updateCardPromotionByIdRequest.PrimaryCardReservedPOT;
            entity.PrimaryCardUsedPOT = updateCardPromotionByIdRequest.PrimaryCardUsedPOT;
            entity.SupplementaryCardUsedPOT = updateCardPromotionByIdRequest.SupplementaryCardUsedPOT;
            entity.UsedPOTExpiryMonth = updateCardPromotionByIdRequest.UsedPOTExpiryMonth;
            entity.SupplementaryCardReservedPOT = updateCardPromotionByIdRequest.SupplementaryCardReservedPOT;
            entity.ReservePromotionPeriod = updateCardPromotionByIdRequest.ReservePromotionPeriod;
            entity.InterestRate = updateCardPromotionByIdRequest.InterestRate;
            entity.IsActive = updateCardPromotionByIdRequest.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
