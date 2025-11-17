using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.UpdateMakeCardFailedReasonById;

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason
{
    public partial class MakeCardFailedReasonController
    {
        /// <summary>
        /// 單筆修改製卡失敗原因
        /// </summary>
        /// <param name="makeCardFailedReasonCode">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{makeCardFailedReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改製卡失敗原因_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改製卡失敗原因_2000_ResEx),
            typeof(修改製卡失敗原因查無此資料_4001_ResEx),
            typeof(修改製卡失敗原因路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateMakeCardFailedReasonById")]
        public async Task<IResult> UpdateMakeCardFailedReasonById(
            [FromRoute] string makeCardFailedReasonCode,
            [FromBody] UpdateMakeCardFailedReasonByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(makeCardFailedReasonCode, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.UpdateMakeCardFailedReasonById
{
    public record Command(string makeCardFailedReasonCode, UpdateMakeCardFailedReasonByIdRequest updateMakeCardFailedReasonByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Hanlder : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Hanlder(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateMakeCardFailedReasonByIdRequest = request.updateMakeCardFailedReasonByIdRequest;

            if (request.makeCardFailedReasonCode != updateMakeCardFailedReasonByIdRequest.MakeCardFailedReasonCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_MakeCardFailedReason.SingleOrDefaultAsync(x =>
                x.MakeCardFailedReasonCode == request.makeCardFailedReasonCode
            );

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.makeCardFailedReasonCode);

            entity.IsActive = updateMakeCardFailedReasonByIdRequest.IsActive;
            entity.MakeCardFailedReasonCode = updateMakeCardFailedReasonByIdRequest.MakeCardFailedReasonCode;
            entity.MakeCardFailedReasonName = updateMakeCardFailedReasonByIdRequest.MakeCardFailedReasonName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.makeCardFailedReasonCode, request.makeCardFailedReasonCode);
        }
    }
}
