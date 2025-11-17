using ScoreSharp.API.Modules.SetUp.LongTermReason.DeleteLongTermReasonById;

namespace ScoreSharp.API.Modules.SetUp.LongTermReason
{
    public partial class LongTermReasonController
    {
        /// <summary>
        /// 刪除單筆長循分期戶理由碼
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /LongTermReason/DeleteLongTermReasonById/FD
        ///
        /// </remarks>
        /// <param name="code">長循分期戶理由碼代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除長循分期戶理由碼_2000_ResEx),
            typeof(刪除長循分期戶理由碼查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteLongTermReasonById")]
        public async Task<IResult> DeleteLongTermReasonById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.LongTermReason.DeleteLongTermReasonById
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
            var single = await _context.SetUp_LongTermReason.SingleOrDefaultAsync(x => x.LongTermReasonCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            _context.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
