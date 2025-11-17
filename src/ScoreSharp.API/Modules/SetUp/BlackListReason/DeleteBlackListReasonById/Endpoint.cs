using ScoreSharp.API.Modules.SetUp.BlackListReason.DeleteBlackListReasonById;

namespace ScoreSharp.API.Modules.SetUp.BlackListReason
{
    public partial class BlackListReasonController
    {
        /// <summary>
        /// 刪除單筆黑名單理由
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router:
        ///
        ///     /BlackListReason/DeleteBlackListReasonById/C7
        ///
        /// </remarks>
        /// <param name="blackListReasonCode">PK</param>
        /// <returns></returns>
        [HttpDelete("{blackListReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除黑名單理由_2000_ResEx),
            typeof(刪除黑名單理由查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteBlackListReasonById")]
        public async Task<IResult> DeleteBlackListReasonById([FromRoute] string blackListReasonCode)
        {
            var result = await _mediator.Send(new Command(blackListReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BlackListReason.DeleteBlackListReasonById
{
    public record Command(string blackListReasonCode) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.SetUp_BlackListReason.SingleOrDefaultAsync(x =>
                x.BlackListReasonCode == request.blackListReasonCode
            );

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.blackListReasonCode);

            _context.SetUp_BlackListReason.Remove(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.blackListReasonCode);
        }
    }
}
