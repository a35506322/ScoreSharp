using ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.UpdateApplicableInterestRateLimitById;

namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit
{
    public partial class ApplicableInterestRateLimitController
    {
        /// <summary>
        /// 修改單筆判斷適用利率額度
        /// </summary>
        /// <param name="seqNo">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改判斷適用利率額度_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改判斷適用利率額度_2000_ResEx),
            typeof(修改判斷適用利率額度路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateApplicableInterestRateLimitById")]
        public async Task<IResult> UpdateApplicableInterestRateLimitById(
            [FromRoute] Guid seqNo,
            [FromBody] UpdateApplicableInterestRateLimitByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.UpdateApplicableInterestRateLimitById
{
    public record Command(Guid seqNo, UpdateApplicableInterestRateLimitByIdRequest updateApplicableInterestRateLimitByIdRequest)
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
            var updateApplicableInterestRateLimitByIdRequest = request.updateApplicableInterestRateLimitByIdRequest;

            if (request.seqNo != updateApplicableInterestRateLimitByIdRequest.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_ApplicableInterestRateLimit.SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            entity.ApplicableInterestRateLimit = updateApplicableInterestRateLimitByIdRequest.ApplicableInterestRateLimit;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.seqNo.ToString(), request.seqNo.ToString());
        }
    }
}
