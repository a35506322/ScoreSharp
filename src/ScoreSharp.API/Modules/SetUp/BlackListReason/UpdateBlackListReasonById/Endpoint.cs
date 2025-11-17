using ScoreSharp.API.Modules.SetUp.BlackListReason.UpdateBlackListReasonById;

namespace ScoreSharp.API.Modules.SetUp.BlackListReason
{
    public partial class BlackListReasonController
    {
        /// <summary>
        /// 單筆修改黑名單理由
        /// </summary>
        /// <param name="blackListReasonCode">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{blackListReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改黑名單理由_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改黑名單理由_2000_ResEx),
            typeof(修改黑名單理由查無此資料_4001_ResEx),
            typeof(修改黑名單理由路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateBlackListReasonById")]
        public async Task<IResult> UpdateBlackListReasonById(
            [FromRoute] string blackListReasonCode,
            [FromBody] UpdateBlackListReasonByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(blackListReasonCode, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BlackListReason.UpdateBlackListReasonById
{
    public record Command(string blackListReasonCode, UpdateBlackListReasonByIdRequest updateBlackListReasonByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateBlackListReasonByIdRequest = request.updateBlackListReasonByIdRequest;

            if (updateBlackListReasonByIdRequest.BlackListReasonCode != request.blackListReasonCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_BlackListReason.SingleOrDefaultAsync(x =>
                x.BlackListReasonCode == request.blackListReasonCode
            );
            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.blackListReasonCode);

            entity.IsActive = updateBlackListReasonByIdRequest.IsActive;
            entity.BlackListReasonName = updateBlackListReasonByIdRequest.BlackListReasonName;
            entity.ReasonStrength = updateBlackListReasonByIdRequest.ReasonStrength;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.blackListReasonCode, request.blackListReasonCode);
        }
    }
}
