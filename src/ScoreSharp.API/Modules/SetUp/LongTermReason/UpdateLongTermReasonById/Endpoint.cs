using ScoreSharp.API.Modules.SetUp.LongTermReason.UpdateLongTermReasonById;

namespace ScoreSharp.API.Modules.SetUp.LongTermReason
{
    public partial class LongTermReasonController
    {
        /// <summary>
        /// 修改單筆長循分期戶理由碼
        /// </summary>
        /// <param name="code">長循分期戶理由代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改長循分期戶理由碼_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改長循分期戶理由碼_2000_ResEx),
            typeof(修改長循分期戶理由碼查無資料_4001_ResEx),
            typeof(修改長循分期戶理由碼路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateLongTermReasonById")]
        public async Task<IResult> UpdateLongTermReasonById([FromRoute] string code, [FromBody] UpdateLongTermReasonByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.LongTermReason.UpdateLongTermReasonById
{
    public record Command(string code, UpdateLongTermReasonByIdRequest updateLongTermReasonByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateLongTermReasonByIdRequest = request.updateLongTermReasonByIdRequest;

            if (request.code != updateLongTermReasonByIdRequest.LongTermReasonCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_LongTermReason.SingleOrDefaultAsync(x => x.LongTermReasonCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            entity.IsActive = updateLongTermReasonByIdRequest.IsActive;
            entity.ReasonStrength = updateLongTermReasonByIdRequest.ReasonStrength;
            entity.LongTermReasonName = updateLongTermReasonByIdRequest?.LongTermReasonName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
