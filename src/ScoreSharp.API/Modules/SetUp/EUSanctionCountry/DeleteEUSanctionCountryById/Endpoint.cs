using ScoreSharp.API.Modules.SetUp.EUSanctionCountry.DeleteEUSanctionCountryById;

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry
{
    public partial class EUSanctionCountryController
    {
        /// <summary>
        /// 刪除單筆EU制裁國家
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /EUSanctionCountry/DeleteEUSanctionCountryById/TW
        ///
        /// </remarks>
        /// <param name="code">EU制裁國家代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除EU制裁國家_2000_ResEx),
            typeof(刪除EU制裁國家查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteEUSanctionCountryById")]
        public async Task<IResult> DeleteEUSanctionCountryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry.DeleteEUSanctionCountryById
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
            var single = await _context.SetUp_EUSanctionCountry.SingleOrDefaultAsync(x => x.EUSanctionCountryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            _context.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
