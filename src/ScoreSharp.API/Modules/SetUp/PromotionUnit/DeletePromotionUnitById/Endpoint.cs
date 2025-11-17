using ScoreSharp.API.Modules.SetUp.PromotionUnit.DeletePromotionUnitById;

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit
{
    public partial class PromotionUnitController
    {
        /// <summary>
        /// 刪除單筆推廣單位
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /PromotionUnit/DeletePromotionUnitById/200
        ///
        /// </remarks>
        /// <param name="code">推廣單位代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除推廣單位_2000_ResEx),
            typeof(刪除推廣單位查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeletePromotionUnitById")]
        public async Task<IResult> DeletePromotionUnitById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.DeletePromotionUnitById
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
            var single = await _context.SetUp_PromotionUnit.SingleOrDefaultAsync(x => x.PromotionUnitCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code.ToString());

            _context.SetUp_PromotionUnit.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess<string>(request.code);
        }
    }
}
