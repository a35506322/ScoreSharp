using ScoreSharp.API.Modules.SetUp.RejectionReason.DeleteRejectionReasonById;

namespace ScoreSharp.API.Modules.SetUp.RejectionReason
{
    public partial class RejectionReasonController
    {
        /// <summary>
        ///  刪除單筆退件原因
        /// </summary>
        /// <remarks>
        ///  Sample Router :
        ///
        ///         /RejectiontReason/DeleteRejectiontReasonById/02
        ///
        /// </remarks>
        /// <param name="rejectiontReasonCode">PK</param>
        /// <returns></returns>
        [HttpDelete("{rejectiontReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除退件原因_2000_ResEx),
            typeof(刪除退件原因查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteRejectiontReasonById")]
        public async Task<IResult> DeleteRejectionReasonById([FromRoute] string rejectiontReasonCode)
        {
            var result = await _mediator.Send(new Command(rejectiontReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.RejectionReason.DeleteRejectionReasonById
{
    public record Command(string rejectiontReasonCode) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_RejectionReason.SingleOrDefaultAsync(x =>
                x.RejectionReasonCode == request.rejectiontReasonCode
            );

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.rejectiontReasonCode);

            _context.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.rejectiontReasonCode);
        }
    }
}
