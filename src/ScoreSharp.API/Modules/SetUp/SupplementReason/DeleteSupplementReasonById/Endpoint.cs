using ScoreSharp.API.Modules.SetUp.SupplementReason.DeleteSupplementReasonById;

namespace ScoreSharp.API.Modules.SetUp.SupplementReason
{
    public partial class SupplementReasonController
    {
        /// <summary>
        ///  刪除單筆補件原因
        /// </summary>
        /// <remarks>
        ///  Sample Router:
        ///
        ///         /SupplementReason/DeleteSupplementReasonById/02
        ///
        /// </remarks>
        /// <param name="supplementReasonCode">PK</param>
        /// <returns></returns>
        [HttpDelete("{supplementReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除補件原因_2000_ResEx),
            typeof(刪除補件原因查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteSupplementReasonById")]
        public async Task<IResult> DeleteSupplementReasonById([FromRoute] string supplementReasonCode)
        {
            var result = await _mediator.Send(new Command(supplementReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.SupplementReason.DeleteSupplementReasonById
{
    public record Command(string supplementReasonCode) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_SupplementReason.SingleOrDefaultAsync(x =>
                x.SupplementReasonCode == request.supplementReasonCode
            );

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.supplementReasonCode);

            _context.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.supplementReasonCode);
        }
    }
}
