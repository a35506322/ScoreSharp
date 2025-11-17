using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.DeleteMakeCardFailedReasonById;

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason
{
    public partial class MakeCardFailedReasonController
    {
        /// <summary>
        /// 刪除單筆製卡失敗原因
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /MakeCardFailedReason/DeleteMakeCardFailedReasonById/13
        ///
        /// </remarks>
        /// <param name="makeCardFailedReasonCode">PK</param>
        /// <returns></returns>
        [HttpDelete("{makeCardFailedReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除製卡失敗原因_2000_ResEx),
            typeof(刪除製卡失敗原因查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteMakeCardFailedReasonById")]
        public async Task<IResult> DeleteMakeCardFailedReasonById([FromRoute] string makeCardFailedReasonCode)
        {
            var result = await _mediator.Send(new Command(makeCardFailedReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.DeleteMakeCardFailedReasonById
{
    public record Command(string makeCardFailedReasonCode) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_MakeCardFailedReason.SingleOrDefaultAsync(x =>
                x.MakeCardFailedReasonCode == request.makeCardFailedReasonCode
            );

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.makeCardFailedReasonCode);

            _context.SetUp_MakeCardFailedReason.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.makeCardFailedReasonCode);
        }
    }
}
