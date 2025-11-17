using ScoreSharp.API.Modules.SetUp.SupplementReason.UpdateSupplementReasonById;

namespace ScoreSharp.API.Modules.SetUp.SupplementReason
{
    public partial class SupplementReasonController
    {
        /// <summary>
        ///  單筆修改補件原因
        /// </summary>
        /// <param name="supplementReasonCode">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{supplementReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改補件原因_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改補件原因_2000_ResEx),
            typeof(修改補件原因查無此資料_4001_ResEx),
            typeof(修改補件原因路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateSupplementReasonById")]
        public async Task<IResult> UpdateSupplementReasonById(
            [FromRoute] string supplementReasonCode,
            [FromBody] UpdateSupplementReasonByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(supplementReasonCode, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.SupplementReason.UpdateSupplementReasonById
{
    public record Command(string supplementReasonCode, UpdateSupplementReasonByIdRequest updateSupplementReasonByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper)
        {
            _context = context;
            _jwthelper = jwtHelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateSupplementReasonByIdRequest = request.updateSupplementReasonByIdRequest;

            if (updateSupplementReasonByIdRequest.SupplementReasonCode != request.supplementReasonCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_SupplementReason.SingleOrDefaultAsync(x =>
                x.SupplementReasonCode == request.supplementReasonCode
            );

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.supplementReasonCode);

            entity.SupplementReasonName = updateSupplementReasonByIdRequest.SupplementReasonName;
            entity.IsActive = updateSupplementReasonByIdRequest.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.supplementReasonCode, request.supplementReasonCode);
        }
    }
}
