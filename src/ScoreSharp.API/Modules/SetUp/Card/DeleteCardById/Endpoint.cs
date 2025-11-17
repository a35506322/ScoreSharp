using ScoreSharp.API.Modules.SetUp.Card.DeleteCardById;

namespace ScoreSharp.API.Modules.SetUp.Card
{
    public partial class CardController
    {
        /// <summary>
        /// 刪除單筆信用卡卡片種類
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /Card/DeleteCardById/33413256
        ///
        /// </remarks>
        /// <param name="code">BIN</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除信用卡卡片種類_2000_ResEx),
            typeof(刪除信用卡卡片種類查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteCardById")]
        public async Task<IResult> DeleteCardById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Card.DeleteCardById
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
            var single = await _context.SetUp_Card.SingleOrDefaultAsync(x => x.BINCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            await _context.SetUp_Card_CardPromotion.Where(x => x.BINCode == single.BINCode).ExecuteDeleteAsync();
            _context.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
