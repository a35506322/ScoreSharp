using ScoreSharp.API.Modules.SetUp.CreditCheckCode.DeleteCreditCheckCodeById;

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode
{
    public partial class CreditCheckCodeController
    {
        /// <summary>
        /// 刪除單筆徵信代碼
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /CreditCheckCode/DeleteCreditCheckCodeById/A04
        ///
        /// </remarks>
        /// <param name="creditCheckCode">PK</param>
        /// <returns></returns>
        [HttpDelete("{creditCheckCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除徵信代碼_2000_ResEx),
            typeof(刪除徵信代碼查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteCreditCheckCodeById")]
        public async Task<IResult> DeleteCreditCheckCodeById([FromRoute] string creditCheckCode)
        {
            var result = await _mediator.Send(new Command(creditCheckCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.DeleteCreditCheckCodeById
{
    public record Command(string creditCheckCode) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_CreditCheckCode.SingleOrDefaultAsync(x => x.CreditCheckCode == request.creditCheckCode);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.creditCheckCode);

            _context.SetUp_CreditCheckCode.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.creditCheckCode);
        }
    }
}
