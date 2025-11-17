using ScoreSharp.API.Modules.SetUp.RejectionReason.UpdateRejectionReasonById;

namespace ScoreSharp.API.Modules.SetUp.RejectionReason
{
    public partial class RejectionReasonController
    {
        /// <summary>
        ///  單筆修改退件原因
        /// </summary>
        /// <param name="rejectionReasonCode">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{rejectionReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改退件原因_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改退件原因_2000_ResEx),
            typeof(修改退件原因查無此資料_4001_ResEx),
            typeof(修改退件原因路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateRejectionReasonById")]
        public async Task<IResult> UpdateRejectionReasonById(
            [FromRoute] string rejectionReasonCode,
            [FromBody] UpdateRejectionReasonByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(rejectionReasonCode, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.RejectionReason.UpdateRejectionReasonById
{
    public record Command(string rejectionReasonCode, UpdateRejectionReasonByIdRequest updateRejectionReasonByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IMapper mapper, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _mapper = mapper;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateRejectionByIdRequest = request.updateRejectionReasonByIdRequest;

            if (updateRejectionByIdRequest.RejectionReasonCode != request.rejectionReasonCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_RejectionReason.SingleOrDefaultAsync(x =>
                x.RejectionReasonCode == request.rejectionReasonCode
            );

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.rejectionReasonCode);

            entity.RejectionReasonName = updateRejectionByIdRequest.RejectionReasonName;
            entity.IsActive = updateRejectionByIdRequest.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.rejectionReasonCode, request.rejectionReasonCode);
        }
    }
}
