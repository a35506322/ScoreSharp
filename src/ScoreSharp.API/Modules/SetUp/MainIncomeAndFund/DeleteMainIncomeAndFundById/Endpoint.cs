using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.DeleteMainIncomeAndFundById;

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund
{
    public partial class MainIncomeAndFundController
    {
        /// <summary>
        /// 刪除單筆主要所得及資金來源
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /MainIncomeAndFund/DeleteMainIncomeAndFundById/2
        ///
        /// </remarks>
        /// <param name="code">主要所得及資金來源代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除主要所得及資金來源_2000_ResEx),
            typeof(刪除主要所得及資金來源查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteMainIncomeAndFundById")]
        public async Task<IResult> DeleteMainIncomeAndFundById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.DeleteMainIncomeAndFundById
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
            var single = await _context.SetUp_MainIncomeAndFund.SingleOrDefaultAsync(x => x.MainIncomeAndFundCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.code.ToString());

            _context.SetUp_MainIncomeAndFund.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code.ToString());
        }
    }
}
