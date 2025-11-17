using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.DeleteHighFinancialSecrecyCountryById;

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry
{
    public partial class HighFinancialSecrecyCountryController
    {
        /// <summary>
        /// 刪除單筆高金融保密國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /HighFinancialSecrecyCountry/DeleteHighFinancialSecrecyCountryById/AE
        ///
        /// </remarks>
        /// <param name="code">國籍代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除單筆高金融保密國家_2000_ResEx),
            typeof(刪除單筆高金融保密國家_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteHighFinancialSecrecyCountryById")]
        public async Task<IResult> DeleteHighFinancialSecrecyCountryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.DeleteHighFinancialSecrecyCountryById
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
            var single = await _context.SetUp_HighFinancialSecrecyCountry.SingleOrDefaultAsync(x =>
                x.HighFinancialSecrecyCountryCode == request.code
            );

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            _context.SetUp_HighFinancialSecrecyCountry.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
