using ScoreSharp.API.Modules.SetUp.CreditCheckCode.UpdateCreditCheckCodeById;

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode
{
    public partial class CreditCheckCodeController
    {
        /// <summary>
        ///  單筆修改徵信代碼
        /// </summary>
        /// <param name="creditCheckCode">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{creditCheckCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改徵信代碼_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改徵信代碼_2000_ResEx),
            typeof(修改徵信代碼查無此資料_4001_ResEx),
            typeof(修改徵信代碼路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateCreditCheckCodeById")]
        public async Task<IResult> UpdateCreditCheckCodeById(
            [FromRoute] string creditCheckCode,
            [FromBody] UpdateCreditCheckCodeByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(creditCheckCode, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.UpdateCreditCheckCodeById
{
    public record Command(string creditCheckCode, UpdateCreditCheckCodeByIdRequest updateCreditCheckCodeByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateCreditCheckCodeByIdRequest = request.updateCreditCheckCodeByIdRequest;

            if (updateCreditCheckCodeByIdRequest.CreditCheckCode != request.creditCheckCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_CreditCheckCode.SingleOrDefaultAsync(x => x.CreditCheckCode == request.creditCheckCode);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.creditCheckCode);

            entity.IsActive = updateCreditCheckCodeByIdRequest.IsActive;
            entity.CreditCheckCodeName = updateCreditCheckCodeByIdRequest.CreditCheckCodeName;
            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.creditCheckCode, request.creditCheckCode);
        }
    }
}
