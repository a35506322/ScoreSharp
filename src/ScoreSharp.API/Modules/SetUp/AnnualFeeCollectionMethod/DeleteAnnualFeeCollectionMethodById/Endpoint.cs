using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.DeleteAnnualFeeCollectionMethodById;

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod
{
    public partial class AnnualFeeCollectionMethodController
    {
        /// <summary>
        /// 刪除單筆年費收取方式
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /AnnualFeeCollectionMethod/DeleteAnnualFeeCollectionMethodById/04
        ///
        /// </remarks>
        /// <param name="code">PK</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除年費收取方式_2000_ResEx),
            typeof(刪除年費收取方式查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteAnnualFeeCollectionMethodById")]
        public async Task<IResult> DeleteAnnualFeeCollectionMethodById([FromRoute] string code)
        {
            var response = await _mediator.Send(new Command(code));
            return Results.Ok(response);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.DeleteAnnualFeeCollectionMethodById
{
    public record Command(string code) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var code = request.code;

            var single = await _context.SetUp_AnnualFeeCollectionMethod.SingleOrDefaultAsync(x => x.AnnualFeeCollectionCode == code);

            if (single == null)
                return ApiResponseHelper.NotFound<string>(null, code);

            _context.Remove(single);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
