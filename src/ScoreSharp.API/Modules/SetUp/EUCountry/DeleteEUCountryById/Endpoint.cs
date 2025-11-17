using ScoreSharp.API.Modules.SetUp.EUCountry.DeleteEUCountryById;

namespace ScoreSharp.API.Modules.SetUp.EUCountry
{
    public partial class EUCountryController
    {
        /// <summary>
        /// 刪除單筆EU國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /EUCountry/DeleteEUCountryById/TW
        ///
        /// </remarks>
        /// <param name="code">EU國家代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除EU國家_2000_ResEx),
            typeof(刪除EU國家查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteEUCountryById")]
        public async Task<IResult> DeleteEUCountryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUCountry.DeleteEUCountryById
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
            var single = await _context.SetUp_EUCountry.SingleOrDefaultAsync(x => x.EUCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            _context.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
