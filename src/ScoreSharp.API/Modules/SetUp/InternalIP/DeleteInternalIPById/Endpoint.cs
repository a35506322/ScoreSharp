using ScoreSharp.API.Modules.SetUp.InternalIP.DeleteInternalIPById;

namespace ScoreSharp.API.Modules.SetUp.InternalIP
{
    public partial class InternalIPController
    {
        /// <summary>
        /// 刪除單筆行內IP
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /InternalIP/DeleteInternalIPById/172.28.234.11
        ///
        /// </remarks>
        /// <param name="ip">PK</param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpDelete("{ip}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(
            typeof(刪除行內IP_2000_ResEx),
            typeof(刪除行內IP查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteInternalIPById")]
        public async Task<IResult> DeleteInternalIPById([FromRoute] string ip)
        {
            var result = await _mediator.Send(new Command(ip));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.InternalIP.DeleteInternalIPById
{
    public record Command(string ip) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_InternalIP.SingleOrDefaultAsync(x => x.IP == request.ip);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.ip);

            _context.SetUp_InternalIP.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.ip);
        }
    }
}
