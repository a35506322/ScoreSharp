using ScoreSharp.API.Modules.SetUp.CardPromotion.DeleteCardPromotionById;

namespace ScoreSharp.API.Modules.SetUp.CardPromotion
{
    public partial class CardPromotionController
    {
        /// <summary>
        /// 刪除單筆信用卡優惠辦法
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /CardPromotion/DeleteCardPromotionById/0001
        ///
        /// </remarks>
        /// <param name="code">信用卡優惠辦法代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除信用卡優惠辦法_2000_ResEx),
            typeof(刪除信用卡優惠辦法查無此資料_4001_ResEx),
            typeof(刪除信用卡優惠辦法此資源已被使用_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteCardPromotionById")]
        public async Task<IResult> DeleteCardPromotionById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CardPromotion.DeleteCardPromotionById
{
    public record Command(string code) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.SetUp_CardPromotion.SingleOrDefaultAsync(x => x.CardPromotionCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            var isUsedCardPromotion = _context.SetUp_Card_CardPromotion.Select(x => x.CardPromotionCode).Contains(request.code);
            if (isUsedCardPromotion)
                return ApiResponseHelper.此資源已被使用<string>(null, request.code);

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
