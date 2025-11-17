using ScoreSharp.API.Modules.SetUp.AddressInfo.DeleteAddressInfoById;

namespace ScoreSharp.API.Modules.SetUp.AddressInfo
{
    public partial class AddressInfoController
    {
        /// <summary>
        /// 刪除單筆地址資訊
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /AddressInfo/DeleteAddressInfoById/72424
        ///
        /// </remarks>
        /// <param name="seqNo">流水號</param>
        /// <returns></returns>
        /// <response code="200">刪除成功返回流水號</response>
        [HttpDelete("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(
            typeof(刪除地址資訊_2000_ResEx),
            typeof(刪除地址資訊查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteAddressInfoById")]
        public async Task<IResult> DeleteAddressInfoById([FromRoute] string seqNo)
        {
            var result = await _mediator.Send(new Command(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AddressInfo.DeleteAddressInfoById
{
    public record Command(string seqNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await context.SetUp_AddressInfo.SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.seqNo);

            context.SetUp_AddressInfo.Remove(single);
            await context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.seqNo);
        }
    }
}
