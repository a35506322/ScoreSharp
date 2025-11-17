using ScoreSharp.API.Modules.SetUp.HighRiskCountry.DeleteHighRiskCountryById;

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry
{
    public partial class HighRiskCountryController
    {
        /// <summary>
        /// 刪除單筆洗錢及資恐高風險國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /HighRiskCountry/DeleteHighRiskCountryById/AE
        ///
        /// </remarks>
        /// <param name="code">國籍代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除洗錢及資恐高風險國家_2000_ResEx),
            typeof(刪除洗錢及資恐高風險國家查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteHighRiskCountryById")]
        public async Task<IResult> DeleteHighRiskCountryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.DeleteHighRiskCountryById
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
            var single = await _context.SetUp_HighRiskCountry.SingleOrDefaultAsync(x => x.HighRiskCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            _context.SetUp_HighRiskCountry.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
